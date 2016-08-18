using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BettingManager 
{
	private GameManager GM;
	private Player m_DealingPlayer;
	private Player m_SmallBindPlayer;
	private Player m_LargeBindPlayer;
	private Player m_FirstPlayer;

	//Provide a reference to the player that had done the latest bet or raise in the current pot
	private Player m_LatestAggressivePlayer; 

	private List<Player> m_Limpers;

	private int fSmallBindValue;
	private int fBigBindValue;
	private int fCurrentBet;
	private int fRaiseMadeCount;

	private float fFirstBetAmount;
	private float fLatestRaiseAmount;

	private bool mb_BetMadeThisRound;

	public Player DealingPlayer   {get{return m_DealingPlayer;} set{m_DealingPlayer = value;}}
	public Player SmallBindPlayer {get{return m_SmallBindPlayer;} set{m_SmallBindPlayer = value;}}
	public Player LargeBindPlayer {get{return m_LargeBindPlayer;} set{m_LargeBindPlayer = value;}}
	public Player FirstPlayer     {get{return m_FirstPlayer;} set{m_FirstPlayer = value;}}

	public Player LatestAggressivePlayer {get{return m_LatestAggressivePlayer;} set{m_LatestAggressivePlayer = value;}}

	public List<Player> Limpers   {get{return m_Limpers;} set{m_Limpers = value;}}

	public int SmallBind          {get{return fSmallBindValue;} set{fSmallBindValue = value;}}
	public int BigBind            {get{return fBigBindValue;} set{fBigBindValue = value;}}
	public int CurrentBet         {get{return fCurrentBet;} set{fCurrentBet = value;}}
	public int RaiseMadeCount     {get{return fRaiseMadeCount;} set{fRaiseMadeCount = value;}}

	public float FirstBetAmount   {get{return fFirstBetAmount;} set{fFirstBetAmount = value;}}
	public float LatestRaiseAmount {get{return fLatestRaiseAmount;} set{fLatestRaiseAmount = value;}}

	public bool BetMadeThisRound  {get{return mb_BetMadeThisRound;} set{mb_BetMadeThisRound = value;}}
	
	public BettingManager(GameManager _GM)
	{
		GM = _GM;
		fSmallBindValue = 5;
		fBigBindValue = 10;

		fFirstBetAmount = 0.0f;
		fLatestRaiseAmount = 0.0f;

		m_Limpers = new List<Player>();

		InitializePlayersDealAndBind();
	}

	public void InitializePlayersDealAndBind()
	{
		int index = Random.Range(0,4);
		m_DealingPlayer = GM.Players[index];
		
		index++;
		if(index > 3){index = 0;}
		
		m_SmallBindPlayer = GM.Players[index];
		
		index++;
		if(index > 3){index = 0;}
		
		m_LargeBindPlayer = GM.Players[index];
		
		index++;
		if(index > 3){index = 0;}
		
		m_FirstPlayer = GM.Players[index];
	}
	
	public void IncreaseBinds()
	{
		fSmallBindValue += 5;
		fBigBindValue += 5;
	}
	
	public void ShiftDealerAndBinds()
	{
		int PlayerAmount = GM.Players.Length - 1;
	
		int NextDealingIndex = m_DealingPlayer.Index;
		if(NextDealingIndex > PlayerAmount){NextDealingIndex = 0;}
		while(GM.Players[NextDealingIndex].Busted)
		{
			NextDealingIndex++;
			if(NextDealingIndex > PlayerAmount){NextDealingIndex = 0;}
		}
		
		int NextSmallBindIndex = NextDealingIndex + 1;
		if(NextSmallBindIndex > PlayerAmount){NextSmallBindIndex = 0;}
		while(GM.Players[NextSmallBindIndex].Busted)
		{
			NextSmallBindIndex++;
			if(NextSmallBindIndex > PlayerAmount){NextSmallBindIndex = 0;}
		}
		
		int NextLargeBindIndex = NextSmallBindIndex + 1;
		if(NextLargeBindIndex > PlayerAmount){NextLargeBindIndex = 0;}
		while(GM.Players[NextLargeBindIndex].Busted)
		{
			NextLargeBindIndex++;
			if(NextLargeBindIndex > PlayerAmount){NextLargeBindIndex = 0;}
		}
		
		int NextFirstPlayerIndex = NextLargeBindIndex + 1;
		if(NextFirstPlayerIndex > PlayerAmount){NextFirstPlayerIndex = 0;}
		while(GM.Players[NextFirstPlayerIndex].Busted)
		{
			NextFirstPlayerIndex++;
			if(NextFirstPlayerIndex > PlayerAmount){NextFirstPlayerIndex = 0;}
		}
		
		m_DealingPlayer = GM.Players[NextDealingIndex];
		m_SmallBindPlayer = GM.Players[NextSmallBindIndex];
		m_LargeBindPlayer = GM.Players[NextLargeBindIndex];
		m_FirstPlayer = GM.Players[NextFirstPlayerIndex];
		
		/*int NextDealingIndex = m_DealingPlayer.Index;
		if(NextDealingIndex > 3){NextDealingIndex = 0;}
		int NextSmallBindIndex = m_SmallBindPlayer.Index;
		if(NextSmallBindIndex > 3){NextSmallBindIndex = 0;}
		int NextLargeBindIndex = m_LargeBindPlayer.Index;
		if(NextLargeBindIndex > 3){NextLargeBindIndex = 0;}
		
		m_DealingPlayer = GM.Players[NextDealingIndex];
		m_SmallBindPlayer = GM.Players[NextSmallBindIndex];
		m_LargeBindPlayer = GM.Players[NextLargeBindIndex];*/
	}
	
	public int GetAmountOfBustedPlayers()
	{
		int busted = 0;
		for(int i = 0; i < GM.Players.Length; i++)
		{
			if(GM.Players[i].Busted){busted++;}
		}
		return busted;
	}

	public TablePosition DeterminePlayerPosition(Player _Player)
	{
		if(_Player.Index == m_FirstPlayer.Index)
			return TablePosition.UnderTheGun;
		else if(_Player.Index == m_DealingPlayer.Index)
			return TablePosition.OnTheButton;
		else if(_Player.Index == m_SmallBindPlayer.Index)
			return TablePosition.SmallBind;
		else
			return TablePosition.BigBind;
	}
}