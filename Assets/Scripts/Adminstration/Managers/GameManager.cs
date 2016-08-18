using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	#region Variables and Getter functions to essential object in Poker game (Deck, Dealer, Players etc..)
	private Deck m_Deck;
	private Dealer m_Dealer;
	private Player[] m_Players;
	private Table m_Table;
	
	public Deck Deck{get{ return m_Deck;}}
	public Dealer Dealer{get{ return m_Dealer;}}
	public Player[] Players{get{ return m_Players;} set{m_Players = value;}}
	public Table Table{get{ return m_Table;}}
	#endregion
	
	#region Variables and Getter functions for UI elements
	private PlayerControl m_Control;
	private RectTransform m_TurnPointer;
	private Slider m_RaiseSlider;
	private Text AnnouncementText;
	private Text HandTypeText;
	private Text RoundText;
	
	private GameObject m_PlayerLeftButton;
	private GameObject m_PlayerRightButton;
	private GameObject m_PlayerAllInButton;
	
	private Vector2[] m_PointerPositions;
	private Vector3[] m_PointerRotations;
	
	public PlayerControl Control{get{return m_Control;} set{m_Control = value;}}
	public Slider RaiseSlider {get{return m_RaiseSlider;} set{m_RaiseSlider = value;}}
	#endregion
	
	#region Variables related to turn/phase transition
	public TurnPhase m_Phase;
	
	public AuctionPhase m_AuctionPhase;
	
	private int m_TurnIndex;
	private int m_PlayerTurn;
	private int m_AuctionRound;//multiple rounds per auction phase
	private int m_Rounds;
	
	public float m_TurnDelay;
	
	private bool mb_ShowdownStarted;
	private bool mb_GameOngoing;
	private bool mb_Selecting;
	private bool mb_WinInBetting;
	private bool mb_TurnDelayRunning;
	
	public int CurrentRoundIndex{get{return m_Rounds;}}
	public int CurrentTurnIndex{get{return m_TurnIndex;}}
	public int CurrentPlayerIndex{get{return m_PlayerTurn;}}
	public TurnPhase Phase{get{return m_Phase;} set{m_Phase = value;}}
	public AuctionPhase AuctionPhase{get{return m_AuctionPhase;} set{m_AuctionPhase = value;}}
	public bool Selecting{get{return mb_Selecting;} set{mb_Selecting = value;}}
	#endregion
	
	#region Variables for instances that are required for AI to function (Database, Managers etc..)
	private CommonMemory m_CommonMemoryLog;
	private BettingManager m_Betting;
	private AuctionManager m_Auction;
	private DecisionReference m_DecisionReference;
	
	public CommonMemory CommonMemoryLog{get{return m_CommonMemoryLog;} set{m_CommonMemoryLog = value;}}
	public BettingManager Betting{get{return m_Betting;} set{m_Betting = value;}}
	public AuctionManager Auction{get{return m_Auction;} set{m_Auction = value;}}
	public DecisionReference Decisions{get{return m_DecisionReference;} set{m_DecisionReference = value;}}
	#endregion
	
	#region Variables visible in inspector to allow modification to game properties
	[System.Serializable]
	public struct PlayerProperties
	{
		public string Name;
		public bool IsPlayer;
		public int StartingStack;
	}
	
	public PlayerProperties[] PlayerSettings;
	#endregion
	
	#region Variables visible in inspector to allow AI modification
	[System.Serializable]
	public struct AIBehaviorPerimeter
	{
		public string Name;
		public EnemyMode Mode;
		public int Aggressiveness;
		public int Tightness;
		public int SkillLevel;
	}
	
	public AIBehaviorPerimeter[] EnemyBehaviors;
	#endregion
	
	// Use this for initialization
	void Start () 
	{
		int Iteration = 0;
//		for(int i = 0; i < (5 - 1); i++)
//		{
//			for(int j = i + 1; j < 5; j++)
//			{
//				Iteration++;
//			}
//		}

		for(int i = 0; i < (5 - 1); i++) // i = 0, i < 3 => 0 to 2 (3 ITERATIONS)
		{
			for(int j = i + 1; j < (5 ); j++)//j =
			{
				Iteration++;
//				for(int k = j + 1; k < 27; k++)
//				{
//				}
			}
		}


//		for(int i = 0; i < (5 - 3); i++)
//		{
//			for(int j = i + 1; j < (5 - 2); j++)
//			{
//				for(int t = j + 1; t < (5 - 1); t++)
//				{
//					for(int k = t + 1; k < 5; k++)
//					{
//						Iteration++;
//					}
//
//				}
//			}
//		}
		Debug.Log("Amt. of iteration = " + Iteration);

		#region Instantiate variables of essential objects in poker game
		m_Deck = new Deck();
		m_Dealer = new Dealer(this);
		
		m_Players = new Player[4];
		m_Players[0] = new Player(1,1000,true);
		m_Players[1] = new Player(2,1000,false);
		m_Players[2] = new Player(3,1000,false);
		m_Players[3] = new Player(4,1000,false);
		
		m_Table = new Table(this);
		#endregion
		
		#region Instantiate variables for UI elements
		m_Control = GetComponent<PlayerControl>();
		
		AnnouncementText = GameObject.Find("AnnoucementText").GetComponent<Text>();
		HandTypeText = GameObject.Find("HandtypeCounter").GetComponent<Text>();
		RoundText = GameObject.Find("RoundCounter").GetComponent<Text>();
		
		m_RaiseSlider = GameObject.Find("RaiseSlider").GetComponent<Slider>();
		
		m_PlayerLeftButton = GameObject.Find("ShowHand Button");
		m_PlayerRightButton = GameObject.Find("Fold Button");
		m_PlayerAllInButton = GameObject.Find("AllIn Button");
		
		m_TurnPointer = GameObject.Find("TurnPointer").GetComponent<RectTransform>();
		
		m_PointerPositions = new Vector2[4];
		m_PointerPositions[0] = new Vector2(0,-40);
		m_PointerPositions[1] = new Vector2(-210,0);
		m_PointerPositions[2] = new Vector2(0,90);
		m_PointerPositions[3] = new Vector2(210,0);
		
		m_PointerRotations = new Vector3[4];
		m_PointerRotations[0] = Vector3.zero;
		m_PointerRotations[1] = new Vector3(0.0f,0.0f,-90.0f);
		m_PointerRotations[2] = new Vector3(0.0f,0.0f,-180.0f);
		m_PointerRotations[3] = new Vector3(0.0f,0.0f,90.0f);
		
		AnnouncementText.enabled = false;
		#endregion
		
		#region Instantiate variables that are essential to transition of game phases
		m_Phase = TurnPhase.Initial;
		m_AuctionPhase = AuctionPhase.NULL;
		
		mb_TurnDelayRunning = false;
		mb_GameOngoing = true;
		mb_ShowdownStarted = false;
		mb_Selecting = false;
		
		m_Rounds = 0;
		#endregion
		
		#region Instantiate variables that are required for AI to function
		m_DecisionReference = new DecisionReference();
		m_CommonMemoryLog = new CommonMemory(this);
		
		m_Betting = new BettingManager(this);
		m_Auction = new AuctionManager(this);
		#endregion
		
		Table.ResetTable();
		m_Betting.InitializePlayersDealAndBind();
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i = 0; i < m_Players.Length; i++)
		{
			m_Players[i].UpdateFinancialStatus();
		}
		
		Table.RefreshPoolMoney();
		
		if(mb_GameOngoing && !mb_TurnDelayRunning)
		{
			if(m_Phase == TurnPhase.Initial)
			{
				Debug.Log("Current Phase: " + Phase);
				
				Table.ResetTable();
				
				for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
					Players[PlayerIndex].FinishPurchasing = false;
				
				#region Reset Turn/Game transition related variables
				m_Rounds++;
				RoundText.text = RoundText.text.Remove(6) + m_Rounds.ToString();
				
				m_PlayerTurn = m_Betting.FirstPlayer.Index - 1;
				if(m_PlayerTurn > 3)
					m_PlayerTurn = 0;
				
				m_Phase = TurnPhase.Betting;
				Betting.BetMadeThisRound = false;
				#endregion
				
				#region Reduce the appropriate amount of money from the player that are going to pay small/big binds for this round
				if(Players[m_Betting.SmallBindPlayer.Index-1].Stack >= m_Betting.SmallBind)
				{
					Players[m_Betting.SmallBindPlayer.Index - 1].Stack -= m_Betting.SmallBind; 
					Players[m_Betting.SmallBindPlayer.Index - 1].OnTheBet += m_Betting.SmallBind; 
					Table.Pot += m_Betting.SmallBind;
					
					CommonMemoryLog.LogMemory(m_Betting.SmallBindPlayer.Index - 1,PlayerAction.PaySmallBind,m_Betting.SmallBind,null,Betting.RaiseMadeCount,"");
				}
				else
				{
					Players[m_Betting.SmallBindPlayer.Index-1].OnTheBet = Players[m_Betting.SmallBindPlayer.Index-1].Stack; 
					Table.Pot += Players[m_Betting.SmallBindPlayer.Index-1].Stack; 
					Players[m_Betting.SmallBindPlayer.Index-1].Stack = 0;
					
					CommonMemoryLog.LogMemory(m_Betting.SmallBindPlayer.Index - 1,PlayerAction.PaySmallBind,Players[m_Betting.SmallBindPlayer.Index-1].Stack,null,Betting.RaiseMadeCount,"Cannot afford full small bind");
				}
				
				if(Players[m_Betting.LargeBindPlayer.Index-1].Stack >= m_Betting.BigBind)
				{
					Players[m_Betting.LargeBindPlayer.Index - 1].Stack -= m_Betting.BigBind; 
					Players[m_Betting.LargeBindPlayer.Index - 1].OnTheBet += m_Betting.BigBind; 
					Table.Pot += m_Betting.BigBind;
					
					CommonMemoryLog.LogMemory(m_Betting.LargeBindPlayer.Index - 1,PlayerAction.PayLargeBind,m_Betting.BigBind,null,Betting.RaiseMadeCount,"");
				}
				else
				{
					Players[m_Betting.LargeBindPlayer.Index-1].OnTheBet = Players[m_Betting.LargeBindPlayer.Index-1].Stack; 
					Table.Pot += Players[m_Betting.LargeBindPlayer.Index-1].Stack; 
					Players[m_Betting.LargeBindPlayer.Index-1].Stack = 0;
					
					CommonMemoryLog.LogMemory(m_Betting.LargeBindPlayer.Index - 1,PlayerAction.PayLargeBind,Players[m_Betting.LargeBindPlayer.Index-1].Stack,null,Betting.RaiseMadeCount,"Cannot afford full large bind");
				}
				
				Betting.LatestAggressivePlayer = Players[m_Betting.LargeBindPlayer.Index - 1];
				#endregion
				
				Debug.Log("Current Phase: " + Phase);
				
				#region Reset all UI elements back to Betting phase
				m_RaiseSlider.minValue = 0;
				m_RaiseSlider.maxValue = Players[0].Stack;
				m_RaiseSlider.value = m_RaiseSlider.minValue;
				
				m_RaiseSlider.enabled = true;
				m_RaiseSlider.gameObject.SetActive(true);
				
				RefreshPointer();
				UpdateHandTypeCounter();
				m_Control.AllInButton.interactable = true;
				
				m_PlayerRightButton.GetComponentInChildren<Text>().text = "Fold";
				#endregion
				
				#region Log the players that are the dealer or under the gun
				CommonMemoryLog.LogMemory(m_Betting.DealingPlayer.Index - 1,PlayerAction.BecomeDealer,0,null,Betting.RaiseMadeCount,"");
				CommonMemoryLog.LogMemory(m_Betting.FirstPlayer.Index - 1,PlayerAction.UnderTheGun,0,null,Betting.RaiseMadeCount,"");
				#endregion
				
				for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
				{
					if(!Players[PlayerIndex].Busted && !Players[PlayerIndex].Fold)
						Players[PlayerIndex].Reveal3Cards();
				}
				
				#region Print which player is dealer / big bind / small bind
				if(m_Betting.DealingPlayer.Index == 1){Debug.Log("Dealer: Player");}
				else{Debug.Log("Dealer: Enemy " + (m_Betting.DealingPlayer.Index - 1));}
				
				if(m_Betting.SmallBindPlayer.Index == 1){Debug.Log("SmallBind: Player");}
				else{Debug.Log("SmallBind: Enemy " + (m_Betting.SmallBindPlayer.Index - 1));}
				
				if(m_Betting.LargeBindPlayer.Index == 1){Debug.Log("LargeBind: Player");}
				else{Debug.Log("LargeBind: Enemy " + (m_Betting.LargeBindPlayer.Index - 1));}
				
				if(m_PlayerTurn == 0){Debug.Log("First turn: Player");}
				else{Debug.Log("First turn: Enemy " + (m_PlayerTurn));}
				#endregion
				
				return;
			}
			else if(m_Phase == TurnPhase.Betting)
			{
				Table.PotCallRequirement = Table.GetHighestBet();
				
				if(!IsBettingCompleted())
				{
					if(mb_WinInBetting)
						return;
					
					if(Utility.HowManyValidPlayersLeft(this) == 1 && !mb_WinInBetting)
						StartCoroutine(WinInBettingCorountine());
					
					Player CurrentPlayer = Players[m_PlayerTurn];
					
					if(CurrentPlayer.Human)
					{
						#region Update the properties of the Rise Slider based on the player's total money
						m_RaiseSlider.minValue = m_Players[m_PlayerTurn].OnTheBet != Table.PotCallRequirement ? (Table.PotCallRequirement - m_Players[m_PlayerTurn].OnTheBet) : 0.0f;
						m_RaiseSlider.maxValue = m_Players[m_PlayerTurn].Stack;
						m_Players[m_PlayerTurn].RaiseValue = (int) m_RaiseSlider.value;
						#endregion
						
						if(!Players[m_PlayerTurn].Fold)
						{
							#region Update the UI appropriately based on the player's changes to slider or the amount of money they bet
							if(m_RaiseSlider.value > 0 && m_RaiseSlider.value > m_RaiseSlider.minValue)
							{
								if(!Betting.BetMadeThisRound)
									m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Bet"; 
								else
									m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Raise";
								
								Players[m_PlayerTurn].OnThePool.text = Players[m_PlayerTurn].OnThePool.text.Remove(1) + 
									(Players[m_PlayerTurn].OnTheBet + Players[m_PlayerTurn].RaiseValue).ToString();
							}
							else if(Players[m_PlayerTurn].OnTheBet == Table.PotCallRequirement)
							{
								m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Check";
							}
							else if(Players[m_PlayerTurn].OnTheBet != Table.PotCallRequirement)
							{
								if(!Betting.BetMadeThisRound)
									m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Bet"; 
								else
									m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Call";
							}
							#endregion
							
							#region When player makes any decision and pressed the corresponding button in the UI, update the game logic accordingly
							if(!m_Control.LeftButtonPressed && !m_Control.RightButtonPressed && !m_Control.AllInPressed)
								return;
							
							if(m_Control.LeftButtonPressed && m_RaiseSlider.value > m_RaiseSlider.minValue)//> 0)
							{
								//								Debug.Log("Player " + CurrentPlayer.Index + ": Raise $" + m_RaiseSlider.value); 
								if(!Betting.BetMadeThisRound)
									Players[m_PlayerTurn].BettingAction = BettingDecision.Bet; 
								else
									Players[m_PlayerTurn].BettingAction = BettingDecision.Raise; 
								
								m_Control.LeftButtonPressed = false;
							}
							else if(m_Control.LeftButtonPressed && Players[m_PlayerTurn].OnTheBet == Table.PotCallRequirement)
							{
								//								Debug.Log("Player " + CurrentPlayer.Index + ": Check"); 
								Players[m_PlayerTurn].BettingAction = BettingDecision.Check; 
								m_Control.LeftButtonPressed = false;
							}
							else if(m_Control.LeftButtonPressed && Players[m_PlayerTurn].OnTheBet != Table.PotCallRequirement)
							{
								//								Debug.Log("Player " + CurrentPlayer.Index + ":Call"); 
								Players[m_PlayerTurn].BettingAction = BettingDecision.Call; 
								m_Control.LeftButtonPressed = false;
							}
							
							if(m_Control.RightButtonPressed)
							{
								Players[m_PlayerTurn].BettingAction = BettingDecision.Fold; 
								m_Control.RightButtonPressed = false;
							}
							
							if(m_Control.AllInPressed)
							{
								//								Debug.Log ("All In: " + Players[m_PlayerTurn].OnTheBet + " , " + Players[m_PlayerTurn].Money); 
								Players[m_PlayerTurn].RaiseValue = (int) m_RaiseSlider.maxValue; 
								Players[m_PlayerTurn].BettingAction = BettingDecision.Raise; m_Control.AllInPressed = false; 
								m_Control.AllInButton.interactable = false;
							}
							#endregion
							
							if(Players[m_PlayerTurn].BettingAction == BettingDecision.NULL)
								return;
							
							Players[m_PlayerTurn].ProcessPlayerBettingAction(Players[m_PlayerTurn].BettingAction);
							
							#region Reset the raise slider for the next turn to prevent the slider from starting in the middle of the values
							m_RaiseSlider.minValue = m_Players[m_PlayerTurn].OnTheBet != Table.PotCallRequirement ? (Table.PotCallRequirement - m_Players[m_PlayerTurn].OnTheBet) : 0.0f;
							m_RaiseSlider.maxValue = m_Players[m_PlayerTurn].Stack;
							m_RaiseSlider.value = m_RaiseSlider.minValue;
							#endregion
							
							Debug.Log("Player " + Players[m_PlayerTurn].BettingAction);
						}
						
						PassToNextPlayer();
						return;
					}
					else
					{
						if(!Players[m_PlayerTurn].Fold)
						{
							Players[m_PlayerTurn].BettingAction = Players[m_PlayerTurn].EnemyAI.DeterminePlayerBetting();

							if(Players[m_PlayerTurn].BettingAction == BettingDecision.NULL)
								Debug.Log("Enemy " + Players[m_PlayerTurn].Index + " need to do something about his turn");
							
							Players[m_PlayerTurn].ProcessPlayerBettingAction(Players[m_PlayerTurn].BettingAction);
							
							Debug.Log("Enemy " + (Players[m_PlayerTurn].Index - 1) + " " + Players[m_PlayerTurn].BettingAction);
						}
						
						PassToNextPlayer();
						return;
					}
				}
				else 
				{
					m_Betting.Limpers.Clear();
					m_Phase = TurnPhase.Auctioning; 
					m_AuctionPhase = AuctionPhase.Initial;
					Debug.Log("transitioning to auction");
					return;
				}
			}
			else if(m_Phase == TurnPhase.Auctioning && m_AuctionPhase != AuctionPhase.NULL)
			{
				if(m_AuctionPhase == AuctionPhase.Initial)
				{
					Debug.Log("Current Phase: " + Phase + " Auction phase: " + m_AuctionPhase);
					#region Change UI properties to fit Auction Phase
					m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Confirm";
					m_PlayerRightButton.GetComponentInChildren<Text>().text = "Forfeit";
					
					AnnouncementText.enabled = false;
					m_PlayerAllInButton.GetComponent<Button>().enabled = false;
					#endregion
					
					#region Reset Game-Logic related variables (Player's variables etc..)
					for(int i = 0; i < Players.Length; i++)
					{
						Players[i].RaiseValue = 0;
						Players[i].PurchaseValue = 0;
						Players[i].BetBeforeAuction = Players[i].OnTheBet;
					}
					#endregion
					
					m_AuctionRound = 0;
					m_AuctionPhase = AuctionPhase.First;
					
					Auction.EffectiveStackForBidding = Table.GetEffectiveStackSize();
					
					Debug.Log("Current Phase: " + Phase + " Auction phase: " + m_AuctionPhase);
					
					PassToNextPlayer();
					return;
				}
				else if(m_AuctionPhase == AuctionPhase.First || m_AuctionPhase == AuctionPhase.Second)
				{
					if(Table.Pool.Count <= 0)
					{
						m_Auction.DealAuctionCards(m_AuctionPhase);
						CommonMemoryLog.LogAuction();
					}
					
					Table.PotCallRequirement = Table.GetHighestBet();
					
					if(!HasAuctionForCardEnd())
					{
						if(Players[m_PlayerTurn].CompletedAuction)
							PassToNextPlayer();
						
						if(Players[m_PlayerTurn].Human && !Players[m_PlayerTurn].Forfeit)
						{
							if(Players[m_PlayerTurn].SelectedCardForAuction == null)
							{
								mb_Selecting = true;
								m_RaiseSlider.gameObject.SetActive(true);
								
								m_RaiseSlider.minValue = 0.0f;
								m_RaiseSlider.maxValue = Auction.EffectiveStackForBidding;
								
								Players[m_PlayerTurn].MoneyUsedToBeBid = (int) m_RaiseSlider.value;
								Players[m_PlayerTurn].OnThePool.text = Players[m_PlayerTurn].OnThePool.text.Remove(1) + 
									(Players[m_PlayerTurn].OnTheBet + Players[m_PlayerTurn].MoneyUsedToBeBid).ToString();
								
								#region Detect whether the player had selected a card from the pool to auction for or forfeit the auction phase, and process the action
								if(m_Control.LeftButtonPressed && Table.SelectedCardPoolSlot != null && Table.SelectedCard != null && m_RaiseSlider.value > 0.0f)
								{
									#region Set the clicked card to be current player's selected card to auction for
									Players[m_PlayerTurn].SelectedCardForAuction = Table.SelectedCard;
									SetCardBeingBet(Players[m_PlayerTurn].SelectedCardForAuction,true);
									#endregion
									
									Players[m_PlayerTurn].MoneyUsedToBeBid = (int) m_RaiseSlider.value;
									Debug.Log("Player bid " + Table.SelectedCard.Suit + " " + Table.SelectedCard.Value + " for " + Players[m_PlayerTurn].MoneyUsedToBeBid); 
									
									#region Reset the UI elements to accommodate the bidding section of the auction
									m_Control.LeftButtonPressed = false;
									m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Bet";
									m_RaiseSlider.value = 0;
									#endregion
									
									PassToNextPlayer();
									m_AuctionRound++;
									return;
								}
								else if((m_Control.LeftButtonPressed && (Table.SelectedCard == null || Table.SelectedCardPoolSlot == null)) ||
								        (m_Control.LeftButtonPressed && m_RaiseSlider.value <= 0.0f))
								{
									m_Control.LeftButtonPressed = false; 
									return;
								}
								else if(m_Control.RightButtonPressed)
								{
									#region Process player's forfeit action and reset the button pressed booleans
									Players[m_PlayerTurn].AuctionAction = AuctionDecision.Forfeit; 
									Players[m_PlayerTurn].ProcessPlayerAuctionAction(Players[m_PlayerTurn].AuctionAction);
									
									m_Control.RightButtonPressed = false; 
									#endregion
									
									PassToNextPlayer();
									m_AuctionRound++;
									return;
								}
								#endregion
							}
						}
						else if(!Players[m_PlayerTurn].Human && !Players[m_PlayerTurn].Forfeit)
						{
							if(Players[m_PlayerTurn].SelectedCardForAuction == null)
							{
								#region Determine whether AI player should participate in the auction and select a specific card from the pool for the AI
								AuctionDecision EnemyDecision = Players[m_PlayerTurn].EnemyAI.DeterminePlayerSelectAuctionCard();
								
								if(EnemyDecision == AuctionDecision.NULL)
								{
									EnemyDecision = Players[m_PlayerTurn].EnemyAI.DeterminePlayerAuctionBetting();
									//									Debug.Log("Enemy" + m_PlayerTurn + " bid " + Players[m_PlayerTurn].SelectedCardForAuction.Suit + " " + Players[m_PlayerTurn].SelectedCardForAuction.Value + " for " + Players[m_PlayerTurn].MoneyUsedToBeBid);
								}
								
								Players[m_PlayerTurn].ProcessPlayerAuctionAction(EnemyDecision);
								#endregion
								
								PassToNextPlayer();
								m_AuctionRound++;
								return;
							}
						}
						
						if(!Players[m_PlayerTurn].Human)
							Auction.PriceCheck = false;
						
						#region Cover any cards on the pool that are not selected to be bid for after the first round of the auction phase
						if(Auction.HasAllPlayersBidForACard())//m_AuctionRound == GetNonBustedPlayerAmount())
						{
							Auction.CoverNonBettedAuctionCards(); 
							Auction.EnableCardPrices();
						}
						#endregion
						
						//if the auction phase has past prelimary round,Check if any card that are being auctioned only have one player auctioning for it, if there is, the one player wins that card -> add that card to the player's pocket
						if(Auction.HasAllPlayersBidForACard())//m_AuctionRound == GetNonBustedPlayerAmount())
						{
							Auction.UpdateCardPrices();
							
							for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
							{
								if(Players[PlayerIndex].Forfeit)
									Players[PlayerIndex].ForfeitText.enabled = true;
							}
							
							#region Go through all the cards in the pool that may be bid for by the players and assign any card completed their bidding to the appropriate winner
							for(int CardIndex = 0; CardIndex < Table.Pool.Count; CardIndex++)
							{
								#region Check for how many eligible players that are bidding for this current card (Haven't busted, folded, forfeited etc..)
								List<Player> PlayersBiddingForTheCard = new List<Player>();
								
								for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
								{
									if(Players[PlayerIndex].SelectedCardForAuction != null && !Players[PlayerIndex].Busted 
									   && !Players[PlayerIndex].Fold && !Players[PlayerIndex].Forfeit 
									   && Utility.IsTwoCardsIdentical(Players[PlayerIndex].SelectedCardForAuction,Table.Pool[CardIndex]))
									{
										PlayersBiddingForTheCard.Add(Players[PlayerIndex]);
									}
								}
								#endregion
								
								if(PlayersBiddingForTheCard.Count == 0)
									continue;
								
								#region If there is only one player left to be bidding on this card, he/she will attain the card and add it to their pocket
								int HighestBid = 0;
								Player PlayerWithHighestBid = PlayersBiddingForTheCard[0];
								
								for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
								{
									if(Players[PlayerIndex].MoneyUsedToBeBid > HighestBid)
									{
										HighestBid = Players[PlayerIndex].MoneyUsedToBeBid;
										PlayerWithHighestBid = Players[PlayerIndex];
									}
								}
								
								if(HighestBid != 0)
								{
									Table.Pool[CardIndex].BeingBet = false;
									PlayerWithHighestBid.AddCardToPocket(Table.Pool[CardIndex]);
									PlayerWithHighestBid.CardsAuctioned.Add(Table.Pool[CardIndex]);
									PlayerWithHighestBid.CompletedAuction = true;
								}
								#endregion
							}
							#endregion
						}
					}
					else
					{
						#region Reset various variables of the table for the next auction phase
						Table.ResetPoolSlotSize();
						Table.CleanCardPoolCards();
						Table.Pool.Clear();
						Table.SelectedCard = null;
						Table.SelectedCardPoolSlot = null;
						#endregion
						
						#region Reset the Card Price UI element and Raise Slider
						Auction.DisableCardPrices();
						Auction.ResetCardPrices();
						m_RaiseSlider.value = 0;
						#endregion
						
						if(Players[0].Forfeit)
							Players[0].RefreshPlayerHand();
						
						#region Reset various variables of all the player on the table
						for(int i = 0; i < Players.Length; i++)
						{
							Players[i].AuctionAction = AuctionDecision.NULL;
							Players[i].SelectedCardForAuction = null;
							Players[i].Forfeit = false;
							Players[i].ForfeitText.enabled = false;
							Players[i].RaiseValue = 0;
							Players[i].PurchaseValue = 0;
							Players[i].MoneyUsedToBeBid = 0;
							Players[i].Raised = false;
							Players[i].SelectedCard = null;
							Players[i].SelectedCardSlot = null;
							Players[i].CompletedAuction = false;
							Players[i].ForfeitText.enabled = false;
						}
						#endregion
						
						m_AuctionRound = 0;			
						PassToNextPlayer();
						
						#region Check for which current auction phase the game is in, then trasition to the appropriate next phase
						if(m_AuctionPhase == AuctionPhase.First)
						{
							m_AuctionPhase = AuctionPhase.Second; 
							m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Confirm";
							m_PlayerRightButton.GetComponentInChildren<Text>().text = "Forfeit";
							m_PlayerAllInButton.GetComponent<Button>().enabled = false;
							
							for(int i = 0; i < m_Players.Length; i++)
							{
								m_Players[i].BetBeforeAuction = m_Players[i].OnTheBet;
							}
							
							Debug.Log("Enter auction phase 2");
						}
						else if(m_AuctionPhase == AuctionPhase.Second)
						{ 
							m_AuctionPhase = AuctionPhase.Third;
							m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Purchase";
							m_PlayerRightButton.GetComponentInChildren<Text>().text = "Pass";
							mb_Selecting = true;
							
							for(int i = 0; i < m_Players.Length; i++)
							{
								m_Players[i].BetBeforeAuction = 0;
							}
							
							Debug.Log("Enter auction phase 3");
						}
						#endregion
					}
				}
				else if(m_AuctionPhase == AuctionPhase.Third)
				{
					#region Show the announcement that declare the purchase phase is happening
					if(AnnouncementText.enabled == false)
					{
						AnnouncementText.enabled = true; 
						AnnouncementText.text = "Purchase one random card for only $" + Auction.CostOfRandomCard.ToString() + " !";
					}
					#endregion
					
					if(Players[m_PlayerTurn].Human)
					{
						#region Check whether the player had decided to purchase a card and they had sufficient money. If so, add the card to their pocket
						if(m_Control.LeftButtonPressed && Players[m_PlayerTurn].Stack > Auction.CostOfRandomCard)
						{
							Players[m_PlayerTurn].Stack -= Auction.CostOfRandomCard;
							Players[m_PlayerTurn].OnTheBet += Auction.CostOfRandomCard;
							
							Card CardPurchased = Deck.DrawSingle();
							Players[m_PlayerTurn].AddCardToPocket(CardPurchased);
							Players[m_PlayerTurn].CardsAuctioned.Add(CardPurchased);
							//							Dealer.AddCardToPlayerPocket(Players[m_PlayerTurn]);
							
							m_Control.LeftButtonPressed = false;
							Players[m_PlayerTurn].FinishPurchasing = true;
							
							CommonMemoryLog.LogMemory(m_PlayerTurn,PlayerAction.Purchase,Auction.CostOfRandomCard,CardPurchased,Betting.RaiseMadeCount,"");
							
							PassToNextPlayer();
							m_AuctionRound++;
							return;
							Debug.Log("Player purchase a card and add to his/her pocket !");
						}
						#endregion
						#region Else, check whether the player decided to forfeit the purchasing
						else if(m_Control.RightButtonPressed)
						{
							Debug.Log("Player skip the purchase of random card !"); 
							m_Control.RightButtonPressed = false; 
							Players[m_PlayerTurn].FinishPurchasing = true; 
							
							CommonMemoryLog.LogMemory(m_PlayerTurn,PlayerAction.Forfeit,0,null,Betting.RaiseMadeCount,"Forfeit purchasing card");
							
							PassToNextPlayer(); 
							m_AuctionRound++;
							return;
						}
						#endregion
					}
					else
					{
						if(Players[m_PlayerTurn].Stack < Auction.CostOfRandomCard)
						{
							Players[m_PlayerTurn].FinishPurchasing = true; 
							CommonMemoryLog.LogMemory(m_PlayerTurn,PlayerAction.Forfeit,0,null,Betting.RaiseMadeCount,"Forfeit purchasing card");
							
							PassToNextPlayer(); 
							m_AuctionRound++;
						}
						
						#region Determine whether the AI player desire to purchase a card and process that action
						else if(true)//Players[m_PlayerTurn].EnemyAI.DeterminePlayerPurchasingCard())
						{
							Players[m_PlayerTurn].Stack -= Auction.CostOfRandomCard;
							Players[m_PlayerTurn].OnTheBet += Auction.CostOfRandomCard;
							
							//							Dealer.AddCardToPlayerPocket(Players[m_PlayerTurn]);
							Card CardPurchased = Deck.DrawSingle();
							Players[m_PlayerTurn].AddCardToPocket(CardPurchased);
							Players[m_PlayerTurn].CardsAuctioned.Add(CardPurchased);
							
							Players[m_PlayerTurn].FinishPurchasing = true;
							
							CommonMemoryLog.LogMemory(m_PlayerTurn,PlayerAction.Purchase,Auction.CostOfRandomCard,CardPurchased,Betting.RaiseMadeCount,"");
							
							PassToNextPlayer();
							m_AuctionRound++;
							Debug.Log("Enemy" + m_PlayerTurn + " purchase a card and add to his/her pocket!");
						}
						else
						{
							Debug.Log("Enemy" + m_PlayerTurn + " skip the purchasing of a pocket card"); 
							Players[m_PlayerTurn].FinishPurchasing = true; 
							
							CommonMemoryLog.LogMemory(m_PlayerTurn,PlayerAction.Forfeit,0,null,Betting.RaiseMadeCount,"Forfeit purchasing card");
							
							PassToNextPlayer(); 
							m_AuctionRound++;
						} 
						#endregion
					}
					
					#region Once all player had purchased/forfeit, proceed to the next phase of auction (End phase)
					if(Auction.HasPlayersDoneWithPurchasing())
					{
						AnnouncementText.enabled = false; AnnouncementText.text = ""; 
						m_AuctionPhase = AuctionPhase.End;
					}
					#endregion
				}
				else if(m_AuctionPhase == AuctionPhase.End)
				{
					PassToNextPlayer();
					
					#region Reset the various UI elements
					m_RaiseSlider.enabled = false;
					m_PlayerAllInButton.GetComponent<Button>().enabled = false;
					m_PlayerLeftButton.GetComponentInChildren<Text>().text = "Confirm";
					m_PlayerRightButton.GetComponentInChildren<Text>().text = "Revert";
					#endregion
					
					#region Reset various game-logic and player variables
					Table.SelectedCard = null;
					Table.SelectedCardPoolSlot = null;
					m_AuctionRound = 0;
					
					for(int i = 0; i < Players.Length; i++)
					{
						Players[i].SelectedCard = null; 
						Players[i].SelectedCardSlot = null;
					}
					#endregion
					
					m_AuctionPhase = AuctionPhase.NULL;
					m_Phase = TurnPhase.Swapping;
					Debug.Log("Current Phase: " + Phase);
				}
			}
			else if(m_Phase == TurnPhase.Swapping)
			{
				if(Players[m_PlayerTurn].Human)
				{
					#region Layout the cards in the player pocket on the table, allowing the player to swap the cards to/from pocket and hand
					if(Table.Pool.Count <= 0)
					{
						for(int i = 0; i < Players[m_PlayerTurn].Pocket.Count; i++){Table.Pool.Add(Players[m_PlayerTurn].Pocket[i]);} 
						Table.RefreshPoolForSwapping(); 
						Players[m_PlayerTurn].StoreCurrentHand();
					}
					#endregion
					
					#region Check if player had confirm the swapping. If so, remove all cards on pool and pass to next player
					if(m_Control.LeftButtonPressed)
					{
						Debug.Log("Swap button pressed"); 
						Table.CleanCardPoolCards(); 
						PassToNextPlayer(); 
						m_AuctionRound++; 
						m_Control.LeftButtonPressed = false;
						//						return;
					}
					#endregion
					#region Check if player had cancel the swapping. If so, revert player hand to pre-swap and remove the cards from the table then pass to next player
					else if(m_Control.RightButtonPressed)
					{
						Debug.Log("Cancel button pressed"); 
						Table.CleanCardPoolCards(); 
						Players[m_PlayerTurn].ReturnToPreviousHand(); 
						PassToNextPlayer(); 
						m_AuctionRound++;
						m_Control.RightButtonPressed = false;
						//						return;
					}
					#endregion
				}
				else
				{
					Debug.Log("Enemy " + (Players[m_PlayerTurn].Index - 1) + "'s pre-sort hand: ");
					Debug.Log("Hand type: " + Evaluator.EvaluateHand(Players[m_PlayerTurn].Hand));
					Debugger.PrintCards(Players[m_PlayerTurn].Hand);
					
					Debug.Log("Enemy " + (Players[m_PlayerTurn].Index - 1) + "'s pre-sort pocket: ");
					Debugger.PrintCards(Players[m_PlayerTurn].Pocket);
					
					Players[m_PlayerTurn].EnemyAI.SortOutHand();
					
					Debug.Log("Enemy " + (Players[m_PlayerTurn].Index - 1) + "'s post-sort hand: ");
					Debug.Log("Hand type: " + Evaluator.EvaluateHand(Players[m_PlayerTurn].Hand));
					Debugger.PrintCards(Players[m_PlayerTurn].Hand);
					
					Debug.Log("Enemy " + (Players[m_PlayerTurn].Index - 1) + "'s post-sort pocket: ");
					Debugger.PrintCards(Players[m_PlayerTurn].Pocket);
					
					PassToNextPlayer();
					m_AuctionRound++;
					//					return;
				}
				
				#region Once all players have gone through the swapping phase, transition into the showdown phase of the round
				if(m_AuctionRound >= Utility.HowManyValidPlayersLeft(this))
				{
					for(int i = 0; i < Players.Length; i++){Players[i].CleanPocket();} 
					m_Phase = TurnPhase.Showdown;
				}
				#endregion
			}
			else if(m_Phase == TurnPhase.Showdown)
			{
				if(mb_ShowdownStarted == false)
				{
					StartCoroutine(ShowdownCorountine());
				}	
			}
			else if(m_Phase == TurnPhase.End)
			{
				if(HasGameEnded())
					mb_GameOngoing = false;
				else
				{
					#region Reset the variables of all the players
					for(int i = 0; i < Players.Length; i++)
					{
						Players[i].OnTheBet = 0;
						Players[i].RaiseValue = 0;
						Players[i].UpdateFinancialStatus();
						Players[i].BettingAction = BettingDecision.NULL;
						Players[i].Called = false;
						Players[i].Checked = false;
						Players[i].Fold = false;
						Players[i].FoldText.enabled = false;
						Players[i].GetSidePotOnly = false;
					}
					#endregion
					
					CommonMemoryLog.RaiseInBet = 0;
					mb_ShowdownStarted = false;
					
					m_Betting.ShiftDealerAndBinds();
					Table.CleanCardPoolCards();
					
					m_Phase = TurnPhase.Initial;
				}
				
				Betting.FirstBetAmount = 0.0f;
				Betting.LatestRaiseAmount = 0.0f;
				Betting.BetMadeThisRound = false;
				Betting.CurrentBet = 0;
				Betting.RaiseMadeCount = 0;
				mb_WinInBetting = false;
			}
		}
		else if(!mb_GameOngoing && !mb_TurnDelayRunning)
		{
			Player Winner = GetOverallWinner();
			AnnouncementText.enabled = true;
			
			if(Winner.Index == 1)
				AnnouncementText.text = "Player Wins !";
			else
				AnnouncementText.text = "Enemy " + (Winner.Index - 1) + " Wins !";
		}
	}
	
	public Player Showdown()
	{
		Debug.Log("Showdown");
		
		for(int i = 1; i < m_Players.Length; i++)
		{
			if(!m_Players[i].Fold && !m_Players[i].Busted)
				m_Players[i].ShowHand();
		}
		
		Player Winner = Evaluator.EvaluateHandWinner(Players[0],Evaluator.EvaluateHandWinner(Players[1],Evaluator.EvaluateHandWinner(Players[2],Players[3])));
		
		return Winner;
	}
	
	public void PrintAllPlayersHand()
	{
		for(int i = 0; i < Players.Length; i++)
		{
			Debug.Log("Player " + (i+1) + ": "); 
			Players[i].PrintHand();
		}
		
		Debug.Log("Player 1's handtype : " + Evaluator.EvaluateHand(Players[0].Hand));
		Debug.Log("Player 2's handtype : " + Evaluator.EvaluateHand(Players[1].Hand));
		Debug.Log("Player 3's handtype : " + Evaluator.EvaluateHand(Players[2].Hand));
		Debug.Log("Player 4's handtype : " + Evaluator.EvaluateHand(Players[3].Hand));
		Debug.Log("Winning player: " + "PLAYER " + Evaluator.EvaluateHandWinner(Players[0],Evaluator.EvaluateHandWinner(Players[1],Evaluator.EvaluateHandWinner(Players[2],Players[3]))).Index);
	}
	
	public bool IsBettingCompleted()
	{
		for(int i = 0; i < m_Players.Length; i++)
		{
			if(!m_Players[i].Busted &&  !m_Players[i].Fold && !m_Players[i].Checked) 
				return false;
		}
		return true;
	}
	
	private IEnumerator ShowdownCorountine()
	{
		mb_ShowdownStarted = true;
		
		#region Disable any unneccessary UI for showdown phase
		DisableHandTypeCounter();
		Table.CleanCardPoolCards();
		#endregion
		
//		PrintAllPlayersHand();
		
		Player WinningPlayer = Showdown();	
		
		yield return new WaitForSeconds(1.0f);
		
		#region Move and rotate the turn pointer towards the winning player
		m_TurnPointer.localPosition = m_PointerPositions[WinningPlayer.Index - 1];
		m_TurnPointer.eulerAngles   = m_PointerRotations[WinningPlayer.Index - 1];
		#endregion
		
		#region Declare winner of the round using text-on-screen
		AnnouncementText.enabled = true;
		if(WinningPlayer.Index == 1)
			AnnouncementText.text = "Player win !";
		else
			AnnouncementText.text = "Enemy " + (WinningPlayer.Index - 1) + " win !";
		#endregion
		
		CheckForPlayerThatGetSidePot();
		
		if(Players[WinningPlayer.Index - 1].GetSidePotOnly)//BROKEN: RESULT IN MORE MONEY THAN BEFORE ON THE TABLE//
		{
			int WinningPlayerBet = Players[WinningPlayer.Index - 1].OnTheBet;
			int MoneyWon = 0;
			
			for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
			{
				if(PlayerIndex != WinningPlayer.Index - 1 && WinningPlayerBet < Players[PlayerIndex].OnTheBet)
				{
					Players[WinningPlayer.Index - 1].Stack += WinningPlayerBet;
					
					Players[PlayerIndex].Stack   += Players[PlayerIndex].OnTheBet - WinningPlayerBet;
					CommonMemoryLog.LogMemory(PlayerIndex,PlayerAction.Win, Players[PlayerIndex].OnTheBet - WinningPlayerBet,null,Betting.RaiseMadeCount,"");
					Players[PlayerIndex].OnTheBet = 0;
					
					MoneyWon += WinningPlayerBet;
				}
				else if(PlayerIndex != WinningPlayer.Index - 1 && WinningPlayerBet >= Players[PlayerIndex].OnTheBet)
				{
					Players[WinningPlayer.Index - 1].Stack += Players[PlayerIndex].OnTheBet;
					MoneyWon += Players[PlayerIndex].OnTheBet; 
					
					Players[PlayerIndex].OnTheBet           = 0;	
				}
			}
			
			Players[WinningPlayer.Index - 1].Stack   += Players[WinningPlayer.Index - 1].OnTheBet;
			Players[WinningPlayer.Index - 1].OnTheBet = 0;
			
			Table.Pot = 0;
			CommonMemoryLog.LogMemory(WinningPlayer.Index - 1,PlayerAction.Win,MoneyWon,null,Betting.RaiseMadeCount,"");
		}
		else
		{
			int MoneyWon = 0;
			
			for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
			{
				Players[WinningPlayer.Index - 1].Stack += Players[PlayerIndex].OnTheBet;
				MoneyWon += Players[PlayerIndex].OnTheBet;
				Players[PlayerIndex].OnTheBet           = 0;
			}
			
			CommonMemoryLog.LogMemory(WinningPlayer.Index - 1, PlayerAction.Win,MoneyWon,null,Betting.RaiseMadeCount,"");
			
			Table.Pot = 0;
		}
		
		yield return new WaitForSeconds(0.5f);
		
		#region Declare whether a player has been busted or not
		for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
		{
			if(Players[PlayerIndex].Stack <= 0 && !Players[PlayerIndex].Busted)
			{
				m_TurnPointer.localPosition = m_PointerPositions[Players[PlayerIndex].Index - 1];
				m_TurnPointer.eulerAngles   = m_PointerRotations[Players[PlayerIndex].Index - 1];
				
				if(Players[PlayerIndex].Index == 1)
					AnnouncementText.text = "Player Busted !";
				else
					AnnouncementText.text = "Enemy " + (Players[PlayerIndex].Index - 1) + " Busted !";
				
				yield return new WaitForSeconds(0.5f);
				
				Players[PlayerIndex].Busted = true;
				Players[PlayerIndex].ClearCardslotsSprite();
				Players[PlayerIndex].BustedText.enabled = true;
			}
		}
		#endregion
		
		AnnouncementText.text = " ";
		AnnouncementText.enabled = true;
		
		m_Phase = TurnPhase.End;
	}
	
	private void PassToNextPlayer()
	{
		if(HasAuctionForCardEnd())
			return;

		if(m_PlayerTurn < 3)
			m_PlayerTurn++;
		else
			m_PlayerTurn = 0;

		while(Players[m_PlayerTurn].Busted || Players[m_PlayerTurn].Fold || Players[m_PlayerTurn].Forfeit)
		{
			if(m_PlayerTurn < 3)
				m_PlayerTurn++;
			else
				m_PlayerTurn = 0;	
		}

		RefreshPointer();

		if(Utility.HowManyValidPlayersLeft(this) == 1 && !mb_WinInBetting)
			StartCoroutine(WinInBettingCorountine());
		
		if(!mb_TurnDelayRunning)
			StartCoroutine(TurnDelay());

		Debug.Log("Current Player Turn: " + m_PlayerTurn);
//		if(HasAuctionForCardEnd())
//			return;
//		
//		if(m_PlayerTurn < 3)
//			m_PlayerTurn++;
//		else
//			m_PlayerTurn = 0;
//		
//		RefreshPointer();
//		
//		if(Players[m_PlayerTurn].Busted || Players[m_PlayerTurn].Fold || Players[m_PlayerTurn].Forfeit)// || Players[m_PlayerTurn].CompletedAuction)
//		{
//			PassToNextPlayer(); 
//		}
//		
//		//		if(m_PlayerTurn  == 0)
//		//			Debug.Log("Passing to next player ! Player" );
//		//		else
//		//			Debug.Log("Passing to next player ! Enemy: " + (Players[m_PlayerTurn].Index - 1));
//		
//		m_TurnIndex++;
//		
////		Debug.Log("Amount Of valid players left: " + Utility.HowManyValidPlayersLeft(this));
//		if(Utility.HowManyValidPlayersLeft(this) == 1 && !mb_WinInBetting)
//		{	
//			StartCoroutine(WinInBettingCorountine());
////			Debug.Log("Transitioning To EndPhase");
//		}
//		
//		Debug.Log("Current Player Turn: " + m_PlayerTurn);
//		if(!mb_TurnDelayRunning)
//			StartCoroutine(TurnDelay());
		
	}
	
	private void RefreshPointer()
	{
		m_TurnPointer.localPosition = m_PointerPositions[m_PlayerTurn];
		m_TurnPointer.eulerAngles = m_PointerRotations[m_PlayerTurn];
	}
	
	private bool HasGameEnded()
	{
		int Busted = 0;
		for(int i = 0; i < Players.Length; i++)
		{
			if(Players[i].Busted){Busted++;}
		}
		
		return Busted >= 3 ? true : false;
	}
	
	private Player GetAuctionWinner()
	{
		for(int i = 0; i < Players.Length; i++)
		{
			if(!Players[i].Busted && !Players[i].Forfeit){return Players[i];}
		}
		return Players[0];
	}
	
	private Player GetOverallWinner()
	{
		for(int i = 0; i < Players.Length; i++)
		{
			if(!Players[i].Busted){return Players[i];}
		}
		return Players[0];
	}
	
	public void UpdateHandTypeCounter()
	{
		HandTypeText.text = Evaluator.EvaluateHand(Players[0].Hand).ToString();
	}
	
	public void DisableHandTypeCounter()
	{
		HandTypeText.text = " ";
	}
	
	public float GetCurrentPlayerBet()
	{
		return m_Players[m_PlayerTurn].RaiseValue;
	}
	
	public int GetNonBustedPlayerAmount()
	{
		int amount = 0;
		for(int i = 0; i < Players.Length; i++){if(!Players[i].Busted && !Players[i].Fold && !Players[i].Forfeit){amount++;}}
		return amount;
	}
	
	public bool HasAuctionForCardEnd()
	{
		for(int i = 0; i < Players.Length; i++)
		{
			if(Players[i].SelectedCardForAuction == null && !Players[i].Busted && !Players[i].Forfeit && !Players[i].Fold)
				return false;
		}
		
		//		for(int i = 0; i < Table.Pool.Count; i++)
		//		{
		//			if(Table.Pool[i].BeingBet == true)
		//				return false;
		//		}
		
		//		Debug.Log("Auction phase has ended !");
		return true;
	}
	
	public void SetCardBeingBet(Card _card, bool _trueornot)
	{
		for(int i = 0; i < Table.Pool.Count; i++)
		{
			if(Table.Pool[i].Suit == _card.Suit && Table.Pool[i].Value == _card.Value)
			{
				Table.Pool[i].BeingBet = _trueornot;
				return;
			}
		}
	}
	
	private void CheckForPlayerThatGetSidePot()
	{
		int HighestBetOnBoard = Table.GetHighestBet();
		Debug.Log("Highest bet on board by Player " + (GetPlayerThroughBet(HighestBetOnBoard).Index) + ": " + Table.GetHighestBet());
		for(int i = 0; i < Players.Length; i++)
		{
			if(!Players[i].Busted && !Players[i].Fold && Players[i].OnTheBet < HighestBetOnBoard) 
			{
				Debug.Log("Player " + Players[i].Index + " side-betting ! Their original bet: " + Players[i].OnTheBet);
				Players[i].GetSidePotOnly = true;
			}
		}
	}
	
	private Player GetPlayerThroughBet(int _Betting)
	{
		for(int i = 0; i < Players.Length; i++)
		{
			if(Players[i].OnTheBet == _Betting){return Players[i]; break;}
		}
		return Players[0];
	}
	
	public int EligiblePlayerCount()
	{
		int EligibleCount = 0;
		for(int i = 0; i < Players.Length; i++)
		{
			if(!Players[i].Busted && !Players[i].Fold && !Players[i].Forfeit)
				EligibleCount++;
		}
		return EligibleCount;
	}
	
	public int FindPlayerRankOnTable(Player _Player)
	{
		List<Player> PlayerRanking = new List<Player>();
		for(int i = 0; i < m_Players.Length; i++)
		{
			if(!m_Players[i].Busted && !m_Players[i].Fold)
				PlayerRanking.Add(m_Players[i]);
		}
		
		bool NoSorting = true;
		Player SwappingPlaceHolder = PlayerRanking[0];
		for(int i = 0; i < PlayerRanking.Count; i++)
		{
			NoSorting = true;
			for(int j = 0; j < PlayerRanking.Count - 1; j++)
			{
				if((PlayerRanking[j].OnTheBet + PlayerRanking[j].Stack) < (PlayerRanking[j+1].OnTheBet + PlayerRanking[j+1].Stack))
				{
					SwappingPlaceHolder = PlayerRanking[j];
					PlayerRanking[j] = PlayerRanking[j+1];
					PlayerRanking[j+1] = SwappingPlaceHolder;
					NoSorting = false;
				}
			}
			if(NoSorting)
				break;
		}
		
		for(int i = 0; i < PlayerRanking.Count; i++)
		{
			if(PlayerRanking[i].Index == _Player.Index)
				return i;
		}
		
		return PlayerRanking.Count - 1;
	}
	
	//Check whether other players except the one current in turn had folded
	public bool HasOtherPlayersFolded()
	{
		for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
		{
			if(PlayerIndex != m_PlayerTurn && !Players[PlayerIndex].Fold)
				return false;
		}
		return true;
	}
	
	private IEnumerator WinInBettingCorountine()
	{
		mb_WinInBetting = true;		
		
		DisableHandTypeCounter();
		Table.CleanCardPoolCards();
		
		Player CurrentPlayer = Players[m_PlayerTurn];
		
		m_TurnPointer.localPosition = m_PointerPositions[CurrentPlayer.Index - 1];
		m_TurnPointer.eulerAngles   = m_PointerRotations[CurrentPlayer.Index - 1];
		
		AnnouncementText.enabled = true;
		if(CurrentPlayer.Index == 1)
			AnnouncementText.text = "Player has won as all other players folded !";
		else
			AnnouncementText.text = "Enemy " + (CurrentPlayer.Index - 1) + " has won as all other players folded !";
		
		for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
		{
			if(PlayerIndex != CurrentPlayer.Index - 1)
			{
				Players[CurrentPlayer.Index - 1].Stack += Players[PlayerIndex].OnTheBet;
				Players[PlayerIndex].OnTheBet           = 0;
			}
			else
			{
				Players[CurrentPlayer.Index - 1].Stack   += Players[CurrentPlayer.Index - 1].OnTheBet;
				Players[CurrentPlayer.Index - 1].OnTheBet = 0;			
			}
		}
		
		Table.Pot = 0;
		
		yield return new WaitForSeconds(1.0f);
		
		for(int PlayerIndex = 0; PlayerIndex < Players.Length; PlayerIndex++)
		{
			if(Players[PlayerIndex].Stack <= 0 && !Players[PlayerIndex].Busted)
			{
				m_TurnPointer.localPosition = m_PointerPositions[Players[PlayerIndex].Index - 1];
				m_TurnPointer.eulerAngles   = m_PointerRotations[Players[PlayerIndex].Index - 1];
				
				if(Players[PlayerIndex].Index == 1)
					AnnouncementText.text = "Player Busted !";
				else
					AnnouncementText.text = "Enemy " + (Players[PlayerIndex].Index - 1) + " Busted !";
				
				yield return new WaitForSeconds(1.0f);
				
				Players[PlayerIndex].ClearCardslotsSprite();
				Players[PlayerIndex].BustedText.enabled = true;
				Players[PlayerIndex].Busted = true;
			}
		}
		
		AnnouncementText.text = " ";
		AnnouncementText.enabled = false;
		
		m_Phase = TurnPhase.End;
	}
	
	public IEnumerator TurnDelay()
	{
		mb_TurnDelayRunning = true;
		yield return new WaitForSeconds(m_TurnDelay);
		mb_TurnDelayRunning = false;
	}
}
