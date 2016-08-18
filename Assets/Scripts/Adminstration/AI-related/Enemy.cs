﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy
{
	private Player m_Player;
	private GameManager m_GMInstance;
	private EnemyMode m_Mode;
	private static DecisionReference Reference;
	
	private int m_Aggressiveness;
	private int m_Tightness;
	private int m_Skill;
	private float[] m_BettingRange;
	private float[] m_PriceLimitForBidding;
	
	private int ValueProjection = 0;
	
	private FuzzyModule m_RaisingModule;
	private FuzzyModule m_BluffingModule;
	private EnemyMemory m_Memory;
	
	private int m_HandImproveRate;
	private Card m_NextCardToBidFor;
	
	private List<Card[]> PossibleCombinations;
	
	public int Aggressiveness   {get{return m_Aggressiveness;} set{m_Aggressiveness = value;}}
	public int Tightness        {get{return m_Tightness;} set{m_Tightness = value;}}
	public int Skill            {get{return m_Skill;} set{m_Skill = value;}}
	public float[] BetRange     {get{return m_BettingRange;} set{m_BettingRange = value;}}
	public EnemyMemory Memory   {get{return m_Memory;} set{m_Memory = value;}}
	public Player PlayerInstance{get{return m_Player;} set{m_Player = value;}}
	public FuzzyModule BluffingModule{get{return m_BluffingModule;} set{m_BluffingModule = value;}}
	
	public Enemy(Player _player, EnemyMode _Mode, int _aggressiveness, int _tightness, int _skill)
	{
		m_Player         = _player;
		m_GMInstance     = m_Player.GManager;
		m_Mode           = _Mode;
		m_Aggressiveness = _aggressiveness;
		m_Tightness      = _tightness;
		m_Skill = _skill;
		
		m_BettingRange         = new float[6];
		m_PriceLimitForBidding = new float[2];
		
		PossibleCombinations   = new List<Card[]>();
		
		m_PriceLimitForBidding[0] = 0.0f;
		m_PriceLimitForBidding[1] = 0.0f;
		
		Memory = new EnemyMemory(this);
		
		if(Reference == null)
			Reference = new DecisionReference();
		
		InitializeFuzzyModules();
	}	
	
	private void InitializeFuzzyModules()
	{
		#region Initialize Raising module
		m_RaisingModule = new FuzzyModule();
		
		m_RaisingModule.CreateFuzzyVariable("Aggressiveness");
		m_RaisingModule.CreateFuzzyVariable("Tightness");
		m_RaisingModule.CreateFuzzyVariable("Position");
		m_RaisingModule.CreateFuzzyVariable("Hand");
		m_RaisingModule.CreateFuzzyVariable("Desirability");
		
		m_RaisingModule.Variables["Aggressiveness"].AddLeftShoulderSet("Defensive",2.0f,0.0f,4.0f);
		m_RaisingModule.Variables["Aggressiveness"].AddCentreTrapeziumSet("Neutral",4.0f,6.0f,2.0f,8.0f);
		m_RaisingModule.Variables["Aggressiveness"].AddRightShoulderSet("Aggressive",8.0f,6.0f,10.0f);
		
		m_RaisingModule.Variables["Tightness"].AddLeftShoulderSet("Loose",2.0f,0.0f,4.0f);
		m_RaisingModule.Variables["Tightness"].AddCentreTrapeziumSet("Neutral",4.0f,6.0f,2.0f,8.0f);
		m_RaisingModule.Variables["Tightness"].AddRightShoulderSet("Tight",8.0f,6.0f,10.0f);
		
		m_RaisingModule.Variables["Position"].AddLeftShoulderSet("Disadvantage",0.25f,0.0f,0.5f);
		m_RaisingModule.Variables["Position"].AddTriangularSet("Neutral",0.5f,0.25f,0.75f);
		m_RaisingModule.Variables["Position"].AddRightShoulderSet("Advantage",0.75f,0.5f,1.0f);
		
		m_RaisingModule.Variables["Hand"].AddLeftShoulderSet("Bad",0.25f,0.0f,0.5f);
		m_RaisingModule.Variables["Hand"].AddTriangularSet("Average",0.5f,0.25f,0.75f);
		m_RaisingModule.Variables["Hand"].AddRightShoulderSet("Good",0.75f,0.5f,1.0f);
		
		m_RaisingModule.Variables["Desirability"].AddLeftShoulderSet("Undesirable",25.0f,0.0f,50.0f);
		m_RaisingModule.Variables["Desirability"].AddTriangularSet("Desirable",50.0f,25.0f,75.0f);
		m_RaisingModule.Variables["Desirability"].AddRightShoulderSet("VeryDesirable",75.0f,50.0f,100.0f);
		
		for(int RaisingIndex = 0; RaisingIndex < DecisionReference.RaisingsReference.Length; RaisingIndex++)
		{
			FuzzySet AggressiveSet = m_RaisingModule.Variables["Aggressiveness"].Sets["Defensive"];
			FuzzySET Aggressiveness = new FuzzySET("Defensive",ref AggressiveSet);
			
			if(DecisionReference.RaisingsReference[RaisingIndex].Aggressiveness == AggressiveLevel.Defensive)
			{
				AggressiveSet = m_RaisingModule.Variables["Aggressiveness"].Sets["Defensive"];
				Aggressiveness = new FuzzySET("Defensive",ref AggressiveSet);
			}
			else if(DecisionReference.RaisingsReference[RaisingIndex].Aggressiveness == AggressiveLevel.Neutral)
			{
				AggressiveSet = m_RaisingModule.Variables["Aggressiveness"].Sets["Neutral"];
				Aggressiveness = new FuzzySET("Neutral",ref AggressiveSet);
			}
			else			
			{
				AggressiveSet = m_RaisingModule.Variables["Aggressiveness"].Sets["Aggressive"];
				Aggressiveness = new FuzzySET("Aggressive",ref AggressiveSet);
			}
			
			FuzzySet TightnessSet = m_RaisingModule.Variables["Tightness"].Sets["Loose"];
			FuzzySET Tightness = new FuzzySET("Loose",ref TightnessSet);
			
			if(DecisionReference.RaisingsReference[RaisingIndex].Tightness == TightnessLevel.Loose)
			{
				TightnessSet = m_RaisingModule.Variables["Tightness"].Sets["Loose"];
				Tightness = new FuzzySET("Loose",ref TightnessSet);
			}
			else if(DecisionReference.RaisingsReference[RaisingIndex].Tightness == TightnessLevel.Neutral)
			{
				TightnessSet = m_RaisingModule.Variables["Tightness"].Sets["Neutral"];
				Tightness = new FuzzySET("Neutral",ref TightnessSet);
			}
			else			
			{
				TightnessSet = m_RaisingModule.Variables["Tightness"].Sets["Tight"];
				Tightness = new FuzzySET("Tight",ref TightnessSet);
			}
			
			FuzzySet PositionSet = m_RaisingModule.Variables["Position"].Sets["Disadvantage"];
			FuzzySET Position = new FuzzySET("Disadvantage",ref PositionSet);
			
			if(DecisionReference.RaisingsReference[RaisingIndex].Position == PositionLevel.Disadvantage)
			{
				PositionSet = m_RaisingModule.Variables["Position"].Sets["Disadvantage"];
				Position = new FuzzySET("Disadvantage",ref PositionSet);
			}
			else if(DecisionReference.RaisingsReference[RaisingIndex].Position == PositionLevel.Neutral)
			{
				PositionSet = m_RaisingModule.Variables["Position"].Sets["Neutral"];
				Position = new FuzzySET("Neutral",ref PositionSet);
			}
			else			
			{
				PositionSet = m_RaisingModule.Variables["Position"].Sets["Advantage"];
				Position = new FuzzySET("Advantage",ref PositionSet);
			}
			
			FuzzySet HandSet = m_RaisingModule.Variables["Hand"].Sets["Bad"];
			FuzzySET Hand = new FuzzySET("Bad",ref HandSet);
			
			if(DecisionReference.RaisingsReference[RaisingIndex].Hand == HandGrade.Bad)
			{
				HandSet = m_RaisingModule.Variables["Hand"].Sets["Bad"];
				Hand = new FuzzySET("Bad",ref HandSet);
			}
			else if(DecisionReference.RaisingsReference[RaisingIndex].Hand == HandGrade.Average)
			{
				HandSet = m_RaisingModule.Variables["Hand"].Sets["Average"];
				Hand = new FuzzySET("Average",ref HandSet);
			}
			else			
			{
				HandSet = m_RaisingModule.Variables["Hand"].Sets["Good"];
				Hand = new FuzzySET("Good",ref HandSet);
			}
			
			FuzzySet DesirabilitySet = m_RaisingModule.Variables["Desirability"].Sets["Undesirable"];
			FuzzySET DesirabilityLevel = new FuzzySET("Undesirable",ref DesirabilitySet);
			
			if(DecisionReference.RaisingsReference[RaisingIndex].Desirability == Desirability.Undesirable)
			{
				DesirabilitySet = m_RaisingModule.Variables["Desirability"].Sets["Undesirable"];
				DesirabilityLevel = new FuzzySET("Undesirable",ref DesirabilitySet);
			}
			else if(DecisionReference.RaisingsReference[RaisingIndex].Desirability == Desirability.Desirable)
			{
				DesirabilitySet = m_RaisingModule.Variables["Desirability"].Sets["Desirable"];
				DesirabilityLevel = new FuzzySET("Desirable",ref DesirabilitySet);
			}
			else			
			{
				DesirabilitySet = m_RaisingModule.Variables["Desirability"].Sets["VeryDesirable"];
				DesirabilityLevel = new FuzzySET("VeryDesirable",ref DesirabilitySet);
			}
			
			m_RaisingModule.AddRule(new FuzzyAND(Aggressiveness,Tightness,Position,Hand),DesirabilityLevel);
		}
		#endregion
		
		#region Initialize Bluffing module
		m_BluffingModule = new FuzzyModule();
		
		m_BluffingModule.CreateFuzzyVariable("ValidPlayersCount");
		m_BluffingModule.CreateFuzzyVariable("StackSize");
		m_BluffingModule.CreateFuzzyVariable("Earnings");
		m_BluffingModule.CreateFuzzyVariable("Desirability");
		
		m_BluffingModule.Variables["ValidPlayersCount"].AddLeftShoulderSet("Low",2.0f,0.0f,3.0f);
		m_BluffingModule.Variables["ValidPlayersCount"].AddTriangularSet("Medium",3.0f,2.0f,4.0f);
		m_BluffingModule.Variables["ValidPlayersCount"].AddLeftDiagonalSet("High",3.0f,4.0f);
		
		m_BluffingModule.Variables["StackSize"].AddLeftShoulderSet("Short",20.0f,0.0f,40.0f);
		m_BluffingModule.Variables["StackSize"].AddCentreTrapeziumSet("Medium",40.0f,60.0f,20.0f,80.0f);
		m_BluffingModule.Variables["StackSize"].AddRightShoulderSet("Deep",80.0f,60.0f,100.0f);
		
		m_BluffingModule.Variables["Earnings"].AddLeftShoulderSet("Low",0.25f,0.0f,0.5f);
		m_BluffingModule.Variables["Earnings"].AddTriangularSet("Medium",0.5f,0.25f,0.75f);
		m_BluffingModule.Variables["Earnings"].AddRightShoulderSet("High",0.75f,0.5f,1.0f);
		
		m_BluffingModule.Variables["Desirability"].AddLeftShoulderSet("Undesirable",25.0f,0.0f,50.0f);
		m_BluffingModule.Variables["Desirability"].AddTriangularSet("Desirable",50.0f,25.0f,75.0f);
		m_BluffingModule.Variables["Desirability"].AddRightShoulderSet("VeryDesirable",75.0f,50.0f,100.0f);
		
		for(int BluffingIndex = 0; BluffingIndex < DecisionReference.BluffingReference.Length; BluffingIndex++)
		{
			FuzzySet ValidPlayerCountSet = m_BluffingModule.Variables["ValidPlayersCount"].Sets["Low"];
			FuzzySET PlayerCount = new FuzzySET("Low",ref ValidPlayerCountSet);
			
			if(DecisionReference.BluffingReference[BluffingIndex].PlayerCount == ValidPlayerCount.Low)
			{
				ValidPlayerCountSet = m_BluffingModule.Variables["ValidPlayersCount"].Sets["Low"];
				PlayerCount = new FuzzySET("Low",ref ValidPlayerCountSet);
			}
			else if(DecisionReference.BluffingReference[BluffingIndex].PlayerCount == ValidPlayerCount.Medium)
			{
				ValidPlayerCountSet = m_BluffingModule.Variables["ValidPlayersCount"].Sets["Medium"];
				PlayerCount = new FuzzySET("Medium",ref ValidPlayerCountSet);
			}
			else			
			{
				ValidPlayerCountSet = m_BluffingModule.Variables["ValidPlayersCount"].Sets["High"];
				PlayerCount = new FuzzySET("High",ref ValidPlayerCountSet);
			}
			
			FuzzySet StackSet = m_BluffingModule.Variables["StackSize"].Sets["Short"];
			FuzzySET Stack = new FuzzySET("Short",ref StackSet);
			
			if(DecisionReference.BluffingReference[BluffingIndex].StackSize == StackSizing.Short)
			{
				StackSet = m_BluffingModule.Variables["StackSize"].Sets["Short"];
				Stack = new FuzzySET("Short",ref StackSet);
			}
			else if(DecisionReference.BluffingReference[BluffingIndex].StackSize == StackSizing.Medium)
			{
				StackSet = m_BluffingModule.Variables["StackSize"].Sets["Medium"];
				Stack = new FuzzySET("Medium",ref StackSet);
			}
			else			
			{
				StackSet = m_BluffingModule.Variables["StackSize"].Sets["Deep"];
				Stack = new FuzzySET("Deep",ref StackSet);
			}
			
			FuzzySet EarningsSet = m_BluffingModule.Variables["Earnings"].Sets["Low"];
			FuzzySET Earnings = new FuzzySET("Low",ref EarningsSet);
			
			if(DecisionReference.BluffingReference[BluffingIndex].Earnings == Earning.Low)
			{
				EarningsSet = m_BluffingModule.Variables["Earnings"].Sets["Low"];
				Earnings = new FuzzySET("Low",ref EarningsSet);
			}
			else if(DecisionReference.BluffingReference[BluffingIndex].Earnings == Earning.Medium)
			{
				EarningsSet = m_BluffingModule.Variables["Earnings"].Sets["Medium"];
				Earnings = new FuzzySET("Medium",ref EarningsSet);
			}
			else			
			{
				EarningsSet = m_BluffingModule.Variables["Earnings"].Sets["High"];
				Earnings = new FuzzySET("High",ref EarningsSet);
			}
			
			FuzzySet DesirabilityLevelSet = m_BluffingModule.Variables["Desirability"].Sets["Undesirable"];
			FuzzySET DesirabilityLevel = new FuzzySET("Undesirable",ref DesirabilityLevelSet);
			
			if(DecisionReference.BluffingReference[BluffingIndex].Desirability == Desirability.Undesirable)
			{
				DesirabilityLevelSet = m_BluffingModule.Variables["Desirability"].Sets["Undesirable"];
				DesirabilityLevel = new FuzzySET("Undesirable",ref DesirabilityLevelSet);
			}
			else if(DecisionReference.BluffingReference[BluffingIndex].Desirability == Desirability.Desirable)
			{
				DesirabilityLevelSet = m_BluffingModule.Variables["Desirability"].Sets["Desirable"];
				DesirabilityLevel = new FuzzySET("Desirable",ref DesirabilityLevelSet);
			}
			else			
			{
				DesirabilityLevelSet = m_BluffingModule.Variables["Desirability"].Sets["VeryDesirable"];
				DesirabilityLevel = new FuzzySET("VeryDesirable",ref DesirabilityLevelSet);
			}
			
			m_RaisingModule.AddRule(new FuzzyAND(PlayerCount,Stack,Earnings),DesirabilityLevel);
		}
		#endregion
	}
	
	private void RefreshBetRange()
	{
		BetRange[0] = 2.0f / 3.0f * (float) m_Player.GManager.Table.Pot;
		BetRange[1] = m_Player.GManager.Table.Pot;
		BetRange[2] = (int) (0.5f * (float) (m_Player.GManager.Betting.CurrentBet * 2 + m_Player.GManager.Table.Pot));
		BetRange[3] = m_Player.GManager.Betting.CurrentBet * 2 + m_Player.GManager.Table.Pot;
		BetRange[4] = 2 * (m_Player.GManager.Betting.CurrentBet * 2 + m_Player.GManager.Table.Pot);
		BetRange[5] = m_Player.Stack;
		
		Debug.Log("Refresh bet range: ");
		Debug.Log("Betrange 0 : " + BetRange[0]);
		Debug.Log("Betrange 1 : " + BetRange[1]);
		Debug.Log("Betrange 2 : " + BetRange[2]);
		Debug.Log("Betrange 3 : " + BetRange[3]);
		Debug.Log("Betrange 4 : " + BetRange[4]);
		Debug.Log("Betrange 5 : " + BetRange[5]);
	}
	
	private bool IsHandWithinPlayerRange(Card[] Range)
	{
		Hands HandType = Evaluator.EvaluateHand(m_Player.Hand);
		Values RepresentativeValue = Values.Two;
		
		if(HandType == Hands.HighCard)
		{
			Values HighestValue = Values.Two;
			
			for(int CardIndex = 0; CardIndex < m_Player.Hand.Count; CardIndex++)
			{
				if(m_Player.Hand[CardIndex].Value > HighestValue)
					HighestValue = m_Player.Hand[CardIndex].Value;
			}
			
			RepresentativeValue = HighestValue;
		}
		else if(HandType == Hands.OnePair)
		{
			for(int FirstCardIndex = 0; FirstCardIndex < m_Player.Hand.Count; FirstCardIndex++)
			{
				bool PairValueFound = false;
				
				for(int SecondCardIndex = 0; SecondCardIndex < m_Player.Hand.Count; SecondCardIndex++)
				{
					if(m_Player.Hand[SecondCardIndex] != m_Player.Hand[FirstCardIndex] && m_Player.Hand[SecondCardIndex].Value == m_Player.Hand[FirstCardIndex].Value)
					{
						RepresentativeValue = m_Player.Hand[SecondCardIndex].Value;
						PairValueFound = true;
						break;
					}
				}
				
				if(PairValueFound)
					break;
			}
		}
		
		if(m_Tightness >= 10)
		{
			if((int)HandType > (int) Hands.OnePair || HandType == Hands.OnePair && (int) RepresentativeValue >= (int) Values.King)
				return true; 
		}
		else if(m_Tightness >= 8 && m_Tightness < 10)
		{
			if((int)HandType > (int) Hands.OnePair || HandType == Hands.OnePair && (int) RepresentativeValue >= (int) Values.Ten)
				return true; 
		}
		else if(m_Tightness >= 6 && m_Tightness < 8)
		{
			if((int)HandType > (int) Hands.OnePair || HandType == Hands.OnePair && (int) RepresentativeValue >= (int) Values.Five)
				return true; 
		}
		else if(m_Tightness >= 4 && m_Tightness < 6)
		{
			if((int)HandType > (int) Hands.HighCard || HandType == Hands.HighCard && (int) RepresentativeValue >= (int) Values.Ten)
				return true; 
		}
		else if(m_Tightness >= 2 && m_Tightness < 4)
		{
			if((int)HandType > (int) Hands.HighCard || HandType == Hands.HighCard && (int) RepresentativeValue >= (int) Values.Seven)
				return true; 
		}
		else if(m_Tightness >= 0 && m_Tightness < 2)
		{
			return true;
		}
		
		return false;
	}
	
	private bool IsPlayerInPosition(TablePosition _Position)
	{
		return _Position == TablePosition.OnTheButton ? true : false;
	}
	
	public BettingDecision DeterminePlayerBetting()
	{
		//if enemy's memory contain a strategy to be made in the blackboard, execute it before any other evaluation for betting decision
		//		if(Memory.Board.Count <= 0)
		//		{
		//			Memory.CheckApplicabilityOfPlays();
		//		}
		//		else
		//		{
		//			BettingDecision Decision = Memory.ExecuteBetPlays();
		//			return Decision;
		//		}
		//

		if(!m_Player.GManager.Betting.BetMadeThisRound)
		{
			//			Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " intends to bet..");
			if(!IsHandWithinPlayerRange(m_Player.Hand.ToArray()))
			{
				//				Debug.Log("Hand not within Enemy " + (m_Player.Index - 1) + "'s range");	
				return BettingDecision.Fold;
			}
			
			//			Debug.Log("Hand within Enemy " + (m_Player.Index - 1) + "'s range");
			
			float BetToBeMade = 0.0f;
			float RaiseEV = -1.0f;
			
			float Pot = (float) m_Player.GManager.Table.Pot;
			float AmountOfValidPlayers = (float) Utility.HowManyValidPlayersLeft(m_Player.GManager);
			
			float PlayerEquity = Evaluator.CalculatePlayerAverageEquity(m_Player) / 100.0f;
			
			float AggressivenessWeight = -Mathf.Pow((Aggressiveness/10.0f),2.0f) + 1.0f;
			float TightnessWeight = Mathf.Pow(Tightness/10.0f,2.0f);
			
			float EquityRequirement = (AggressivenessWeight + TightnessWeight)/2.0f ; 
			
			EquityRequirement = Mathf.Clamp(EquityRequirement,0.1f,0.9f);
			
			//			Debug.Log("Aggressiveness weight: " + AggressivenessWeight);
			//			Debug.Log("Tightness weight: " + TightnessWeight);
			//			Debug.Log("Equity required to continue this hand: " + EquityRequirement + " Enemy" + (m_Player.Index - 1) + "'s equity: " + PlayerEquity);
			
			if(PlayerEquity < EquityRequirement)//NOTE. THE EQUITY REQUIREMENT OF 50% NEED TO VARY BASED ON PLAYER'S TIGHTNESS AND AGGRESSIVENESS
				return BettingDecision.Fold;
			
			BetToBeMade = (float) ((3 + m_Player.GManager.Betting.Limpers.Count) * m_Player.GManager.Betting.BigBind);
			
			//			Debug.Log("Bet that Enemy " + (m_Player.Index - 1) + " is going to place: (3 + " + m_Player.GManager.Betting.Limpers.Count + ") * " + m_Player.GManager.Betting.BigBind + " = " + BetToBeMade);
			
			float MoneyEarnedByRaising = 0.0f;
			if(Skill <= 5)
			{
				MoneyEarnedByRaising = (AmountOfValidPlayers - 1) * BetToBeMade;
			}
			else if(Skill > 5)
			{
				for(int PlayerIndex = 0; PlayerIndex < m_GMInstance.Players.Length; PlayerIndex++)
				{
					if(m_GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index && !m_GMInstance.Players[PlayerIndex].Busted && !m_GMInstance.Players[PlayerIndex].Fold)
					{
						float ProbabilityOfRaising = 0.0f;
						
						//FIND THE PROBABILITY OF EACH OF THE OTHER VALID PLAYERS RAISING
						//FACTOR. TENDENCY OF RAISING, AGGRESSIVENESS OF PLAYER, HOW THEIR EQUITY FARE AGAINST OTHER VALID PLAYERS, STACK LEVEL
						int TotalActionsInBetting = 0;
						int TotalRaiseInBetting = 0;
						
						for(int LogIndex = 0; LogIndex < m_GMInstance.CommonMemoryLog.MemoryLog.Count; LogIndex++)
						{
							if(m_GMInstance.CommonMemoryLog.MemoryLog[LogIndex].PlayerIndex == m_GMInstance.Players[PlayerIndex].Index && m_GMInstance.CommonMemoryLog.MemoryLog[LogIndex].T_Phase == TurnPhase.Betting)
							{
								TotalActionsInBetting++;
								
								if(m_GMInstance.CommonMemoryLog.MemoryLog[LogIndex].P_Action == PlayerAction.Raise)
									TotalRaiseInBetting++;
							}
						}
						
						float TendencyToRaise = (float) TotalRaiseInBetting / (float) TotalActionsInBetting * 100.0f;
						
						float StackWeighting = Mathf.Clamp(PlayerInstance.Stack,0.0f,100.0f * m_GMInstance.Betting.BigBind) / (100.0f * m_GMInstance.Betting.BigBind);
						//						Debug.Log("Stack weight: " + StackWeighting + " Current stack size: " + Mathf.Clamp(PlayerInstance.Stack,0.0f,100.0f * m_GMInstance.Betting.BigBind) + "/" + (100.0f * m_GMInstance.Betting.BigBind));
						
						float PlayerAverageEquity = Evaluator.CalculatePlayerAverageEquity(m_GMInstance.Players[PlayerIndex]);
						float AverageEquityWeighting = Mathf.Pow((PlayerAverageEquity/100.0f - 1.0f),2.0f) + 1.0f;
						//						Debug.Log("AveageEquity weight: " + AverageEquityWeighting + " AverageEquity: " + PlayerAverageEquity);
						
						float EstimatedAggroWeighting = Mathf.Pow((Memory.OpponentsAggressiveness[PlayerIndex]/10.0f),2.0f);
						//						Debug.Log("Est.Aggro weight: " + EstimatedAggroWeighting + " Est.Aggro: " + Memory.OpponentsAggressiveness[PlayerIndex]);
						
						float TendencyOfRaisingWeighting = Mathf.Pow((TendencyToRaise/100.0f - 1.0f),2.0f) + 1.0f;
						//						Debug.Log("TendencyOfRaising weight: " + TendencyOfRaisingWeighting + " Tendency of raising: " + TendencyToRaise);
						
						//						//ADD THE MULTIPLICATION OF THEIR RAISING PROBABILITY WITH THE AMOUNT OF MONEY THEY NEED TO CALL TOGETHER TO GET THE MONEYEARNEDBYRAISING
						//						ProbabilityOfRaising /= 100.0f;
						MoneyEarnedByRaising += ProbabilityOfRaising * BetToBeMade;
					}
				}
				
			}
			
			RaiseEV = PlayerEquity * (MoneyEarnedByRaising) - (1.0f - PlayerEquity) * (BetToBeMade);//NOTE. 
			
			//Determine the EV of Calling as well to see whether calling or raising is a more +EV action
			float CallEV = PlayerEquity * (Pot - m_Player.OnTheBet) - (1.0f - PlayerEquity) * (m_Player.GManager.Table.GetHighestBet());
			
			//			Debug.Log("EV for raising: " + PlayerEquity + " x (" + MoneyEarnedByRaising + ") - (1.0 - " + PlayerEquity + ") x " + BetToBeMade + " = " + RaiseEV);
			//			Debug.Log("EV for caling: " + PlayerEquity + " x (" + Pot + " - " + m_Player.OnTheBet + ") - (1.0 - " + PlayerEquity + ") x (" + m_Player.GManager.Table.GetHighestBet() + ") = " + CallEV);
			
			if(RaiseEV < 0.0f && CallEV < 0.0f)
			{	
				return BettingDecision.Fold;
			}
			else if(CallEV > RaiseEV)
			{	
				m_Player.GManager.Betting.Limpers.Add(m_Player);
				
				if(PlayerInstance.OnTheBet == m_GMInstance.Table.GetHighestBet())
					return BettingDecision.Check;
				
				return BettingDecision.Call;
			}
			
			//			Debug.Log("Bet to be made by Enemy" + (m_Player.Index - 1) + ": " + BetToBeMade);
			
			m_Player.RaiseValue = (int) BetToBeMade + (m_GMInstance.Table.GetHighestBet() - PlayerInstance.OnTheBet);
			
			return BettingDecision.Bet;
		}
		
		if(!IsHandWithinPlayerRange(m_Player.Hand.ToArray()))
		{
			//			Debug.Log("Folded due to hand not within range");
			return BettingDecision.Fold;
		}
		
		HandGrade Grade = Evaluator.DetermineHandGrade(m_Player);
		
		//		Debug.Log("Enemy " + (m_Player.Index - 1) + "'s hand grade: " + Grade);
		
		if(Grade == HandGrade.Bad)
		{
			if(m_Player.OnTheBet == m_Player.GManager.Table.GetHighestBet())
				return BettingDecision.Check;
			
			if(Evaluator.DeterminePlayerStackSize(m_Player) == StackSizing.Short)
			{
				//				Debug.Log("Enemy " + (m_Player.Index - 1) + " folded due to bad hand and having short stack"); 
				return BettingDecision.Fold;
			}
			
			return BettingDecision.Call;
		}
		
		//if notbad / good, decide whether to semi-bluff, set-mining, check-raise, raise or call/check/fold
		else if(Grade == HandGrade.Average || Grade == HandGrade.Good)
		{
			//calculate possibility of raising
			//being in position, first player to enter the pot, hands, stack, amount of money will affect the probability
			
			//Check whether player want to regular raise (first player to in the round)
			if(m_Player.GManager.Betting.RaiseMadeCount == 0)
			{
				//				Debug.Log("enemy " + (m_Player.Index - 1) + " will be thinking of raising the pot");
				
				m_RaisingModule.Fuzzify("Aggressiveness", (float) m_Aggressiveness);
				m_RaisingModule.Fuzzify("Tightness", (float) m_Tightness);
				m_RaisingModule.Fuzzify("Position", (float) Evaluator.DeterminePlayerPositionAdvantage(m_Player) / 3.0f);
				m_RaisingModule.Fuzzify("Hand", ((float) Evaluator.DeterminePlayerHandLevelINT(m_Player)/22.0f));
				
				float DesirabilityToRaise = m_RaisingModule.Defuzzify("Desirability");
				
				if(!m_Player.GManager.CommonMemoryLog.HasAnyPlayerEnterPot())
				{
					//					Debug.Log("Enemy " + (m_Player.Index - 1) + " will be the first player to enter the pot");
					DesirabilityToRaise += 20.0f;
				}
				else
				{
					//					Debug.Log("Enemy " + (m_Player.Index - 1) + " will not be the first player to enter the pot");
				}
				
				//				Debug.Log("enemy " + (m_Player.Index - 1) + "'s desirability to raise: " + DesirabilityToRaise);
				
				if(DesirabilityToRaise >= 50.0f)
				{
					//					Debug.Log("Enemy " + (m_Player.Index - 1) + " intend to raise the pot");
					
					TablePosition PlayerPosition = m_Player.GManager.Betting.DeterminePlayerPosition(m_Player);
					
					if(PlayerPosition == TablePosition.UnderTheGun)
						m_Player.RaiseValue = (int) (Random.Range(2.75f,3.25f) * (float) m_Player.GManager.Betting.BigBind);
					
					else if(PlayerPosition == TablePosition.OnTheButton)
						m_Player.RaiseValue = (int) (Random.Range(2.25f,2.75f) * m_Player.GManager.Betting.BigBind);
					
					else if(PlayerPosition == TablePosition.SmallBind)
						m_Player.RaiseValue = (int) (Random.Range(3.5f,4.0f) *  m_Player.GManager.Betting.BigBind);
					
					else
						m_Player.RaiseValue = (int) (Random.Range(3.75f,4.25f) * m_Player.GManager.Betting.BigBind);
					
					if(m_Player.RaiseValue > 0.0f)
						m_Player.RaiseValue += m_GMInstance.Table.GetHighestBet() - m_Player.OnTheBet;
					
					m_Player.RaiseValue = Mathf.Clamp(m_Player.RaiseValue,0,m_Player.Stack);
					
					//					Debug.Log("Enemy " + (m_Player.Index - 1) + " will be raising $" + m_Player.RaiseValue + "(FROM: " + m_Player.OnTheBet + " TO: " + (m_Player.OnTheBet + m_Player.RaiseValue) + ")");
					
					return BettingDecision.Raise;
				}
			}
			
			//If there is already a raise in this betting round, check whether player want to re-raise
			else if(m_Player.GManager.Betting.RaiseMadeCount >= 1)
			{
				//				Debug.Log("Enemy " + (m_Player.Index - 1) + " will be thinking of re-raising the pot");
				if(PlayerInstance.OnTheBet < m_GMInstance.Table.GetHighestBet() && PlayerInstance.Stack > m_GMInstance.Table.GetHighestBet() - PlayerInstance.OnTheBet
				   || PlayerInstance.OnTheBet == m_GMInstance.Table.GetHighestBet() && PlayerInstance.Stack > 0.0f)
				{
					//CALCULATE DESIRABILTY OF PLAYER TO USE 3 BET FOR VALUE
					//FACTORS. POSITION ON BOARD (LATER THE POSITION THE BETTER EG. ON THE BUTTON), HAND LEVEL/HAND GRADE/HAND TYPE, THE LOOSENESS OF OTHER PLAYERS, HOW LOW IS THE SKILL LEVEL OF OTHER PLAYERS, EQUITY AGAINST THE ENEMY'S POSSIBLE HAND (ADVANTAGE)
					float DesirabilityTo3BetValue = 0.0f;
					//					Debug.Log("Calculating 3 bet value desirability: " );
					
					TablePosition PlayerPosition = m_GMInstance.Betting.DeterminePlayerPosition(PlayerInstance);
					float PlayerPositionWeighting = -Mathf.Pow((((float) PlayerPosition / 3.0f) - 1.0f),2.0f) + 1.0f;
					//					Debug.Log("PlayerPosition: " + PlayerPosition + " Weight of player position: " + PlayerPositionWeighting);
					
					int PlayerHandLevel = Evaluator.DeterminePlayerHandLevelINT(PlayerInstance);
					float PlayerHandWeight = (float) PlayerHandLevel / 22.0f;
					//					Debug.Log("PlayerHandGrade: " + PlayerHandLevel + " Weight of player's hand: " + PlayerHandWeight);
					
					float ValidPlayerCount = 0.0f;
					float TotalTightnessWeight = 0.0f;
					float TotalSkillWeight = 0.0f;
					
					for(int PlayerIndex = 0; PlayerIndex < m_GMInstance.Players.Length; PlayerIndex++)
					{
						if(!m_GMInstance.Players[PlayerIndex].Busted && !m_GMInstance.Players[PlayerIndex].Fold && m_GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index)
						{
							TotalTightnessWeight += Mathf.Pow(((Memory.OpponentTightness[PlayerIndex]/10.0f) - 1.0f),2.0f);
							TotalSkillWeight += Mathf.Pow(((float) Skill / 5.0f - 1.0f),2.0f); //-(Mathf.Pow(((float) Memory.OpponentsSkillLevel[PlayerIndex]/5.0f - 1.0f),2.0f)) + 1.0f;
							ValidPlayerCount++;
							//						Debug.Log("Skill level: " + (float) Memory.OpponentsSkillLevel[PlayerIndex]);
						}
					}
					
					float AverageTightnessWeight = TotalTightnessWeight/ValidPlayerCount;
					float AverageSkillWeight = TotalSkillWeight/ValidPlayerCount;
					//					Debug.Log("TotalTightnessWeight/ValidPlayerCount: " + TotalTightnessWeight + "/" + ValidPlayerCount + "  Average Tightness Weight: " + AverageTightnessWeight); 
					//					Debug.Log("TotalSkillWeight/ValidPlayerCount: " + TotalSkillWeight + "/" + ValidPlayerCount + " Average Skill Weight: " + AverageSkillWeight);
					
					float PlayerAverageEquity = Evaluator.CalculatePlayerAverageEquity(PlayerInstance);
					float AverageEquityWeight = PlayerAverageEquity/100.0f;//Mathf.Pow(PlayerAverageEquity/100.0f,2.0f);//-Mathf.Pow((PlayerAverageEquity/100.0f - 1.0f),2.0f) + 1.0f;
					//					Debug.Log("PlayerAverageEquity: " + PlayerAverageEquity + " Average Equity Weight: " + AverageEquityWeight);
					
					float AggressivenessWeight = Mathf.Pow((float) Aggressiveness/10.0f,2.0f);
					//					Debug.Log("Enemy " + (PlayerInstance.Index - 1) + "'s aggressiveness: " + Aggressiveness + " Aggressiveness Weight: " + AggressivenessWeight);
					
					DesirabilityTo3BetValue = (PlayerPositionWeighting + PlayerHandWeight + AverageTightnessWeight + AverageSkillWeight + AverageEquityWeight + AggressivenessWeight)/6.0f;
					//					Debug.Log("Initial 3Bet value desirability: " + DesirabilityTo3BetValue);
					
					//CALCULATE DESIRABILITY OF PLAYER TO USE 3 BET FOR BLUFF
					//FACTORS. TENDENCY OF PLAYER FOLDING TO A RAISE, THE POSITION OF THE PLAYER THAT MADE THE PREVIOUS RAISE (LATE POSITION WILL BE PREFERED), USED AGAINST TIGHT AND AGGRESSIVE PLAYERS,POSITION ON BOARD
					float DesirabilityTo3BetBluff = 0.0f;
					
					//					Debug.Log("Calculating 3 bet bluff desirability: ");
					
					//DETERMINE WHETHER PLAYER WILL BE CAPABLE OF USING 3 BET AS A BLUFF BY REFERENCING THEIR SKIL LEVEL
					if(Skill > 3f)
					{
						TablePosition PositionOfLastRaise = m_GMInstance.Betting.DeterminePlayerPosition(m_GMInstance.Betting.LatestAggressivePlayer);
						float LastRaisedPosWeight = -Mathf.Pow(((float) PositionOfLastRaise/3.0f - 1.0f),2.0f) + 1.0f;
						//						Debug.Log("PositionOfLastRaised: " + PositionOfLastRaise + " Weight of LastRaisedPos: " + LastRaisedPosWeight);
						
						TablePosition PositionOfPlayer = m_GMInstance.Betting.DeterminePlayerPosition(PlayerInstance);
						float PlayerPosWeight = -Mathf.Pow(((float) PositionOfPlayer/3.0f - 1.0f),2.0f) + 1.0f;
						//						Debug.Log("PositionOfPlayer: " + PositionOfPlayer + " Weight of PlayerPos: " + PlayerPosWeight);
						
						float TotalTendencyOfFoldingToRaise = 0.0f;
						
						for(int PlayerIndex = 0; PlayerIndex < m_GMInstance.Players.Length; PlayerIndex++)
						{
							if(!m_GMInstance.Players[PlayerIndex].Busted && !m_GMInstance.Players[PlayerIndex].Fold)
							{
								int TotalAmountOfActions = 0;
								int TotalAmountOfFoldingToRaise = 0;
								
								for(int FirstLogIndex = 0; FirstLogIndex < m_GMInstance.CommonMemoryLog.MemoryLog.Count; FirstLogIndex++)
								{
									if(m_GMInstance.CommonMemoryLog.MemoryLog[FirstLogIndex].P_Action == PlayerAction.Raise)
									{
										int LogLimitIndex = FirstLogIndex + 4;
										
										if(LogLimitIndex > m_GMInstance.CommonMemoryLog.MemoryLog.Count)
											LogLimitIndex = FirstLogIndex + (m_GMInstance.CommonMemoryLog.MemoryLog.Count - 1 - FirstLogIndex);
										
										for(int SecondLogIndex = FirstLogIndex + 1; SecondLogIndex < LogLimitIndex; SecondLogIndex++)
										{
											if(m_GMInstance.CommonMemoryLog.MemoryLog[SecondLogIndex].PlayerIndex == m_GMInstance.Players[PlayerIndex].Index)
											{
												TotalAmountOfActions++;
												
												if(m_GMInstance.CommonMemoryLog.MemoryLog[SecondLogIndex].P_Action == PlayerAction.Fold)
												{
													TotalAmountOfFoldingToRaise++;
												}
											}
										}
									}
								}
								
								if(TotalAmountOfActions != 0 && TotalAmountOfFoldingToRaise != 0)
									TotalTendencyOfFoldingToRaise += (float) TotalAmountOfFoldingToRaise / (float) TotalAmountOfActions;
								
								if(TotalAmountOfActions == 0)
								{
									float PlayerAggroWeight = Mathf.Pow(((float)Aggressiveness/10.0f),2.0f);//-Mathf.Pow(((float) Aggressiveness / 10.0f - 1.0f),2.0f) + 1.0f;
									float PlayerTightnessWeight = Mathf.Pow(((float) Memory.OpponentsAggressiveness[PlayerIndex] / 10.0f - 1.0f),2.0f); //-Mathf.Pow((float) Tightness/10.0f,2.0f) + 1.0f;
									
									TotalTendencyOfFoldingToRaise += (PlayerAggroWeight + PlayerTightnessWeight)/2.0f;
									
//									Debug.Log("No data to referenced..");
//									Debug.Log("PlayerAggroWeight: " + PlayerAggroWeight + " PlayerTightnessWeight: " + PlayerTightnessWeight);
//									Debug.Log("Simulated Tendency: " + (PlayerAggroWeight + PlayerTightnessWeight)/2.0f);
								}
							}
						}
						
						float FoldingtoRaiseWeight = -Mathf.Pow((TotalTendencyOfFoldingToRaise/(float) Utility.HowManyValidPlayersLeft(m_GMInstance) - 1.0f),2.0f) + 1.0f;
						//						Debug.Log("Tendency of players folding to raise: " + TotalTendencyOfFoldingToRaise + " Weight of players folding to raise: " + FoldingtoRaiseWeight);
						
						int AmountOfTightAggressivePlayers = 0;
						
						for(int PlayerIndex = 0; PlayerIndex < m_GMInstance.Players.Length; PlayerIndex++)
						{
							if(m_GMInstance.Players[PlayerIndex].Index != PlayerInstance.Index && Memory.OpponentTightness[PlayerIndex] > 5 && Memory.OpponentsAggressiveness[PlayerIndex] > 5)
							{
								AmountOfTightAggressivePlayers++;
							}
						}
						
						int TightAggroPlayersWeight = AmountOfTightAggressivePlayers / Utility.HowManyValidPlayersLeft(m_GMInstance);
						//						Debug.Log("Total amount of TightAggro Players: " + AmountOfTightAggressivePlayers + " Weight of TightAggro Player Amt.: " + TightAggroPlayersWeight);
						
						float PlayerAggressivenessWeight = Mathf.Pow((float) Aggressiveness/10.0f,2.0f);
						//						Debug.Log("Enemy " + (PlayerInstance.Index - 1) + "'s aggressiveness: " + Aggressiveness + " Aggressiveness Weight: " + AggressivenessWeight);
						
						DesirabilityTo3BetBluff = (LastRaisedPosWeight + PlayerPosWeight + FoldingtoRaiseWeight + (float) TightAggroPlayersWeight + PlayerAggressivenessWeight)/5.0f;
						//						Debug.Log("Initial 3Bet Bluff desirability: " + DesirabilityTo3BetBluff);
					}
					else
					{
						DesirabilityTo3BetBluff = 0.0f;
						//						Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " is not skilled enough to perform a 3 bet bluff");
					}
					
					float ReductionFactor = 0.225f;
					if(Aggressiveness > 5)
						ReductionFactor -= 0.125f * ((float) Aggressiveness - 5.0f)/5.0f;
					else if(Aggressiveness < 5)
						ReductionFactor += 0.2f + (5.0f - (float) Aggressiveness)/5.0f;
					
					//					Debug.Log("Reduction factor: " + ReductionFactor);
					//					Debug.Log("Reduce value desirability by " + ((m_GMInstance.Betting.RaiseMadeCount - 1) * ReductionFactor * DesirabilityTo3BetValue) * DesirabilityTo3BetValue + " and reduce bluff desirability by: " + ((m_GMInstance.Betting.RaiseMadeCount - 1) * 0.25f) * DesirabilityTo3BetBluff + " due to it being a re-raise");
					
					DesirabilityTo3BetValue -= ((m_GMInstance.Betting.RaiseMadeCount - 1) * ReductionFactor) * DesirabilityTo3BetValue;
					DesirabilityTo3BetBluff -= ((m_GMInstance.Betting.RaiseMadeCount - 1) * ReductionFactor) * DesirabilityTo3BetBluff;
					
					if(DesirabilityTo3BetValue < 0.0f)
						DesirabilityTo3BetValue = 0.0f;
					if(DesirabilityTo3BetBluff < 0.0f)
						DesirabilityTo3BetBluff = 0.0f;
					
					
					//					Debug.Log("Final Desirability to 3 bet for value: " + DesirabilityTo3BetValue);
					//					Debug.Log("Final Desirability to 3 bet for bluff: " + DesirabilityTo3BetBluff);
					
					if(DesirabilityTo3BetValue >= 0.5f || DesirabilityTo3BetBluff >= 0.5f)
					{
						float LastRaise = (float) m_GMInstance.Betting.LatestRaiseAmount;
						
						PlayerInstance.RaiseValue = (int) (Random.Range(2.0f,3.5f) * LastRaise) + (m_GMInstance.Table.GetHighestBet() - PlayerInstance.OnTheBet);
						
						PlayerInstance.RaiseValue = Mathf.Clamp(PlayerInstance.RaiseValue,0,PlayerInstance.Stack);
						
						//						Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " decided to 3 bet for " + PlayerInstance.RaiseValue);
						
						return BettingDecision.Raise;
					}
					
					//					Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " decided not to 3 bet");
				}
				
			}
		}
		
		//DETERMINE WHETHER PLAYER WILL CALL/CHECK OR FOLD THEIR HANDS
		//FACTORS. POT ODDS, EV OF CALLING/CHECKING/FOLDING, PLAYER EQUITY, AGGRESSIVENESS(HIGH TO CALL, LOW TO FOLD)
		//		Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " is considering Calling/Checking or Folding...");
		
		float AverageEquity = Evaluator.CalculatePlayerAverageEquity(PlayerInstance) / 100.0f;
		
		float EVForAction = 0.0f;
		
		if(PlayerInstance.OnTheBet < m_GMInstance.Table.GetHighestBet())
			EVForAction = (AverageEquity) * (m_GMInstance.Table.Pot - PlayerInstance.OnTheBet) - (1.0f - AverageEquity) * (m_GMInstance.Table.GetHighestBet() - PlayerInstance.OnTheBet);
		else
			EVForAction = AverageEquity * (m_GMInstance.Table.Pot - PlayerInstance.OnTheBet) - (1.0f - AverageEquity) * (0.0f);
		
		//		Debug.Log("EV of Calling / Checking : " + EVForAction);
		
		if(EVForAction >= 0.0f && (m_GMInstance.Table.GetHighestBet() - PlayerInstance.OnTheBet) < PlayerInstance.Stack)
		{
			if(PlayerInstance.OnTheBet < m_GMInstance.Table.GetHighestBet())
				return BettingDecision.Call;
			else
				return BettingDecision.Check;
		}
		
		return BettingDecision.Fold;
	}
	
	public AuctionDecision DeterminePlayerSelectAuctionCard()
	{
		//if(Memory.Board.Count >= 1)
		//{
		//	AuctionDecision Decision = Memory.ExecuteAuctionPlays();
		//	return Decision;
		//}
		
		if(PlayerInstance.GManager.AuctionPhase == AuctionPhase.First)
		{
			//IF LOW/AVERAGE SKILL LEVEL, DETERMINE WHICH CARD WILL BRING THE MOST IMPROVEMENT TO THE PLAYER'S HAND AT THE MOMENT
			//NOTE. 7 IS JUST A TEMPORARY PLACE HOLDER FOR REQUIREMENT TO GET HIGH SKILL LEVEL
			
//			#region Determine the card to be bid
			Card CardWithMostImprovement = new Card(Suits.NULL,Values.NULL);
			
			//			Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " thinking of bidding for a card...");
			
			if(Skill <= 3)
			{
				Hands HighestHandType = Evaluator.EvaluateHand(m_Player.Hand);
				int HighestHandLevel = Evaluator.DetermineHandLevelINT(m_Player.Hand.ToArray());
//				Debugger.PrintCards(PlayerInstance.Hand);
				//				Debug.Log("Current HandType: " + HighestHandType + " Current HandLevel: " + HighestHandLevel);
				
				for(int CardIndex = 0; CardIndex < m_GMInstance.Table.Pool.Count; CardIndex++)
				{
					for(int HandCardIndex = 0; HandCardIndex < m_Player.Hand.Count; HandCardIndex++)
					{
						Card[] PossibleHand = m_Player.Hand.ToArray();
						PossibleHand[HandCardIndex] = m_GMInstance.Table.Pool[CardIndex];
//						Debugger.PrintCards(PossibleHand);
						
						Hands PossibleHandType = Evaluator.EvaluateHand(PossibleHand);
						int PossibleHandLevel = Evaluator.DetermineHandLevelINT(PossibleHand);
						
						//						Debug.Log("Possible Hand Type : " + PossibleHandType + " Possible Hand Value: " + PossibleHandLevel);
						
						if(PossibleHandType > HighestHandType)
						{
							HighestHandType = PossibleHandType;
							HighestHandLevel = PossibleHandLevel;
							CardWithMostImprovement = m_GMInstance.Table.Pool[CardIndex];
							//							Debug.Log("Superor hand detected based on superior handtype");
						}
						else if(PossibleHandType == HighestHandType && PossibleHandLevel > HighestHandLevel)
						{
							HighestHandType = PossibleHandType;
							HighestHandLevel = PossibleHandLevel;
							CardWithMostImprovement = m_GMInstance.Table.Pool[CardIndex];
							//							Debug.Log("Superior hand detected based on superior hand level");
						}
					}
				}
				
				m_HandImproveRate = (HighestHandType - Evaluator.EvaluateHand(m_Player.Hand)) * (HighestHandLevel/2);
				
				//				Debug.Log("CardWithMostImprovement: " + CardWithMostImprovement.Suit + CardWithMostImprovement.Value);
				//				Debug.Log("New HandType: " + HighestHandType + " New HandLevel: " + HighestHandLevel);
				//				Debug.Log("Improve rate: " + m_HandImproveRate);
			}
			
			//IF HIGH SKILL LEVEL, GO THROUGH EACH CARD DETERMINE FURTHER POSSIBLE HAND THAT CAN BE BE FORMED
			else if(Skill > 3)
			{
				PossibleCombinations = new List<Card[]>();
				int HighestAmountOfPossibleHands = 0;
				Hands HighestHandFormed = Hands.HighCard;
				int HighestCombination = HighestAmountOfPossibleHands * (int) HighestHandFormed;
				
				List<Card[]> BestPossibleHands = new List<Card[]>();
				
//				Debug.Log("Going through all cards in pool and find the amount of possible hands they can formed..");

				Debug.Log("Pool card used to consider possible hands: " + m_GMInstance.Table.Pool[0].Suit + m_GMInstance.Table.Pool[0].Value);
				
				//THE CARD THAT PROVIDE THE HIGHEST IMPROVEMENT AND MOST WAY OF IMPROVING WILL WILL BE THE CARD TO BE BID
				m_Player.Pocket.Add(m_GMInstance.Table.Pool[0]);
				
				float StartTime = Time.realtimeSinceStartup;
				LinkedList<Card[]> PossibleHands = Evaluator.DeterminePossibleHand(m_Player,true,true);
				float EndTime = Time.realtimeSinceStartup;
				
				m_Player.Pocket.Remove(m_GMInstance.Table.Pool[0]);
				
				Debug.Log("Base hand: ");
				Debugger.PrintCards(m_Player.Hand);
				Debug.Log("Base pocket: ");
				Debugger.PrintCards(m_Player.Pocket);
				
				Debug.Log("Possible Hands (Amout: " + PossibleHands.Count + "): ");
//				Utility.PrintListOfHands(PossibleHands);
				Debug.Log("Time Taken to calculate possible hands (in seconds): " + (EndTime - StartTime));

				#region Loop through all the cards in the pool, Calculate all the possible superior hands that maybe formed using that card with player's cards. The card that bring the most and highest combination will be the bidded card
//				for(int CardIndex = 0; CardIndex < m_GMInstance.Table.Pool.Count; CardIndex++)
//				{
//					Debug.Log("Pool card used to consider possible hands: " + m_GMInstance.Table.Pool[CardIndex].Suit + m_GMInstance.Table.Pool[CardIndex].Value);
//
//					//THE CARD THAT PROVIDE THE HIGHEST IMPROVEMENT AND MOST WAY OF IMPROVING WILL WILL BE THE CARD TO BE BID
//					m_Player.Pocket.Add(m_GMInstance.Table.Pool[CardIndex]);
//
//					float StartTime = Time.realtimeSinceStartup;
//					Card[][] PossibleHands = Evaluator.DeterminePlayerPossibleHand(m_Player,true,true);
//					float EndTime = Time.realtimeSinceStartup;
//
//					m_Player.Pocket.Remove(m_GMInstance.Table.Pool[CardIndex]);
//
//					Debug.Log("Base hand: ");
//					Debugger.PrintCards(m_Player.Hand);
//					Debug.Log("Base pocket: ");
//					Debugger.PrintCards(m_Player.Pocket);
//
//					Debug.Log("Possible Hands (AMOUNT: " + PossibleHands.Length + ": ");
//					Utility.PrintArrayOfHands(PossibleHands);
//					Debug.Log("Time Taken to calculate possible hands (in seconds): " + (EndTime - StartTime));
//					
//					//STORE THE FURTHER POSSIBLE SUPERIOR HANDS THAT CAN BE FORMED INTO A LIST
////					#region Remove any possible hands that do not have that one card and is inferior to the player's current hand
////					Hands CurrentHandType = Evaluator.EvaluateHand(m_Player.Hand);
////					int CurrentHandLevel = Evaluator.DetermineHandLevelINT(m_Player.Hand.ToArray());
////					
////					for(int HandIndex = 0; HandIndex < PossibleHands.Count; HandIndex++)
////					{
////						if(!Utility.DoesHandContainThisCard(PossibleHands[HandIndex],m_GMInstance.Table.Pool[CardIndex]))
////						{
////							Hands PossibleHandType = Evaluator.EvaluateHand(PossibleHands[HandIndex]); 
////							int PossibleHandLevel = Evaluator.DetermineHandLevelINT(PossibleHands[HandIndex]);
////							
////							if(PossibleHandType < CurrentHandType)
////							{
////								PossibleHands.Remove(PossibleHands[HandIndex]);
////							}
////							else if(PossibleHandType == CurrentHandType && PossibleHandLevel < CurrentHandLevel)
////							{
////								PossibleHands.Remove(PossibleHands[HandIndex]);
////							}
////						}
////					}
////					#endregion
//					
////					//CALCULATE HOW MANY OF THOSE HANDS IT CAN FORMED AND HOW HIGH IT CAN BE (THE HIGHEST HANDS AND HIGHEST AMOUNT OF OUTS WILL WIN)
////					Hands HighestHandType = Hands.HighCard;
////					
////					for(int HandIndex = 0; HandIndex < PossibleHands.Count; HandIndex++)
////					{
////						Hands PossibleHandType = Evaluator.EvaluateHand(PossibleHands[HandIndex]);
////						
////						if(PossibleHandType > HighestHandType)
////							HighestHandType = PossibleHandType;
////					}
////					
////					if(PossibleHands.Count * (int) HighestHandType > HighestCombination)
////					{
////						HighestAmountOfPossibleHands = PossibleHands.Count;
////						HighestHandFormed = HighestHandType;
////						HighestCombination = PossibleHands.Count * (int) HighestHandType;
////						CardWithMostImprovement = m_GMInstance.Table.Pool[CardIndex];
////						BestPossibleHands = PossibleHands;
////					}
////					
////					Debug.Log("Current Card; " + m_GMInstance.Table.Pool[CardIndex].Suit + m_GMInstance.Table.Pool[CardIndex].Value);
////					Debug.Log("Highest Card: " + CardWithMostImprovement.Suit + CardWithMostImprovement.Value);
////					Debug.Log("Current Hand Combinations = " + PossibleHands.Count + " x " + (int) HighestHandType + " = " + PossibleHands.Count * (int) HighestHandType + " Highest Hand Combinations: " + HighestCombination);
////				}
////				#endregion
//				
////				PossibleCombinations.Clear();
//				
////				#region Go through all the possible superior hands and extract what extra cards (including the bidded card) is needed to form those hand as a combination
////				for(int HandIndex = 0; HandIndex < BestPossibleHands.Count; HandIndex++)
////				{
////					List<Card> Combination = new List<Card>();
////					
////					for(int CardIndex = 0; CardIndex < BestPossibleHands[HandIndex].Length; CardIndex++)
////					{
////						bool IsUnique = true;
////						
////						for(int HandCardIndex = 0; HandCardIndex < m_Player.Hand.Count; HandCardIndex++)
////						{
////							if(Utility.IsTwoCardsIdentical(BestPossibleHands[HandIndex][CardIndex],m_Player.Hand[HandCardIndex]))
////								IsUnique = false;
////						}
////						
////						if(IsUnique)
////							Combination.Add(BestPossibleHands[HandIndex][CardIndex]);
////					}
////					
////					if(Combination.Count > 0)
////						PossibleCombinations.Add(Combination.ToArray());
//				}
				#endregion
			}
			
			if(CardWithMostImprovement == null || (CardWithMostImprovement.Suit == Suits.NULL && CardWithMostImprovement.Value == Values.NULL))
			{
//				Debug.Log("None of the pol cards will improve Enemy " + (PlayerInstance.Index - 1) + "'s hand, so the Enemy will forfeit");
				return AuctionDecision.Forfeit;
			}
			
			//DETERMINE WHETHER PLAYER SHOULD ATTEMPT TO BID FOR THAT CARD
			//FACTOR. CONTESTABILITY OF THE CARD, AGGRESSIVENESS OF THE PLAYER, HOW MANY OUTS TO SUPERIOR HAND (HIGH SKILL LEVEL),
			//HOW MUCH IMPROVEMENT IT MADE TO CURRENT HAND (LOW SKILL LEVEL), CURRENT STACK LEVEL 
			
			#region Calculate whether player will desire to bid for that card
//			Debug.Log("Determining desirability to bid for that card...");
			float DesirabilityToBid = 0.0f;
			
			Card[] PoolCards = m_GMInstance.Table.Pool.ToArray();
			PoolCards = Utility.SortHandByCards(PoolCards,true);
			
			int CardRanking = m_GMInstance.Auction.DetermineRankInPool(CardWithMostImprovement);
			
			if(CardRanking == 5)
				DesirabilityToBid += 25.0f;
			
			else if(CardRanking == 4)
				DesirabilityToBid += 20.0f;
			
			else if(CardRanking == 3)
				DesirabilityToBid += 15.0f;
			
			else if(CardRanking == 2)
				DesirabilityToBid += 10.0f;
			
			else if(CardRanking == 1)
				DesirabilityToBid += 5.0f;
			
//			Debug.Log("CardRank: " + CardRanking + " Current DesirabilityToBid: " + DesirabilityToBid);
			
			if(Aggressiveness > 5)
				DesirabilityToBid += ((float) Aggressiveness - 5.0f)/5.0f * 10.0f;
			
//			Debug.Log("Aggressiveness: " + Aggressiveness + " Current DesirabilityToBid: " + DesirabilityToBid);
			
			StackSizing CurrentStackSize = Evaluator.DeterminePlayerStackSize(m_Player);
			
			if(CurrentStackSize == StackSizing.Short)
				DesirabilityToBid -= 10.0f;
			
			else if(CurrentStackSize == StackSizing.Medium || CurrentStackSize == StackSizing.Deep)
				DesirabilityToBid += 5.0f;
			
//			Debug.Log("Stacksize: " + CurrentStackSize + " Current DesirabilityToBid: " + DesirabilityToBid);
			
			if(Skill > 3)
				DesirabilityToBid += PossibleCombinations.Count * 0.5f;
			
//			else if(Skill <= 3)
//				DesirabilityToBid += m_HandImproveRate * 0.5f;
			
//			Debug.Log("Skill level: " + Skill + " Current DesirabilityToBid: " + DesirabilityToBid);
			#endregion
			
//			Debug.Log("Final DesirabilityToBid: " + DesirabilityToBid);
			
			if(DesirabilityToBid > 50.0f)
			{
//				Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " decide to bid for the card, " + CardWithMostImprovement.Suit + CardWithMostImprovement.Value);
				PlayerInstance.SelectedCardForAuction = CardWithMostImprovement;
				return AuctionDecision.NULL;
			}
			
			return AuctionDecision.Forfeit;
		}
		else if(PlayerInstance.GManager.AuctionPhase == AuctionPhase.Second)
		{
			Card CardWithMostImprovement = new Card(Suits.Clubs,Values.Two);
			List<Card[]> CombinationIncurredByTheCard = new List<Card[]>();
			
			if(Skill >= 7)
			{
				//GO THROUGH ALL THE CARDS IN THE POOL
				// IF HIGH SKIL LEVEL, CHECK WHETHER ANY CARDS IN POOL CORRESPOND TO THE FURTHER POSSIBLE HANDS STORED IN THE LIST
				int HighestOuts = 0;
				Card CardWithHighestOuts = new Card(Suits.Clubs,Values.Two);
				
				for(int PoolCardIndex = 0; PoolCardIndex < m_GMInstance.Table.Pool.Count; PoolCardIndex++)
				{
					List<Card[]> CurrentCombination = new List<Card[]>();
					
					for(int CombinationIndex = 0; CombinationIndex < PossibleCombinations.Count; CombinationIndex++)
					{
						bool CardFound = false;
						
						for(int CardIndex = 0; CardIndex < PossibleCombinations[CombinationIndex].Length; CardIndex++)
						{
							if(Utility.IsTwoCardsIdentical(m_GMInstance.Table.Pool[PoolCardIndex],PossibleCombinations[CombinationIndex][CardIndex]))
								CardFound = true;
						}
						
						if(CardFound)
							CurrentCombination.Add(PossibleCombinations[CombinationIndex]);
					}
					
					if(CurrentCombination.Count > HighestOuts)
					{
						HighestOuts = CurrentCombination.Count;
						CardWithHighestOuts = m_GMInstance.Table.Pool[PoolCardIndex];
						CombinationIncurredByTheCard = CurrentCombination;
					}
				}
				
				
				//BID FOR THE CARD THAT CORRESSPOND WITH THE MOST FURTHER POSSIBLE HANDS
				if(CombinationIncurredByTheCard.Count > 0)
				{
					CardWithMostImprovement = CardWithHighestOuts;
				}
				//IF THERE IS NO CARD CORESPSONDING WITH THE FURTHER POSSIBLE HANDS, PLAYER WILL PICK THE MOST IMPROVEMENT CARD
				else if(CombinationIncurredByTheCard.Count == 0)
				{
					Hands HighestHandType = Evaluator.EvaluateHand(m_Player.Hand);
					int HighestHandLevel = Evaluator.DetermineHandLevelINT(m_Player.Hand.ToArray());
					
					for(int CardIndex = 0; CardIndex < m_GMInstance.Table.Pool.Count; CardIndex++)
					{
						for(int HandCardIndex = 0; HandCardIndex < m_Player.Hand.Count; HandCardIndex++)
						{
							Card[] PossibleHand = m_Player.Hand.ToArray();
							PossibleHand[HandCardIndex] = m_GMInstance.Table.Pool[CardIndex];
							
							Hands PossibleHandType = Evaluator.EvaluateHand(PossibleHand);
							int PossibleHandLevel = Evaluator.DetermineHandLevelINT(PossibleHand);
							
							if(PossibleHandType > HighestHandType)
							{
								HighestHandType = PossibleHandType;
								HighestHandLevel = PossibleHandLevel;
								CardWithMostImprovement = m_GMInstance.Table.Pool[CardIndex];
							}
							else if(PossibleHandType == HighestHandType && PossibleHandLevel > HighestHandLevel)
							{
								HighestHandType = PossibleHandType;
								HighestHandLevel = PossibleHandLevel;
								CardWithMostImprovement = m_GMInstance.Table.Pool[CardIndex];
							}
						}
					}
					
					m_HandImproveRate = (HighestHandType - Evaluator.EvaluateHand(m_Player.Hand)) * (HighestHandLevel/2);
				}
			}
			else if(Skill < 7)
			{
				//GO THROUGH ALL THE CARDS IN THE POOL
				// IF LOW/AVERAGE SKILL LEVEL, ONCE AGIAN DETERMINE WHICH CARD WILL BRING THE MOST IMPROVEMENT TO THE PLAYER'S HANDS IN THE MOEMENT
				Hands HighestHandType = Evaluator.EvaluateHand(m_Player.Hand);
				int HighestHandLevel = Evaluator.DetermineHandLevelINT(m_Player.Hand.ToArray());
				
				for(int CardIndex = 0; CardIndex < m_GMInstance.Table.Pool.Count; CardIndex++)
				{
					for(int HandCardIndex = 0; HandCardIndex < m_Player.Hand.Count; HandCardIndex++)
					{
						Card[] PossibleHand = m_Player.Hand.ToArray();
						PossibleHand[HandCardIndex] = m_GMInstance.Table.Pool[CardIndex];
						
						Hands PossibleHandType = Evaluator.EvaluateHand(PossibleHand);
						int PossibleHandLevel = Evaluator.DetermineHandLevelINT(PossibleHand);
						
						if(PossibleHandType > HighestHandType)
						{
							HighestHandType = PossibleHandType;
							HighestHandLevel = PossibleHandLevel;
							CardWithMostImprovement = m_GMInstance.Table.Pool[CardIndex];
						}
						else if(PossibleHandType == HighestHandType && PossibleHandLevel > HighestHandLevel)
						{
							HighestHandType = PossibleHandType;
							HighestHandLevel = PossibleHandLevel;
							CardWithMostImprovement = m_GMInstance.Table.Pool[CardIndex];
						}
					}
				}
				
				m_HandImproveRate = (HighestHandType - Evaluator.EvaluateHand(m_Player.Hand)) * (HighestHandLevel/2);
			}
			
			//			if(CardWithMostImprovement == 
			
			//DETERMINE WHETHER PLAYER SHOULD ATTEMPT TO BID FOR THAT CARD
			//FACTOR. CONTESTABILITY OF THE CARD, AGGRESSIVENESS OF THE PLAYER, DOES IT COMPLETE A FURTHER POSSIBLE HAND (HIGH SKILL LEVEL)
			//HOW MUCH IMPROVEMENT IT MADE TO CURRENT HAND (LOW SKIL LEVEL & HIGH SKIL LEVEL), CURRENT STACK LEVEL
			float DesirabilityToBid = 0.0f;
			
			Card[] PoolCards = m_GMInstance.Table.Pool.ToArray();
			PoolCards = Utility.SortHandByCards(PoolCards,true);
			
			int CardRanking = 0;
			
			for(int CardIndex = 0; CardIndex < PoolCards.Length; CardIndex++)
			{
				if(Utility.IsTwoCardsIdentical(CardWithMostImprovement,PoolCards[CardIndex]))
				{
					CardRanking = CardIndex + 1;
					break;
				}
			}
			
			if(CardRanking == 5)
				DesirabilityToBid += 25.0f;
			
			else if(CardRanking == 4)
				DesirabilityToBid += 20.0f;
			
			else if(CardRanking == 3)
				DesirabilityToBid += 15.0f;
			
			else if(CardRanking == 2)
				DesirabilityToBid += 10.0f;
			
			else if(CardRanking == 1)
				DesirabilityToBid += 5.0f;
			
			if(Aggressiveness > 5)
				DesirabilityToBid += ((float) Aggressiveness - 5.0f)/5.0f * 10.0f;
			
			StackSizing CurrentStackSize = Evaluator.DeterminePlayerStackSize(m_Player);
			
			if(CurrentStackSize == StackSizing.Short)
				DesirabilityToBid -= 10.0f;
			
			else if(CurrentStackSize == StackSizing.Medium || CurrentStackSize == StackSizing.Deep)
				DesirabilityToBid += 5.0f;
			
			if(Skill >= 7)
				DesirabilityToBid += CombinationIncurredByTheCard.Count * 0.5f;
			
			else if(Skill < 7)
				DesirabilityToBid += m_HandImproveRate * 0.5f;
			
			if(DesirabilityToBid > 50.0f)
			{
				PlayerInstance.SelectedCardForAuction = CardWithMostImprovement;
				return AuctionDecision.NULL;
			}
			
			return AuctionDecision.Forfeit;
		}
		
		return AuctionDecision.NULL;
	} 
	
	public AuctionDecision DeterminePlayerAuctionBetting()
	{
		//		if(Memory.Board.Count >= 1)
		//		{
		//			AuctionDecision Decision = Memory.ExecuteAuctionPlays();
		//			return Decision;
		//		}
		
		if(PlayerInstance.GManager.AuctionPhase == AuctionPhase.First)
		{
			//IF THERE ISNT PRICE LIMIT AT WHICH THE PLAYER WILL BET FOR THE TWO CARDS, DETERMINE PRICE LIMIT FOR TWO CARDS
			//THE PRICE LIMIT WILL BE DETERMINED BASED ON STACK TO POT RATIO THAT CAN BE CALCULATED BY DIVIDING THE EFFECTIVE STACK SIZE AT THAT POINT OVER THE SIZE OF THE POT
			RefreshBiddingLimit();
			Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " bidding limit: 1) " + m_PriceLimitForBidding[0] + " 2) " + m_PriceLimitForBidding[1]);
			
			//CALCULATE A RATIO AGAINST THE PRICE LIMIT TO DETERMINE THE ACTUAL AMOUNT OF MONEY TO BID THE FIRST CARD
			//NOTE. THE RANKING OF THE CARD IN THE POOL (VALUE AND SUIT BASED), AGGRESSIVENESS will increase the amount of bid for the first card
			int CardRank = m_GMInstance.Auction.DetermineRankInPool(PlayerInstance.SelectedCardForAuction);
			//			float RankWeight = CardRank/5.0f;
			
			float AggressivenessWeight = 0.0f;
			if(Aggressiveness > 5)
				AggressivenessWeight = (float) Aggressiveness - 5.0f;
			else if(Aggressiveness < 5)
				AggressivenessWeight = 5.0f - (float) Aggressiveness;
			
			float BiddingAmount = CardRank * 0.25f + AggressivenessWeight * 0.05f;
			BiddingAmount = Mathf.Clamp(BiddingAmount,0.0f, m_PriceLimitForBidding[0]);
			
			PlayerInstance.MoneyUsedToBeBid = (int) BiddingAmount;
			
			return AuctionDecision.Bid;
		}
		else if(PlayerInstance.GManager.AuctionPhase == AuctionPhase.Second)
		{
			RefreshBiddingLimit();
			Debug.Log("Enemy " + (PlayerInstance.Index - 1) + " bidding limit: 1) " + m_PriceLimitForBidding[0] + " 2) " + m_PriceLimitForBidding[1]);
			
			//CALCULATE A RATIO AGAINST THE PRICE LIMIT TO DETERMINE THE ACTUAL AMOUNT OF MONEY TO BID THE FIRST CARD
			//NOTE. THE RANKING OF THE CARD IN THE POOL (VALUE AND SUIT BASED), AGGRESSIVENESS will increase the amount of bid for the first card
			int CardRank = m_GMInstance.Auction.DetermineRankInPool(PlayerInstance.SelectedCardForAuction);
			//			float RankWeight = CardRank/5.0f;
			Debug.Log("Rank of selected card: " + CardRank);
			
			float AggressivenessWeight = 0.0f;
			if(Aggressiveness > 5)
				AggressivenessWeight = (float) Aggressiveness - 5.0f;
			else if(Aggressiveness < 5)
				AggressivenessWeight = 5.0f - (float) Aggressiveness;
			Debug.Log("Aggressiveness factor: " + AggressivenessWeight);
			
			float BiddingAmount = CardRank * 0.25f + AggressivenessWeight * 0.05f;
			BiddingAmount = Mathf.Clamp(BiddingAmount,0.0f, m_PriceLimitForBidding[1]);
			Debug.Log("Bidding Amount: " + BiddingAmount);
			
			PlayerInstance.MoneyUsedToBeBid = (int) BiddingAmount;
			
			return AuctionDecision.Bid;
		}
		
		return AuctionDecision.Forfeit;
	}
	
	public bool DeterminePlayerPurchasingCard()
	{
		if(PlayerInstance.Stack < m_GMInstance.Auction.CostOfRandomCard)
		{
			return false;
		}
		
		//IF PLAYER HAS LOW/AVERAGE SKILL LEVEL
		if(Skill < 7)
		{
			float DesirabilityToPurchase = 0.0f;
			
			//RANDOM CHANCE OF PURCHASING OR NOT
			if(Random.Range(0,1) == 1)
				DesirabilityToPurchase += 50.0f;
			
			//FACTORS LIKE AGGRESSIVENESS AND STACK LEVEL WILL AFFECT THE RANDOM CHANCE
			StackSizing PlayerStackSize = Evaluator.DeterminePlayerStackSize(PlayerInstance);
			
			if(PlayerStackSize == StackSizing.Deep)
				DesirabilityToPurchase += 10.0f;
			else
				DesirabilityToPurchase -= 10.0f;
			
			if(Aggressiveness > 5)
				DesirabilityToPurchase += ((float) Aggressiveness - 5.0f)/5.0f * 10.0f;
			
			else if(Aggressiveness < 5)
				DesirabilityToPurchase -= ((float) Aggressiveness - 5.0f)/5.0f * 10.0f;
			
			if(DesirabilityToPurchase >= 50.0f)
				return true;
			
			else
				return false;
		}
		//ELSE IF PLAYER HAS HIGH SKILL LEVEL
		else if(Skill >= 7)
		{
			//CHECK WHETHER PLAYER'S CURRENT HAND STILL HAVE WHAT KIND OF IMPROVEMENT POTENTIAL POSSIBLE WITH JUST ONE MORE CARD
			for(int CombinationIndex = 0; CombinationIndex < PossibleCombinations.Count; CombinationIndex++)
			{
				if(PossibleCombinations[CombinationIndex].Length < 3)
					PossibleCombinations.Remove(PossibleCombinations[CombinationIndex]);
			}
			
			if(PossibleCombinations.Count <= 0)
				return false;
			
			List<Card> CardsThatWillImproveHand = new List<Card>();
			
			for(int CombinationIndex = 0; CombinationIndex < PossibleCombinations.Count; CombinationIndex++)
			{
				for(int CardIndex = 0; CardIndex < PossibleCombinations[CombinationIndex].Length; CardIndex++)
				{
					bool CardIsUnique = true;
					
					for(int PocketCardIndex = 0; PocketCardIndex < m_Player.Pocket.Count; PocketCardIndex++)
					{
						if(Utility.IsTwoCardsIdentical(PossibleCombinations[CombinationIndex][CardIndex],m_Player.Pocket[PocketCardIndex]))
						{	
							CardIsUnique = false;
							break;
						}
					}
					
					if(!CardIsUnique)
						break;
					
					for(int HandCardIndex = 0; HandCardIndex < m_Player.Hand.Count; HandCardIndex++)
					{
						if(Utility.IsTwoCardsIdentical(PossibleCombinations[CombinationIndex][CardIndex],m_Player.Hand[HandCardIndex]))
						{
							CardIsUnique = false;
							break;
						}
					}
					
					if(CardIsUnique)
						CardsThatWillImproveHand.Add(PossibleCombinations[CombinationIndex][CardIndex]);
				}
			}
			
			//CALCULATE THE PROBABILITY OF DRAWING A CARD OF THAT POTENTIAL 
			float ProbabilityOfDrawingOutCard = (float) CardsThatWillImproveHand.Count/(float) m_GMInstance.Deck.Cards.Count * 100.0f;
			
			//IF THAT PROBABILITY IS OF A CERTAIN LEVEL, THE PLAYER WILL PURCHASE A CARD. ELSE, FORFEIT
			if(ProbabilityOfDrawingOutCard >= 30.0f)
				return true;
			else
				return false;
		}
		
		return false;
	}
	
	public void SortOutHand()
	{
		Card[] CurrentHand   = m_Player.Hand.ToArray();
		Card[] CurrentPocket = m_Player.Pocket.ToArray();
		Card[] HighestHand   = (Card[]) CurrentHand.Clone();
		int HighestHandValue = DecisionReference.GetValueOfCurrentHand(new List<Card> (CurrentHand));
		
		for(int PocketSlot = 0; PocketSlot < CurrentPocket.Length; PocketSlot++)
		{
			Card PocketCard = CurrentPocket[PocketSlot];
			
			for(int HandSlot = 0; HandSlot < CurrentHand.Length; HandSlot++)
			{
				Card[] PossibleHand    = (Card[]) CurrentHand.Clone();
				PossibleHand[HandSlot] = PocketCard;
				
				int PossibleHandValue = DecisionReference.GetValueOfCurrentHand(new List<Card> (PossibleHand));
				
				if(PossibleHandValue > HighestHandValue)
				{
					HighestHand      = (Card[]) PossibleHand.Clone();
					HighestHandValue = DecisionReference.GetValueOfCurrentHand(new List<Card> (HighestHand));
				}
				else if(PossibleHandValue == HighestHandValue)
				{
					int PossibleHandScore = 0;
					for(int PossibleHandSlot = 0; PossibleHandSlot < PossibleHand.Length; PossibleHandSlot++)
					{
						PossibleHandScore += (int) PossibleHand[PossibleHandSlot].Suit + (int) PossibleHand[PossibleHandSlot].Value;
					}
					
					int HighestHandScore = 0;
					for(int HighestHandSlot = 0; HighestHandSlot < HighestHand.Length; HighestHandSlot++)
					{
						HighestHandScore += (int) HighestHand[HighestHandSlot].Suit + (int) HighestHand[HighestHandSlot].Value;
					}
					
					if(PossibleHandScore > HighestHandScore)
					{
						HighestHand      = (Card[]) PossibleHand.Clone();
						HighestHandValue = DecisionReference.GetValueOfCurrentHand(new List<Card> (HighestHand));
					}
				}
			}
			
			for(int HandSlot = 0; HandSlot < CurrentHand.Length; HandSlot++)
			{
				if(HighestHand[HandSlot].Suit != CurrentHand[HandSlot].Suit || HighestHand[HandSlot].Value != CurrentHand[HandSlot].Value)
				{
					for(int SubPocketSlot = 0; SubPocketSlot < CurrentPocket.Length; SubPocketSlot++)
					{
						if(HighestHand[HandSlot].Suit == CurrentPocket[SubPocketSlot].Suit && HighestHand[HandSlot].Value == CurrentPocket[SubPocketSlot].Value)
						{
							Card CardSwapToPocket = CurrentHand[HandSlot];
							
							CurrentHand[HandSlot]     = CurrentPocket[SubPocketSlot];
							CurrentPocket[SubPocketSlot] = CardSwapToPocket;
						}
					}
				}
			}
		}
		
		m_Player.Hand   = new List<Card>(CurrentHand);
		m_Player.Pocket = new List<Card>(CurrentPocket);
	}
	
	public float CalculateMoneyToBeBid()
	{
		return 0.0f;
	}
	
	public void RefreshBiddingLimit()
	{
		if(m_PriceLimitForBidding[0] == 0.0f && m_PriceLimitForBidding[1] == 0.0f)
		{
			float StackToPot = m_GMInstance.Table.GetEffectiveStackSize() / m_GMInstance.Table.Pot;
			if(StackToPot >= 3.5f && StackToPot < 5.5f)
			{
				m_PriceLimitForBidding[0] = 0.75f * m_GMInstance.Table.Pot;
				m_PriceLimitForBidding[1] = 0.75f * m_GMInstance.Table.Pot;
			}
			else if(StackToPot >= 5.5f && StackToPot < 7.0f)
			{
				m_PriceLimitForBidding[0] = 1.25f * m_GMInstance.Table.Pot;
				m_PriceLimitForBidding[1] = 0.75f * m_GMInstance.Table.Pot;
			}
			else if(StackToPot >= 7.0f && StackToPot < 9.0f)
			{
				m_PriceLimitForBidding[0] = 1.125f * m_GMInstance.Table.Pot;
				m_PriceLimitForBidding[1] = 1.125f * m_GMInstance.Table.Pot;
			}
			else if(StackToPot >= 9.0f && StackToPot < 13.0f)
			{
				m_PriceLimitForBidding[0] = 1.375f * m_GMInstance.Table.Pot;
				m_PriceLimitForBidding[1] = 1.125f * m_GMInstance.Table.Pot;
			}
			else if(StackToPot >= 13.0f)
			{
				m_PriceLimitForBidding[0] = 1.5f * m_GMInstance.Table.Pot;
				m_PriceLimitForBidding[1] = 1.5f * m_GMInstance.Table.Pot;
			}
		}
	}
}