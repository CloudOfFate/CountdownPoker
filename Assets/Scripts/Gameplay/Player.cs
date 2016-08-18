using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player
{
	private GameManager GM;
	private Enemy m_EnemyAI;
	
	private GameObject m_OnScreenHand;
	private GameObject m_SelectedCardSlot;
	
	private Card m_SelectedCard;
	private Card m_CardSelectedForAuction;
	
	private Text m_TotalCapital;
	private Text m_OnThePool;
	private Text m_RaiseCounter;
	private Text m_BustedText;
	private Text m_FoldText;
	private Text m_ForfeitText;
	
	private int m_Index;
	private int m_Stack;
	private int m_OnTheBet;
	private int m_BetBeforeAuction;
	private int m_RaiseValue;
	private int m_PurchaseValue;
	private int m_MoneyUsedToBid;
	
	private bool m_Human;
	private bool bFold;
	private bool bCalled;
	private bool bChecked;
	private bool bRaise;
	private bool bBusted;
	private bool bForfeit;
	private bool bCompletedAuction;
	private bool bFinishPurchasing;
	private bool bGetSidePotOnly;
	
	private BettingDecision m_PlayerBettingAction;
	private AuctionDecision m_PlayerAuctionAction;
	
	private Card m_CardDesired;
	private List<Card> m_CardPocket;
	
	private GameObject[] m_CardSlots;
	private GameObject[] m_PocketSlots;
	private GameObject[] m_RevealSlots;
	
	private List<Card> m_Hand;
	private List<Card> m_AuctionCards;
	
	private Card[] m_PreSwapHand;
	private Card[] m_RevealedCards;
	
	public  GameManager GManager         {get{return GM;} set{GM = value;}}
	public  Enemy EnemyAI                {get{return m_EnemyAI;} set{m_EnemyAI = value;}}
	
	public  List<Card> Hand              {get {return m_Hand;} set{m_Hand = value;}}
	public  List<Card> CardsAuctioned    {get{return m_AuctionCards;} set{m_AuctionCards = value;}}
	
	public Card SelectedCard             {get {return m_SelectedCard;} set{m_SelectedCard = value;}}
	public Card SelectedCardForAuction   {get {return m_CardSelectedForAuction;} set{m_CardSelectedForAuction = value;}}
	public GameObject SelectedCardSlot   {get {return m_SelectedCardSlot;} set{m_SelectedCardSlot = value;}}
	
	public Text OnThePool                {get{return m_OnThePool;} set{m_OnThePool = value;}}
	public Text RaiseCounter             {get{return m_RaiseCounter;} set{m_RaiseCounter = value;}}
	public Text BustedText               {get{return m_BustedText;} set{m_BustedText = value;}}
	public Text FoldText                 {get{return m_FoldText;} set{m_FoldText = value;}}
	public Text ForfeitText              {get{return m_ForfeitText;} set{m_ForfeitText = value;}}
	
	public int Index                     {get {return m_Index;}}
	public int Stack                     {get {return m_Stack;} set{m_Stack = value;}}
	public int OnTheBet                  {get{return m_OnTheBet;} set{m_OnTheBet = value;}}
	public int BetBeforeAuction          {get{return m_BetBeforeAuction;} set{m_BetBeforeAuction = value;}}
	public int RaiseValue                {get{return m_RaiseValue;} set{m_RaiseValue = value;}}
	public int PurchaseValue             {get{return m_PurchaseValue;} set{m_PurchaseValue = value;}}
	public int MoneyUsedToBeBid          {get{return m_MoneyUsedToBid;} set{m_MoneyUsedToBid = value;}}
	
	public bool Human                    {get {return m_Human;}}
	public bool Fold                     {get{return bFold;} set{bFold = value;}}
	public bool Called                   {get{return bCalled;} set{bCalled = value;}}
	public bool Checked                  {get{return bChecked;} set{bChecked = value;}}
	public bool Raised                   {get{return bRaise;} set{bRaise = value;}}
	public bool Busted                   {get{return bBusted;} set{bBusted = value;}}
	public bool Forfeit                  {get{return bForfeit;} set{bForfeit = value;}}
	public bool CompletedAuction         {get{return bCompletedAuction;} set{bCompletedAuction = value;}}
	public bool FinishPurchasing         {get{return bFinishPurchasing;} set{bFinishPurchasing = value;}}
	public bool GetSidePotOnly           {get{return bGetSidePotOnly;} set{bGetSidePotOnly = value;}}
	
	public BettingDecision BettingAction {get{return m_PlayerBettingAction;} set{m_PlayerBettingAction = value;}}
	public AuctionDecision AuctionAction {get{return m_PlayerAuctionAction;} set{m_PlayerAuctionAction = value;}}
	
	public Card CardDesired              {get{return m_CardDesired;} set{m_CardDesired = value;}}
	public List<Card> Pocket             {get{return m_CardPocket;} set{m_CardPocket = value;}}
	
	public GameObject[] PocketSlots      {get{return m_PocketSlots;} set{m_PocketSlots = value;}}
	public GameObject[] RevealSlots      {get{return m_RevealSlots;} set{m_RevealSlots = value;}}
	
	public Card[] PreSwapHand            {get{return m_PreSwapHand;} set{m_PreSwapHand = value;}}
	public Card[] RevealedCards          {get{return m_RevealedCards;} set{m_RevealedCards = value;}}
	
	public Player(int _Index,int _Money,bool _human)
	{
		m_Index = _Index;
		m_Stack = _Money;
		m_Human = _human;
		m_Hand = new List<Card>();
		
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		
		if(m_Human)
		{
			m_OnScreenHand = GameObject.Find("Player's Hand");
			m_TotalCapital = GameObject.Find("Player").transform.GetChild(2).GetComponent<Text>();
			m_OnThePool    = GameObject.Find("Player").transform.GetChild(3).GetComponent<Text>();
			m_BustedText   = GameObject.Find("Player").transform.GetChild(4).GetComponent<Text>();
			m_FoldText     = GameObject.Find("Player").transform.GetChild(5).GetComponent<Text>();
			m_ForfeitText  = GameObject.Find("Player").transform.GetChild(6).GetComponent<Text>();
			m_RaiseCounter = GameObject.Find("Player").transform.GetChild(7).GetComponent<Text>();
			
			GameObject PlayerPocket = GameObject.Find("Player's Pocket");
			m_PocketSlots = new GameObject[PlayerPocket.transform.childCount];
			for(int i = 0; i < PlayerPocket.transform.childCount; i++)
			{
				m_PocketSlots[i] = PlayerPocket.transform.GetChild(i).gameObject;
			}
			
			GameObject PlayerRevealedCards = GameObject.Find("Player's Revealed Cards");
			m_RevealSlots = new GameObject[PlayerRevealedCards.transform.childCount];
			for(int i = 0; i < PlayerRevealedCards.transform.childCount; i++)
			{
				m_RevealSlots[i] = PlayerRevealedCards.transform.GetChild(i).gameObject;
			}
		}
		else
		{
			m_EnemyAI = new Enemy(this,GManager.EnemyBehaviors[Index-2].Mode,
			                      GManager.EnemyBehaviors[Index-2].Aggressiveness,
			                      GManager.EnemyBehaviors[Index-2].Tightness,
			                      GManager.EnemyBehaviors[Index-2].SkillLevel);
			
			//			Debug.Log("Enemy " + (m_Index - 1) + "'s aggressiveness: " + m_EnemyAI.Aggressiveness);
			//			Debug.Log("Enemy " + (m_Index - 1) + "'s tightness: " + m_EnemyAI.Tightness);
			//			Debug.Log("Enemy " + (m_Index - 1) + "'s skill: " + m_EnemyAI.Skill);
			//			m_EnemyAI = new Enemy(this,Random.Range(0,11),0,0);
			
			m_OnScreenHand = GameObject.Find("Enemy" + (_Index - 1) + "'s Hand");
			m_TotalCapital = GameObject.Find("Enemy " + (_Index - 1)).transform.GetChild(1).GetComponent<Text>();
			m_OnThePool    = GameObject.Find("Enemy " + (_Index - 1)).transform.GetChild(2).GetComponent<Text>();	
			m_BustedText   = GameObject.Find("Enemy " + (_Index - 1)).transform.GetChild(3).GetComponent<Text>();
			m_FoldText     = GameObject.Find("Enemy " + (_Index - 1)).transform.GetChild(4).GetComponent<Text>();
			m_ForfeitText  = GameObject.Find("Enemy " + (_Index - 1)).transform.GetChild(5).GetComponent<Text>();
			
			GameObject EnemyPocket = GameObject.Find("Enemy" + (_Index - 1) + "'s Pocket");
			m_PocketSlots = new GameObject[EnemyPocket.transform.childCount];
			for(int i = 0; i < EnemyPocket.transform.childCount; i++)
			{
				m_PocketSlots[i] = EnemyPocket.transform.GetChild(i).gameObject;
			}
			
			GameObject EnemyRevealedCards = GameObject.Find("Enemy" + (_Index - 1) + "'s Revealed Cards");
			m_RevealSlots = new GameObject[EnemyRevealedCards.transform.childCount];
			for(int i = 0; i < EnemyRevealedCards.transform.childCount; i++)
			{
				m_RevealSlots[i] = EnemyRevealedCards.transform.GetChild(i).gameObject;
			}
		}
		
		m_PlayerBettingAction = BettingDecision.NULL;
		
		m_TotalCapital.text = m_TotalCapital.text.Remove(7) + m_Stack.ToString(); 
		m_BustedText.enabled = false;
		m_FoldText.enabled = false;
		m_ForfeitText.enabled = false;
		if(Human){m_RaiseCounter.enabled = false;}
		
		m_SelectedCard = null;
		m_SelectedCardSlot = null;
		
		bFold = false;
		bCalled = false;
		bChecked = false;
		bRaise = false;
		bBusted = false;
		bCompletedAuction = false;
		bFinishPurchasing = false;
		bGetSidePotOnly = false;
		
		m_OnTheBet = 0;
		m_RaiseValue = 0;
		m_MoneyUsedToBid = 0;
		
		m_CardPocket = new List<Card>();
		
		m_CardSlots = new GameObject[5];
		for(int i = 0; i < m_CardSlots.Length; i++)
		{
			m_CardSlots[i] = m_OnScreenHand.transform.GetChild(i).gameObject;
			m_CardSlots[i].GetComponent<SpriteRenderer>().sprite = null;
		}
		
		m_AuctionCards = new List<Card>();
		m_RevealedCards = new Card[3];
	}
	
	public void AddCardToHand(Card _Card)
	{
		m_Hand.Add(_Card);
		AddCardToFreeSlot(_Card);
		if(m_Hand.Count > 5){Debug.Log("Too many cards in hand !");}
	}
	
	public void AddCardsToHand(Card[] _Cards)
	{
		for(int i = 0; i < _Cards.Length; i++)
		{
			AddCardToHand(_Cards[i]);
		}
	}
	
	public void PrintHand()
	{
		for(int i = 0; i < Hand.Count; i++){Debug.Log("Card " + (i+1) + ": " + Hand[i].Suit + Hand[i].Value);}
	}
	
	public void AddCardToFreeSlot(Card _Card)
	{
		int nFreeSlot = 0;
		for(int i = 0; i < m_CardSlots.Length; i++)
		{
			if(m_CardSlots[i].GetComponent<SpriteRenderer>().sprite == null){nFreeSlot = i; break;}
		}
		
		if(Human)
		{
			Sprite AddedCardSprite = Deck.CardsSprite[new Vector2((float)_Card.Suit,(float)_Card.Value)];
			m_CardSlots[nFreeSlot].GetComponent<SpriteRenderer>().sprite = AddedCardSprite;
		}
		else
		{
			Sprite AddedCardBackSprite = Deck.CardsSprite[new Vector2(3,13)];
			m_CardSlots[nFreeSlot].GetComponent<SpriteRenderer>().sprite = AddedCardBackSprite;
		}
	}
	
	public void CoverCardSlotsSprite()
	{
		for(int i = 0; i < m_CardSlots.Length; i++)
		{
			if(m_Human)
				m_CardSlots[i].GetComponent<SpriteRenderer>().sprite = null;
			else
			{
				Sprite ReplacementCardSprite = Deck.CardsSprite[new Vector2(3, 13)];
				m_CardSlots[i].GetComponent<SpriteRenderer>().sprite = ReplacementCardSprite;
			}
		} 
	}
	
	public void ClearCardslotsSprite()
	{
		Debug.Log("ClearCards");
		for(int i = 0; i < m_CardSlots.Length; i++)
			m_CardSlots[i].GetComponent<SpriteRenderer>().sprite = null;
	}
	
	public void CoverCards()
	{
		for(int i = 0; i < m_CardSlots.Length; i++)
		{
			Sprite ReplacementCardSprite = Deck.CardsSprite[new Vector2(3, 13)];
			m_CardSlots[i].GetComponent<SpriteRenderer>().sprite = ReplacementCardSprite;
		} 
	}
	
	public void ShowHand()
	{
		if(!m_Human)
		{
			for(int i = 0; i < m_CardSlots.Length; i++)
			{
				Sprite ReplacementCardSprite = Deck.CardsSprite[new Vector2((float) m_Hand[i].Suit, (float) m_Hand[i].Value)];
				m_CardSlots[i].GetComponent<SpriteRenderer>().sprite = ReplacementCardSprite;
			}
		}
	}
	
	public void RefreshPlayerHand()
	{
		for(int i = 0; i < m_Hand.Count; i++)
		{
			Sprite ReplacementCardSprite = Deck.CardsSprite[new Vector2((float) m_Hand[i].Suit, (float) m_Hand[i].Value)];
			m_CardSlots[i].GetComponent<SpriteRenderer>().sprite = ReplacementCardSprite;
		}
	}
	
	public void ProcessPlayerBettingAction(BettingDecision _Action)
	{
		if(_Action == BettingDecision.Bet)
		{
			m_Stack -= m_RaiseValue;
			m_OnTheBet += m_RaiseValue;
			GM.Table.Pot += m_RaiseValue;	
			
			GM.Betting.CurrentBet = m_RaiseValue;
			
			Debug.Log("Bet Made: " + m_RaiseValue);
			
			GM.CommonMemoryLog.LogMemory(Index - 1,PlayerAction.Bet,m_RaiseValue,null,GManager.Betting.RaiseMadeCount,"");
			
			GM.Betting.LatestRaiseAmount = m_RaiseValue;
			
			m_RaiseValue = 0;
			GM.RaiseSlider.value = 0;
			
			bCalled = true;
			for(int i = 0; i < GM.Players.Length; i++)
			{
				if(i != m_Index)
					bCalled = false;
			}
			
			if(GM.Betting.FirstBetAmount == 0.0f)
			{
				GM.Betting.FirstBetAmount =  GM.Betting.CurrentBet;
			}
			
			GM.Betting.RaiseMadeCount++;
			GM.Betting.LatestAggressivePlayer = this;
			GM.Betting.BetMadeThisRound = true;
		}
		else if(_Action == BettingDecision.Check)
		{
			bCalled = true;
			bChecked = true;
			
			GM.CommonMemoryLog.LogMemory(Index - 1,PlayerAction.Check,0,null,GManager.Betting.RaiseMadeCount,"");
		}
		else if(_Action == BettingDecision.Call)
		{
			if(GM.Table.PotCallRequirement > (OnTheBet + Stack))
			{
				int ValueToBeBet = Stack;
				m_OnTheBet += ValueToBeBet;
				m_Stack = 0;
				GM.Table.Pot += ValueToBeBet;
				
				GM.CommonMemoryLog.LogMemory(Index - 1, PlayerAction.Call, ValueToBeBet, null, GManager.Betting.RaiseMadeCount, "Call with all of its remaining money");
			}
			else
			{
				int ValueToBeBet = GM.Table.PotCallRequirement - m_OnTheBet;
				m_OnTheBet += ValueToBeBet;
				m_Stack -= ValueToBeBet;
				GM.Table.Pot += ValueToBeBet;
				
				GM.CommonMemoryLog.LogMemory(Index - 1, PlayerAction.Call, ValueToBeBet, null, GManager.Betting.RaiseMadeCount, "");
			}
			
			bCalled = true;
		}
		else if(_Action == BettingDecision.Raise)
		{
			m_Stack -= m_RaiseValue;
			m_OnTheBet += m_RaiseValue;
			GM.Table.Pot += m_RaiseValue;	
			
			GM.CommonMemoryLog.LogMemory(Index - 1,PlayerAction.Raise,m_RaiseValue,null, GManager.Betting.RaiseMadeCount,"");
			
			GM.Betting.LatestRaiseAmount = m_RaiseValue;
			
			GM.RaiseSlider.value = 0;
			
			bCalled = true;
			for(int i = 0; i < GM.Players.Length; i++)
			{
				if(i != m_Index)
					bCalled = false;
			}
			
			GM.Betting.RaiseMadeCount++;
			GM.Betting.LatestAggressivePlayer = this;
			
			m_RaiseValue = 0;
		}
		else if(_Action == BettingDecision.Fold)
		{
			bFold = true;
			m_FoldText.enabled = true;
			CoverCards();
			
			GM.CommonMemoryLog.LogMemory(Index - 1, PlayerAction.Fold, 0, null, GManager.Betting.RaiseMadeCount, "");
		}
	}
	
	public void ProcessPlayerAuctionAction(AuctionDecision _Action)
	{
		if(_Action == AuctionDecision.Bid)
		{
			if(!Human)
			{
				m_MoneyUsedToBid = 5;//(int) EnemyAI.CalculateMoneyToBeBid();
				
				if(m_RaiseValue == 0)
				{
					ProcessPlayerAuctionAction(AuctionDecision.Forfeit);
					return;
				}
			}
			
			m_Stack -= m_MoneyUsedToBid;
			m_OnTheBet += m_MoneyUsedToBid;
			m_PurchaseValue += m_MoneyUsedToBid;
			GM.Table.Pot += m_MoneyUsedToBid;
			
			if(Human)
				Debug.Log("Player bid " + SelectedCardForAuction.Suit + SelectedCardForAuction.Value + " for " + m_MoneyUsedToBid);
			else
				Debug.Log("Enemy " + (Index - 1) + " bid " +  SelectedCardForAuction.Suit + SelectedCardForAuction.Value + " for " + m_MoneyUsedToBid);
			
			Debug.Log("Current Highest Price for " + SelectedCardForAuction.Suit + " " + SelectedCardForAuction.Value + ": " + GM.Table.GetHighestBidForACard(SelectedCardForAuction));
			
			GM.CommonMemoryLog.LogMemory(Index - 1,PlayerAction.Bid,m_RaiseValue,m_CardSelectedForAuction, GManager.Betting.RaiseMadeCount,"");
			
			m_RaiseValue = 0;
			GM.RaiseSlider.value = 0;
			
			bRaise = true;
			AuctionAction = AuctionDecision.NULL;
		}
		else if(_Action == AuctionDecision.Forfeit)
		{
			bForfeit = true;
			m_RaiseValue = 0;
			GM.RaiseSlider.value = 0;
			m_ForfeitText.enabled = true;
			
			if(Index == 1)
				GM.DisableHandTypeCounter();
			
			CoverCards();
			
			Card Temp = SelectedCardForAuction;
			SelectedCardForAuction = null;
			
			//if there is no player bidding for this card anymore,
			if(Temp != null && !GM.Auction.IsPlayerCompetingThisCard(Temp))
			{
				for(int i = 0; i < GM.Table.Pool.Count; i++)
				{
					if(GM.Table.Pool[i] != null && GM.Table.Pool[i].Suit == Temp.Suit && GM.Table.Pool[i].Value == Temp.Value)
						GM.Table.Pool[i].BeingBet = false;
				}
			}
			
			if(Temp != null)
				GM.CommonMemoryLog.LogMemory(Index - 1,PlayerAction.Forfeit,0,m_CardSelectedForAuction, GManager.Betting.RaiseMadeCount,"");
			else
				GM.CommonMemoryLog.LogMemory(Index - 1,PlayerAction.Forfeit,0,null, GManager.Betting.RaiseMadeCount,"");
			
			AuctionAction = AuctionDecision.NULL;
		}
	}
	
	public BettingDecision DecideEnemyBetting()
	{
		Hands CurrentHandType = Evaluator.EvaluateHand(m_Hand);
		
		if(CurrentHandType == Hands.HighCard)
		{
			if(m_Index != GM.Betting.LargeBindPlayer.Index)
			{
				if(m_Stack == 0||m_OnTheBet == GM.Table.GetHighestBet())
					return BettingDecision.Check;
				
				int random = Random.Range(0,2);
				
				if(random == 0)
				{
					if(!GM.Betting.BetMadeThisRound)
						return BettingDecision.Bet;
					
					return BettingDecision.Call;
				}
				else if(random == 1)
					return BettingDecision.Fold;
			}
			else
			{
				if(m_OnTheBet != 0 && m_OnTheBet == GM.Table.GetHighestBet())
					return BettingDecision.Check;
				
				return BettingDecision.Call;
			}
		}
		else
		{
			if(m_Stack == 0)
				return BettingDecision.Check;
			
			if(m_OnTheBet != GM.Table.GetHighestBet())
			{
				if(!GM.Betting.BetMadeThisRound)
					return BettingDecision.Bet;
				
				return BettingDecision.Call;
			}
			else if(m_OnTheBet == GM.Table.GetHighestBet())
				return BettingDecision.Check;
		}
		
		return BettingDecision.Fold;
	}
	
	public Card DecideEnemyAuctionTarget()
	{
		int BestCardScore = 0;
		int BestCardIndex = 0;
		
		for(int i = 0; i < GM.Table.Pool.Count; i++)
		{
			int score =  (int) GM.Table.Pool[i].Value;
			
			if(score > BestCardScore)
			{
				BestCardScore = score; 
				BestCardIndex = i;
			}
		}
		
		return GM.Table.Pool[BestCardIndex];
	}
	
	public AuctionDecision DecideEnemyAuctioning()
	{
		int random = Random.Range(0,5);
		
		if(random == 0)
			return AuctionDecision.Forfeit;
		
		m_MoneyUsedToBid = GM.Table.GetHighestBidForACard(SelectedCardForAuction) + 10 - m_PurchaseValue;
		
		Debug.Log("Enemy raising: " + GM.Table.GetHighestBidForACard(SelectedCardForAuction) + " + 10");
		
		if((m_RaiseValue + m_OnTheBet) > m_Stack)
			return AuctionDecision.Forfeit;
		
		return AuctionDecision.Bid;
	}
	
	public bool DecideEnemyPurchase()
	{
		return Random.Range(0,2) == 0 ? true : false;
	}
	
	public void PerformEnemyHandSwapping()
	{
		int random = Random.Range(0,Pocket.Count);
		
		if(Pocket.Count < random + 1)
			return;
		
		Hand[random] = Pocket[random];
	}
	
	public void StoreCurrentHand()
	{
		m_PreSwapHand = new Card[Hand.Count];
		
		for(int i = 0; i < Hand.Count; i++)
			m_PreSwapHand[i] = Hand[i];
	}
	
	public void ReturnToPreviousHand()
	{
		for(int i = 0; i < m_PreSwapHand.Length; i++)
			Hand[i] = m_PreSwapHand[i];
	}
	
	public void UpdateFinancialStatus()
	{
		if(m_Stack < 0)
			m_Stack = 0;
		
		m_TotalCapital.text = m_TotalCapital.text.Remove(7) + m_Stack.ToString();
		m_OnThePool.text = m_OnThePool.text.Remove(1) + m_OnTheBet.ToString(); 
		
		if(Human && (m_RaiseValue > 0 || m_MoneyUsedToBid > 0))
		{
			m_RaiseCounter.enabled = true; 
			
			if(GM.Phase == TurnPhase.Betting)
				m_RaiseCounter.text = m_RaiseCounter.text.Remove(1) + m_RaiseValue.ToString();
			
			else if(GM.Phase == TurnPhase.Auctioning)
				m_RaiseCounter.text = m_RaiseCounter.text.Remove(1) + m_MoneyUsedToBid.ToString();
		}
		else if(Human && m_RaiseValue <= 0 && m_MoneyUsedToBid <= 0)
		{	
			m_RaiseCounter.enabled = false;
		}
	}
	
	public void IncreaseRaiseValue()
	{
		m_RaiseValue += 5;
	}
	
	public void AddCardToPocket(Card _card)
	{
		Pocket.Add(_card);
		for(int i = 0; i < m_PocketSlots.Length; i++)
		{
			if(m_PocketSlots[i].GetComponent<SpriteRenderer>().sprite == null)
			{
				m_PocketSlots[i].GetComponent<SpriteRenderer>().sprite = Deck.CardsSprite[new Vector2((float) _card.Suit,(float) _card.Value)]; 
				return;
			}
		}
	}
	
	public void CleanPocket()
	{
		Pocket.Clear();
		for(int i = 0; i < PocketSlots.Length; i++)
			PocketSlots[i].GetComponent<SpriteRenderer>().sprite = null;
	}
	
	public void Reveal3Cards()
	{
		int CardIndex = Random.Range (0,5); 
		List<int> PreviousIndexes = new List<int>();
		
		PreviousIndexes.Add(CardIndex);
		
		for(int Cycle = 0; Cycle < 3; Cycle++)
		{
			Card RandomCard = Hand[CardIndex];
			
			m_RevealedCards[Cycle] = RandomCard;
			m_RevealSlots[Cycle].GetComponent<SpriteRenderer>().sprite = Deck.CardsSprite[new Vector2((float) RandomCard.Suit, (float) RandomCard.Value)];
			
			int NewIndex = Random.Range(0,5);
			
			while(!IsGeneratedIndexValid(NewIndex,PreviousIndexes))
				NewIndex = Random.Range(0,5);
			
			PreviousIndexes.Add(NewIndex);
			
			CardIndex = NewIndex;
		}
	}
	
	private bool IsGeneratedIndexValid(int _Index, List<int> _PreviousIndexes)
	{
		for(int Index = 0; Index < _PreviousIndexes.Count; Index++)
		{
			if(_PreviousIndexes[Index] == _Index)
				return false;
		}
		
		return true;
	}
}
