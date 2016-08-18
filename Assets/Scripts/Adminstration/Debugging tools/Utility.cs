using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utility
{
	//Utility is a class that is used for debugging purposes wherey any function that is used for debugging 
	//can be added here
	
	//A function that is used to check whether a variable is empty or not and print different messages 
	//respectively
	public static void CheckEmpty<varType>(varType _Variable)
	{
		if (_Variable == null){Debug.Log("EmptyTest: Variable is empty");}
		else {Debug.Log("EmptyTest: Variable is not empty");}
	}
	
	//A function that is used to draw a cross on the screen based the position, color and size inputted through 
	//the perimeter
	public static void DrawCross(Vector2 _Pos, Color _Color, float _CrossSize)
	{
		//The four corners of the cross are calculated based on the position given and the size of the cross
		Vector2 topLeft = new Vector2(_Pos.x - _CrossSize, _Pos.y + _CrossSize);
		Vector2 topRight = new Vector2(_Pos.x + _CrossSize, _Pos.y + _CrossSize);
		Vector2 botLeft = new Vector2(_Pos.x - _CrossSize, _Pos.y - _CrossSize);
		Vector2 botRight = new Vector2(_Pos.x + _CrossSize, _Pos.y - _CrossSize);
		
		//Draw two lines on the screen from top left to bottom right and from top right to bottom left
		//to form a cross
		Debug.DrawLine(topLeft,botRight,_Color,Mathf.Infinity,true);
		Debug.DrawLine(topRight,botLeft,_Color,Mathf.Infinity,true);
	}
	
	public static void DrawLine(Vector2 _Start, Vector2 _End)
	{
		Debug.DrawLine(_Start,_End,Color.red,Mathf.Infinity,true);
	}
	
	public static void DebugVector(Vector2 _Pos)
	{
		Debug.Log(_Pos.ToString("F4"));
	}
	
	public static void DrawCircleCross(Vector2 _Center, float _Radius, Color _Color)
	{
		Debug.DrawLine(new Vector2(_Center.x - _Radius, _Center.y),new Vector2(_Center.x + _Radius, _Center.y),_Color,Mathf.Infinity,true);
		Debug.DrawLine(new Vector2(_Center.x, _Center.y + _Radius),new Vector2(_Center.x, _Center.y - _Radius),_Color,Mathf.Infinity,true);
	}
	
	public static float Distance(Vector2 _First, Vector2 _Second)
	{
		return (_First - _Second).sqrMagnitude;
	}

	public static void PrintListOfHands(List<Card[]> _Range)
	{
		for(int HandIndex = 0; HandIndex < _Range.Count; HandIndex++)
		{
			string Message = "";//"Hand " + (HandIndex) + ": ";
			
			for(int CardIndex = 0; CardIndex < _Range[HandIndex].Length; CardIndex++)
			{
				Message += _Range[HandIndex][CardIndex].Suit.ToString() + _Range[HandIndex][CardIndex].Value.ToString() + "  ";
			}
			
			Debug.Log(Message);
		}
	}

	public static void PrintArrayOfHands(Card[][] _Range)
	{
		for(int HandIndex = 0; HandIndex < _Range.Length; HandIndex++)
		{
			if(_Range[HandIndex] == null)
				break;

			string Message = "Hand " + (HandIndex) + ": ";
			
			for(int CardIndex = 0; CardIndex < _Range[HandIndex].Length; CardIndex++)
			{
				Message += _Range[HandIndex][CardIndex].Suit.ToString() + _Range[HandIndex][CardIndex].Value.ToString() + "  ";
			}
			
			Debug.Log(Message);
		}
	}
	
	public static Card[] GetBestPostSwapHand(Player _PlayerInQuestion)
	{
		Card[] CurrentHand   = _PlayerInQuestion.Hand.ToArray();
		Card[] CurrentPocket = _PlayerInQuestion.Pocket.ToArray();
		Card[] HighestHand   = (Card[]) CurrentHand.Clone();
		int HighestHandValue = DecisionReference.GetValueOfCurrentHand(new List<Card> (CurrentHand));
		
		for(int PocketSlot = 0; PocketSlot < CurrentPocket.Length; PocketSlot++)
		{
			Card PocketCard = CurrentPocket[PocketSlot];
			
			for(int HandSlot = 0; HandSlot < CurrentHand.Length; HandSlot++)
			{
				Card[] PossibleHand    = (Card[]) CurrentHand.Clone();
				PossibleHand[HandSlot] = PocketCard;
				
				int PossibleHandValue = DecisionReference.GetValueOfCurrentHand(new List<Card> (PossibleHand));
				
				if(PossibleHandValue > HighestHandValue)
				{
					HighestHand      = (Card[]) PossibleHand.Clone();
					HighestHandValue = DecisionReference.GetValueOfCurrentHand(new List<Card> (HighestHand));
				}
				else if(PossibleHandValue == HighestHandValue)
				{
					int PossibleHandScore = 0;
					for(int PossibleHandSlot = 0; PossibleHandSlot < PossibleHand.Length; PossibleHandSlot++)
					{
						PossibleHandScore += (int) PossibleHand[PossibleHandSlot].Suit + (int) PossibleHand[PossibleHandSlot].Value;
					}
					
					int HighestHandScore = 0;
					for(int HighestHandSlot = 0; HighestHandSlot < HighestHand.Length; HighestHandSlot++)
					{
						HighestHandScore += (int) HighestHand[HighestHandSlot].Suit + (int) HighestHand[HighestHandSlot].Value;
					}
					
					if(PossibleHandScore > HighestHandScore)
					{
						HighestHand      = (Card[]) PossibleHand.Clone();
						HighestHandValue = DecisionReference.GetValueOfCurrentHand(new List<Card> (HighestHand));
					}
				}
			}
			
			for(int HandSlot = 0; HandSlot < CurrentHand.Length; HandSlot++)
			{
				if(HighestHand[HandSlot].Suit != CurrentHand[HandSlot].Suit || HighestHand[HandSlot].Value != CurrentHand[HandSlot].Value)
				{
					for(int SubPocketSlot = 0; SubPocketSlot < CurrentPocket.Length; SubPocketSlot++)
					{
						if(HighestHand[HandSlot].Suit == CurrentPocket[SubPocketSlot].Suit && HighestHand[HandSlot].Value == CurrentPocket[SubPocketSlot].Value)
						{
							Card CardSwapToPocket = CurrentHand[HandSlot];
							
							CurrentHand[HandSlot]     = CurrentPocket[SubPocketSlot];
							CurrentPocket[SubPocketSlot] = CardSwapToPocket;
						}
					}
				}
			}
		}
		
		return CurrentHand;
	}
	
	public static int HowManyValidPlayersLeft(GameManager _GM)
	{
		int ValidCount = 0;
		
		for(int PlayerIndex = 0; PlayerIndex < _GM.Players.Length; PlayerIndex++)
		{
			if(!_GM.Players[PlayerIndex].Busted && !_GM.Players[PlayerIndex].Fold)
				ValidCount++;		
		}
		
		return ValidCount;
	}
	
	public static bool IsTwoCardsIdentical (Card _First, Card _Second)
	{
		if(_First.Suit == _Second.Suit && _First.Value == _Second.Value)
			return true;
		
		return false;
	}
	
	public static List<Card[]> SortListOfHands(List<Card[]> _HandList, bool _HighestToLowest)
	{
		for(int FirstIndex = 0; FirstIndex < _HandList.Count - 1; FirstIndex++)
		{
			for(int SecondIndex = FirstIndex + 1; SecondIndex < _HandList.Count; SecondIndex++)
			{
				if(_HighestToLowest && !Evaluator.IsFirstHandSuperiorOverSecond(_HandList[FirstIndex],_HandList[SecondIndex]))
				{
					Card[] TempHand = _HandList[FirstIndex];
					_HandList[FirstIndex] = _HandList[SecondIndex];
					_HandList[SecondIndex] = TempHand;
				}
				else if(!_HighestToLowest && !Evaluator.IsFirstHandSuperiorOverSecond(_HandList[FirstIndex],_HandList[SecondIndex]))
				{
					Card[] TempHand = _HandList[FirstIndex];
					_HandList[FirstIndex] = _HandList[SecondIndex];
					_HandList[SecondIndex] = TempHand;
				}
			}
		}
		
		return _HandList;
	}
	
	public static bool IsTwoHandContainingSameCards(Card[] _FirstHand, Card[] _SecondHand)
	{
		bool IsUnique = true;

		for(int FirstCardIndex = 0; FirstCardIndex < _FirstHand.Length; FirstCardIndex++)
		{
			IsUnique = true;

			for(int SecondCardIndex = 0; SecondCardIndex < _SecondHand.Length; SecondCardIndex++)
			{
				if(IsTwoCardsIdentical(_FirstHand[FirstCardIndex],_SecondHand[SecondCardIndex]))
					IsUnique = false;	
			}

			if(IsUnique)
				return false;
		}

		return true;

//		int FirstHandScore = 0;
//		int SecondHandScore = 0;
//
//		for(int CardIndex = 0; CardIndex < _FirstHand.Length; CardIndex++)
//		{
//			FirstHandScore += ((int) _FirstHand[CardIndex].Suit + 1) + ((int) _FirstHand[CardIndex].Value + 1);
//		}
//
//		FirstHandScore *= (int) Evaluator.EvaluateHand(_FirstHand) + 1;
//
//		for(int CardIndex = 0; CardIndex < _SecondHand.Length; CardIndex++)
//		{
//			SecondHandScore += ((int) _SecondHand[CardIndex].Suit + 1) + ((int) _SecondHand[CardIndex].Value + 1);
//		}
//
//		SecondHandScore *= (int) Evaluator.EvaluateHand(_SecondHand) + 1;
//
//		return FirstHandScore == SecondHandScore ? true : false;
	}

	public static bool IsHandAlreadyInPossibleHands(LinkedList<Card[]> _HandList, Card[] _Hand)
	{
		LinkedListNode<Card[]> TraverseNode = _HandList.First;

		while(TraverseNode != null)
		{
			if(IsTwoHandContainingSameCards(TraverseNode.Value, _Hand))
				return true;

			TraverseNode = TraverseNode.Next; 
		}

		return false;
	}

	public static void RemoveDuplicationFromHandList(LinkedList<Card[]> _HandList)
	{
		if(_HandList.First == null)
			return;

		LinkedListNode<Card[]> ReferenceNode = _HandList.First;
		LinkedListNode<Card[]> TransitionNode = ReferenceNode.Next;

		while(ReferenceNode	!= null)
		{
			LinkedListNode<Card[]> RunnerNode = ReferenceNode.Next;

			while(RunnerNode != null)
			{
				if(IsTwoHandContainingSameCards(ReferenceNode.Value,RunnerNode.Value))
				{
					TransitionNode = RunnerNode.Next;

					_HandList.Remove(RunnerNode);

					RunnerNode = TransitionNode;
					continue;
				}

				RunnerNode = RunnerNode.Next;
			}

			ReferenceNode = ReferenceNode.Next;
		}
	}

	public static void RemoveDuplicationFromHandList(List<Card[]> _HandList)
	{
		for(int FirstHandIndex = 0; FirstHandIndex < _HandList.Count - 1; FirstHandIndex++)
		{
			for(int SecondHandIndex = FirstHandIndex + 1; SecondHandIndex < _HandList.Count; SecondHandIndex++)
			{
				if(IsTwoHandContainingSameCards(_HandList[FirstHandIndex],_HandList[SecondHandIndex]))
					_HandList.RemoveAt(FirstHandIndex);
			}
		}
	}

	public static void RemoveDuplicationFromHandList(Card[][] _HandList)
	{
		for(int FirstHandIndex = 0; FirstHandIndex < _HandList.Length - 1; FirstHandIndex++)
		{
			if(_HandList[FirstHandIndex] == null)
				break;

			for(int SecondHandIndex = FirstHandIndex + 1; SecondHandIndex < _HandList.Length; SecondHandIndex++)
			{
				if(_HandList[SecondHandIndex] == null)
					break;

				if(IsTwoHandContainingSameCards(_HandList[FirstHandIndex],_HandList[SecondHandIndex]))
				{
					_HandList[SecondHandIndex] = new Card[0];
				}
			}
		}
	}
	
	public static bool DoesHandContainThisCard (Card[] _Hand, Card _Card)
	{
		for(int CardIndex = 0; CardIndex < _Hand.Length; CardIndex++)
		{
			if(IsTwoCardsIdentical(_Hand[CardIndex],_Card))
				return true;
		}
		return false;
	}
	
	public static Card[] SortHandByCards (Card[] _Hand, bool _HighestToLowest)
	{
		if(_HighestToLowest)
		{
			for(int FirstCardIndex = 0; FirstCardIndex < _Hand.Length - 1; FirstCardIndex++)
			{
				for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < _Hand.Length; SecondCardIndex++)
				{
					if(_Hand[FirstCardIndex].Value < _Hand[SecondCardIndex].Value
					   || (_Hand[FirstCardIndex].Value == _Hand[SecondCardIndex].Value && _Hand[FirstCardIndex].Suit < _Hand[SecondCardIndex].Suit))
					{
						Card TempCard = _Hand[FirstCardIndex];
						_Hand[FirstCardIndex] = _Hand[SecondCardIndex];
						_Hand[SecondCardIndex] = TempCard;
					}
				}
			}
		}
		else
		{
			for(int FirstCardIndex = 0; FirstCardIndex < _Hand.Length - 1; FirstCardIndex++)
			{
				for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < _Hand.Length; SecondCardIndex++)
				{
					if(_Hand[FirstCardIndex].Value > _Hand[SecondCardIndex].Value
					   || (_Hand[FirstCardIndex].Value == _Hand[SecondCardIndex].Value && _Hand[FirstCardIndex].Suit > _Hand[SecondCardIndex].Suit))
					{
						Card TempCard = _Hand[FirstCardIndex];
						_Hand[FirstCardIndex] = _Hand[SecondCardIndex];
						_Hand[SecondCardIndex] = TempCard;
					}
				}
			}
		}
		
		return _Hand;
	}

	private static int PartitionHands(List<Card[]> _Hands, int _LowestIndex, int _HighestIndex)
	{
//		Debug.Log("Highest index: " + _HighestIndex);
		Card[] PivotHand = _Hands[_HighestIndex];
		int PivotValue = Evaluator.DetermineHandLevelINT(PivotHand);
	    int i = _LowestIndex - 1;

	    for (int j = _LowestIndex; j < _HighestIndex; j++)
	    {
	        if (Evaluator.DetermineHandLevelINT(_Hands[j]) <= PivotValue)
	        {
	            i++;
	            SwapHands(_Hands, i, j);
	        }
	    }
		SwapHands(_Hands, i + 1, _HighestIndex);
	    return i + 1;
	}

	private static int PartitionHands(Card[][] _Hands, int _LowestIndex, int _HighestIndex)
	{
//		Debugger.CheckEmpty<Card[][]>(_Hands);
		Card[] PivotHand = _Hands[_HighestIndex];

//		Debugger.CheckEmpty<Card[]>(PivotHand);

		int PivotValue = Evaluator.DetermineHandLevelINT(PivotHand);

	    int i = _LowestIndex - 1;

	    for (int j = _LowestIndex; j < _HighestIndex; j++)
	    {
	        if (Evaluator.DetermineHandLevelINT(_Hands[j]) <= PivotValue)
	        {
	            i++;
	            SwapHands(_Hands, i, j);
	        }
	    }
		SwapHands(_Hands, i + 1, _HighestIndex);
	    return i + 1;
	}

	private static void SwapHands(List<Card[]> _Hands, int _FirstIndex, int _SecondIndex)
	{
//		Debug.Log("Swap Hand " + (_FirstIndex + 1) + " with Hand " + (_SecondIndex + 1));

		Card[] temp = _Hands[_FirstIndex];
		_Hands[_FirstIndex] = _Hands[_SecondIndex];
		_Hands[_SecondIndex] = temp;

	}

	private static void SwapHands(Card[][] _Hands, int _FirstIndex, int _SecondIndex)
	{
//		Debug.Log("Swap Hand " + (_FirstIndex + 1) + " with Hand " + (_SecondIndex + 1));

		Card[] temp = _Hands[_FirstIndex];
		_Hands[_FirstIndex] = _Hands[_SecondIndex];
		_Hands[_SecondIndex] = temp;

	}

	public static void QuickSortHands(List<Card[]> _HandList, int _LowestIndex, int _HighestIndex)
	{
		int NextPivot = 0;

		if(_LowestIndex < _HighestIndex)
		{
			NextPivot = PartitionHands(_HandList,_LowestIndex,_HighestIndex);
			QuickSortHands(_HandList,_LowestIndex,NextPivot - 1);
			QuickSortHands(_HandList,NextPivot + 1,_HighestIndex);
		}
	}

	public static void QuickSortHands(Card[][] _HandList, int _LowestIndex, int _HighestIndex)
	{
		int NextPivot = 0;

		if(_LowestIndex < _HighestIndex)
		{
			NextPivot = PartitionHands(_HandList,_LowestIndex,_HighestIndex);
			QuickSortHands(_HandList,_LowestIndex,NextPivot - 1);
			QuickSortHands(_HandList,NextPivot + 1,_HighestIndex);
		}
	}

	public static int CountSuitOccurance(Card[] _Hand, Suits _Suit)
	{
		int OccuranceInHand = 0;

		for(int CardIndex = 0; CardIndex < _Hand.Length; CardIndex++)
		{
			if(_Hand[CardIndex].Suit == _Suit)
				OccuranceInHand++;
		}

		return OccuranceInHand;
	}

	public static int CountSuitOccurance(List<Card> _Hand, Suits _Suit)
	{
		int OccuranceInHand = 0;

		for(int CardIndex = 0; CardIndex < _Hand.Count; CardIndex++)
		{
			if(_Hand[CardIndex].Suit == _Suit)
				OccuranceInHand++;
		}

		return OccuranceInHand;
	}

	public static int CountValueOccurance(Card[] _Hand, Values _Value)
	{
		int OccuranceInHand = 0;

		for(int CardIndex = 0; CardIndex < _Hand.Length; CardIndex++)
		{
			if(_Hand[CardIndex].Value == _Value)
				OccuranceInHand++;
		}

		return OccuranceInHand;
	}

	public static int CountValueOccurance(List<Card> _Hand, Values _Value)
	{
		int OccuranceInHand = 0;

		for(int CardIndex = 0; CardIndex < _Hand.Count; CardIndex++)
		{
			if(_Hand[CardIndex].Value == _Value)
				OccuranceInHand++;
		}

		return OccuranceInHand;
	}

	public static void RemoveValuesFromDeck(List<Card> _DeckCards, Values _Value)
	{
		for(int CardIndex = 0; CardIndex < _DeckCards.Count; CardIndex++)
		{
			if(_DeckCards[CardIndex].Value == _Value)
				_DeckCards.RemoveAt(CardIndex);
		}
	}

	public static void RemoveSuitsFromDeck(List<Card> _DeckCards, Suits _Suit)
	{
		for(int CardIndex = 0; CardIndex < _DeckCards.Count; CardIndex++)
		{
			if(_DeckCards[CardIndex].Suit == _Suit)
				_DeckCards.RemoveAt(CardIndex);
		}
	}

	public static Card[] GetAllOfPlayerCards(Player _Player)
	{
		if(_Player.Pocket.Count == 0)
			return _Player.Hand.ToArray();

		Card[] PlayerCards = new Card[_Player.Hand.Count + _Player.Pocket.Count];

		for(int CardIndex = 0; CardIndex < _Player.Hand.Count + _Player.Pocket.Count; CardIndex++)
		{
			if(CardIndex < _Player.Hand.Count)
				PlayerCards[CardIndex] = _Player.Hand[CardIndex];
			else
				PlayerCards[CardIndex] = _Player.Pocket[CardIndex - _Player.Hand.Count];
		}

		return PlayerCards;
	}

	public static Card[] CaptureCardTypeFromPlayer(Player _Player, Suits[] _SuitRange, Values[] _ValueRange)
	{
		if(_SuitRange.Length == 0 && _ValueRange.Length == 0)
			return new Card[0];

		Card[] PlayerCards = new Card[_Player.Hand.Count + _Player.Pocket.Count];
		for(int CardIndex = 0; CardIndex < _Player.Hand.Count + _Player.Pocket.Count; CardIndex++)
		{
			if(CardIndex < _Player.Hand.Count)
				PlayerCards[CardIndex] = _Player.Hand[CardIndex];
			else
				PlayerCards[CardIndex] = _Player.Pocket[CardIndex - _Player.Hand.Count];
		}

		List<Card> ValidCards = new List<Card>();

		if(_ValueRange.Length == 0)
		{
			for(int CardIndex = 0; CardIndex < PlayerCards.Length; CardIndex++)
			{
				for(int SuitIndex = 0; SuitIndex < _SuitRange.Length; SuitIndex++)
				{
					if(PlayerCards[CardIndex].Suit == _SuitRange[SuitIndex])
					{
						ValidCards.Add(PlayerCards[CardIndex]);
						break;
					}
				}
			}

			return ValidCards.ToArray();
		}

		if(_SuitRange.Length == 0)
		{
			for(int CardIndex = 0; CardIndex < PlayerCards.Length; CardIndex++)
			{
				for(int ValueIndex = 0; ValueIndex < _ValueRange.Length; ValueIndex++)
				{
					if(PlayerCards[CardIndex].Value == _ValueRange[ValueIndex])
					{
						ValidCards.Add(PlayerCards[CardIndex]);
						break;
					}
				}
			}

			return ValidCards.ToArray();
		}

		for(int CardIndex = 0; CardIndex < PlayerCards.Length; CardIndex++)
		{
			for(int SuitIndex = 0; SuitIndex < _SuitRange.Length; SuitIndex++)
			{
				if(_SuitRange[SuitIndex] == PlayerCards[CardIndex].Suit)
				{
					for(int ValueIndex = 0; ValueIndex < _ValueRange.Length; ValueIndex++)
					{
						if(_ValueRange[ValueIndex] == PlayerCards[CardIndex].Value)
						{
							ValidCards.Add(PlayerCards[CardIndex]);
							break;
						}	
					}
				}
			}
		}

		return ValidCards.ToArray();
	}

	public static Card[] CaptureCardTypeFromDeck(Deck _Deck, Suits[] _SuitRange, Values[] _ValueRange)
	{
		if(_SuitRange.Length == 0 && _ValueRange.Length == 0)
			return new Card[0];

		List<Card> ValidCards = new List<Card>();

		if(_ValueRange.Length == 0)
		{
			for(int CardIndex = 0; CardIndex < _Deck.Cards.Count; CardIndex++)
			{
				for(int SuitIndex = 0; SuitIndex < _SuitRange.Length; SuitIndex++)
				{
					if(_Deck.Cards[CardIndex].Suit == _SuitRange[SuitIndex])
					{
						ValidCards.Add(_Deck.Cards[CardIndex]);
						break;
					}
				}
			}

			return ValidCards.ToArray();
		}

		if(_SuitRange.Length == 0)
		{
			for(int CardIndex = 0; CardIndex < _Deck.Cards.Count; CardIndex++)
			{
				for(int ValueIndex = 0; ValueIndex < _ValueRange.Length; ValueIndex++)
				{
					if(_Deck.Cards[CardIndex].Value == _ValueRange[ValueIndex])
					{
						ValidCards.Add(_Deck.Cards[CardIndex]);
						break;
					}
				}
			}

			return ValidCards.ToArray();
		}

		for(int CardIndex = 0; CardIndex < _Deck.Cards.Count; CardIndex++)
		{
			for(int SuitIndex = 0; SuitIndex < _SuitRange.Length; SuitIndex++)
			{
				if(_SuitRange[SuitIndex] == _Deck.Cards[CardIndex].Suit)
				{
					for(int ValueIndex = 0; ValueIndex < _ValueRange.Length; ValueIndex++)
					{
						if(_ValueRange[ValueIndex] == _Deck.Cards[CardIndex].Value)
						{
							ValidCards.Add(_Deck.Cards[CardIndex]);
							break;
						}	
					}
				}
			}
		}

		return ValidCards.ToArray();
	}

	public static bool IsCardInDeck(Deck _Deck, Card _Card)
	{
		for(int CardIndex = 0; CardIndex < _Deck.Cards.Count; CardIndex++)
		{
			if(IsTwoCardsIdentical(_Card,_Deck.Cards[CardIndex]))
				return true;
		}
		return false;
	}
}
