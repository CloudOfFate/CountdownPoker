using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Debugger : MonoBehaviour {
	
	private GameManager GM;
	
	// Use this for initialization
	void Start () 
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	public static void CheckEmpty<varType>(varType _Variable)
	{
		if (_Variable == null)
		{
			Debug.Log("EmptyTest: Variable is empty");
		}
		else
		{
			Debug.Log("EmptyTest: Variable is not empty");
		}
	}
	
	public static void PrintCards(List<Card> _Cards)
	{
		if(_Cards.Count == 5)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value +
			          "    Card 2: " + (_Cards[1].Suit.ToString())[0] + " " + _Cards[1].Value +
			          "    Card 3: " + (_Cards[2].Suit.ToString())[0] + " " + _Cards[2].Value +
			          "    Card 4: " + (_Cards[3].Suit.ToString())[0] + " " + _Cards[3].Value +
			          "    Card 5: " + (_Cards[4].Suit.ToString())[0] + " " + _Cards[4].Value );
		}
		else if(_Cards.Count == 4)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value +
			          "    Card 2: " + (_Cards[1].Suit.ToString())[0] + " " + _Cards[1].Value +
			          "    Card 3: " + (_Cards[2].Suit.ToString())[0] + " " + _Cards[2].Value +
			          "    Card 4: " + (_Cards[3].Suit.ToString())[0] + " " + _Cards[3].Value);
		}
		else if(_Cards.Count == 3)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value +
			          "    Card 2: " + (_Cards[1].Suit.ToString())[0] + " " + _Cards[1].Value +
			          "    Card 3: " + (_Cards[2].Suit.ToString())[0] + " " + _Cards[2].Value);
		}
		else if(_Cards.Count == 2)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value +
			          "    Card 2: " + (_Cards[1].Suit.ToString())[0] + " " + _Cards[1].Value);
		}
		else if(_Cards.Count == 1)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value);
		}
		else
		{
			Debug.Log("No card in pocket !");
		}
	}
	
	public static void PrintCards(Card[] _Cards)
	{
		if(_Cards.Length == 5)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value +
			          "    Card 2: " + (_Cards[1].Suit.ToString())[0] + " " + _Cards[1].Value +
			          "    Card 3: " + (_Cards[2].Suit.ToString())[0] + " " + _Cards[2].Value +
			          "    Card 4: " + (_Cards[3].Suit.ToString())[0] + " " + _Cards[3].Value +
			          "    Card 5: " + (_Cards[4].Suit.ToString())[0] + " " + _Cards[4].Value );
		}
		else if(_Cards.Length == 4)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value +
			          "    Card 2: " + (_Cards[1].Suit.ToString())[0] + " " + _Cards[1].Value +
			          "    Card 3: " + (_Cards[2].Suit.ToString())[0] + " " + _Cards[2].Value +
			          "    Card 4: " + (_Cards[3].Suit.ToString())[0] + " " + _Cards[3].Value);
		}
		else if(_Cards.Length == 3)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value +
			          "    Card 2: " + (_Cards[1].Suit.ToString())[0] + " " + _Cards[1].Value +
			          "    Card 3: " + (_Cards[2].Suit.ToString())[0] + " " + _Cards[2].Value);
		}
		else if(_Cards.Length == 2)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value +
			          "    Card 2: " + (_Cards[1].Suit.ToString())[0] + " " + _Cards[1].Value);
		}
		else if(_Cards.Length == 1)
		{
			Debug.Log("Card 1: " + (_Cards[0].Suit.ToString())[0] + " " + _Cards[0].Value);
		}
		else
		{
			Debug.Log("No card in pocket !");
		}
	}
}
