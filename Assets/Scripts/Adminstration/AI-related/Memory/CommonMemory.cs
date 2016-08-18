using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonMemory 
{
	private GameManager GM;

	private int AmountOfRaiseInBet;
	
	private List<MemoryEntry> MemoryLogs;
	private List<AuctionEntry> AuctionLogs;

	public int RaiseInBet {get{return AmountOfRaiseInBet;} set{AmountOfRaiseInBet = value;}}
	public List<MemoryEntry> MemoryLog{get{return MemoryLogs;}}
	public List<AuctionEntry> AuctionLog{get{return AuctionLogs;}}

	public struct MemoryEntry
	{
		public int RoundNum;
		public int TurnNum;
		public TurnPhase T_Phase;
		public AuctionPhase A_Phase;
		public int PlayerIndex;
		public PlayerAction P_Action;
		public int MoneyUsedForAction;
		public Card CardInQuestion;
		public string Comment;
		public int RaiseMadeInRound;
	}

	public struct AuctionEntry
	{
		public int RoundNum;
		public AuctionPhase A_Phase;
		public Card[] CardBeingAuctioned;
	}

	public CommonMemory(GameManager _GM)
	{
		GM = _GM;
		AmountOfRaiseInBet = 0;

		MemoryLogs = new List<MemoryEntry>();
		AuctionLogs = new List<AuctionEntry>();
	}

	public void LogMemory(int _PlayerIndex,PlayerAction _ActionMade, int _MoneyUsed, Card _CardUsed, int _RaiseMadeCount, string _Comment)
	{
		MemoryEntry NewEntry = new MemoryEntry();

		NewEntry.RoundNum = GM.CurrentRoundIndex;
		NewEntry.TurnNum = GM.CurrentTurnIndex;
		NewEntry.T_Phase = GM.Phase;
		NewEntry.A_Phase = GM.AuctionPhase;
		NewEntry.PlayerIndex = _PlayerIndex;
		NewEntry.P_Action = _ActionMade;
		NewEntry.MoneyUsedForAction = _MoneyUsed;
		NewEntry.RaiseMadeInRound = _RaiseMadeCount;

		if(NewEntry.P_Action == PlayerAction.Raise)
			AmountOfRaiseInBet++;

		if(_CardUsed != null)
			NewEntry.CardInQuestion = _CardUsed;

		NewEntry.Comment = _Comment;

		MemoryLogs.Add(NewEntry);

//		if(NewEntry.PlayerIndex == 1)
//			Debug.Log("Log Player's action of " + _ActionMade);
//		else
//			Debug.Log("Log Enemy " + _PlayerIndex + "'s action of " + _ActionMade);
	}

	public MemoryEntry ExtractPastMemoryLog (int _TurnsAgo)
	{
		return MemoryLogs[MemoryLogs.Count - _TurnsAgo];
	}

	public bool HasAnyPlayerEnterPot()
	{
		for(int EntryIndex = MemoryLog.Count - 1; EntryIndex > 0; EntryIndex--)
		{
			if(MemoryLog[EntryIndex].RoundNum == GM.CurrentRoundIndex && MemoryLog[EntryIndex].P_Action == PlayerAction.PayLargeBind)
			{
				for(int SearchIndex = EntryIndex + 1; SearchIndex < MemoryLog.Count; SearchIndex++)
				{
					if(MemoryLog[SearchIndex].RoundNum == GM.CurrentRoundIndex && (MemoryLog[SearchIndex].P_Action == PlayerAction.Bet || MemoryLog[SearchIndex].P_Action == PlayerAction.Call || MemoryLog[SearchIndex].P_Action == PlayerAction.Raise))
						return true;
					else if(MemoryLog[SearchIndex].RoundNum > GM.CurrentRoundIndex)
						break;
				}

				break;
			}
		}

		return false;
	}

	public void LogAuction()
	{
		AuctionEntry NewEntry = new AuctionEntry();
		NewEntry.RoundNum = GM.CurrentRoundIndex;
		NewEntry.A_Phase = GM.AuctionPhase;

		Card[] CardOnTable = new Card[GM.Table.Pool.Count];
		for(int CardIndex = 0; CardIndex < CardOnTable.Length; CardIndex++)
		{
			CardOnTable[CardIndex] = GM.Table.Pool[CardIndex];
		}
		NewEntry.CardBeingAuctioned = CardOnTable;

		AuctionLogs.Add(NewEntry);
	}

	public AuctionEntry ExtractPastAuctionLog (int _TurnsAgo)
	{
		return AuctionLogs[AuctionLogs.Count - _TurnsAgo];
	}

	public void PrintMemoryLog()
	{
		for(int LogIndex = 0; LogIndex < MemoryLogs.Count; LogIndex++)
		{
			Debug.Log("RoundNum: " + MemoryLogs[LogIndex].RoundNum +
					" TurnNum: " + MemoryLogs[LogIndex].TurnNum +
					" T_Phase: " + MemoryLogs[LogIndex].T_Phase +
					" A_Phase: " + MemoryLogs[LogIndex].A_Phase +
					" PlayerIndex: " + MemoryLogs[LogIndex].PlayerIndex +
					" P_Action: " + MemoryLogs[LogIndex].P_Action + 
					" MoneyUsedForAction: " + MemoryLogs[LogIndex].MoneyUsedForAction +
					" CardInQuestion: " + MemoryLogs[LogIndex].CardInQuestion +
					" Comment: " + MemoryLogs[LogIndex].Comment);
		}
	}

	public void PrintAuctionLog()
	{
		for(int LogIndex = 0; LogIndex < AuctionLogs.Count; LogIndex++)
		{
			if(AuctionLogs[LogIndex].CardBeingAuctioned.Length == 5)
			{
				Debug.Log("RoundNum: " + AuctionLogs[LogIndex].RoundNum +
					" TurnNum: " + AuctionLogs[LogIndex].A_Phase +
					" CardsBeingAuctioned: " + 
					AuctionLogs[LogIndex].CardBeingAuctioned[0].Suit + AuctionLogs[LogIndex].CardBeingAuctioned[0].Value + " " +
					AuctionLogs[LogIndex].CardBeingAuctioned[1].Suit + AuctionLogs[LogIndex].CardBeingAuctioned[1].Value + " " +
					AuctionLogs[LogIndex].CardBeingAuctioned[2].Suit + AuctionLogs[LogIndex].CardBeingAuctioned[2].Value + " " +
					AuctionLogs[LogIndex].CardBeingAuctioned[3].Suit + AuctionLogs[LogIndex].CardBeingAuctioned[3].Value + " " +
					AuctionLogs[LogIndex].CardBeingAuctioned[4].Suit + AuctionLogs[LogIndex].CardBeingAuctioned[4].Value );
			}
			else if(AuctionLogs[LogIndex].CardBeingAuctioned.Length == 3)
			{
				Debug.Log("RoundNum: " + AuctionLogs[LogIndex].RoundNum +
					" TurnNum: " + AuctionLogs[LogIndex].A_Phase +
					" CardsBeingAuctioned: " + 
					AuctionLogs[LogIndex].CardBeingAuctioned[0].Suit + AuctionLogs[LogIndex].CardBeingAuctioned[0].Value + " " +
					AuctionLogs[LogIndex].CardBeingAuctioned[1].Suit + AuctionLogs[LogIndex].CardBeingAuctioned[1].Value + " " +
					AuctionLogs[LogIndex].CardBeingAuctioned[2].Suit + AuctionLogs[LogIndex].CardBeingAuctioned[2].Value );
			}
		}
	}
}
