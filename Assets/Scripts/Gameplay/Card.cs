using UnityEngine;
using System.Collections;

public class Card 
{
	private Suits  m_Suit;
	private Values m_Value;
	private bool   m_BeingBet;
	
	public Suits  Suit{get{return m_Suit;} set{m_Suit = value;}}
	public Values Value{get{return m_Value;} set{m_Value = value;}}
	public bool   BeingBet{get{return m_BeingBet;} set{m_BeingBet = value;}}
	
	public Card (Suits _Suit, Values _value)
	{
		m_Suit  = _Suit;
		m_Value = _value;
	}	
}
