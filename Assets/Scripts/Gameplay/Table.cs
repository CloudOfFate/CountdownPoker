using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Table 
{
	private GameManager  m_GM;
	private List<Card>   m_Pool;
	private GameObject   m_CardPool;
	private GameObject[] m_PoolCardSlots;
	private GameObject   m_SelectedCardPoolSlot;
	private Card         m_SelectedCard;
	private int          mf_PotMoney;
	private int          mf_PotCallRequirement;
	private Text         m_PotText;

	public GameManager  GM {get{ return m_GM;}}
	public List<Card>   Pool {get{ return m_Pool;}}
	public GameObject[] PoolCardSlots{get {return m_PoolCardSlots;}}
	public GameObject   SelectedCardPoolSlot{get {return m_SelectedCardPoolSlot;} set{m_SelectedCardPoolSlot = value;}}
	public Card         SelectedCard{get{return m_SelectedCard;} set{m_SelectedCard = value;}}
	public int          Pot {get{return mf_PotMoney;} set{mf_PotMoney = value;}}
	public int          PotCallRequirement {get{return mf_PotCallRequirement;} set{mf_PotCallRequirement = value;}}
	public Text         PotText{get{return m_PotText;} set{m_PotText = value;}}

	public Table(GameManager _GameManager)
	{
		m_GM = _GameManager;
		m_Pool = new List<Card>();
		
		m_SelectedCardPoolSlot = null;
		
		m_CardPool = GameObject.Find("Card Pool");
		m_PotText = GameObject.Find("PoolMoneyText").GetComponent<Text>();
		
		m_PoolCardSlots = new GameObject[5];
		for(int i = 0; i < m_PoolCardSlots.Length; i++)
			m_PoolCardSlots[i] = m_CardPool.transform.GetChild(i).gameObject;
	}
	
	public void ResetTable()
	{
		Pool.Clear();
		
		CleanCardPoolCards();
		
		for(int i = 0; i < GM.Players.Length; i++)
		{
			GM.Players[i].Hand.Clear(); 
			if(!GM.Players[i].Busted){GM.Players[i].CoverCardSlotsSprite();}
		}
		
		mf_PotMoney = 0;
		
		GM.Deck.Reset();
		
		GM.Deck.Shuffle();
		
		GM.Dealer.DealHandToAllPlayers();
	}
	
	public void PlaceAuctionCards()
	{
		GM.Dealer.DealCardsToPool();
		
		for(int i = 0; i < m_Pool.Count; i++)
		{
			PlaceCardOntoPool(m_Pool[i]);
		}
	}
	
	public void PrintPool()
	{
		Debug.Log("Pool count: " + m_Pool.Count);
		for(int i = 0; i < Pool.Count; i++)
			Debug.Log("Card " + (i+1) + ": " + Pool[i].Suit + Pool[i].Value);
	}
	
	public void PlaceCardOntoPool(Card _card)
	{
		int nEmptyCardSlotIndex = 0;
		for(int i = 0; i < m_PoolCardSlots.Length; i++)
		{
			if(m_PoolCardSlots[i].GetComponent<SpriteRenderer>().sprite == null)
			{
				nEmptyCardSlotIndex = i; 
				break;
			}
		}
		
		Sprite InsertCardSprite = Deck.CardsSprite[new Vector2((float) _card.Suit, (float) _card.Value)];
		m_PoolCardSlots[nEmptyCardSlotIndex].GetComponent<SpriteRenderer>().sprite = InsertCardSprite;
	}
	
	public void CleanCardPoolCards()
	{
		for(int i = 0; i < m_PoolCardSlots.Length; i++)
			m_PoolCardSlots[i].GetComponent<SpriteRenderer>().sprite = null;
	}

	public void RefreshPool()
	{
		for(int i = 0; i < m_Pool.Count; i++)
		{
			Sprite ReplacementCardSprite = Deck.CardsSprite[new Vector2((float) m_Pool[i].Suit, (float) m_Pool[i].Value)];
			m_PoolCardSlots[i].GetComponent<SpriteRenderer>().sprite = ReplacementCardSprite;
		}
	}
	
	public void RefreshPoolForAuction(AuctionPhase _Phase)
	{
		for(int i = 0; i < m_Pool.Count; i++)
		{
			Sprite ReplacementCardSprite = Deck.CardsSprite[new Vector2((float) m_Pool[i].Suit, (float) m_Pool[i].Value)];

			if(_Phase == AuctionPhase.First)
				m_PoolCardSlots[i].GetComponent<SpriteRenderer>().sprite = ReplacementCardSprite;
			else if(_Phase == AuctionPhase.Second)
				m_PoolCardSlots[i+1].GetComponent<SpriteRenderer>().sprite = ReplacementCardSprite;
		}
	}
	
	public void RefreshPoolForSwapping()
	{
		if(Pool.Count == 1)
		{
			Sprite SoleCardSprite = Deck.CardsSprite[new Vector2((float) Pool[0].Suit, (float) Pool[0].Value)];
			m_PoolCardSlots[2].GetComponent<SpriteRenderer>().sprite = SoleCardSprite;
		}
		else if(Pool.Count == 2)
		{
			Sprite FirstCardSprite = Deck.CardsSprite[new Vector2((float) Pool[0].Suit, (float) Pool[0].Value)];
			m_PoolCardSlots[1].GetComponent<SpriteRenderer>().sprite = FirstCardSprite;

			Sprite SecondCardSprite = Deck.CardsSprite[new Vector2((float) Pool[1].Suit, (float) Pool[1].Value)];
			m_PoolCardSlots[3].GetComponent<SpriteRenderer>().sprite = SecondCardSprite;
		}
		else if(Pool.Count == 3)
		{
			Sprite FirstCardSprite = Deck.CardsSprite[new Vector2((float) Pool[0].Suit, (float) Pool[0].Value)];
			m_PoolCardSlots[1].GetComponent<SpriteRenderer>().sprite = FirstCardSprite;

			Sprite SecondCardSprite = Deck.CardsSprite[new Vector2((float) Pool[1].Suit, (float) Pool[1].Value)];
			m_PoolCardSlots[2].GetComponent<SpriteRenderer>().sprite = SecondCardSprite;

			Sprite ThirdCardSprite = Deck.CardsSprite[new Vector2((float) Pool[2].Suit, (float) Pool[2].Value)];
			m_PoolCardSlots[3].GetComponent<SpriteRenderer>().sprite = ThirdCardSprite;
		}
	}
	
	public int GetHighestBet()
	{
		int HighestBet = 0;
		
		for(int i = 0; i < GM.Players.Length; i++)
		{
			if(GM.Players[i].OnTheBet > HighestBet)
				HighestBet = GM.Players[i].OnTheBet;
		}
		
		return HighestBet;
	}
	
	public Player GetPlayerWithHighestBet()
	{
		int HighestBet = GetHighestBet();

		for(int i = 0; i < GM.Players.Length; i++)
		{
			if(GM.Players[i].OnTheBet == HighestBet)
				return GM.Players[i];
		}

		return GM.Players[0];
	}
	
	public void ResetCardSlotSize()
	{
		for(int i = 0; i < m_PoolCardSlots.Length; i++)
		{
			m_PoolCardSlots[i].transform.localScale = new Vector3(7.0f,7.0f,1.0f); 
		}
	}
	
	public void ResetPoolSlotSize()
	{
		for(int i = 0; i < m_PoolCardSlots.Length; i++)
		{
			m_PoolCardSlots[i].transform.localScale = new Vector3(5.0f,5.0f,5.0f); 
		}
	}
	
	public void RefreshPoolMoney()
	{
		if(mf_PotMoney > 0)
		{
			m_PotText.enabled = true; 
			m_PotText.text = m_PotText.text.Remove(1) + mf_PotMoney.ToString();
			return;
		}

		m_PotText.enabled = false;
	}
	
	public int GetHighestBidForACard(Card _Card)
	{
		int HighestBid = 0;

		for(int i = 0; i < GM.Players.Length; i++)
		{
			if(GM.Players[i].SelectedCardForAuction != null 
			&& Utility.IsTwoCardsIdentical(GM.Players[i].SelectedCardForAuction,_Card) 
			&& GM.Players[i].MoneyUsedToBeBid > HighestBid)
			{
				HighestBid = GM.Players[i].PurchaseValue;
			}
		}
		return HighestBid;
	}

	public int RetrieveTotalMoneyInGame()
	{
		int TotalMoney = 0;

	    for(int PlayerSlot = 0; PlayerSlot < GM.Players.Length; PlayerSlot++)
			TotalMoney += GM.Players[PlayerSlot].Stack + GM.Players[PlayerSlot].OnTheBet;

		return TotalMoney;
	}

	public int GetEffectiveStackSize()
	{
		int LowestStack = 400 * GM.Betting.BigBind;

		for(int PlayerIndex = 0; PlayerIndex < GM.Players.Length; PlayerIndex++)
		{
			if(GM.Players[PlayerIndex].Stack < LowestStack)
				LowestStack = GM.Players[PlayerIndex].Stack;
		}

		return LowestStack;
	}

	public int GetAmountOfOverlappingPlayerDone(Player _Player)
	{
		int PlayerStack = _Player.Stack + _Player.OnTheBet;
		int OverlappingDone = 0;

		for(int PlayerIndex = 0; PlayerIndex < GM.Players.Length; PlayerIndex++)
		{
			if(GM.Players[PlayerIndex].Index != _Player.Index && GM.Players[PlayerIndex].Stack + GM.Players[PlayerIndex].OnTheBet > PlayerStack)
				OverlappingDone++;
		}

		return OverlappingDone;
	}
}
