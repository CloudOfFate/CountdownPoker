using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dealer 
{
	private GameManager m_GM;
	public  GameManager GM {get{return m_GM;}}
	
	public Dealer(GameManager _GM)
	{
		m_GM = _GM;
	}
	
	public void DealCardToPool()
	{
		GM.Table.Pool.Add(GM.Deck.DrawSingle());
	}
	
	public void DealCardsToPool()
	{
		for(int i = 0; i < 5; i++)
			DealCardToPool();
	}
	
	public void AddCardToPlayerPocket(Player _Player)
	{
		_Player.AddCardToPocket(GM.Deck.DrawSingle());
	}
	
	public void DealHandToPlayer(Player _Player)
	{
		_Player.AddCardsToHand(GM.Deck.DrawMulti(5));
	}
	
	public void DealHandToAllPlayers()
	{
		for(int i = 0; i < GM.Players.Length; i++)
		{
			if(!GM.Players[i].Busted)
				GM.Players[i].AddCardsToHand(GM.Deck.DrawMulti(5));
		}
	}
}
