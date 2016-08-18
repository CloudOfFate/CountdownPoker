using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatisticTest : MonoBehaviour
 {
	private Deck ExperimentDeck;
	private List<Card> Hand;

	public int HandSize = 5;

	public int LoopNum = 1;


	public bool InProgress = false;

	public bool AutoLoop = false;

	public bool StartTest = false;

 	void Start () 
	{
		ExperimentDeck = new Deck();
		Hand = new List<Card>();

		ExperimentDeck.Reset();
		ExperimentDeck.Shuffle();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(AutoLoop)
		{
			if(StartTest)
			{
				ExperimentDeck.Reset();
				ExperimentDeck.Shuffle();
				DealHand();
				PrintHand();

				LoopNum--;
				if(LoopNum <= 0)
				{
					LoopNum = 1;
					StartTest = false;
					AutoLoop = false;
				}
			}
		}
		else
		{
			if(StartTest)
			{
				ExperimentDeck.Reset();
				ExperimentDeck.Shuffle();
				DealHand();
				PrintHand();
				StartTest = false;
			}
		}
	}

	void DealHand()
	{
		Hand.Clear();

		for(int CardNum = 0; CardNum < HandSize; CardNum++)
		{
			Hand.Add(ExperimentDeck.DrawSingle());
		}
	}

	void PrintHand()
	{
		int Index = 0;
		for(int i = 0; i < HandSize; i++)
		{
			if(Hand[i].Value > Hand[Index].Value)
				Index = i;
		}

		Hands Handtype = Evaluator.EvaluateHand(Hand);
		if(Handtype == Hands.HighCard)
			Debug.Log("Hand Type: " + Handtype + " Highest Card: " + Hand[Index].Value); 
		else if(Handtype == Hands.OnePair)
		{
			Values PairValue = Values.Ace;
			for(int a = 0; a < Hand.Count; a++)
			{
				for(int b = 0; b < Hand.Count; b++)
				{
					if(Hand[b] != Hand[a] && Hand[b].Value == Hand[a].Value)
					{
						PairValue = Hand[a].Value;
						break;
					}
				}
			}
			Debug.Log("Hand Type: " + Handtype + " Pair Value: " + PairValue);
		}
		else
		{
			Debug.Log("Hand Type: " + Handtype);
		}

		string PrintMessage = "Dealt: ";
		for(int CardNum = 0; CardNum < HandSize; CardNum++)
		{
			PrintMessage += Hand[CardNum].Suit.ToString() + Hand[CardNum].Value.ToString() + " ";
		}
		Debug.Log(PrintMessage);
	}
}
