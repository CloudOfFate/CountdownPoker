using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMemory
{
	private Enemy        EnemyInstance;
	private GameManager  GMInstance;
	private CommonMemory CommonMemoryInstance;
	private Plays        CurrentPlay;
	private Phase        CurrentPhase;

	private Dictionary<string,Play> EnemyPlays;

	private List<HandLogEntry>[]  HandLogs; 
	private List<AggroLogEntry>[] AggroLogs; 
	private List<PlayLogEntry>[]  OpponentPlaysLogs; 

	private List<PlayLogEntry> PersonalPlaysLogs;

	private Queue<BoardEntry> Blackboard;

	private Dictionary<int,int> TightnessScores;
	private List<int> TightnessScoreKeys;

	private int[] OpponentsTightnessScore;
	private int[] OpponentsAggressivenessScore;
	private int[] OpponentsSkillLevelScore;

	public Queue<BoardEntry> Board {get{return Blackboard;} set{Blackboard = value;}}
	public List<PlayLogEntry>[] OpponentPlayLog {get{return OpponentPlaysLogs;} set{OpponentPlaysLogs = value;}}
	public int[] OpponentTightness {get{return OpponentsTightnessScore;}}
	public int[] OpponentsAggressiveness {get{return OpponentsAggressivenessScore;}}
	public int[] OpponentsSkillLevel {get{return OpponentsSkillLevelScore;}}

	private enum Phase {One,Two,Three,NULL};

	#region Initialization of Structs
	public struct HandLogEntry
	{
		public int RoundNum;
		public int PlayerIndex;
		 
		//The following 3 Hand data will be based on the opponent's pre-sorted hand
		public Hands HandType;
		public Values HighestValue;
		public Values LowestValue;

		public Card[] HandCards;
		public Card[] BiddedCards;
	}

	public struct AggroLogEntry
	{
		public int RoundNum;
		public int PlayerIndex;

		public int BetPlaced;

		public int NumOfRaises;
		public int HighestRaise;
		public int LowestRaise;

		public int NumOfCall;
		public int HighestCall;
		public int LowestCall;

		public int NumOfCheck;

		public bool Folded;
	}

	public struct PlayLogEntry
	{
		public int RoundNum;
		public int PlayerIndex;

		public Plays PlayMade;
		public Card[] Hand;
	}

	public struct BoardEntry
	{
		public int RoundIndex;

		public Card[] Hand;
		public Card[] Pocket;

		public PlayerAction ActionToBeMade;
	}
	#endregion

	public EnemyMemory(Enemy _Enemy)
	{
		EnemyInstance        = _Enemy;
		GMInstance           = EnemyInstance.PlayerInstance.GManager;
		CommonMemoryInstance = GMInstance.CommonMemoryLog;
		CurrentPlay          = Plays.NULL;
		CurrentPhase         = Phase.NULL;

		EnemyPlays = new Dictionary<string, Play>();
		EnemyPlays.Add("Bluffing",new TotalBluffPlay(EnemyInstance));
		EnemyPlays.Add("SemiBluffing",new SemiBluffPlay(EnemyInstance));
		EnemyPlays.Add("CheckRaise",new CheckRaisingPlay(EnemyInstance));
		EnemyPlays.Add("SetMining",new SetMiningPlay(EnemyInstance));

		HandLogs  = new List<HandLogEntry>[GMInstance.Players.Length];
		AggroLogs = new List<AggroLogEntry>[GMInstance.Players.Length];
		OpponentPlaysLogs  = new List<PlayLogEntry>[GMInstance.Players.Length];

		PersonalPlaysLogs = new List<PlayLogEntry>();

		Blackboard = new Queue<BoardEntry>();

		TightnessScores = new Dictionary<int, int>();
		TightnessScores.Add(0,0);
		TightnessScores.Add(2,0);
		TightnessScores.Add(4,0);
		TightnessScores.Add(6,0);
		TightnessScores.Add(8,0);
		TightnessScores.Add(10,0);

		TightnessScoreKeys = new List<int>(TightnessScores.Keys);

		OpponentsTightnessScore      = new int[GMInstance.Players.Length];
		OpponentsAggressivenessScore = new int[GMInstance.Players.Length];
		OpponentsSkillLevelScore     = new int[GMInstance.Players.Length];

		for(int OppoIndex = 0; OppoIndex < GMInstance.Players.Length; OppoIndex++)
		{
			OpponentsTightnessScore[OppoIndex]      = 5;
			OpponentsAggressivenessScore[OppoIndex] = 5;
			OpponentsSkillLevelScore[OppoIndex]     = 3;
		}
	}

	public void LogOpponentHand(Player _Opponent)
	{
		HandLogEntry NewEntry = new HandLogEntry();

		NewEntry.RoundNum    = GMInstance.CurrentRoundIndex;
		NewEntry.PlayerIndex = _Opponent.Index - 1;

		List<Card> OppoHand = new List<Card>(_Opponent.PreSwapHand);

		NewEntry.HandType    = Evaluator.EvaluateHand(OppoHand);

		Values HighestValue = OppoHand[0].Value;
		Values LowestValue  = OppoHand[0].Value;

		for(int HandIndex = 0; HandIndex < OppoHand.Count; HandIndex++)
		{
			if(OppoHand[HandIndex].Value > HighestValue)
				HighestValue = OppoHand[HandIndex].Value;

			if(OppoHand[HandIndex].Value < LowestValue)
				LowestValue = OppoHand[HandIndex].Value;
		}

		NewEntry.HighestValue = HighestValue;
		NewEntry.LowestValue  = LowestValue;

		NewEntry.HandCards = _Opponent.PreSwapHand;

		Card[] CardsBidded = new Card[3];

		if(_Opponent.Pocket.Count >= 1)
		{
			Card[] CardsGotten      = _Opponent.CardsAuctioned.ToArray();
			int AmountOfBiddedCards = CardsGotten.Length;

			if(_Opponent.FinishPurchasing)
				AmountOfBiddedCards -= 1;

			CardsBidded = new Card[AmountOfBiddedCards];
			
			for(int CardIndex = 0; CardIndex < AmountOfBiddedCards; CardIndex++)
				CardsBidded[CardIndex] = CardsGotten[CardIndex];
		}

		NewEntry.BiddedCards = CardsBidded;

		HandLogs[_Opponent.Index - 1].Add(NewEntry);
	}

	public void LogOpponentAggro(Player _Opponent)
	{
		AggroLogEntry NewEntry = new AggroLogEntry();

		NewEntry.RoundNum    = GMInstance.CurrentRoundIndex;
		NewEntry.PlayerIndex = _Opponent.Index - 1;
		NewEntry.BetPlaced = GMInstance.Betting.CurrentBet;

		int RaiseCount = 0;
		int MaxRaise   = 0;
		int MinRaise   = 99999;

		int CallCount = 0;
		int MaxCall   = 0;
		int MinCall   = 99999;

		int CheckCount = 0;

		bool FoldCheck = false;

		for(int LogIndex = 0; LogIndex < CommonMemoryInstance.MemoryLog.Count; LogIndex++)
		{
			if(CommonMemoryInstance.MemoryLog[LogIndex].RoundNum == NewEntry.RoundNum && CommonMemoryInstance.MemoryLog[LogIndex].PlayerIndex == NewEntry.PlayerIndex)
			{
				if(CommonMemoryInstance.MemoryLog[LogIndex].P_Action == PlayerAction.Raise)
				{
					RaiseCount++;

					if(CommonMemoryInstance.MemoryLog[LogIndex].MoneyUsedForAction > MaxRaise)
						MaxRaise = CommonMemoryInstance.MemoryLog[LogIndex].MoneyUsedForAction;

					if(CommonMemoryInstance.MemoryLog[LogIndex].MoneyUsedForAction < MinRaise)
						MinRaise = CommonMemoryInstance.MemoryLog[LogIndex].MoneyUsedForAction;
				}
				else if(CommonMemoryInstance.MemoryLog[LogIndex].P_Action == PlayerAction.Call)
				{
					CallCount++;

					if(CommonMemoryInstance.MemoryLog[LogIndex].MoneyUsedForAction > MaxCall)
						MaxCall = CommonMemoryInstance.MemoryLog[LogIndex].MoneyUsedForAction;

					if(CommonMemoryInstance.MemoryLog[LogIndex].MoneyUsedForAction < MinCall)
						MinCall = CommonMemoryInstance.MemoryLog[LogIndex].MoneyUsedForAction;
				}
				else if(CommonMemoryInstance.MemoryLog[LogIndex].P_Action == PlayerAction.Check)
				{
					RaiseCount++;
				}
				else if(CommonMemoryInstance.MemoryLog[LogIndex].P_Action == PlayerAction.Fold)
				{
					FoldCheck = true;
					break;
				}
			}
		}

		NewEntry.NumOfRaises  = RaiseCount;
		NewEntry.HighestRaise = MaxRaise;
		NewEntry.LowestRaise  = MinRaise;

		NewEntry.NumOfCall   = CallCount;
		NewEntry.HighestCall = MaxCall;
		NewEntry.LowestCall  = MinCall;

		NewEntry.NumOfCheck  = CheckCount;

		NewEntry.Folded = FoldCheck;

		AggroLogs[_Opponent.Index - 1].Add(NewEntry);
	}

	public void LogOpponentPlay(Player _Opponent, Plays _PlayMade)
	{
		PlayLogEntry NewEntry = new PlayLogEntry();

		NewEntry.RoundNum = GMInstance.CurrentRoundIndex;
		NewEntry.PlayerIndex = _Opponent.Index - 1;

		NewEntry.PlayMade = _PlayMade; //NOTE. NEED TO GET A WAY FOR ENEMY TO DETECT THE OPPONENT PLAY AND STORE THE TYPE OF PLAY MADE SOMEWHERE BEFORE THIS LOGGING PROCESS

		NewEntry.Hand = _Opponent.Hand.ToArray();

		OpponentPlaysLogs[_Opponent.Index - 1].Add(NewEntry);
	}

	//NOTE. ADJUSTMENT FOR EVALUATED SKILL SCORE AND AGGRESSIVENESS SCORE IS NOT DONE YET
	public void RefreshOpponentsScore()
	{
		#region Adjust the evaluated tightness score for all opponents
		for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
		{
			//If the player that is being iterated is this player, skip and go to the next player
			if(PlayerIndex == EnemyInstance.PlayerInstance.Index - 1)
				continue;
			
			//Reset the Tightness score template to recalculate the score
			foreach(int Key in TightnessScoreKeys)
				TightnessScores[Key] = 0;

			#region Go through each of the specific opponent's hand that they continue their bet with, determine around where is their main betting range. 
			for(int HandLogIndex = 0; HandLogIndex < HandLogs[PlayerIndex].Count; HandLogIndex++)
			{
				if(HandLogs[PlayerIndex][HandLogIndex].HandType == Hands.HighCard)
				{
					if((int) HandLogs[PlayerIndex][HandLogIndex].HighestValue >= 9)
						TightnessScores[4]++;
					else if((int) HandLogs[PlayerIndex][HandLogIndex].HighestValue >= 6)
						TightnessScores[2]++;
					else 
						TightnessScores[0]++;	
				}
				else if(HandLogs[PlayerIndex][HandLogIndex].HandType == Hands.OnePair)
				{
					Values PairValue = Values.Ace;
					Card[] Hand = HandLogs[PlayerIndex][HandLogIndex].HandCards;

					for(int a = 0; a < Hand.Length; a++)
					{
						for(int b = 0; b < Hand.Length; b++)
						{
							if(Hand[b] != Hand[a] && Hand[b].Value == Hand[a].Value)
							{
								PairValue = Hand[a].Value;
								break;
							}
						}
					}

					if((int) PairValue >= 12)
						TightnessScores[10]++;
					else if((int) PairValue >= 9)
						TightnessScores[8]++;
					else if((int) PairValue >= 4)
						TightnessScores[6]++;
					else
						TightnessScores[4]++;
				}
				else
				{
					TightnessScores[10]++;
				}
			}
			#endregion

			int MostBetRange = 0;
			foreach(int Key in TightnessScoreKeys)
			{
				if(TightnessScores[Key] > TightnessScores[MostBetRange])
					MostBetRange = Key;
			}

			OpponentsTightnessScore[PlayerIndex] = MostBetRange;
		}
		#endregion

		#region Adjust the evaluated Aggressiveness score for all oppponents
		for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
		{
			//If the player that is being iterated is this player, skip and go to the next player
			if(PlayerIndex == EnemyInstance.PlayerInstance.Index - 1)
				continue;

			
		}
		#endregion

		#region Adjust the evaluated Skill score for all opponents
		for(int PlayerIndex = 0; PlayerIndex < GMInstance.Players.Length; PlayerIndex++)
		{
			//If the player that is being iterated is this player, skip and go to the next player
			if(PlayerIndex == EnemyInstance.PlayerInstance.Index - 1)
				continue;

			
		}
		#endregion
	}

	public void LogPersonalPlay(Plays _PlayMade)
	{
		PlayLogEntry NewEntry = new PlayLogEntry();

		NewEntry.RoundNum = GMInstance.CurrentRoundIndex;
		NewEntry.PlayerIndex = EnemyInstance.PlayerInstance.Index - 1;

		NewEntry.PlayMade = _PlayMade;
		NewEntry.Hand = EnemyInstance.PlayerInstance.Hand.ToArray();

		PersonalPlaysLogs.Add(NewEntry);
	}

	public void RecordBlackboardEntry(PlayerAction _Action)
	{
		BoardEntry NewEntry = new BoardEntry();
		NewEntry.RoundIndex = GMInstance.CurrentRoundIndex;
		NewEntry.Hand = EnemyInstance.PlayerInstance.Hand.ToArray();
		NewEntry.Pocket = EnemyInstance.PlayerInstance.Pocket.ToArray();
		NewEntry.ActionToBeMade = _Action;

		Board.Enqueue(NewEntry);
	}

	public void CheckApplicabilityOfPlays()
	{
		float HighestApplicability = 0.0f;
		Play HighestPlay = EnemyPlays["SemiBluffing"];

		List<string> Keys = new List<string>(EnemyPlays.Keys);

//		Debug.Log("Will Enemy " + (EnemyInstance.PlayerInstance.Index - 1) + " make a play ?");

		foreach(string Key in Keys)
		{
			float ApplicabilityOfPlay = EnemyPlays[Key].ApplicabilityOfPlay();

//			Debug.Log(Key + "'s applicability : " + ApplicabilityOfPlay);

			if(ApplicabilityOfPlay > HighestApplicability)
			{
				HighestApplicability = ApplicabilityOfPlay;
				HighestPlay = EnemyPlays[Key];
			}
		}

		if(HighestApplicability >= 50.0f)
		{
			BoardEntry NewEntry = new BoardEntry();
			NewEntry.RoundIndex = GMInstance.CurrentRoundIndex;
			NewEntry.Hand = EnemyInstance.PlayerInstance.Hand.ToArray();
			NewEntry.Pocket = EnemyInstance.PlayerInstance.Pocket.ToArray();
			NewEntry.ActionToBeMade = HighestPlay.CorrespondingAction;

			Board.Enqueue(NewEntry);

//			Debug.Log("Enemy " + (EnemyInstance.PlayerInstance.Index - 1) + " made a play !");
		}
	}

	public BettingDecision ExecuteBetPlays()
	{
		if(Board.Peek().ActionToBeMade == PlayerAction.Bluffing)
			EnemyPlays["Bluffing"].ExecuteBetPlay();

		else if(Board.Peek().ActionToBeMade == PlayerAction.SemiBluffing)
			EnemyPlays["SemiBluffing"].ExecuteBetPlay();

		else if(Board.Peek().ActionToBeMade == PlayerAction.SetMining)
			EnemyPlays["SetMining"].ExecuteBetPlay();

		else if(Board.Peek().ActionToBeMade == PlayerAction.CheckRaise)
			EnemyPlays["CheckRaise"].ExecuteBetPlay();

		return BettingDecision.NULL;
	}

	public AuctionDecision ExecuteAuctionPlays()
	{
		if(Board.Peek().ActionToBeMade == PlayerAction.Bluffing)
			EnemyPlays["Bluffing"].ExecuteAuctionPlay();
		
		else if(Board.Peek().ActionToBeMade == PlayerAction.SemiBluffing)
			EnemyPlays["SemiBluffing"].ExecuteAuctionPlay();
		
		else if(Board.Peek().ActionToBeMade == PlayerAction.SetMining)
			EnemyPlays["SetMining"].ExecuteAuctionPlay();
		
		else if(Board.Peek().ActionToBeMade == PlayerAction.CheckRaise)
			EnemyPlays["CheckRaise"].ExecuteAuctionPlay();
		
		return AuctionDecision.NULL;
	}
}