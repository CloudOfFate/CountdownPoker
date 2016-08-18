using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AuctionManager 
{
	private GameManager GM;
	
	private int m_AuctionIndex;
	
	private Card m_AuctionCard;
	public Card AuctionCard{get {return m_AuctionCard;} set{m_AuctionCard = value;}}
	
	private int m_CostOfRandomCard;
	public int CostOfRandomCard{get{return m_CostOfRandomCard;} set{m_CostOfRandomCard = value;}}
	
	private Text[] m_CardPrices;
	public Text[] CardPrices{get{return m_CardPrices;} set{m_CardPrices = value;}}
	
	private int[] m_Prices;
	public int[] Prices{get{return m_Prices;} set{m_Prices = value;}}
	
	private bool m_PriceCheck;
	public bool PriceCheck{get{return m_PriceCheck;} set{m_PriceCheck = value;}}
	
	private int m_EffectiveStackForBidding;
	public int EffectiveStackForBidding {get{return m_EffectiveStackForBidding;} set{m_EffectiveStackForBidding = value;}}
	
	public AuctionManager(GameManager _GM)
	{
		GM = _GM;
		m_AuctionIndex = 0;
		m_CostOfRandomCard = 5;
		
		m_CardPrices = new Text[5];
		Transform CardPricesObject = GameObject.Find("CardPrices").transform;
		for(int i = 0; i < m_CardPrices.Length; i++)
		{
			m_CardPrices[i] = CardPricesObject.GetChild(i).GetComponent<Text>();
			m_CardPrices[i].enabled = false;
		}
		
		m_Prices = new int[5];
		m_PriceCheck = false;
	}
	
	public void DealAuctionCards(AuctionPhase _Phase)
	{
		if(_Phase == AuctionPhase.First)
		{
			for(int i = 0; i < 5; i++){GM.Dealer.DealCardToPool();}
		}
		else if(_Phase == AuctionPhase.Second)
		{
			for(int i = 0; i < 3; i++){GM.Dealer.DealCardToPool();}
		}
		
		GM.Table.RefreshPoolForAuction(_Phase);
	}
	
	public void SetCurrentAuctionCard()
	{
		m_AuctionCard = GM.Table.Pool[m_AuctionIndex];
		
		for(int i = 0; i < GM.Table.Pool.Count; i++)
		{
			if(i == m_AuctionIndex){GM.Table.PoolCardSlots[i].transform.localScale = new Vector3(6.0f,6.0f,1.0f);}
			else{GM.Table.PoolCardSlots[i].transform.localScale = new Vector3(5.0f,5.0f,1.0f);}
		}
		
		m_AuctionIndex++;
	}
	
	public bool HasAllCardBeenAuctioned()
	{
		return m_AuctionIndex >= GM.Table.Pool.Count ? true : false;
	}
	
	public void CoverNonBettedAuctionCards()
	{
		for(int i = 0; i < GM.Table.Pool.Count; i++)
		{
			if(!GM.Table.Pool[i].BeingBet)
			{
				Sprite ReplacementSprite = Deck.CardsSprite[new Vector2(3,13)];
				if(GM.AuctionPhase == AuctionPhase.First){GM.Table.PoolCardSlots[i].GetComponent<SpriteRenderer>().sprite = ReplacementSprite;}
				else if(GM.AuctionPhase == AuctionPhase.Second){GM.Table.PoolCardSlots[i+1].GetComponent<SpriteRenderer>().sprite = ReplacementSprite;}
			}
		}
	}
	
	public bool HasPlayersDoneWithPurchasing()
	{
		for(int i = 0; i < GM.Players.Length; i++)
		{
			if(!GM.Players[i].Busted && !GM.Players[i].Fold && !GM.Players[i].FinishPurchasing){return false;}
		}
		Debug.Log("All players have done purchasing");
		return true;
	}
	
	public void EnableCardPrices()
	{
		for(int i = 0; i < GM.Table.PoolCardSlots.Length; i++)
		{
			if(GM.Table.PoolCardSlots[i].GetComponent<SpriteRenderer>().sprite != null && GM.Table.PoolCardSlots[i].GetComponent<SpriteRenderer>().sprite != Deck.CardsSprite[new Vector2(3,13)])
			{
				m_CardPrices[i].enabled = true;
			}
		}
	}
	
	public void DisableCardPrices()
	{
		for(int i = 0; i < m_CardPrices.Length; i++){m_CardPrices[i].enabled = false;}
	}
	
	public void ResetCardPrices()
	{
		for(int i = 0; i < m_Prices.Length; i++)
		{
			m_Prices[i] = 0;
			m_CardPrices[i].text = m_CardPrices[i].text.Remove(1) + m_Prices[i].ToString();
		}
	}
	
	public void UpdateCardPrices()
	{
		Debug.Log("Update card prices");
		for(int PriceTxtIndex = 0; PriceTxtIndex < m_Prices.Length; PriceTxtIndex++)
		{
			if(m_CardPrices[PriceTxtIndex].enabled == true)
			{
				int HighestBetForCurrentCard = 0;
				for(int PlayerIndex = 0; PlayerIndex < GM.Players.Length; PlayerIndex++)
				{
					if(GM.Players[PlayerIndex].SelectedCardForAuction == null
					   || (GM.Players[PlayerIndex].SelectedCardForAuction.Suit == Suits.NULL && GM.Players[PlayerIndex].SelectedCardForAuction.Value == Values.NULL))
						continue;
					
					if(GM.AuctionPhase == AuctionPhase.First 
					   && !GM.Players[PlayerIndex].Busted && !GM.Players[PlayerIndex].Fold && !GM.Players[PlayerIndex].Forfeit 
					   && GM.Table.Pool.Count > PriceTxtIndex 
					   && Utility.IsTwoCardsIdentical(GM.Players[PlayerIndex].SelectedCardForAuction,GM.Table.Pool[PriceTxtIndex]) 
					   && GM.Players[PlayerIndex].PurchaseValue > HighestBetForCurrentCard)
					{
						HighestBetForCurrentCard = GM.Players[PlayerIndex].PurchaseValue;
					}
					else if(GM.AuctionPhase == AuctionPhase.Second 
					        && !GM.Players[PlayerIndex].Busted && !GM.Players[PlayerIndex].Fold && !GM.Players[PlayerIndex].Forfeit 
					        && GM.Table.Pool.Count > PriceTxtIndex 
					        && GM.Players[PlayerIndex].SelectedCardForAuction.Suit == GM.Table.Pool[PriceTxtIndex - 1].Suit && GM.Players[PlayerIndex].SelectedCardForAuction.Value == GM.Table.Pool[PriceTxtIndex - 1].Value 
					        && GM.Players[PlayerIndex].PurchaseValue > HighestBetForCurrentCard)
					{
						HighestBetForCurrentCard = GM.Players[PlayerIndex].PurchaseValue;
					}
				}
				m_Prices[PriceTxtIndex] = HighestBetForCurrentCard;
				m_CardPrices[PriceTxtIndex].text = m_CardPrices[PriceTxtIndex].text.Remove(1) + HighestBetForCurrentCard.ToString();
			}
		}
	}
	
	public bool IsPlayerCompetingThisCard(Card _card)
	{
		if(_card == null){return false;}
		
		for(int i = 0; i < GM.Players.Length; i++)
		{
			if(!GM.Players[i].Fold && !GM.Players[i].Forfeit && GM.Players[i].SelectedCardForAuction != null && (GM.Players[i].SelectedCardForAuction.Suit == null ||GM.Players[i].SelectedCardForAuction.Suit == _card.Suit) && (GM.Players[i].SelectedCardForAuction.Value == null ||GM.Players[i].SelectedCardForAuction.Value == _card.Value)){return true;}
		}
		return false;
	} 
	
	public bool HasAllPlayersBidForACard()
	{
		for(int PlayerIndex = 0; PlayerIndex < GM.Players.Length; PlayerIndex++)
		{
			if(!GM.Players[PlayerIndex].Busted && !GM.Players[PlayerIndex].Fold && !GM.Players[PlayerIndex].Forfeit)
			{
				if(GM.Players[PlayerIndex].SelectedCardForAuction == null 
				   || (GM.Players[PlayerIndex].SelectedCardForAuction.Suit == Suits.NULL && GM.Players[PlayerIndex].SelectedCardForAuction.Value == Values.NULL))
				{
					return false;
				}
			}
		}
		
		return true;
	}
	
	public int DetermineRankInPool(Card _Card)
	{
		List<Card> PoolRanking = new List<Card>(GM.Table.Pool);
		
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
			if(Utility.IsTwoCardsIdentical(_Card,PoolRanking[PoolIndex]))
			{
				BiddingCardRank = PoolIndex + 1;
				break;
			}
		}
		
		return BiddingCardRank;
	}
}
