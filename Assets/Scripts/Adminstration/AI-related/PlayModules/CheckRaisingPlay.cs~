using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckRaisingPlay : Play 
{
	private enum Purpose {Value,Bluff,NULL};

	private Purpose PurposeOfPlay;

	public CheckRaisingPlay(Enemy _Enemy) : base(_Enemy)
	{
		EnemyInstance = _Enemy;
		PurposeOfPlay = Purpose.NULL;

		CorrespondingAction = PlayerAction.CheckRaise;
	}

	public override float ApplicabilityOfPlay ()
	{
		float Applicability = 0.0f;

		//CHECK IF PLAYER CAN EVEN CHECK IN THIS TURN. IF THE PLAYER CANNNOT, RETURN 0 APPLICABILITY FOR THIS PLAY
		if(PlayerInstance.OnTheBet != GMInstance.Table.GetHighestBet())
			return 0.0f;

		//DETERMINE IF THIS PLAY WILL BE USED FOR VALUE OR AS A BLUFF BY CHECK THE HAND GRADE FOR THE PLAYER
		HandGrade PlayerHandGrade = Evaluator.DetermineHandGrade(PlayerInstance);

		//IF THE HAND GRADE IS GOOD, USE CHECK RAISE AS VALUE. 
		if(PlayerHandGrade == HandGrade.Good)
		{
			//CHECK WHETHER THE PLAYER IS FIRST TO ACT/ON THE BUTTON, INCREASE THE APPLICABILITY SIGNIFICANTLY
			if(GMInstance.Betting.DeterminePlayerPosition(PlayerInstance) == TablePosition.UnderTheGun)
				Applicability += 20.0f;

			//IF THIS POT IS A MULTI-WAY POT (MORE THAN TWO PLAYER CURRENTLY IN THE POT) INCREASE THE APPLICABILITY OF THIS PLAY
			List<Player> PlayersInPot = new List<Player>();

			for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
			{
				if(!GMInstance.Players[PlayerIndex].Busted && !GMInstance.Players[PlayerIndex].Fold)
					PlayersInPot.Add(GMInstance.Players[PlayerIndex]);
			}

			if(PlayersInPot.Count > 2)
				Applicability += 15.0f;
				
			//CHECK WHETHER THE ENEMY IS AGGRESSIVE/LOOSE ENOUGH TO RAISE SO THE PLAYER CAN RE-RAISE
			for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
			{
				if(GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index)
				{
					int EvaluatedAggressiveness = EnemyInstance.Memory.OpponentsAggressiveness[PlayerIndex];
					int EvaluatedTightness = EnemyInstance.Memory.OpponentTightness[PlayerIndex];
					int EvaluatedSkillLevel = EnemyInstance.Memory.OpponentsSkillLevel[PlayerIndex];

					if(EvaluatedAggressiveness > 5)
						Applicability += (float) EvaluatedAggressiveness - 5.0f;

					if(EvaluatedTightness < 5)
						Applicability += 5.0f - EvaluatedTightness;

					if(EvaluatedSkillLevel > 2.5f)
						Applicability -= (float) EvaluatedSkillLevel - 2.5f;
				}
			}

			//CHECK OTHER PLAYERS' TENDENCY TO RAISE OR BET SO THAT PLAYER CAN RERAISE
			int TotalPlayerActions = 0;
			int TotalPlayerRaiseBet = 0;

			for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
			{
				for(int LogEntry = 0; LogEntry < GMInstance.CommonMemoryLog.MemoryLog.Count; LogEntry++)
				{
					if(GMInstance.CommonMemoryLog.MemoryLog[LogEntry].PlayerIndex == GMInstance.Players[PlayerIndex].Index
					&& GMInstance.CommonMemoryLog.MemoryLog[LogEntry].T_Phase == TurnPhase.Betting)
					{
						TotalPlayerActions++;

						if(GMInstance.CommonMemoryLog.MemoryLog[LogEntry].P_Action == PlayerAction.Bet ||
						   GMInstance.CommonMemoryLog.MemoryLog[LogEntry].P_Action == PlayerAction.Raise)
						{
							TotalPlayerRaiseBet++;
						}
					}
				}
			}

			float RaiseBetPercentage = (float) TotalPlayerRaiseBet / (float) TotalPlayerActions;

			if(RaiseBetPercentage > 50.0f)
				Applicability += (RaiseBetPercentage - 50.0f)/50.0f * 25.0f;
			else
				Applicability -= (50.0f - RaiseBetPercentage)/50.0f * 25.0f;

			return Applicability;
		}
		//ELSE, USE CHECK RAISE AS BLUFF
		else if(PlayerHandGrade == HandGrade.Average || PlayerHandGrade == HandGrade.Bad)
		{
			//BASED ON HOW EARLY THE POSITION OF THE PLAYER IN THIS BET, INCREASE THE APPLICABILITY OF THIS PLAY SIGNIFICANTLY
			TablePosition PositionOfPlayer = GMInstance.Betting.DeterminePlayerPosition(PlayerInstance);
			if(PositionOfPlayer == TablePosition.UnderTheGun)
				Applicability += 20.0f;
			
			//CALCULATE THE ENEMY'S PROBABILITY TO FOLD IF THE PLAYER MAKE A RAISE, INCREASE/DECREASE THE APPLICABILITY OF THIS PLAY 
			int TotalActions = 0;
			int TotalFoldAfterRaise = 0;

			for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
			{
				if(GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index)
				{
					for(int LogIndex = 0; LogIndex < GMInstance.CommonMemoryLog.MemoryLog.Count; LogIndex++)
					{
						if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].PlayerIndex != GMInstance.Players[PlayerIndex].Index
						&& GMInstance.CommonMemoryLog.MemoryLog[LogIndex].T_Phase == TurnPhase.Betting
						&& GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Raise)
						{
							TotalActions++;

							for(int NextLogIndex = LogIndex + 1; NextLogIndex < LogIndex + 4; NextLogIndex++)
							{
								if(GMInstance.CommonMemoryLog.MemoryLog[LogIndex].PlayerIndex == GMInstance.Players[PlayerIndex].Index
								&& GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Fold)
								{
									TotalFoldAfterRaise++;
								}
							}
						}
					}
				}
			}

			float ProbabilityOfFoldingAfterRaise = (float) TotalFoldAfterRaise / (float) TotalActions;

			Applicability += ProbabilityOfFoldingAfterRaise * 20.0f;

			//CALCULATE THE PROFITABILITY OF CHECK RAISING BY CALCULATE THE EV USING THE FOLD EQUITY AND POT EQUITY
			//IF THE PROFITABILITY IS POSITIVE, INCREASE THE APPLICABILITY OF THIS PLAY SIGNIFICANTLY

			float FoldEquity = ProbabilityOfFoldingAfterRaise * (float) GMInstance.Table.Pot;

			float PlayerAverageEquity = Evaluator.CalculatePlayerAverageEquity(PlayerInstance);

			float PotEquityLose = (1.0f - PlayerAverageEquity) * -(PlayerInstance.OnTheBet);
			float PotEquityWin = PlayerAverageEquity * (GMInstance.Table.Pot - PlayerInstance.OnTheBet);

			float CheckRaiseEV = FoldEquity + (1.0f - ProbabilityOfFoldingAfterRaise) * (PotEquityWin + PotEquityLose);

			if(CheckRaiseEV > 0.0f)
				Applicability += 30.0f;

			return Applicability;
		}
		
		//FINALIZED THE PURPOSE OF THIS PLAY
		//RETURN THE CALCULATED APPLICABILITY
		return Applicability;
	}

	public override void StartPlay ()
	{
		PlayInUse = true;
		CurrentPhase = Phase.One;
	}

	public override BettingDecision ExecuteBetPlay ()
	{
		if(CurrentPhase == Phase.One)
		{
			CurrentPhase = Phase.Two;
			return BettingDecision.Check;
		}
		else if(CurrentPhase == Phase.Two)
		{
			//CHECK IF THERE IS RAISE MADE SINCE PLAYER HAD CHECKED PREVIOUS TURN. IF THERE IS, THE PLAYER WILL RAISE THE POT.
			//NOTE.RE-RAISE STRONGLY AROUND TWICE THE RAISE MADE BY OTHER PLAYERS
			bool RaiseHadBeenMade = false;

			for(int FirstLogIndex = GMInstance.CommonMemoryLog.MemoryLog.Count - 1; FirstLogIndex > 0; FirstLogIndex--)
			{
				if(GMInstance.CommonMemoryLog.MemoryLog[FirstLogIndex].RoundNum == GMInstance.CurrentRoundIndex &&
				   GMInstance.CommonMemoryLog.MemoryLog[FirstLogIndex].PlayerIndex == PlayerInstance.Index &&
				   GMInstance.CommonMemoryLog.MemoryLog[FirstLogIndex].P_Action == PlayerAction.Check)
				{
					for(int SecondLogIndex = FirstLogIndex + 1; SecondLogIndex < GMInstance.CommonMemoryLog.MemoryLog.Count; SecondLogIndex++)
					{
						if(GMInstance.CommonMemoryLog.MemoryLog[SecondLogIndex].RoundNum == GMInstance.CurrentRoundIndex &&
						   GMInstance.CommonMemoryLog.MemoryLog[SecondLogIndex].PlayerIndex != PlayerInstance.Index &&
						   GMInstance.CommonMemoryLog.MemoryLog[SecondLogIndex].P_Action == PlayerAction.Raise)
						{
							RaiseHadBeenMade = true;
							break;
						}
					}

					if(RaiseHadBeenMade)
					break;
				}
			}

			if(RaiseHadBeenMade)
			{
				PlayerInstance.RaiseValue = 2 * (int) GMInstance.Betting.LatestRaiseAmount;
				return BettingDecision.Raise;
			}

			//IF ALL PLAYERS CHECK, THE BET WILL BE ENDED
			//THIS PLAY WILL THEN BE ENDED

			EndPlay();
			return BettingDecision.NULL;
		}

		return BettingDecision.NULL;
	}

	public override AuctionDecision ExecuteAuctionPlay ()
	{
		return AuctionDecision.NULL;
	}

	public override void EndPlay ()
	{
		PlayInUse = false;
		PurposeOfPlay = Purpose.NULL;
	}
}
