using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck {

	private static List<Card> m_Reference;
	private List<Card> m_Cards;
	private static Dictionary<Vector2,Sprite> m_CardsSprites; 

	public List<Card> Cards {get{return m_Cards;} set{m_Cards = value;}}
	public static Dictionary<Vector2,Sprite> CardsSprite {get {return m_CardsSprites;}}
	
	public Deck()
	{
		m_Reference = new List<Card>();
		m_CardsSprites = new Dictionary<Vector2, Sprite>();
		GenerateDeck();
		Shuffle();
	}
	
	private void GenerateDeck()
	{
		//Generate a deck's data 
		int nCardCount = 0;
		for(int a = 0; a < 4; a++)
		{
			for(int b = 0; b < 13; b++)
			{
				m_Reference.Add(new Card((Suits)a,(Values)b));
				nCardCount++;
			}
		}
		Reset();

		//Generate a deck's sprites
		for(int i = 0; i < 4; i++)
		{
			for(int j = 0; j < 13; j++)
			{
				m_CardsSprites.Add(new Vector2(i,j),SpriteRepository.GetSpriteFromSheet("Sprites/Poker cards sprite_" + (i * 13 + j).ToString()));
			}
		}
		m_CardsSprites.Add(new Vector2(3,13),SpriteRepository.GetSpriteFromSheet("Sprites/Poker cards sprite_" + (3 * 13 + 13).ToString()));
	}
	
	public void Reset()
	{
		m_Cards = new List<Card>(m_Reference);
	}
	
	public void Shuffle()
	{
		for(int i = 0; i < m_Cards.Count; i++)
		{
			Card CurrentCard = m_Cards[i];
			int RandomCardIndex = Random.Range(i,m_Cards.Count);
			m_Cards[i] = m_Cards[RandomCardIndex];
			m_Cards[RandomCardIndex] = CurrentCard;
		}
	}
	
	public Card DrawSingle()
	{
		Card CardDrawn = m_Cards[0];
		m_Cards.RemoveAt(0);
		
		return CardDrawn;
	}
	
	public Card[] DrawMulti(int _CardAmt)
	{
		Card[] CardsDrawn = new Card[_CardAmt];
		
		for(int i = 0; i < _CardAmt; i++)
		{
			Card CardDrawn = m_Cards[0];
			m_Cards.RemoveAt(0);
			
			CardsDrawn[i] = CardDrawn;
		}
		
		return CardsDrawn;
	}
	
	private void PrintDeckCards()
	{
		for(int i = 0; i < m_Cards.Count; i++)
		{
			Debug.Log("Card " + i + ": " + m_Cards[i].Suit + m_Cards[i].Value);
		}
	}

	public void RemoveSpecificCard (Card _SpecifiedCard)
	{
		for(int CardIndex = 0; CardIndex < m_Cards.Count; CardIndex++)
		{
			if(m_Cards[CardIndex].Suit == _SpecifiedCard.Suit && m_Cards[CardIndex].Value == _SpecifiedCard.Value)
			{
				m_Cards.RemoveAt(CardIndex);
				return;
			}
		}
	}
}
