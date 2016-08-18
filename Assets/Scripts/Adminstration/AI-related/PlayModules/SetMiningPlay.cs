using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetMiningPlay : Play
{
	private int TurnAfterMineSet = 0;

	private float[] PriceLimitForBidding;

	private Card PairFirstCard;
	private Card PairSecondCard;
	private Values PairRepresentativeValue;

	private Card CardToBeBid;

	public SetMiningPlay(Enemy _Enemy) : base(_Enemy)
	{
		EnemyInstance = _Enemy;
		CurrentPhase = Phase.NULL;

		TurnAfterMineSet = 0;
		PriceLimitForBidding = new float[2];
		PriceLimitForBidding[0] = 0.0f;
		PriceLimitForBidding[1] = 0.0f;

		CorrespondingAction = PlayerAction.SetMining;
	}

	public override float ApplicabilityOfPlay ()
	{
		//CHECK IF THERE IS ANY RAISE THAT PLAYER NEED TO CALL NOW. IF THERE IS, THIS PLAY IS POSSIBLE. ELSE, RETURN 0 APPLICABILITY
		if(!PlayerInstance.Fold && PlayerInstance.OnTheBet < GMInstance.Table.PotCallRequirement)
			return 0.0f;

		//CHECK IF PLAYER HAS ONE PAIR IN HIS HAND. IF THERE ISN'T, THIS PLAY IS IMPOSSIBLE, RETURN 0 APPLICABILITY
		if(Evaluator.EvaluateHand(PlayerInstance.Hand.ToArray()) != Hands.OnePair)
			return 0.0f;

		float Applicability = 50.0f;

		//CHECK THE VALUE OF PLAYER'S POCKET PAIR. ANY PAIRS ABOVE 7s OR 9s WILL START TO DECREASE THE APPLICABILITY OF THIS PLAY (22s - 66s is base range)
		//NOTE. PERIMETERS LIKE TIGHTNESS AND AGGRESSIVENESS WILL AFFECT THE RANGE OF PAIRS THAT PLAYER WILL PROCEED WITH

		#region FIND THE REPRESENTATIVE VALUE OF THE PAIR
		Values RepresentativePairValue = Values.Two;

		for(int FirstIndex = 0; FirstIndex < PlayerInstance.Hand.Count; FirstIndex++)
		{
			bool RepresentativeValueFound = false;

			for(int SecondIndex = 0; SecondIndex < PlayerInstance.Hand.Count; SecondIndex++)
			{
				if(PlayerInstance.Hand[SecondIndex] != PlayerInstance.Hand[FirstIndex] && PlayerInstance.Hand[SecondIndex].Value == PlayerInstance.Hand[FirstIndex].Value)
				{
					RepresentativePairValue = PlayerInstance.Hand[FirstIndex].Value;
					RepresentativeValueFound = true;

					PairRepresentativeValue = RepresentativePairValue;
					PairFirstCard = PlayerInstance.Hand[FirstIndex];
					PairSecondCard = PlayerInstance.Hand[SecondIndex];

					break;
				}
			}

			if(RepresentativeValueFound)
				break;
		}


		#endregion

		#region FIND THE MAXIMUM PAIR VALUE (ADJUSTMENT ARE DONE THROUGH AGGRESSIVENESS AND TIGHTNESS)
		Values MaxPairValue = Values.Six;

		if(EnemyInstance.Aggressiveness > 5)
		{
			float AdjustmentToMaxValue = ((float) EnemyInstance.Aggressiveness - 5.0f)/5.0f * 2.0f;
			MaxPairValue += (int) AdjustmentToMaxValue; 
		}

		if(EnemyInstance.Tightness < 5)
		{
			float AdjustmentToMaxValue = (5.0f - (float) EnemyInstance.Aggressiveness)/5.0f * 5.0f;
			MaxPairValue += (int) AdjustmentToMaxValue; 
		}
		#endregion

		if(RepresentativePairValue <= MaxPairValue)
			Applicability += 25.0f;
		else if(RepresentativePairValue > MaxPairValue)
			Applicability -= 10.0f * ((float) RepresentativePairValue - (float) MaxPairValue);

		//CHECK THE STACK SIZE OF THE REMAINING PLAYERS IN THE POT, THE DEEPER THEIR STACK IS, THE MORE APPLICABILE THIS PLAY WILL BE
		//NOTE. BY DEEP STACK, IT MEANT AROUND 25X THE FIRST BET/RAISE IN THIS ROUND
		//NOTE. AGGRESSIVENESS WILL DECREASE THE STACKSIZE NEEDED TO BE CONSIDERED AS "DEEP STACKED"
		float StackRequirement = 0.0f;
		float RequirementMultiplier = 25.0f;

		if(EnemyInstance.Aggressiveness > 5)
			RequirementMultiplier = 25.0f + (((float) EnemyInstance.Aggressiveness - 5.0f)/5.0f * -10.0f);

		if(GMInstance.Betting.LatestRaiseAmount == 0.0f)
			StackRequirement = RequirementMultiplier * (float) GMInstance.Betting.FirstBetAmount;

		else
			StackRequirement = RequirementMultiplier * (float) GMInstance.Betting.LatestRaiseAmount;

		for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
		{
			if(GMInstance.Players[PlayerIndex].Stack >= StackRequirement)
				Applicability += 5.0f;
			else
				Applicability -= 10.0f;
		}

		//CHECK WHETHER PLAYER IS IN THE BUTTON, IT WILL INCREASE THE APPLICABILITY OF PLAY SIGNIFICANTLY.
		if(GMInstance.Betting.DeterminePlayerPosition(PlayerInstance) == TablePosition.OnTheButton)
			Applicability += 10.0f;
		else
			Applicability -= 25.0f;

		//DETERMINE WHETHER THE PLAYER THAT OPEN/RAISE THE POT WILL CONTINUE WITH THEIR HAND AND BET
		//NOTE. EVALUATE THEIR HAND RANGE, THE TIGHTER AND HIGHER THE RANGE, THE MORE CHANCE THEY WILL CONTINUE. THE HAND WITHIN THE RANGE MUST BE AT LEAST ACE HIGH AND ABOVE
		//      EVALUATE THEIR AGGRESSIVENESS, IT WILL CONTRIBUTE IN THEIR CHANCE OF CONTINUING THE BET
		//CAUTION. WHEN IN HIGHER RANGE, BE AWARE OF OVERPAIR IN THAT RANGE. DECREASE THE APPLICABILITY FOR EACH POSSIBLE OVERPAIR
		//FACTOR. RATIO OF SUITABLE HANDS TO TOTAL HANDS, THAT PLAYER'S AGGRESSIVENESS, TENDENCY TO FOLD/CHECK AS COMPARED TO TENDENCY TO RAISE
		Player LatestAggressivePlayer = GMInstance.Betting.LatestAggressivePlayer;
		List<Card[]> Range = Evaluator.DetermineRangeOfOpponent(PlayerInstance,LatestAggressivePlayer,true);
		List<Card[]> EligibleHands = new List<Card[]>();

		for(int HandIndex = 0; HandIndex < Range.Count; HandIndex++)
		{
			Hands HandType = Evaluator.EvaluateHand(Range[HandIndex]);

			if(HandType >= Hands.OnePair)
			{	
				EligibleHands.Add(Range[HandIndex]);
			}
			else if(HandType == Hands.HighCard)
			{
				Values RepresentativeValue = Values.Two;

				for(int FirstCardIndex = 0; FirstCardIndex < Range[HandIndex].Length; FirstCardIndex++)
				{
					bool PairValueFound = false;

					for(int SecondCardIndex = 0; SecondCardIndex < Range[HandIndex].Length; SecondCardIndex++)
					{
						if(Range[HandIndex][SecondCardIndex] != Range[HandIndex][FirstCardIndex] && Range[HandIndex][SecondCardIndex].Value == Range[HandIndex][FirstCardIndex].Value)
						{
							RepresentativeValue = Range[HandIndex][SecondCardIndex].Value;
							PairValueFound = true;
							break;
						}
					}

					if(PairValueFound)
						break;
				}

				if(RepresentativePairValue == Values.Ace)
					EligibleHands.Add(Range[HandIndex]);
			}
		}

		float EligibleRatio = (float) EligibleHands.Count/(float) Range.Count;

		int TotalAmountOfActions = 0;
		int AmountOfCheckingFolding = 0;
		int AmountOfRaising = 0;

		for(int LogIndex = 0; LogIndex < GMInstance.CommonMemoryLog.MemoryLog.Count; LogIndex++)
		{
			if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].PlayerIndex == LatestAggressivePlayer.Index)
			{
				TotalAmountOfActions++;

				if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Raise)
					AmountOfRaising++;
				
				else if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Check || GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Fold)
					AmountOfCheckingFolding++; 
			}
		}

		if(TotalAmountOfActions == 0)
			return 0.0f;

		float CheckFoldPercentage = AmountOfCheckingFolding/TotalAmountOfActions * 100.0f;
		float RaisingPercentage = AmountOfRaising/TotalAmountOfActions * 100.0f;

		if(CheckFoldPercentage >= RaisingPercentage)
			Applicability = (CheckFoldPercentage - RaisingPercentage)/RaisingPercentage * 25.0f;
		else if(RaisingPercentage > CheckFoldPercentage)
			Applicability -= 25.0f;

		int LatestPlayerAggressiveness = EnemyInstance.Memory.OpponentsAggressiveness[LatestAggressivePlayer.Index - 1];

		if(LatestPlayerAggressiveness >= 5.0f)
			Applicability += ((float) LatestPlayerAggressiveness - 5.0f)/5.0f * 10.0f;

		else if(LatestPlayerAggressiveness < 5.0f)
			Applicability += (5.0f - (float) LatestPlayerAggressiveness)/5.0f * -10.0f;

		//EVALUATE THE TENDENCY OF OTHER PLAYERS AFTER THE PLAYER (SMALL-BIND, BIG-BIND) AND CHECK THEIR TENDENCY TO SIMPLY CALL
		//NOTE. IF THERE ARE PLAYERS WITH TENDENCY TO FOLD ALOT (FISH), INCREASE THE APPLICABILITY OF THIS PLAY
		//NOTE. IF THERE ARE AGGRESSIVE PLAYERS THAT MAY RE-RAISE, DECREASE THE APPLICABILITY OF THIS PLAY
		for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
		{
			if(GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index && GMInstance.Players[PlayerIndex].Index != LatestAggressivePlayer.Index)
			{
				int TotalAmountOfPlayerActions = 0;
				int AmountOfPlayerChecking = 0;
				int AmountOfPlayerFolding = 0;
				int AmountOfPlayerRaising = 0;

				for(int LogIndex = 0; LogIndex < GMInstance.CommonMemoryLog.MemoryLog.Count; LogIndex++)
				{
					if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].PlayerIndex == GMInstance.Players[PlayerIndex].Index)
					{
						TotalAmountOfActions++;

						if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Check)
							AmountOfPlayerChecking++;

						else if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Fold)
							AmountOfPlayerFolding++;

						if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Raise)
							AmountOfPlayerRaising++;
					}
				}

				float CheckingRate = (float) AmountOfPlayerChecking / (float) TotalAmountOfActions * 100.0f;
				float FoldingRate = (float) AmountOfPlayerFolding / (float) TotalAmountOfActions * 100.0f;
				float RaisingRate = (float) AmountOfPlayerRaising / (float) TotalAmountOfActions * 100.0f;

				if(FoldingRate >= 50.0f)
					Applicability += 10.0f;

				if(RaisingRate > 30.0f)
					Applicability -= 15.0f;

				if(CheckingRate > 30.0f)
					Applicability += 10.0f;
			}
		}

		return Applicability;
	}

	public override void StartPlay ()
	{
		PlayInUse = true;
		CurrentPhase = Phase.One;
	}

	public override BettingDecision ExecuteBetPlay ()
	{
		//SET THE MINE BY CALLING THE PREVIOUS RAISE AND INCREASE THE TURNAFTERMINESET COUNTER
		//NOTE. THE COUNTER TRACK HOW MANY TIME THE BET HAS GO AROUND THE TABLE AFTER THE CALL TO THE RAISE
		if(CurrentPhase == Phase.One && TurnAfterMineSet == 0)
		{
			TurnAfterMineSet++;
			CurrentPhase = Phase.Two;
			return BettingDecision.Call;
		}

		//CHECK WHETHER THE ROUND HAS ENTER AN AUCTION PHASE, IF IT DOESNT:
		if(CurrentPhase == Phase.Two && GMInstance.Phase != TurnPhase.Auctioning && TurnAfterMineSet > 0)
		{
			//THE TURN COME BACK TO THE PLAYER, CHECK WHETHER ANY PLAYER RERAISE AGAIN.
			bool HasAnyPlayerReraise = false;

			for(int LogIndex = GMInstance.CommonMemoryLog.MemoryLog.Count; LogIndex > GMInstance.CommonMemoryLog.MemoryLog.Count - 4; LogIndex--)
			{
				if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].RoundNum == GMInstance.CurrentRoundIndex 
				   && GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Raise)
				{
				   HasAnyPlayerReraise = true;
				   break;
				}
			}

			//DETERMINE IS IT WORTH TO CALL THE RAISE OR 3 BET. IF BOTH MEAN IS NOT WORTH, FOLD THE HAND
			if(HasAnyPlayerReraise)
			{
				List<Player> PlayersRemainingInPot = new List<Player>();

				for(int RaiseLogIndex = GMInstance.CommonMemoryLog.MemoryLog.Count; RaiseLogIndex > GMInstance.CommonMemoryLog.MemoryLog.Count - 4; RaiseLogIndex--)
				{
					if(GMInstance.CommonMemoryLog.MemoryLog[RaiseLogIndex].RoundNum == GMInstance.CurrentRoundIndex 
					   && GMInstance.CommonMemoryLog.MemoryLog[RaiseLogIndex].P_Action == PlayerAction.Raise
					   && GMInstance.CommonMemoryLog.MemoryLog[RaiseLogIndex].PlayerIndex != PlayerInstance.Index)
				    {
						PlayersRemainingInPot.Add(GMInstance.Players[GMInstance.CommonMemoryLog.MemoryLog[RaiseLogIndex].PlayerIndex - 1]);
						
						for(int CallLogIndex = RaiseLogIndex + 1; CallLogIndex < GMInstance.CommonMemoryLog.MemoryLog.Count; CallLogIndex++)
						{
							if(GMInstance.CommonMemoryLog.MemoryLog[CallLogIndex].RoundNum == GMInstance.CurrentRoundIndex 
								&& GMInstance.CommonMemoryLog.MemoryLog[CallLogIndex].P_Action == PlayerAction.Call
								&& GMInstance.CommonMemoryLog.MemoryLog[CallLogIndex].PlayerIndex != PlayerInstance.Index)
							{
								PlayersRemainingInPot.Add(GMInstance.Players[GMInstance.CommonMemoryLog.MemoryLog[CallLogIndex].PlayerIndex - 1]);
							}
						}

						break;
				    }
				}

				float FoldMinPercentage = 50.0f;

				int TotalPlayersAction = 0;
				int TotalPlayersCallFold = 0;
				int TotalPlayersReraise = 0;

				for(int PlayerIndex = 0; PlayerIndex < PlayersRemainingInPot.Count; PlayerIndex++)
				{
					int TotalAmountOfActions = 0;
					int AmountOfCallsFold = 0;
					int AmountOfReraise = 0;

					for(int LogIndex = 0; LogIndex < GMInstance.CommonMemoryLog.MemoryLog.Count; LogIndex++)
					{
						if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].RoundNum == GMInstance.CurrentRoundIndex 
						&& GMInstance.CommonMemoryLog.MemoryLog[LogIndex].PlayerIndex != PlayersRemainingInPot[PlayerIndex].Index)
					    {
							TotalAmountOfActions++;

							if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Call || GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Fold)
								AmountOfCallsFold++;

							else if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Raise && GMInstance.CommonMemoryLog.MemoryLog[LogIndex].RaiseMadeInRound > 1)
								AmountOfReraise++;
					    }
					}

					TotalPlayersAction += TotalAmountOfActions;
					TotalPlayersCallFold += AmountOfCallsFold;
					TotalPlayersReraise += AmountOfReraise;
				}

				TotalPlayersAction /= PlayersRemainingInPot.Count;
				TotalPlayersCallFold /= PlayersRemainingInPot.Count;
				TotalPlayersReraise /= PlayersRemainingInPot.Count;

				float AverageCallFoldPercentage = (float) TotalPlayersCallFold / (float) TotalPlayersAction * 100.0f;
				float AverageReraisePercentage = (float) TotalPlayersReraise / (float) TotalPlayersAction * 100.0f;

				float PlayerAverageEquity = Evaluator.CalculatePlayerAverageEquity(PlayerInstance);

				float CallingEV = PlayerAverageEquity * ((float) GMInstance.Table.Pot) - (1.0f - PlayerAverageEquity) * ((float) PlayerInstance.OnTheBet);
				float RaisingEV = PlayerAverageEquity * ((float) PlayersRemainingInPot.Count * 5.0f) - (1.0f - PlayerAverageEquity) * (5.0f); //NOTE. THE 5.0F IS JUST A PLACEHOLDER TO SEE WHETHER THE 5.0F WILL WORK

				if(AverageCallFoldPercentage >= 50.0f && CallingEV > 0.0f && AverageCallFoldPercentage >= AverageReraisePercentage)
					return BettingDecision.Call;

				else if(AverageReraisePercentage >= 50.0f && AverageReraisePercentage >= AverageCallFoldPercentage)
				{
					if(RaisingEV > 0.0f)
						return BettingDecision.Raise;
					else
						return BettingDecision.Fold;
				}

				return BettingDecision.Fold;
			}
			else if(!HasAnyPlayerReraise && PlayerInstance.OnTheBet == GMInstance.Table.GetHighestBet())
			{
				return BettingDecision.Check;
			}
		}
		
		//iF IT DOES:
			//EXIT THIS FUNCTION
		CurrentPhase = Phase.One;
		return BettingDecision.NULL;
	}

	public override AuctionDecision ExecuteAuctionPlay ()
	{
		//IF THE ROUND IS NOT IN AUCTION PHASE
			// EXIT THIS FUNCTION
		if(GMInstance.Phase != TurnPhase.Auctioning)
			return AuctionDecision.NULL;

		if(CurrentPhase == Phase.One)
		{
			Hands HighestHandType = Evaluator.EvaluateHand(PlayerInstance.Hand);
			CardToBeBid = new Card(Suits.Clubs,Values.Two);
			bool CardFound = false;

			for(int PoolIndex = 0; PoolIndex < GMInstance.Table.Pool.Count; PoolIndex++)
			{
				CardFound = false;

				for(int CardIndex = 0; CardIndex < PlayerInstance.Hand.Count; CardIndex++)
				{
					List<Card> PossibleHand = new List<Card>(PlayerInstance.Hand.ToArray());
					PossibleHand[CardIndex] = GMInstance.Table.Pool[PoolIndex];
				
					if(Evaluator.EvaluateHand(PossibleHand) == Hands.ThreeOfAKind)
					{
						CardToBeBid = GMInstance.Table.Pool[PoolIndex];
						CardFound = true;
						break;
					}
				}

				if(CardFound)
					break;
			}

			if(!CardFound)
			{
				//SINCE THERE ISNT ANY CARD IN POOL THAT HELP FORM A SET, DETERMINE WHETHER THE PLAYER IS SMART AND AGGRESSIVE ENOUGH TO BID FOR A CARD THAT HELP TO GIVE AN ILLUSION OF A MONSTER HAND

				//CHECK WHETHER THERE A CARD THAT COMBINE WITH REVEAL CARDS THAT CAN FORM THAT MONSTER HAND
				Card CardInducingBestHand = new Card(Suits.Clubs,Values.Two);
				int HighestHandValue = Evaluator.DetermineHandLevelINT(PlayerInstance.Hand.ToArray());

				for(int CardIndex = 0; CardIndex < GMInstance.Table.Pool.Count; CardIndex++)
				{
					PlayerInstance.Pocket.Add(GMInstance.Table.Pool[CardIndex]);
					LinkedList<Card[]> PossibleHands = Evaluator.DeterminePossibleHand(PlayerInstance,false,true);
					LinkedListNode<Card[]> TraverseNode = PossibleHands.First;

					for(int HandIndex = 0; HandIndex < PossibleHands.Count; HandIndex++)
					{
						if(Utility.DoesHandContainThisCard(TraverseNode.Value,GMInstance.Table.Pool[CardIndex]) 
						   && Evaluator.DetermineHandLevelINT(TraverseNode.Value) > HighestHandValue)
						{
							HighestHandValue = Evaluator.DetermineHandLevelINT(TraverseNode.Value);
							CardInducingBestHand = GMInstance.Table.Pool[CardIndex];
						}

						TraverseNode = TraverseNode.Next;
					}
				}

				if(HighestHandValue <= Evaluator.DetermineHandLevelINT(PlayerInstance.Hand.ToArray()))
					return AuctionDecision.Forfeit;

				//CHECK WHETHER THE PLAYER IS SMART/AGGRRESIVE ENOUGH
				if(EnemyInstance.Skill <= 2.5f || EnemyInstance.Aggressiveness < 7.5f)
					return AuctionDecision.Forfeit;

				//CHECK WHETHER THAT CARD IS HIGHLY CONTESTED OR NOT, IF IT IS, TRY TO DETEREMINE THE PLAYERS THAT WILL CONTEST FOR THAT CARD. 
				int PlayerHandValue = Evaluator.DeterminePlayerHandLevelINT(PlayerInstance);
				List<Player> PlayersContestingSameCard = new List<Player>();

				for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
				{
					if(GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index)
					{
						List<Card[]> EnemyRangeBeforeImprovement = Evaluator.DetermineRangeOfOpponent(PlayerInstance, GMInstance.Players[PlayerIndex],false);

						int HighestValueBeforeImprovement = 0;
						int SuperiorHandsBeforeImprovement = 0;

						for(int HandIndex = 0; HandIndex < EnemyRangeBeforeImprovement.Count; HandIndex++)
						{
							int PossibleHandValue = Evaluator.DetermineHandLevelINT(EnemyRangeBeforeImprovement[HandIndex]);

							if(PossibleHandValue > HighestValueBeforeImprovement)
								HighestValueBeforeImprovement = PossibleHandValue;

							if(PossibleHandValue > PlayerHandValue)
								SuperiorHandsBeforeImprovement++;
						}

						GMInstance.Players[PlayerIndex].Pocket.Add(CardInducingBestHand);

						List<Card[]> EnemyRangeAfterImprovement = Evaluator.DetermineRangeOfOpponent(PlayerInstance, GMInstance.Players[PlayerIndex],true);

						int HighestValueAfterImprovement = 0;
						int SuperiorHandsAfterImprovement = 0;

						for(int HandIndex = 0; HandIndex < EnemyRangeAfterImprovement.Count; HandIndex++)
						{
							int PossibleHandValue = Evaluator.DetermineHandLevelINT(EnemyRangeAfterImprovement[HandIndex]);

							if(PossibleHandValue > HighestValueAfterImprovement)
								HighestValueAfterImprovement = PossibleHandValue;

							if(PossibleHandValue > PlayerHandValue)
								SuperiorHandsAfterImprovement++;
						}

						GMInstance.Players[PlayerIndex].Pocket.Remove(CardInducingBestHand);

						if(HighestValueAfterImprovement * SuperiorHandsAfterImprovement > HighestValueBeforeImprovement * SuperiorHandsBeforeImprovement)
							PlayersContestingSameCard.Add(GMInstance.Players[PlayerIndex]);
					}
				}

				if(PlayersContestingSameCard.Count == 0)
				{
					PlayerInstance.SelectedCardForAuction = CardInducingBestHand;
					return AuctionDecision.NULL;
				}

				//DETERMINE WHETHER THEY HAVE TENDENCY OF FORFEITING AUCTION. IF NOT, CHECK WHETHER THE PLAYER HAS A SIGNIFICANT STACK LEAD OVER THOSE PLAYERS.
				int TotalAmountOfActions = 0;
				int TotalAmountOfForfeit = 0;

				for(int PlayerIndex = 0; PlayerIndex < PlayersContestingSameCard.Count; PlayerIndex++)
				{
					for(int LogIndex = 0; LogIndex < GMInstance.CommonMemoryLog.MemoryLog.Count; LogIndex++)
					{
						if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].PlayerIndex == PlayersContestingSameCard[PlayerIndex].Index
						   && GMInstance.CommonMemoryLog.MemoryLog[LogIndex].T_Phase == TurnPhase.Auctioning)
						{
							TotalAmountOfActions++;

							if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Forfeit)
								TotalAmountOfForfeit++;
						}
					}	
				}

				float ForfeitingPercentage = (float) TotalAmountOfForfeit / (float) TotalAmountOfActions;

				//IF THOSE PLAYERS HAVE A TENDENCY OF FORFETING AUCTION OR THEY ARE SIGNIFICANTLY SHORTER STACK THAN THE PLAYER, THE PLAYER WILL CONTEST FOR THE CARD
				float ForfeitingRequirement = 50.0f;

				if(EnemyInstance.Aggressiveness > 5)
					ForfeitingRequirement += ((float) EnemyInstance.Aggressiveness - 5.0f)/5.0f * -20.0f;

				else if(EnemyInstance.Aggressiveness < 5)
					ForfeitingRequirement += (5.0f - (float) EnemyInstance.Aggressiveness)/5.0f * 20.0f;

				float OtherPlayersStack = 0.0f;

				for(int PlayerIndex = 0; PlayerIndex < PlayersContestingSameCard.Count; PlayerIndex++)
				{
					OtherPlayersStack += PlayersContestingSameCard[PlayerIndex].Stack;
				}

				float AverageStack = OtherPlayersStack / (float) PlayersContestingSameCard.Count;

				if(AverageStack <= PlayerInstance.Stack)
					ForfeitingRequirement -= 10.0f;

				else
					ForfeitingRequirement += 5.0f;

				if(ForfeitingPercentage >= ForfeitingRequirement)
				{
					CardToBeBid = CardInducingBestHand;
					PlayerInstance.SelectedCardForAuction = CardInducingBestHand;
					return AuctionDecision.NULL;
				}
				//ELSE, FORFEIT THE AUCTION
				else
				{
					return AuctionDecision.Forfeit;
				}

//				float DesirabilityToForfeit = 0.0f;
//
//				Deck SimulatedDeck = new Deck();
//
//				for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
//				{
//					if(GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index)
//					{
//						for(int HandIndex = 0; HandIndex < GMInstance.Players[PlayerIndex].Hand; HandIndex++)
//							SimulatedDeck.RemoveSpecificCard(GMInstance.Players[PlayerIndex].Hand[HandIndex]);
//						
//						for(int PocketIndex = 0; PocketIndex < GMInstance.Players[PlayerIndex].Pocket; PocketIndex++)
//							SimulatedDeck.RemoveSpecificCard(GMInstance.Players[PlayerIndex].Pocket[PocketIndex]);
//					}
//				}
//
//				for(int CardIndex = 0; CardIndex < GMInstance.Table.Pool.Count; CardIndex++)
//				{
//					SimulatedDeck.RemoveSpecificCard(GMInstance.Table.Pool[CardIndex]);
//				}
//
//				int AmountOfOutsForSet = 0;
//				for(int CardIndex = 0; CardIndex < SimulatedDeck.Cards.Count; CardIndex++)
//				{
//					if(SimulatedDeck.Cards[CardIndex].Value == PairRepresentativeValue 
//					   && SimulatedDeck.Cards[CardIndex].Suit != PairFirstCard.Suit
//					   && SimulatedDeck.Cards[CardIndex].Suit != PairSecondCard.Suit)
//					{
//						AmountOfOutsForSet++;
//					}
//				}
//
//				float ProbabilityOfDrawingSet = (float) AmountOfOutsForSet / (float) SimulatedDeck.Cards.Count;
//
//				DesirabilityToForfeit += ProbabilityOfDrawingSet/6.0f * 30.0f;
//
//				int AmountOfTopTierCardsInPool = 0;
//				for(int CardIndex = 0; CardIndex < GMInstance.Table.Pool.Count; CardIndex++)
//				{
//					if(GMInstance.Table.Pool[CardIndex].Value == Values.Ten || GMInstance.Table.Pool[CardIndex].Value == Values.Jack || GMInstance.Table.Pool[CardIndex].Value == Values.Queen
//					   GMInstance.Table.Pool[CardIndex].Value == Values.King)
//					{
//						AmountOfTopTierCardsInPool++;
//				    }
//				}
//
//				DesirabilityToForfeit -= AmountOfOutsForSet * 5.0f;
//
//				int PlayersCompletingHandsSuperiorThanSet = 0;
//				for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
//				{
//					if(GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index)
//					{
//						Card CardSelectedByPlayer = GMInstance.Players[PlayerIndex].SelectedCardForAuction;
//						GMInstance.Players[PlayerIndex].Pocket.Add(CardSelectedByPlayer);
//						List<Card[]> PlayerRange = Evaluator.DetermineRangeOfOpponent(PlayerIndex,GMInstance.Players[PlayerIndex],true);
//						GMInstance.Players[PlayerIndex].Pocket.Remove(CardSelectedByPlayer);
//
//						for(int HandIndex = 0; HandIndex < PlayerRange.Count; HandIndex++)
//						{
//							if(Evaluator.EvaluateHand(PlayerRange[HandIndex]) > Hands.ThreeOfAKind)
//							{
//								PlayersCompletingHandsSuperiorThanSet++;
//								break;
//							}
//						}
//					}
//				}
//
//				DesirabilityToForfeit -= AmountOfOutsForSet * 5.0f;
//
//				if(EnemyInstance.Aggressiveness > 5.0f)
//					DesirabilityToForfeit -= ((float) EnemyInstance.Aggressiveness - 5.0f)/5.0f * 15.0f;
//
//				else if(EnemyInstance.Aggressiveness < 5.0f)
//					DesirabilityToForfeit += (5.0f - (float) EnemyInstance.Aggressiveness)/5.0f * 15.0f;
//
//				if(DesirabilityToForfeit >= 50.0f)
//					return AuctionDecision.Forfeit;
//
//				PlayerInstance.SelectedCardForAuction = GMInstance.Table.Pool[Random.Range(0,GMInstance.Table.Pool.Count)];
//				return AuctionDecision.NULL;
			}

			PlayerInstance.SelectedCardForAuction = CardToBeBid;
			return AuctionDecision.NULL;
		}
		else if(CurrentPhase == Phase.Two)
		{
			if(PriceLimitForBidding[0] == 0.0f && PriceLimitForBidding[1] == 0.0f)
			{
				float StackToPot = GMInstance.Table.GetEffectiveStackSize() / GMInstance.Table.Pot;
				if(StackToPot >= 3.5f && StackToPot < 5.5f)
				{
					PriceLimitForBidding[0] = 0.75f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 0.75f * GMInstance.Table.Pot;
				}
				else if(StackToPot >= 5.5f && StackToPot < 7.0f)
				{
					PriceLimitForBidding[0] = 1.25f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 0.75f * GMInstance.Table.Pot;
				}
				else if(StackToPot >= 7.0f && StackToPot < 9.0f)
				{
					PriceLimitForBidding[0] = 1.125f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 1.125f * GMInstance.Table.Pot;
				}
				else if(StackToPot >= 9.0f && StackToPot < 13.0f)
				{
					PriceLimitForBidding[0] = 1.375f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 1.125f * GMInstance.Table.Pot;
				}
				else if(StackToPot >= 13.0f)
				{
					PriceLimitForBidding[0] = 1.5f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 1.5f * GMInstance.Table.Pot;
				}
			}

			List<Card> PoolRanking = new List<Card>(GMInstance.Table.Pool);

			for(int FirstIndex = 0; FirstIndex < PoolRanking.Count - 1; FirstIndex++)
			{
				for(int SecondIndex = FirstIndex + 1; SecondIndex < PoolRanking.Count; SecondIndex++)
				{
					if(PoolRanking[FirstIndex].Value <  PoolRanking[SecondIndex].Value)
					{
						Card TempCard = PoolRanking[FirstIndex];
						PoolRanking[FirstIndex] = PoolRanking[SecondIndex];
						PoolRanking[SecondIndex] = TempCard;
					}
					else if(PoolRanking[FirstIndex].Value == PoolRanking[SecondIndex].Value)
					{
						if(PoolRanking[FirstIndex].Suit < PoolRanking[SecondIndex].Suit)
						{
							Card TempCard = PoolRanking[FirstIndex];
							PoolRanking[FirstIndex] = PoolRanking[SecondIndex];
							PoolRanking[SecondIndex] = TempCard;
						}
					}
				} 
			}

			int BiddingCardRank = 0;

			for(int PoolIndex = 0; PoolIndex < PoolRanking.Count; PoolIndex++)
			{
				if(CardToBeBid.Suit == PoolRanking[PoolIndex].Suit && CardToBeBid.Value == PoolRanking[PoolIndex].Value)
				{
					BiddingCardRank = PoolIndex + 1;
					break;
				}
			}

			float ClampedAggressiveness = 0.0f;

			if(EnemyInstance.Aggressiveness == 5)
			{
				ClampedAggressiveness = 0.0f;
			}
			else if(EnemyInstance.Aggressiveness < 5)
			{
				ClampedAggressiveness = (5.0f - (float) EnemyInstance.Aggressiveness) * -1.0f;
			}
			else if(EnemyInstance.Aggressiveness > 5)
			{
				ClampedAggressiveness = ((float) EnemyInstance.Aggressiveness - 5.0f) * 1.0f;
			}

			float BiddingAmount = BiddingCardRank * 0.25f + ClampedAggressiveness * 0.05f;
			BiddingAmount = Mathf.Clamp(BiddingAmount,0.0f, PriceLimitForBidding[0]);

			PlayerInstance.RaiseValue = (int) BiddingAmount;
			return AuctionDecision.Bid;
		}
		else if(CurrentPhase == Phase.Three)
		{
			//CHECK WHETHER PLAYER CURRENTLY HAS A SET FOR THEIR HAND/POCKET. 
			Hands HandTypeWithPocket = Evaluator.EvaluatePlayerHandWithPocket(PlayerInstance);

			if(HandTypeWithPocket >= Hands.ThreeOfAKind)
			{
				//IF THEY DO, CHECK WHETHER ANY OF THE POOL CARDS HELP TO STRENGTH THE PLAYER'S HAND TO BE SUPERIOR THAN A SET
				Card CardBringingHighestValue = new Card(Suits.Clubs,Values.Two);
				Hands HighestHandType = Hands.HighCard;

				for(int CardIndex = 0; CardIndex < GMInstance.Table.Pool.Count; CardIndex++)
				{
					PlayerInstance.Pocket.Add(GMInstance.Table.Pool[CardIndex]);
					Hands PossibleHighestHandType = Evaluator.EvaluatePlayerHandWithPocket(PlayerInstance);
					PlayerInstance.Pocket.Remove(GMInstance.Table.Pool[CardIndex]);

					if(PossibleHighestHandType > HighestHandType)
					{
						HighestHandType = PossibleHighestHandType;
						CardBringingHighestValue = GMInstance.Table.Pool[CardIndex];
					}
				}

				if(HighestHandType > HandTypeWithPocket)
				{
					//CHECK WHETHER THE PLAYER INTEND TO BID FOR ANOTHER CARD. IF THEY DO, BID FOR THAT CARD
					//FACTOR. AGGRESSIVENESS OF THAT PLAYER, PROBABILITY OF WINNING ENEMY WITH THE "NEW" HAND, 
					//HOW MUCH THE CARD WILL BE CONTESTED , HOW MUCH PLAYER'S STACK STAND IN THE TABLE
					float DesirabilityToGetNewCard = 0.0f;

					#region Calculate probability of winning with a hand form with the new card
					int TotalEnemyPossibleHands = 0;
					int TotalEnemyLosingHands = 0;

					int ActivePlayersInPot = 0;

					for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
					{
						if(!GMInstance.Players[PlayerIndex].Busted && !GMInstance.Players[PlayerIndex].Fold && !GMInstance.Players[PlayerIndex].Forfeit)
						{
							ActivePlayersInPot++;

							if(GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index)
							{
								GMInstance.Players[PlayerIndex].Pocket.Add(CardBringingHighestValue);
								List<Card[]> EnemyRange = Evaluator.DetermineRangeOfOpponent(PlayerInstance,GMInstance.Players[PlayerIndex],true);
								GMInstance.Players[PlayerIndex].Pocket.Remove(CardBringingHighestValue);
								
								for(int HandIndex = 0; HandIndex < EnemyRange.Count; HandIndex++)
								{
									TotalEnemyPossibleHands++;
									
									Hands PossibleHandType = Evaluator.EvaluateHand(EnemyRange[HandIndex]);
									
									if(HighestHandType >= PossibleHandType)
										TotalEnemyLosingHands++;
								}
							}
						} 
					}

					float ProbabilityOfWinningEnemy = TotalEnemyLosingHands/TotalEnemyPossibleHands;
					#endregion

					DesirabilityToGetNewCard += 25.0f * ProbabilityOfWinningEnemy;

					#region Find how far the players will go for the new card by finding the tier of the card in the pool
					Card[] PoolCardsByRange = Utility.SortHandByCards(GMInstance.Table.Pool.ToArray(),true);
					int RankOfContestingCard = 0;

					for(int CardIndex = 0; CardIndex < PoolCardsByRange.Length; CardIndex++)
					{
						if(Utility.IsTwoCardsIdentical(CardBringingHighestValue,PoolCardsByRange[CardIndex]))
						{
							RankOfContestingCard = CardIndex + 1;
							break;
						}
					}
					#endregion

					DesirabilityToGetNewCard += RankOfContestingCard * 5.0f;

					#region Find the ranking of the player on the table in terms of stack
					int PlayerCount = 0;
					int PlayerRankByStack = 0;
					int[] StackRanking = new int[ActivePlayersInPot];
	
					for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
					{
						if(!GMInstance.Players[PlayerIndex].Busted && !GMInstance.Players[PlayerIndex].Fold 
						&& !GMInstance.Players[PlayerIndex].Forfeit)
						{
							StackRanking[PlayerCount] = GMInstance.Players[PlayerIndex].Stack;
							PlayerCount++;
						}
					}

					for(int FirstStackIndex = 0; FirstStackIndex < StackRanking.Length; FirstStackIndex++)
					{
						for(int SecondStackIndex = 0; SecondStackIndex < StackRanking.Length; SecondStackIndex++)
						{
							if(StackRanking[FirstStackIndex] < StackRanking[SecondStackIndex])
							{
								int TempStack = StackRanking[FirstStackIndex];
								StackRanking[FirstStackIndex] = StackRanking[SecondStackIndex];
								StackRanking[SecondStackIndex] = TempStack;
							}
						}
					}

					for(int StackIndex = 0; StackIndex < StackRanking.Length; StackIndex++)
					{
						if(StackRanking[StackIndex] == PlayerInstance.Stack)
						{
							PlayerRankByStack = StackIndex + 1;
							break;
						}
					}
					#endregion

					DesirabilityToGetNewCard += PlayerRankByStack * 5.0f;

					if(EnemyInstance.Aggressiveness > 5)
					{
						DesirabilityToGetNewCard += ((float) EnemyInstance.Aggressiveness - 5.0f)/5.0f * 10.0f;
					}
					else if(EnemyInstance.Aggressiveness < 5)
					{
						DesirabilityToGetNewCard -= (5.0f - (float) EnemyInstance.Aggressiveness)/5.0f * 10.0f;
					}

					if(DesirabilityToGetNewCard > 50.0f)
					{
						PlayerInstance.SelectedCardForAuction = CardBringingHighestValue;
						return AuctionDecision.NULL;
					}

					return AuctionDecision.Forfeit;
				}
				//ELSE, FORFEIT
				else
				{
					return AuctionDecision.Forfeit;
				}

				return AuctionDecision.NULL;
			}

			else if(HandTypeWithPocket < Hands.ThreeOfAKind)
			{
				//IF THE PLAYER STILL HAS A HAND THAT IS WEAKER THAN A SET, GO THROUGH THE POOL CARDS TO SEE IF THERE IS ANY THAT IMPROVE THE PLAYER HAND
				//DETERMINE WHICH CARD WILL HELPED TO MOST AND BID FOR THAT CARD.
				Card CardBringingHighestValue = new Card(Suits.Clubs,Values.Two);
				Hands HighestHandType = Hands.HighCard;

				for(int CardIndex = 0; CardIndex < GMInstance.Table.Pool.Count; CardIndex++)
				{
					PlayerInstance.Pocket.Add(GMInstance.Table.Pool[CardIndex]);
					Hands PossibleHighestHandType = Evaluator.EvaluatePlayerHandWithPocket(PlayerInstance);
					PlayerInstance.Pocket.Remove(GMInstance.Table.Pool[CardIndex]);

					if(PossibleHighestHandType > HighestHandType)
					{
						HighestHandType = PossibleHighestHandType;
						CardBringingHighestValue = GMInstance.Table.Pool[CardIndex];
					}
				}

				if(HighestHandType > HandTypeWithPocket)
				{
					float DesirabilityToBidNewCard = 100.0f;

					//BY DEFAULT, THE PLAYER WILL DEFINITELY WANT TO GET A CARD TO IMPROVE THEIR SMALL ONE PAIR HAND. BUT IF THERE ARE FACTORS THAT STATE 
					//IT IS BETTER TO NOT BID, SAVE MONEY FROM THIS BET AND LOOK AHEAD TO THE NEXT ROUND
					//FACTORS. HOW MUCH DO THIS CARD IMPROVE THE HAND (HAND LEVEL), HOW CONTESTABLE THIS CARD IS AND HOW DEEP THE PLAYER STACK IS CURRENTLY

					#region Determine the ranking of the new card in the pool (THE LOWER THE BETTER AS NO OTHER PLAYER WILL COMPETE FOR IT)
					int RankOfNewCard = 0;
					Card[] PoolCards = GMInstance.Table.Pool.ToArray();
					PoolCards = Utility.SortHandByCards(PoolCards,false);

					for(int CardIndex = 0; CardIndex < PoolCards.Length; CardIndex++)
					{
						if(Utility.IsTwoCardsIdentical(PoolCards[CardIndex],CardBringingHighestValue))
						{
							RankOfNewCard = CardIndex + 1;
							break;
						}
					}
					#endregion;

					StackSizing PlayerStackSize = Evaluator.DeterminePlayerStackSize(PlayerInstance);

					int ImprovementInHandType = (int) HighestHandType - (int) HandTypeWithPocket;

					DesirabilityToBidNewCard -= (5.0f * (float) RankOfNewCard);

					if(PlayerStackSize == StackSizing.Short)
						DesirabilityToBidNewCard -= 20.0f;

					if(ImprovementInHandType < 2)
						DesirabilityToBidNewCard -= 10.0f;

					if(DesirabilityToBidNewCard >= 50.0f)
					{
						PlayerInstance.SelectedCardForAuction = CardBringingHighestValue;
						return AuctionDecision.NULL;
					}

					return AuctionDecision.Forfeit;
				}
				//ELSE, FORFEIT
				else
				{
					return AuctionDecision.Forfeit;
				}

				return AuctionDecision.NULL;
			} 

			return AuctionDecision.NULL;
		}
		else if(CurrentPhase == Phase.Four)
		{
			if(PriceLimitForBidding[0] == 0.0f && PriceLimitForBidding[1] == 0.0f)
			{
				float StackToPot = GMInstance.Table.GetEffectiveStackSize() / GMInstance.Table.Pot;
				if(StackToPot >= 3.5f && StackToPot < 5.5f)
				{
					PriceLimitForBidding[0] = 0.75f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 0.75f * GMInstance.Table.Pot;
				}
				else if(StackToPot >= 5.5f && StackToPot < 7.0f)
				{
					PriceLimitForBidding[0] = 1.25f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 0.75f * GMInstance.Table.Pot;
				}
				else if(StackToPot >= 7.0f && StackToPot < 9.0f)
				{
					PriceLimitForBidding[0] = 1.125f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 1.125f * GMInstance.Table.Pot;
				}
				else if(StackToPot >= 9.0f && StackToPot < 13.0f)
				{
					PriceLimitForBidding[0] = 1.375f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 1.125f * GMInstance.Table.Pot;
				}
				else if(StackToPot >= 13.0f)
				{
					PriceLimitForBidding[0] = 1.5f * GMInstance.Table.Pot;
					PriceLimitForBidding[1] = 1.5f * GMInstance.Table.Pot;
				}
			}

			List<Card> PoolRanking = new List<Card>(GMInstance.Table.Pool);

			for(int FirstIndex = 0; FirstIndex < PoolRanking.Count - 1; FirstIndex++)
			{
				for(int SecondIndex = FirstIndex + 1; SecondIndex < PoolRanking.Count; SecondIndex++)
				{
					if(PoolRanking[FirstIndex].Value <  PoolRanking[SecondIndex].Value)
					{
						Card TempCard = PoolRanking[FirstIndex];
						PoolRanking[FirstIndex] = PoolRanking[SecondIndex];
						PoolRanking[SecondIndex] = TempCard;
					}
					else if(PoolRanking[FirstIndex].Value == PoolRanking[SecondIndex].Value)
					{
						if(PoolRanking[FirstIndex].Suit < PoolRanking[SecondIndex].Suit)
						{
							Card TempCard = PoolRanking[FirstIndex];
							PoolRanking[FirstIndex] = PoolRanking[SecondIndex];
							PoolRanking[SecondIndex] = TempCard;
						}
					}
				} 
			}

			int BiddingCardRank = 0;

			for(int PoolIndex = 0; PoolIndex < PoolRanking.Count; PoolIndex++)
			{
				if(CardToBeBid.Suit == PoolRanking[PoolIndex].Suit && CardToBeBid.Value == PoolRanking[PoolIndex].Value)
				{
					BiddingCardRank = PoolIndex + 1;
					break;
				}
			}

			float ClampedAggressiveness = 0.0f;

			if(EnemyInstance.Aggressiveness == 5)
			{
				ClampedAggressiveness = 0.0f;
			}
			else if(EnemyInstance.Aggressiveness < 5)
			{
				ClampedAggressiveness = (5.0f - (float) EnemyInstance.Aggressiveness) * -1.0f;
			}
			else if(EnemyInstance.Aggressiveness > 5)
			{
				ClampedAggressiveness = ((float) EnemyInstance.Aggressiveness - 5.0f) * 1.0f;
			}

			float BiddingAmount = BiddingCardRank * 0.25f + ClampedAggressiveness * 0.05f;
			BiddingAmount = Mathf.Clamp(BiddingAmount,0.0f, PriceLimitForBidding[0]);

			PlayerInstance.RaiseValue = (int) BiddingAmount;
			return AuctionDecision.Bid;
		}
		else if(CurrentPhase == Phase.Five)
		{
			Hands PossibleHighestHandType = Evaluator.EvaluatePlayerHandWithPocket(PlayerInstance);

			if(PossibleHighestHandType >= Hands.ThreeOfAKind)
			{
				if(Random.Range(0,2) == 1)
				{
					PlayerInstance.Stack -= GMInstance.Auction.CostOfRandomCard;
					PlayerInstance.OnTheBet += GMInstance.Auction.CostOfRandomCard;
				
					Card CardPurchased = GMInstance.Deck.DrawSingle();
					PlayerInstance.AddCardToPocket(CardPurchased);
					PlayerInstance.CardsAuctioned.Add(CardPurchased);
				}

				PlayerInstance.FinishPurchasing = true;

				//AS AUCTION END HERE AND SORTING/SHOWDOWN WILL IMMEDIATELY HAPPEN, THIS PLAY WILL BE COMPLETED 
				EndPlay();
			}
			else
			{
				//GO THROUGH ALL THE POSSIBLE HANDS THAT PLAYER CAN FORM FROM THIS SITUATION
				LinkedList<Card[]> PossibleHands = Evaluator.DeterminePossibleHand(PlayerInstance,true,true);
				LinkedListNode<Card[]> TraverseHand = PossibleHands.First;
				List<Card[]> SuperiorHandWithOneCardNeeded = new List<Card[]>();

				Hands CurrentHandType = Evaluator.EvaluateHand(PlayerInstance.Hand);

				//FIND IF THERE IS ANY POSSIBLE SUPERIOR HAND THAT ONLY NEED ONE MORE CARD TO FORM
				for(int HandIndex = 0; HandIndex < PossibleHands.Count; HandIndex++)
				{
					if(Evaluator.EvaluateHand(TraverseHand.Value) > CurrentHandType)
					{
						int MissingCardsCount = 0;

						for(int CardIndex = 0; CardIndex < TraverseHand.Value.Length; CardIndex++)
						{
							bool CardIsUnique = true;

							for(int PocketIndex = 0; PocketIndex < PlayerInstance.Pocket.Count; PocketIndex++)
							{
								if(Utility.IsTwoCardsIdentical(TraverseHand.Value[CardIndex],PlayerInstance.Pocket[PocketIndex]))
									CardIsUnique = false;
							}

							for(int HandCardIndex = 0; HandCardIndex < PlayerInstance.Hand.Count; HandCardIndex++)
							{
								if(Utility.IsTwoCardsIdentical(TraverseHand.Value[CardIndex],PlayerInstance.Hand[HandCardIndex]))
									CardIsUnique = false;
							}

							if(CardIsUnique == false)
								break;
							
							else
								MissingCardsCount++;
						}

						if(MissingCardsCount == 1)
							SuperiorHandWithOneCardNeeded.Add(TraverseHand.Value);
					}

					TraverseHand = TraverseHand.Next;
				}

				Utility.RemoveDuplicationFromHandList(SuperiorHandWithOneCardNeeded);

				if(SuperiorHandWithOneCardNeeded.Count == 0)
					return AuctionDecision.Forfeit;

				//CALCULATE THE PROBABILITY OF GETTING THE ONE CARDS NEEDED TO FORM ANY ONE OF THOSE HAND
				List<Card> CardsToFormHands = new List<Card>();

				for(int HandIndex = 0; HandIndex < SuperiorHandWithOneCardNeeded.Count; HandIndex++)
				{
					Card UniqueCard = new Card(Suits.Clubs,Values.Two);

					for(int CardIndex = 0; CardIndex < SuperiorHandWithOneCardNeeded[HandIndex].Length; CardIndex++)
					{
						bool CardFound = true;

						for(int PocketIndex = 0; PocketIndex < PlayerInstance.Pocket.Count; PocketIndex++)
						{
							if(Utility.IsTwoCardsIdentical(SuperiorHandWithOneCardNeeded[HandIndex][CardIndex],PlayerInstance.Pocket[PocketIndex]))
								CardFound = false;
						}

						for(int HandCardIndex = 0; HandCardIndex < PlayerInstance.Hand.Count; HandCardIndex++)
						{
							if(Utility.IsTwoCardsIdentical(SuperiorHandWithOneCardNeeded[HandIndex][CardIndex],PlayerInstance.Hand[HandCardIndex]))
								CardFound = false;
						}
	
						if(CardFound)
						{
							CardsToFormHands.Add(SuperiorHandWithOneCardNeeded[HandIndex][CardIndex]);
							break;	
						}
					}
				}

				float ProbabilityOfDrawingSuperiorHand = (float) CardsToFormHands.Count / (float) GMInstance.Deck.Cards.Count; 

				//IF THAT PROBABILITY IS ABOVE A CERTRAIN PERCENTAGE, PURCHASE A CARD
				float PercentageRequirement = 40.0f;

				if(EnemyInstance.Aggressiveness > 5)
					PercentageRequirement -= ((float) EnemyInstance.Aggressiveness - 5.0f)/5.0f * 20.0f;

				else if(EnemyInstance.Aggressiveness < 5)
					PercentageRequirement += (5.0f - (float) EnemyInstance.Aggressiveness)/5.0f * 20.0f;

				if(ProbabilityOfDrawingSuperiorHand * 100.0f > PercentageRequirement)
				{
					PlayerInstance.Stack -= GMInstance.Auction.CostOfRandomCard;
					PlayerInstance.OnTheBet += GMInstance.Auction.CostOfRandomCard;
				
					Card CardPurchased = GMInstance.Deck.DrawSingle();
					PlayerInstance.AddCardToPocket(CardPurchased);
					PlayerInstance.CardsAuctioned.Add(CardPurchased);
				}
			}

			PlayerInstance.FinishPurchasing = true;

			EndPlay();

			return AuctionDecision.NULL;
		}

		return AuctionDecision.NULL;
	}

	public override void EndPlay ()
	{
		PlayInUse = false;
		CurrentPhase = Phase.NULL;
		TurnAfterMineSet = 0;

		PriceLimitForBidding = null;

		PairFirstCard = null;
		PairSecondCard = null;

		PairRepresentativeValue = Values.Two;
	}
}
