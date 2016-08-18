using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TotalBluffPlay : Play 
{
	private List<Card[]> SetNeededForSuperiorHand;
	private float[] PriceLimitForBidding;
	private Card NextCardToBidFor;

	public TotalBluffPlay(Enemy _Enemy) : base(_Enemy)
	{
		EnemyInstance = _Enemy;
		PriceLimitForBidding = new float[2];
		PriceLimitForBidding[0] = 0.0f;
		PriceLimitForBidding[1] = 0.0f;

		CorrespondingAction = PlayerAction.Bluffing;
	}

	public override float ApplicabilityOfPlay ()
	{
		//CHECK HOW BAD THE PLAY HAND IS, IF HAND IS NOT BAD, A TOTAL BLUFF WILL NOT BE APPLICABLE. THEREFORE, 0 APPLICABILITY
		if(Evaluator.DetermineHandGrade(PlayerInstance) != HandGrade.Bad)
			return 0.0f;

		//CHECK THE RISKINESS OF CURRENT POT BEFORE BLUFFING
		//If the pot is too risky to continue, fold. Else, continue.
		//NOTE. RISKINESS OF THE POT WILL BE RATED USING FUZZY LOGIC
		//NOTE. AGGRESSIVENESS WILL ALSO CONTRIBUTE WITH HOW "RISKY" A POT NEED TO BE TO DETER THIS BLUFF
		float AmountOfValidPlayers = Utility.HowManyValidPlayersLeft(GMInstance);
		float PlayerStackInBB = EnemyInstance.PlayerInstance.Stack / GMInstance.Betting.BigBind;
		float PlayerEarningsRatio = ((AmountOfValidPlayers * GMInstance.Table.GetEffectiveStackSize()) - GMInstance.Table.Pot)/
									(AmountOfValidPlayers * GMInstance.Table.GetEffectiveStackSize());  

		EnemyInstance.BluffingModule.Fuzzify("ValidPlayersCount",Utility.HowManyValidPlayersLeft(GMInstance));
		EnemyInstance.BluffingModule.Fuzzify("StackSize",PlayerStackInBB);
		EnemyInstance.BluffingModule.Fuzzify("Earnings",PlayerEarningsRatio);

		float DesirabilityToRiskBluffing = EnemyInstance.BluffingModule.Defuzzify("Desirability");

		float AggressivenessModifier = 0.0f;

		float OnTheButtonModifier = 0.0f;
		if(GMInstance.Betting.DeterminePlayerPosition(EnemyInstance.PlayerInstance) == TablePosition.OnTheButton)
			OnTheButtonModifier += 10.0f;

		if(EnemyInstance.Aggressiveness == 5)
			AggressivenessModifier = 0.0f;

		else if(EnemyInstance.Aggressiveness > 5 && EnemyInstance.Aggressiveness <= 10)
			AggressivenessModifier = ( ((float) EnemyInstance.Aggressiveness - 5.0f) / 5.0f) * 25.0f;

		else if(EnemyInstance.Aggressiveness < 5 && EnemyInstance.Aggressiveness >= 0)
			AggressivenessModifier = ( (5.0f - (float) EnemyInstance.Aggressiveness) / 5.0f) * -25.0f;

		return (DesirabilityToRiskBluffing + AggressivenessModifier + OnTheButtonModifier);
//		if((DesirabilityToRiskBluffing + AggressivenessModifier + OnTheButtonModifier) < 50.0f)
//		{
//			CurrentPhase = Phase.NULL;
//			return (DesirabilityToRiskBluffing + AggressivenessModifier + OnTheButtonModifier);
//		}
//
//		if(IsThereRaiseBeforePlayer)
//			CurrentPhase = Phase.Two;
//
//		return DesirabilityToRiskBluffing + AggressivenessModifier + OnTheButtonModifier;
	}

	public override void StartPlay ()
	{
		PlayInUse = true;
		CurrentPhase = Phase.One;

		//Check if any player before this player had raised
		bool IsThereRaiseBeforePlayer = false;

		for(int LogIndex = CommonMemoryInstance.MemoryLog.Count - 1; LogIndex > 0; LogIndex--)
		{
			if(CommonMemoryInstance.MemoryLog[LogIndex].RoundNum == GMInstance.CurrentRoundIndex 
			   && CommonMemoryInstance.MemoryLog[LogIndex].PlayerIndex != EnemyInstance.PlayerInstance.Index 
			   && CommonMemoryInstance.MemoryLog[LogIndex].P_Action == PlayerAction.Raise)
			{
				IsThereRaiseBeforePlayer = true;
				break;
			}
		}

		if(IsThereRaiseBeforePlayer)
			CurrentPhase = Phase.Two;
	}

	public override BettingDecision ExecuteBetPlay ()
	{
		if(CurrentPhase == Phase.NULL)
			return BettingDecision.NULL;

		if(CurrentPhase == Phase.One)
		{
			//MAKE THE FIRST RAISE OF THE POT
			//Since there is no player before this player raised, this player shall raise the pot to deter players off
			TablePosition PlayerPosition = GMInstance.Betting.DeterminePlayerPosition(EnemyInstance.PlayerInstance);

			if(PlayerPosition == TablePosition.UnderTheGun)
				EnemyInstance.PlayerInstance.RaiseValue = 3 * GMInstance.Betting.BigBind;
						
			else if(PlayerPosition == TablePosition.OnTheButton)
				EnemyInstance.PlayerInstance.RaiseValue = (int) (2.5f * GMInstance.Betting.BigBind);
						
			else if(PlayerPosition == TablePosition.SmallBind)
				EnemyInstance.PlayerInstance.RaiseValue = (int) (Random.Range(3.5f,4.0f) *  GMInstance.Betting.BigBind);

			else
				EnemyInstance.PlayerInstance.RaiseValue = 4 * GMInstance.Betting.BigBind;
						
			EnemyInstance.PlayerInstance.RaiseValue = Mathf.Clamp(EnemyInstance.PlayerInstance.RaiseValue,0,EnemyInstance.PlayerInstance.Stack);

			Debug.Log("Enemy " + (EnemyInstance.PlayerInstance.Index - 1) + " will be bluffing and raising $" + EnemyInstance.PlayerInstance.RaiseValue);

			CurrentPhase = Phase.Two;

			return BettingDecision.Raise;
		}
		else if(CurrentPhase == Phase.Two)
		{
			//FURTHER RE-RAISE TO DETER THE REMAINING PLAYERS
			//If player had already raised once or more in attempt, check whether a further re-raise (3 bet or 4 bet) will then deter other players
			//NOTE. GO THROUGH THE RANGE FOR THE OTHER PLAYERS, CHECK FOR THE PROBABILITY OF THEM HAVING THE TOP TOP TIER HAND. THE HIGHER THE PROBABILITY, THE LOWER THE CHANCE THAT PLAYER WILL RE-RAISE
			//NOTE. IF THERE IS MORE THAN ONE OTHER PLAYERS, THESE PROBABILITIES WILL BE ADDED TO GET AN AVERAGE PROBABILITY

			int AmountOfOtherPlayers = 0;
			float TotalProbability = 0.0f;
			for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
			{
				if(GMInstance.Players[PlayerIndex].Index != EnemyInstance.PlayerInstance.Index && !GMInstance.Players[PlayerIndex].Busted && !GMInstance.Players[PlayerIndex].Fold)
				{	
					AmountOfOtherPlayers++;
					TotalProbability += Evaluator.FindPlayerProabilityofSpecificHandGrade(EnemyInstance.PlayerInstance,GMInstance.Players[PlayerIndex],HandGrade.Good);
				}
			}

			float AverageTopHandProbability = TotalProbability / AmountOfOtherPlayers;
			float ChanceOfReraise = 100.0f - AverageTopHandProbability;

			//NOTE. FOR EVERY OTHER PLAYER THAT IS BEEN COVERED BY THIS PLAYER'S STACK, INCREASE THE CHANCE OF RE-RAISE AS WELL
			int OverlappingDone = GMInstance.Table.GetAmountOfOverlappingPlayerDone(EnemyInstance.PlayerInstance);
			ChanceOfReraise += 5.0f * OverlappingDone;

			//NOTE. IF THE PLAYER IS ON THE BUTTON, INCREASE THE CHANCE OF RE-RAISE AS WELL
			if(GMInstance.Betting.DeterminePlayerPosition(EnemyInstance.PlayerInstance) == TablePosition.OnTheButton)
				ChanceOfReraise += 10.0f;

			//NOTE. FOR EVERY SUB-SEQUENT RE-RAISE, REDUCE THE CHANCE OF RE-RAISE SIGNIFICANTLY 
			if(CommonMemoryInstance.RaiseInBet >= 1)
				ChanceOfReraise -= 10.0f * CommonMemoryInstance.RaiseInBet;

			//if the chance of re-raise is good enough, re-raise once more
			if(ChanceOfReraise > 50.0f)
				return BettingDecision.Raise;

			//Else, player will fold their hand to quickly back off from a high stake pot
			return BettingDecision.Fold;
		}

		return BettingDecision.NULL;
	}

	public override AuctionDecision ExecuteAuctionPlay ()
	{
		//MAIN IDEA OF AUCTION PHASE - FORCE PLAYER TO FOLD BY BIDDING CARDS THAT COMBINE WITH THE PLAYER'S REVEALED CARD WILL ALMOST BUT FORM A MONSTER HAND
		//NEED TO GIVE THE ILLUSION OF A MONSTER HAND
		//NOTE. MONSTER HAND DEPENDS BASED ON ALL THE REMAINING PLAYER'S HAND RANGE, THE TOP FEW HANDS WILL BE MONSTER HAND

		if(SetNeededForSuperiorHand == null)
		{
			Card[] PlayerHand = PlayerInstance.Hand.ToArray();
			List<Card[]> SuperiorHands = new List<Card[]>();

			#region Go through all the possible combination of hands against the enemy, any hand combination that is superior to the player's hand will be collected into a list
			for(int EnemyIndex = 0; EnemyIndex < GMInstance.Players.Length; EnemyIndex++)
			{
				if(GMInstance.Players[EnemyIndex].Index != PlayerInstance.Index)
				{
					List<Card[]> RangeAgainstEnemy = Evaluator.DetermineRangeOfOpponent(PlayerInstance,GMInstance.Players[EnemyIndex],false);

					for(int HandIndex = 0; HandIndex < RangeAgainstEnemy.Count; HandIndex++)
					{
						Card[] PossibleHand = RangeAgainstEnemy[HandIndex];

						if(Evaluator.IsFirstHandSuperiorOverSecond(PossibleHand,PlayerHand))
							SuperiorHands.Add(PossibleHand);
					}
				}
			}
			#endregion

			List<Card[]> TopSuperiorHands = new List<Card[]>();

			#region Sort all the superior hand combination from the best to the worst and pick out the top 10 hand combination, store them in another list
			for(int FirstIndex = 0; FirstIndex < SuperiorHands.Count; FirstIndex++)
			{
				for(int SecondIndex = FirstIndex + 1; SecondIndex < SuperiorHands.Count; SecondIndex++)
				{
					if(!Evaluator.IsFirstHandSuperiorOverSecond(SuperiorHands[FirstIndex],SuperiorHands[SecondIndex]))
					{
						Card[] TempHand = SuperiorHands[FirstIndex];
						SuperiorHands[FirstIndex] = SuperiorHands[SecondIndex];
						SuperiorHands[SecondIndex] = TempHand;
					}
				}
			}


			for(int TopIndex = 0; TopIndex < 10; TopIndex++)
			{
				TopSuperiorHands.Add(SuperiorHands[TopIndex]);
			}
			#endregion

			#region Pick out the sets of two card that is needed to attain the top 10 superior hand, store them in a list
			for(int HandIndex = 0; HandIndex < TopSuperiorHands.Count; HandIndex++)
			{
				Card[] PossibleSet = new Card[2];
				int CardCount = 0;
				
				for(int CardIndex = 0; CardIndex < TopSuperiorHands[HandIndex].Length; CardIndex++)
				{
					bool IsItASetCard = true;

					for(int RevealedIndex = 0; RevealedIndex < PlayerInstance.RevealedCards.Length; RevealedIndex++)
					{
						if(Utility.IsTwoCardsIdentical(TopSuperiorHands[HandIndex][CardIndex],PlayerInstance.RevealedCards[RevealedIndex]))
							IsItASetCard = false;
					}

					if(IsItASetCard)
					{
						PossibleSet[CardCount] = TopSuperiorHands[HandIndex][CardIndex];
						CardCount++;
					}

					if(CardCount >= 2)
						break;
				}

				SetNeededForSuperiorHand.Add(PossibleSet);
			}
			#endregion

			CurrentPhase = Phase.One;
		}

		//PHASE 1 - FIND THE 1ST SUITABLE CARD TO MAKE THE PLAY
		//DETERMINE IF THERE'S EVEN A CARD THAT CAN HELP TO FORM AND GIVE THE ILLUSION, BID FOR IT
		//ELSE, FORFEIT
		if(CurrentPhase == Phase.One)
		{
			for(int PoolIndex = 0; PoolIndex < GMInstance.Table.Pool.Count; PoolIndex++)
			{
				Card CurrentPoolCard = GMInstance.Table.Pool[PoolIndex];

				for(int SetIndex = 0; SetIndex < SetNeededForSuperiorHand.Count; SetIndex++)
				{
					if(CurrentPoolCard.Suit == SetNeededForSuperiorHand[SetIndex][0].Suit && CurrentPoolCard.Value == SetNeededForSuperiorHand[SetIndex][0].Value)
					{
						PlayerInstance.SelectedCardForAuction = CurrentPoolCard;
						NextCardToBidFor = SetNeededForSuperiorHand[SetIndex][1];
					}	
		            else if(CurrentPoolCard.Suit == SetNeededForSuperiorHand[SetIndex][1].Suit && CurrentPoolCard.Value == SetNeededForSuperiorHand[SetIndex][1].Value)
		            {
						PlayerInstance.SelectedCardForAuction = CurrentPoolCard;
						NextCardToBidFor = SetNeededForSuperiorHand[SetIndex][0];
					}
				}
			}

			if(NextCardToBidFor == null)
			{	
				CurrentPhase = Phase.Three;
				return AuctionDecision.Forfeit;
			}

			CurrentPhase = Phase.Two;
			return AuctionDecision.NULL;
		}

		//PHASE 2 - BIDDING FOR THE CARD
		//AGGRESSIVELY BID FOR THE CARD TO DETER PLAYERS AND FORCING THEM TO FOLD THEIR HAND, ALL IN FOR THAT CARD IF NEED BE
		else if(CurrentPhase == Phase.Two)
		{
			//IF THERE ISNT PRICE LIMIT AT WHICH THE PLAYER WILL BET FOR THE TWO CARDS, DETERMINE PRICE LIMIT FOR TWO CARDS
			//THE PRICE LIMIT WILL BE DETERMINED BASED ON STACK TO POT RATIO THAT CAN BE CALCULATED BY DIVIDING THE EFFECTIVE STACK SIZE AT THAT POINT OVER THE SIZE OF THE POT
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

			//CALCULATE A RATIO AGAINST THE PRICE LIMIT TO DETERMINE THE ACTUAL AMOUNT OF MONEY TO BID THE FIRST CARD
			//NOTE. THE RANKING OF THE CARD IN THE POOL (VALUE AND SUIT BASED), AGGRESSIVENESS will increase the amount of bid for the first card
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
				if(NextCardToBidFor.Suit == PoolRanking[PoolIndex].Suit && NextCardToBidFor.Value == PoolRanking[PoolIndex].Value)
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

		//PHASE 3 - FIND THE 2ND SUITABLE CARD IF NEEDED TO MAKE THAT PLAY
		// DETERMINE IF PLAYER NEED A SECOND CARD TO MAKE THAT ILLUSION. iF YES, AGGRESSIVELY BID FOR THAT CARD. ELSE, FORFEIT THIS AUCTION PHASE
		else if(CurrentPhase == Phase.Three)
		{
			for(int PoolIndex = 0; PoolIndex < GMInstance.Table.Pool.Count; PoolIndex++)
			{
				if(GMInstance.Table.Pool[PoolIndex].Suit == NextCardToBidFor.Suit && GMInstance.Table.Pool[PoolIndex].Value == NextCardToBidFor.Value)
				{
					PlayerInstance.SelectedCardForAuction = GMInstance.Table.Pool[PoolIndex];
					break;
				}
			}

			if(PlayerInstance.SelectedCardForAuction == null)
			{	
				CurrentPhase = Phase.Five;
				return AuctionDecision.Forfeit;
			}

			CurrentPhase = Phase.Four;
			return AuctionDecision.NULL;
		}

		//PHASE 4 - BIDDING FOR THE 2ND CARD
		//AGGRESSIVELY BID FOR THE CARD TO DETER PLAYERS AND FORCING THEM TO FOLD THEIR HAND, ALL IN FOR THAT CARD IF NEED BE
		else if(CurrentPhase == Phase.Four)
		{
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
				if(NextCardToBidFor.Suit == PoolRanking[PoolIndex].Suit && NextCardToBidFor.Value == PoolRanking[PoolIndex].Value)
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
			BiddingAmount = Mathf.Clamp(BiddingAmount,0.0f, PriceLimitForBidding[1]);

			PlayerInstance.RaiseValue = (int) BiddingAmount;

			return AuctionDecision.Bid;
		}

		//PHASE 5 - PURCHASING CARD
		//RANDOM CHOICE OF WHETHER PURCHASING OR NOT TO CONFUSE THE PLAYER
		else if(CurrentPhase == Phase.Five)
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

		return AuctionDecision.NULL;
	}

	public override void EndPlay ()
	{
		SetNeededForSuperiorHand.Clear();
		PriceLimitForBidding = new float[2];
		NextCardToBidFor = null;

		CurrentPhase = Phase.NULL;
		PlayInUse = false;
	}
}
