using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour 
{
	private GameManager GM;
	private Player      MainPlayer;
	private Button      m_AllInButton;
	private bool        mb_LeftButtonPressed;
	private bool        mb_RightButtonPressed;
	private bool        mb_AllInPressed;

	public Button AllInButton{get{return m_AllInButton;} set{m_AllInButton = value;}}
	public bool   LeftButtonPressed{get{return mb_LeftButtonPressed;} set{mb_LeftButtonPressed = value;}}
	public bool   RightButtonPressed{get{return mb_RightButtonPressed;} set{mb_RightButtonPressed = value;}}
	public bool   AllInPressed{get{return mb_AllInPressed;} set{mb_AllInPressed = value;}}

	// Use this for initialization
	void Start () 
	{
		GM = GetComponent<GameManager>();
		MainPlayer = GM.Players[0];
		
		LeftButtonPressed = false;
		RightButtonPressed = false;
		AllInPressed = false;
		
		m_AllInButton = GameObject.Find("AllIn Button").GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
			ActionSelectCard();
	}
	
	private void ActionSelectCard()
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		
		if(hit.transform == null || !GM.Selecting)
			return;

		if(hit.transform.gameObject.tag == "CardPool" && hit.transform.GetComponent<SpriteRenderer>().sprite != null && GM.Phase == TurnPhase.Auctioning)
		{
			Debug.Log("Selected Pool's card !"); 
			GM.Table.ResetPoolSlotSize();
			
			GM.Table.SelectedCardPoolSlot = hit.transform.gameObject;

			if(GM.AuctionPhase == AuctionPhase.First)
				GM.Table.SelectedCard = GM.Table.Pool[int.Parse(GM.Table.SelectedCardPoolSlot.name.Substring(10)) - 1];
			else if(GM.AuctionPhase == AuctionPhase.Second)
				GM.Table.SelectedCard = GM.Table.Pool[int.Parse(GM.Table.SelectedCardPoolSlot.name.Substring(10)) - 2];

			GM.Table.SelectedCardPoolSlot.transform.localScale *= 1.25f;
		}
		
		if(hit.transform.gameObject.tag == "CardPool" && hit.transform.GetComponent<SpriteRenderer>().sprite != null && GM.Phase == TurnPhase.Swapping)
		{
			GameObject CardSlotClicked = hit.transform.gameObject;
			
			//if player had not selected any card from his hand to swap, store the currently selected card from the card pool for future swapping with a card from the player hand
			if(GM.Table.SelectedCardPoolSlot == null)
			{
				GM.Table.SelectedCardPoolSlot = CardSlotClicked;
				Debug.Log("Selected a card from pool");
			}
			
			if(MainPlayer.SelectedCardSlot == null && GM.Table.SelectedCardPoolSlot != null)
				GM.Table.SelectedCardPoolSlot = CardSlotClicked;
			
			//if player had selected a card from his hand before he selected the current card from the cardpool, perform the card swap
			if(MainPlayer.SelectedCardSlot != null && GM.Table.SelectedCardPoolSlot != null)
			{
				int PlayerHandIndex = int.Parse(MainPlayer.SelectedCardSlot.name.Substring(10)) - 1;
				int PlayerPoolIndex = 0;

				if(GM.Table.Pool.Count == 1)
					PlayerPoolIndex = 0;//even tho selected pool index is 2 but it is not the original index in the pool data structure
				else if(GM.Table.Pool.Count == 2)
				{
					if(GM.Table.SelectedCardPoolSlot.name.Contains("2"))
						PlayerPoolIndex = 0;
					else
						PlayerPoolIndex = 1;
				}
				else if(GM.Table.Pool.Count == 3)
				{
					if(GM.Table.SelectedCardPoolSlot.name.Contains("2"))
						PlayerPoolIndex = 0;
					else if(GM.Table.SelectedCardPoolSlot.name.Contains("3"))
						PlayerPoolIndex = 1;
					else if(GM.Table.SelectedCardPoolSlot.name.Contains("4"))
						PlayerPoolIndex = 2;
				}
			
				Card SelectedHandCard = GM.Players[MainPlayer.Index - 1].Hand[PlayerHandIndex];
				Card SelectedPoolCard = GM.Table.Pool[PlayerPoolIndex];
				
				GM.Players[MainPlayer.Index - 1].Hand[PlayerHandIndex] = SelectedPoolCard;
				GM.Table.Pool[PlayerPoolIndex] = SelectedHandCard;
				
				GM.Players[MainPlayer.Index - 1].RefreshPlayerHand();
				GM.Table.RefreshPoolForSwapping();
				
				MainPlayer.SelectedCardSlot = null;
				GM.Table.SelectedCardPoolSlot = null;

				GM.UpdateHandTypeCounter();

				Debug.Log("Selected a card from hand");
				Debug.Log("Swap the selected hand card and pool card with each other");
			}
		}
		if(hit.transform.gameObject.tag == "PlayerCards" && hit.transform.GetComponent<SpriteRenderer>().sprite != null && GM.Phase == TurnPhase.Swapping)
		{
			GameObject CardSlotClicked = hit.transform.gameObject;
			
			//if player has not selected any card from the card pool, store the currently selected card by the player from their hand
			if(MainPlayer.SelectedCardSlot == null)
				MainPlayer.SelectedCardSlot = CardSlotClicked;
			
			if(MainPlayer.SelectedCardSlot != null && GM.Table.SelectedCardPoolSlot == null)
				MainPlayer.SelectedCardSlot = CardSlotClicked;
			
			if(MainPlayer.SelectedCardSlot != null && GM.Table.SelectedCardPoolSlot != null)
			{
				int PlayerHandIndex = int.Parse(CardSlotClicked.name.Substring(10)) - 1;
				int PlayerPoolIndex = 0;

				if(GM.Table.Pool.Count == 1)
					PlayerPoolIndex = 0;//even tho selected pool index is 2 but it is not the original index in the pool data structure
				else if(GM.Table.Pool.Count == 2)
				{
					if(GM.Table.SelectedCardPoolSlot.name.Contains("2"))
						PlayerPoolIndex = 0;
					else
						PlayerPoolIndex = 1;
				}
				else if(GM.Table.Pool.Count == 3)
				{
					if(GM.Table.SelectedCardPoolSlot.name.Contains("2"))
						PlayerPoolIndex = 0;
					else if(GM.Table.SelectedCardPoolSlot.name.Contains("3"))
						PlayerPoolIndex = 1;
					else if(GM.Table.SelectedCardPoolSlot.name.Contains("4"))
						PlayerPoolIndex = 2;
				}
				
				Card SelectedHandCard = GM.Players[MainPlayer.Index - 1].Hand[PlayerHandIndex];
				Card SelectedPoolCard = GM.Table.Pool[PlayerPoolIndex];
				
				GM.Players[MainPlayer.Index - 1].Hand[PlayerHandIndex] = SelectedPoolCard;
				GM.Table.Pool[PlayerPoolIndex] = SelectedHandCard;
				
				GM.Players[MainPlayer.Index - 1].RefreshPlayerHand();
				GM.Table.RefreshPoolForSwapping();
				
				MainPlayer.SelectedCardSlot = null;
				GM.Table.SelectedCardPoolSlot = null;

				GM.UpdateHandTypeCounter();

				Debug.Log("Selected a card from hand");
				Debug.Log("Swap the selected hand card and pool card with each other");
			}
		}
		
	}
	
	public void ActionLeftButton()
	{
		if(GM.Phase == TurnPhase.Betting || GM.Phase == TurnPhase.Auctioning || GM.Phase == TurnPhase.Swapping)
		{
			Debug.Log("Left button pressed"); 
			mb_LeftButtonPressed = true;
		}
	}
	
	public void ActionRightButton()
	{
		if(GM.Phase == TurnPhase.Betting || GM.Phase == TurnPhase.Auctioning || GM.Phase == TurnPhase.Swapping)
		{
			Debug.Log("Right button pressed");
			mb_RightButtonPressed = true;
		}
	}
	
	public void ActionAllInButton()
	{
		if(GM.Phase == TurnPhase.Betting || GM.Phase == TurnPhase.Auctioning)
		{
			Debug.Log("All in pressed"); 
			mb_AllInPressed = true;
		}
	}
}
