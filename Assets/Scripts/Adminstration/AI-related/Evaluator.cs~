using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Evaluator
{
	private static bool IsHandOnePair(Card[] _Hand)
	{
		for(int a = 0; a < _Hand.Length; a++)
		{
			for(int b = 0; b < _Hand.Length; b++)
			{
				if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
					return true;
			}
		}
		return false;
	}
	
	private static bool IsHandTwoPair(Card[] _Hand)
	{
		bool FirstPairCheck = false;
		bool SecondPairCheck = false;
		int FirstPairValue = 0;

		for(int a = 0; a < _Hand.Length; a++)
		{
			for(int b = 0; b < _Hand.Length; b++)
			{
				if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
				{
					if(FirstPairCheck == false && SecondPairCheck == false)
					{
						FirstPairValue = (int) _Hand[a].Value; 
						FirstPairCheck = true;
					}
					if((int) _Hand[a].Value != FirstPairValue && FirstPairCheck == true && SecondPairCheck == false)
					{
						SecondPairCheck = true;
					}
					break;
				}
			}

			if(FirstPairCheck == true && SecondPairCheck == true)
				return true;
		}
		
		return false;
	}
	
	private static bool IsHandThreeOfAKind(Card[] _Hand)
	{
		int CheckAgainst = 0;
	
		for(int a = 0; a < _Hand.Length; a++)
		{
			for(int b = 0; b < _Hand.Length; b++)
			{
				if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
				{
					CheckAgainst++;
				}
			}
			if(CheckAgainst >= 2)
				return true;
			else
				CheckAgainst = 0;
		}
		
		return false;
	}
	
	private static bool IsHandStraight(Card[] _Hand)
	{
		int LowestCardValue = 99;
		
		for(int i = 0; i < _Hand.Length; i++)
		{
			if((int) _Hand[i].Value < LowestCardValue)
				LowestCardValue = (int) _Hand[i].Value;
		}
		
		int CheckCardValue = LowestCardValue;
		int NextCardValue = LowestCardValue + 1;
		
		for(int a = 0; a < _Hand.Length - 1; a++)
		{	
			for(int i = 0; i < _Hand.Length; i++)
			{
				if((int) _Hand[i].Value == NextCardValue)
				{
					CheckCardValue = NextCardValue; 
					break;
				}
			}
			
			if(CheckCardValue == NextCardValue)
				NextCardValue++;
			else if(CheckCardValue != NextCardValue)
				return false;
		}
		
		return true;
	}
	
	private static bool IsHandFlush(Card[] _Hand)
	{
		for(int a = 0; a < _Hand.Length; a++)
		{
			for(int b = 0; b < _Hand.Length; b++)
			{
				if(_Hand[b] != _Hand[a] && _Hand[b].Suit != _Hand[a].Suit)
					return false;
			}
		}
		return true;
	}
	
	private static bool IsHandFullhouse(Card[] _Hand)
	{
		int FSFirstIndex = 0;
		int FSSecondIndex = 0;
		
		for(int a = 0; a < _Hand.Length; a++)
		{
			for(int b = 0; b < _Hand.Length; b++)
			{
				if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
				{
					FSFirstIndex = a;
					FSSecondIndex = b;
					break;
				}
			}
		}
		
		int IdenticalCounter = 0;
		int CheckValue = 0;
		
		for(int a = 0; a < _Hand.Length; a++)
		{
			if(a != FSFirstIndex && a != FSSecondIndex)
			{
				if(IdenticalCounter == 0)
				{
					CheckValue = (int) _Hand[a].Value;
					IdenticalCounter++;
				}
				else if(IdenticalCounter >= 1 && IdenticalCounter < 3)
				{
					if((int) _Hand[a].Value != CheckValue)
						return false;
					else
						IdenticalCounter++;
				}
			}
		}
		
		if(IdenticalCounter >= 3)
			return true;

		return false;
	}
	
	private static bool IsHandFourOfAKind(Card[] _Hand)
	{
		int CheckAgainst = 0;
		
		for(int a = 0; a < _Hand.Length; a++)
		{
			for(int b = 0; b < _Hand.Length; b++)
			{
				if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
					CheckAgainst++;
			}
			if(CheckAgainst >= 3)
				return true;
			else
				CheckAgainst = 0;
		}
		
		return false;
	}
	
	private static bool IsHandStraightFlush(Card[] _Hand)
	{
		if(IsHandFlush(_Hand) && IsHandStraight(_Hand))
			return true;

		return false;
	}
	
	private static bool IsHandRoyalFlush(Card[] _Hand)
	{
		if(IsHandFlush(_Hand) && IsHandStraight(_Hand))
		{
			for(int i = 0; i < _Hand.Length; i++)
			{
				if((int) _Hand[i].Value < 8)
					return false;
			}
			return true;
		}
		
		return false;
	}

	public static Hands EvaluateHand(List<Card> _Hand)
	{
		Card[] Hand = _Hand.ToArray();

		if(IsHandRoyalFlush(Hand))
			return Hands.RoyalFlush;
		
		if(IsHandStraightFlush(Hand))
			return Hands.StraightFlush;
		
		if(IsHandFourOfAKind(Hand))
			return Hands.FourOfAKind;
		
		if(IsHandFullhouse(Hand))
			return Hands.FullHouse;
		
		if(IsHandFlush(Hand))
			return Hands.Flush;
		
		if(IsHandStraight(Hand))
			return Hands.Straight;
		
		if(IsHandThreeOfAKind(Hand))
			return Hands.ThreeOfAKind;
		
		if(IsHandTwoPair(Hand))
			return Hands.TwoPair;
		
		if(IsHandOnePair(Hand))
			return Hands.OnePair;
		
		return Hands.HighCard;
	}

	public static Hands EvaluateHand(Card[] _Hand)
	{
		if(IsHandRoyalFlush(_Hand))
			return Hands.RoyalFlush;
		
		if(IsHandStraightFlush(_Hand))
			return Hands.StraightFlush;
		
		if(IsHandFourOfAKind(_Hand))
			return Hands.FourOfAKind;
		
		if(IsHandFullhouse(_Hand))
			return Hands.FullHouse;
		
		if(IsHandFlush(_Hand))
			return Hands.Flush;
		
		if(IsHandStraight(_Hand))
			return Hands.Straight;
		
		if(IsHandThreeOfAKind(_Hand))
			return Hands.ThreeOfAKind;
		
		if(IsHandTwoPair(_Hand))
			return Hands.TwoPair;
		
		if(IsHandOnePair(_Hand))
			return Hands.OnePair;
		
		return Hands.HighCard;
	}

	public static Card[] GetEffectiveHandWithPocket (Player _Player)
	{
		List<Card[]> PossibleHandsWithPocket = new List<Card[]>();
		Card[] PlayerHand = _Player.Hand.ToArray();
		Card[] PlayerPocket = _Player.Pocket.ToArray();
		Card[] PossibleHand = new Card[5];

		PossibleHandsWithPocket.Add(_Player.Hand.ToArray());

		if(PlayerPocket.Length > 0)
		{
			#region If player has one pocket card or more, pass the first pocket card through their hand to find the possible resultant hands
			for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
			{
				PossibleHand = new Card[5];
				PossibleHand[0] = PlayerHand[0]; 
				PossibleHand[1] = PlayerHand[1];
				PossibleHand[2] = PlayerHand[2];
				PossibleHand[3] = PlayerHand[3];
				PossibleHand[4] = PlayerHand[4];
				
				PossibleHand[CardIndex] = PlayerPocket[0];

				PossibleHandsWithPocket.Add(PossibleHand);
			}
			#endregion

			#region If player has two pocket cards or more, pass the first and second pocket cards to find the possible resultant hands
			if(PlayerPocket.Length > 1)
			{
				for(int FirstCardIndex = 0; FirstCardIndex < PlayerHand.Length - 1; FirstCardIndex++)
				{
					for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PlayerHand.Length; SecondCardIndex++)
					{
						PossibleHand = new Card[PlayerHand.Length];
						PossibleHand[0] = PlayerHand[0]; 
						PossibleHand[1] = PlayerHand[1];
						PossibleHand[2] = PlayerHand[2];
						PossibleHand[3] = PlayerHand[3];
						PossibleHand[4] = PlayerHand[4];
						
						PossibleHand[FirstCardIndex] = PlayerPocket[0];
						PossibleHand[SecondCardIndex] = PlayerPocket[1];
						
						PossibleHandsWithPocket.Add(PossibleHand);
					}
				}
			}
			#endregion

			#region If player has three pocket cards, pass all three pocket cards to find the possible resultant hands
			if(PlayerPocket.Length > 2)
			{
				for(int FirstCardIndex = 0; FirstCardIndex < PlayerHand.Length - 2; FirstCardIndex++)
				{
					for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PlayerHand.Length - 1; SecondCardIndex++)
					{
						for(int ThirdCardIndex = SecondCardIndex + 1; ThirdCardIndex < PlayerHand.Length; ThirdCardIndex++)
						{
							PossibleHand = new Card[PlayerHand.Length];
							PossibleHand[0] = PlayerHand[0];
							PossibleHand[1] = PlayerHand[1];
							PossibleHand[2] = PlayerHand[2];
							PossibleHand[3] = PlayerHand[3];
							PossibleHand[4] = PlayerHand[4];
							
							PossibleHand[FirstCardIndex] = PlayerPocket[0];
							PossibleHand[SecondCardIndex] = PlayerPocket[1];
							PossibleHand[ThirdCardIndex] = PlayerPocket[2];
							
							PossibleHandsWithPocket.Add(PossibleHand);
						}
					}
				}
			}
			#endregion
		}

		Card[] IteratedHand = PossibleHandsWithPocket[0];
		Hands HighestHandType = Evaluator.EvaluateHand(IteratedHand);

		for(int HandIndex = 0; HandIndex < PossibleHandsWithPocket.Count; HandIndex++)
		{
			Hands CurrentHandType = Evaluator.EvaluateHand(PossibleHandsWithPocket[HandIndex]);

			if(CurrentHandType > HighestHandType)
			{
				HighestHandType = CurrentHandType;
				IteratedHand = PossibleHandsWithPocket[HandIndex];
			}
		}

		return IteratedHand;
	}

	public static Hands EvaluatePlayerHandWithPocket (Player _Player)
	{
		List<Card[]> PossibleHandsWithPocket = new List<Card[]>();
		Card[] PlayerHand = _Player.Hand.ToArray();
		Card[] PlayerPocket = _Player.Pocket.ToArray();
		Card[] PossibleHand = new Card[5];

		PossibleHandsWithPocket.Add(_Player.Hand.ToArray());

		if(PlayerPocket.Length > 0)
		{
			#region If player has one pocket card or more, pass the first pocket card through their hand to find the possible resultant hands
			for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
			{
				PossibleHand = new Card[5];
				PossibleHand[0] = PlayerHand[0]; 
				PossibleHand[1] = PlayerHand[1];
				PossibleHand[2] = PlayerHand[2];
				PossibleHand[3] = PlayerHand[3];
				PossibleHand[4] = PlayerHand[4];
				
				PossibleHand[CardIndex] = PlayerPocket[0];

				PossibleHandsWithPocket.Add(PossibleHand);
			}
			#endregion

			#region If player has two pocket cards or more, pass the first and second pocket cards to find the possible resultant hands
			if(PlayerPocket.Length > 1)
			{
				for(int FirstCardIndex = 0; FirstCardIndex < PlayerHand.Length - 1; FirstCardIndex++)
				{
					for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PlayerHand.Length; SecondCardIndex++)
					{
						PossibleHand = new Card[PlayerHand.Length];
						PossibleHand[0] = PlayerHand[0]; 
						PossibleHand[1] = PlayerHand[1];
						PossibleHand[2] = PlayerHand[2];
						PossibleHand[3] = PlayerHand[3];
						PossibleHand[4] = PlayerHand[4];
						
						PossibleHand[FirstCardIndex] = PlayerPocket[0];
						PossibleHand[SecondCardIndex] = PlayerPocket[1];
						
						PossibleHandsWithPocket.Add(PossibleHand);
					}
				}
			}
			#endregion

			#region If player has three pocket cards, pass all three pocket cards to find the possible resultant hands
			if(PlayerPocket.Length > 2)
			{
				for(int FirstCardIndex = 0; FirstCardIndex < PlayerHand.Length - 2; FirstCardIndex++)
				{
					for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PlayerHand.Length - 1; SecondCardIndex++)
					{
						for(int ThirdCardIndex = SecondCardIndex + 1; ThirdCardIndex < PlayerHand.Length; ThirdCardIndex++)
						{
							PossibleHand = new Card[PlayerHand.Length];
							PossibleHand[0] = PlayerHand[0];
							PossibleHand[1] = PlayerHand[1];
							PossibleHand[2] = PlayerHand[2];
							PossibleHand[3] = PlayerHand[3];
							PossibleHand[4] = PlayerHand[4];
							
							PossibleHand[FirstCardIndex] = PlayerPocket[0];
							PossibleHand[SecondCardIndex] = PlayerPocket[1];
							PossibleHand[ThirdCardIndex] = PlayerPocket[2];
							
							PossibleHandsWithPocket.Add(PossibleHand);
						}
					}
				}
			}
			#endregion
		}

		Hands HighestHandType = Evaluator.EvaluateHand(_Player.Hand);

		for(int HandIndex = 0; HandIndex < PossibleHandsWithPocket.Count; HandIndex++)
		{
			Hands CurrentHandType = Evaluator.EvaluateHand(PossibleHandsWithPocket[HandIndex]);

			if(CurrentHandType > HighestHandType)
			{
				HighestHandType = CurrentHandType;
			}
		}

		return HighestHandType;
	}

	public static Player EvaluateHandWinner(Player _first, Player _second)
	{
		if(_first.Fold || _first.Busted){return _second;}
		if(_second.Fold || _second.Busted){return _first;}
	
		Hands P1HandType = EvaluateHand(_first.Hand);
		Hands P2HandType = EvaluateHand(_second.Hand);
		int P1HandTypeScore = (int) P1HandType;
		int P2HandTypeScore = (int) P2HandType;
		
		if(P1HandTypeScore != P2HandTypeScore){return P1HandTypeScore > P2HandTypeScore ? _first : _second;}
		
		if(P1HandType == Hands.HighCard)
		{
			int P1HighestValue = 0;
			int P1HighestSuit = 0;
			int P2HighestValue = 0;
			int P2HighestSuit = 0;
			for(int i = 0; i < 5; i++)
			{
				if((int) _first.Hand[i].Value > P1HighestValue){P1HighestValue = (int) _first.Hand[i].Value; P1HighestSuit = (int) _first.Hand[i].Suit;}
				if((int) _second.Hand[i].Value > P2HighestValue){P2HighestValue = (int) _second.Hand[i].Value; P2HighestSuit = (int) _second.Hand[i].Suit;}
			}
			if(P1HighestValue != P2HighestValue) {return P1HighestValue > P2HighestValue ? _first : _second;}
			else{return P1HighestSuit > P2HighestSuit ? _first : _second;}
		}
		else if(P1HandType == Hands.OnePair)
		{
			int P1PairValue = 0;
			int P1HighestSuit = 0;
			int P2PairValue = 0;
			int P2HighestSuit = 0;

			for(int a = 0; a < 5; a++)
			{
				for(int b = 0; b < 5; b++)
				{
					if(_first.Hand[b] != _first.Hand[a] && _first.Hand[b].Value == _first.Hand[a].Value)
					{
						P1PairValue = (int) _first.Hand[a].Value; 
						P1HighestSuit = (int) _first.Hand[a].Suit + (int) _first.Hand[b].Suit;
						break;
					}
				}
			}
			for(int a = 0; a < 5; a++)
			{
				for(int b = 0; b < 5; b++)
				{
					if(_second.Hand[b] != _second.Hand[a] && _second.Hand[b].Value == _second.Hand[a].Value)
					{
						P2PairValue = (int) _second.Hand[a].Value; 
						P2HighestSuit = (int) _second.Hand[a].Suit + (int) _second.Hand[b].Suit;
						break;
					}
				}
			}
			
			if(P1PairValue != P2PairValue){return P1PairValue > P2PairValue ? _first : _second;}
			else{return P1HighestSuit > P2HighestSuit ? _first : _second;}
			
		}
		else if(P1HandType == Hands.TwoPair)
		{
			int P1FirstPairValue = 0;
			int P1FirstPairSuitsScore = 0;
			int P1SecondPairValue = 0;
			int P1SecondPairSuitsScore = 0;
			int P2FirstPairValue = 0;
			int P2FirstPairSuitsScore = 0;
			int P2SecondPairValue = 0;
			int P2SecondPairSuitsScore = 0;

			for(int a = 0; a < _first.Hand.Count; a++)
			{
				for(int b = 0; b < _first.Hand.Count; b++)
				{
					if(_first.Hand[b] != _first.Hand[a] && _first.Hand[b].Value == _first.Hand[a].Value && (int) _first.Hand[a].Value != P1FirstPairValue && (int) _first.Hand[a].Value != P1SecondPairValue)
					{
						if(P1FirstPairValue == 0){P1FirstPairValue = (int) _first.Hand[a].Value; P1FirstPairSuitsScore = (int)_first.Hand[a].Suit + (int)_first.Hand[b].Suit;}
						else{P1SecondPairValue = (int) _first.Hand[a].Value; P1SecondPairSuitsScore = (int)_first.Hand[a].Suit + (int)_first.Hand[b].Suit;}
					}

					if(_second.Hand[b] != _second.Hand[a] && _second.Hand[b].Value == _second.Hand[a].Value && (int) _second.Hand[a].Value != P1FirstPairValue && (int) _second.Hand[a].Value != P1SecondPairValue)
					{
						if(P2FirstPairValue == 0){P2FirstPairValue = (int) _second.Hand[a].Value; P2FirstPairSuitsScore = (int)_second.Hand[a].Suit + (int)_second.Hand[b].Suit;}
						else{P2SecondPairValue = (int) _second.Hand[a].Value; P2SecondPairSuitsScore = (int) _second.Hand[a].Suit + (int) _second.Hand[b].Suit;}
					}
				}
			}

			if((P1FirstPairValue > P2FirstPairValue) || (P1FirstPairValue > P2SecondPairValue) || (P1SecondPairValue > P2FirstPairValue) || (P1SecondPairValue > P2SecondPairValue)){return _first;}
			if((P2FirstPairValue > P1FirstPairValue) || (P2FirstPairValue > P2SecondPairValue) || (P1SecondPairValue > P2FirstPairValue) || (P1SecondPairValue > P2SecondPairValue)){return _second;}

			return P1FirstPairSuitsScore > P2SecondPairSuitsScore ? _first : _second;
		}
		else if(P1HandType == Hands.ThreeOfAKind)
		{
			int P1ThreeKindValue = 0;
			int P1ThreeKindSuits = 0;
			int P2ThreeKindValue = 0;
			int P2ThreeKindSuits = 0;
			int CheckAgainst = 0;
			
			for(int a = 0; a < _first.Hand.Count; a++)
			{
				for(int b = 0; b < _first.Hand.Count; b++)
				{
					if(_first.Hand[b] != _first.Hand[a] && _first.Hand[b].Value == _first.Hand[a].Value)
					{
						P1ThreeKindValue = (int) _first.Hand[a].Value;
						P1ThreeKindSuits += (int) _first.Hand[b].Suit;
						CheckAgainst++;
					}
				}
				if(CheckAgainst >= 2){break;}
				else{CheckAgainst = 0; P1ThreeKindValue = 0;P1ThreeKindSuits = 0;}
			}

			CheckAgainst = 0;

			for(int a = 0; a < _second.Hand.Count; a++)
			{
				for(int b = 0; b < _second.Hand.Count; b++)
				{
					if(_second.Hand[b] != _second.Hand[a] && _second.Hand[b].Value == _second.Hand[a].Value)
					{
						P2ThreeKindSuits += (int) _second.Hand[b].Value;
						P2ThreeKindValue = (int) _second.Hand[a].Value;
						CheckAgainst++;
					}
				}
				if(CheckAgainst >= 2){break;}
				else{CheckAgainst = 0; P2ThreeKindValue = 0; P1ThreeKindSuits = 0;}
			}

			if(P1ThreeKindValue != P2ThreeKindValue){ return P1ThreeKindValue > P2ThreeKindValue ? _first : _second;}
			else{return P1ThreeKindSuits > P2ThreeKindSuits ? _first : _second;} 
		}
		else if(P1HandType == Hands.Straight)
		{
			int P1StraightValue = 0;
			int P1StraightSuits = 0;
			int P2StraightValue = 0;
			int P2StraightSuits = 0;

			for(int i = 0; i < _first.Hand.Count; i++)
			{
				P1StraightValue += (int) _first.Hand[i].Value;
				P1StraightSuits += (int) _first.Hand[i].Suit;
				P2StraightValue += (int) _second.Hand[i].Value;
				P2StraightSuits += (int) _second.Hand[i].Suit;
			}

			if(P1StraightValue != P2StraightValue){return P1StraightValue > P2StraightValue ? _first : _second;}
			else{return P1StraightSuits > P2StraightSuits ? _first : _second;}
		}
		else if(P1HandType == Hands.Flush)
		{
			if(_first.Hand[0].Suit != _second.Hand[0].Suit){return (int) _first.Hand[0].Suit > (int) _second.Hand[0].Suit ? _first : _second;}

			int P1FlushValue = 0;
			int P2FlushValue = 0;
			for(int i = 0; i < _first.Hand.Count; i++)
			{
				P1FlushValue += _first.Hand[i].Value;
				P2FlushValue += _second.Hand[i].Value;
			}

			return P1FlushValue > P2FlushValue ? _first : _second;
		}
		else if(P1HandType == Hands.FullHouse)
		{
			int P1FirstSetValue = 0;
			int P1FirstIndex = 0;
			int P1SecondIndex = 0;
			int P2FirstSetValue = 0;
			int P2FirstIndex = 0;
			int P2SecondIndex = 0;
			
			for(int a = 0; a < _first.Hand.Count; a++)
			{
				for(int b = 0; b < _first.Hand.Count; b++)
				{
					if(P1FirstSetValue != 0 && _first.Hand[b] != _first.Hand[a] && _first.Hand[b].Value == _first.Hand[a].Value)
					{
						P1FirstSetValue = (int) _first.Hand[a].Value;
						P1FirstIndex = a;
						P1SecondIndex = b;
					}
					else if(P2FirstSetValue != 0 && _second.Hand[b] != _second.Hand[a] && _second.Hand[b].Value == _second.Hand[a].Value)
					{
						P2FirstSetValue = (int) _second.Hand[a].Value;
						P2FirstIndex = a;
						P2SecondIndex = b;
					}
				}
			}

			int P1ThreeValue = 0;
			int P1ThreeSuits = 0;
			int P2ThreeValue = 0;
			int P2ThreeSuits = 0;


			for(int a = 0; a < _first.Hand.Count; a++)
			{
				if(a != P1FirstIndex && a != P1SecondIndex)
				{
					P1ThreeSuits = (int) _first.Hand[a].Suit;
					P1ThreeValue = (int) _first.Hand[a].Value;
				}

				if(a != P2FirstIndex && a != P2SecondIndex)
				{
					P2ThreeSuits = (int) _second.Hand[a].Suit;
					P2ThreeValue = (int) _second.Hand[a].Value;
				}
			}

			if(P1ThreeValue != P2ThreeValue){return P1ThreeValue > P2ThreeValue ? _first : _second;}
			return P1ThreeSuits > P2ThreeSuits ? _first : _second;
		}
		else if(P1HandType == Hands.FourOfAKind)
		{
			int P1FourValue = 0;
			int P1FourSuit = 0;
			int P1CheckAgainst = 0;
			int P2FourValue = 0;
			int P2FourSuit = 0;
			int P2CheckAgainst = 0;

			for(int a = 0; a < _first.Hand.Count; a++)
			{
				for(int b = 0; b < _first.Hand.Count; b++)
				{
					if(_first.Hand[b] != _first.Hand[a] && _first.Hand[b].Value == _first.Hand[a].Value)
					{
						P1FourValue = (int) _first.Hand[a].Value;
						P1FourSuit = (int) _first.Hand[a].Suit;
						P1CheckAgainst++;
					}

					if(_second.Hand[b] != _second.Hand[a] && _second.Hand[b].Value == _second.Hand[a].Value)
					{
						P2FourValue = (int) _second.Hand[a].Value;
						P2FourSuit = (int) _second.Hand[a].Suit;
						P2CheckAgainst++;
					}
				}
				if(P1CheckAgainst < 3){P1CheckAgainst = 0;}
				if(P2CheckAgainst < 3){P2CheckAgainst = 0;}
			}

			if(P1FourValue != P2FourValue){return P1FourValue > P2FourValue ? _first : _second;}
			return P1FourSuit > P2FourSuit ? _first : _second;
		}
		else if(P1HandType == Hands.StraightFlush)
		{
			int P1StraightValue = 0;
			int P1StraightSuits = (int) _first.Hand[0].Suit;
			int P2StraightValue = 0;
			int P2StraightSuits = (int) _second.Hand[0].Suit;
			
			for(int i = 0; i < _first.Hand.Count; i++)
			{
				P1StraightValue += (int) _first.Hand[i].Value;
				P2StraightValue += (int) _second.Hand[i].Value;
			}
			
			if(P1StraightValue != P2StraightValue){return P1StraightValue > P2StraightValue ? _first : _second;}
			else{return P1StraightSuits > P2StraightSuits ? _first : _second;}
		}
		else if(P1HandType == Hands.RoyalFlush)
		{
			int P1SuitValue = (int) _first.Hand[0].Suit;
			int P2SuitValue = (int) _second.Hand[0].Suit;
			return P1SuitValue > P2SuitValue ? _first : _second;
		}
		return null;
	}

	public static bool IsFirstHandSuperiorOverSecond(Card[] _First, Card[] _Second)
	{
		Hands P1HandType = EvaluateHand(_First);
		Hands P2HandType = EvaluateHand(_Second);
		int P1HandTypeScore = (int) P1HandType;
		int P2HandTypeScore = (int) P2HandType;

		if(P1HandTypeScore != P2HandTypeScore)
			return P1HandTypeScore > P2HandTypeScore ? true : false;

		if(P1HandType == Hands.HighCard)
		{
			int P1HighestValue = 0;
			int P1HighestSuit = 0;
			int P2HighestValue = 0;
			int P2HighestSuit = 0;
			for(int i = 0; i < 5; i++)
			{
				if((int) _First[i].Value > P1HighestValue)
				{
					P1HighestValue = (int) _First[i].Value; 
					P1HighestSuit = (int) _First[i].Suit;
				}
				if((int) _Second[i].Value > P2HighestValue)
				{
					P2HighestValue = (int) _Second[i].Value; 
					P2HighestSuit = (int) _Second[i].Suit;
				}
			}
			if(P1HighestValue != P2HighestValue) 
				return P1HighestValue > P2HighestValue ? true : false;
			else
				return P1HighestSuit > P2HighestSuit ? true : false;
		}
		else if(P1HandType == Hands.OnePair)
		{
			int P1PairValue = 0;
			int P1HighestSuit = 0;
			int P2PairValue = 0;
			int P2HighestSuit = 0;

			for(int a = 0; a < 5; a++)
			{
				for(int b = 0; b < 5; b++)
				{
					if(_First[b] != _First[a] &&_First[b].Value == _First[a].Value)
					{
						P1PairValue = (int)_First[a].Value; 
						P1HighestSuit = (int) _First[a].Suit + (int) _First[b].Suit;
						break;
					}
				}
			}
			for(int a = 0; a < 5; a++)
			{
				for(int b = 0; b < 5; b++)
				{
					if(_Second[b] !=_Second[a] && _Second[b].Value == _Second[a].Value)
					{
						P2PairValue = (int) _Second[a].Value; 
						P2HighestSuit = (int) _Second[a].Suit + (int) _Second[b].Suit;
						break;
					}
				}
			}
			
			if(P1PairValue != P2PairValue)
				return P1PairValue > P2PairValue ? true : false;
			else
				return P1HighestSuit > P2HighestSuit ? true : false;
			
		}
		else if(P1HandType == Hands.TwoPair)
		{
			int P1FirstPairValue = 0;
			int P1FirstPairSuitsScore = 0;
			int P1SecondPairValue = 0;
			int P1SecondPairSuitsScore = 0;
			int P2FirstPairValue = 0;
			int P2FirstPairSuitsScore = 0;
			int P2SecondPairValue = 0;
			int P2SecondPairSuitsScore = 0;

			for(int a = 0; a < _First.Length; a++)
			{
				for(int b = 0; b < _First.Length; b++)
				{
					if(_First[b] != _First[a] && _First[b].Value == _First[a].Value && (int) _First[a].Value != P1FirstPairValue && (int) _First[a].Value != P1SecondPairValue)
					{
						if(P1FirstPairValue == 0)
						{
							P1FirstPairValue = (int) _First[a].Value; 
							P1FirstPairSuitsScore = (int)_First[a].Suit + (int)_First[b].Suit;
						}
						else
						{
							P1SecondPairValue = (int) _First[a].Value; 
							P1SecondPairSuitsScore = (int)_First[a].Suit + (int)_First[b].Suit;
						}
					}

					if(_Second[b] != _Second[a] && _Second[b].Value == _Second[a].Value && (int) _Second[a].Value != P1FirstPairValue && (int) _Second[a].Value != P1SecondPairValue)
					{
						if(P2FirstPairValue == 0)
						{
							P2FirstPairValue = (int) _Second[a].Value; 
							P2FirstPairSuitsScore = (int)_Second[a].Suit + (int)_Second[b].Suit;
						}
						else
						{
							P2SecondPairValue = (int) _Second[a].Value; 
							P2SecondPairSuitsScore = (int) _Second[a].Suit + (int) _Second[b].Suit;
						}
					}
				}
			}

			if((P1FirstPairValue > P2FirstPairValue) || (P1FirstPairValue > P2SecondPairValue) || (P1SecondPairValue > P2FirstPairValue) || (P1SecondPairValue > P2SecondPairValue))
				return true;
			if((P2FirstPairValue > P1FirstPairValue) || (P2FirstPairValue > P2SecondPairValue) || (P1SecondPairValue > P2FirstPairValue) || (P1SecondPairValue > P2SecondPairValue))
				return false;

			return P1FirstPairSuitsScore > P2SecondPairSuitsScore ? true : false;
		}
		else if(P1HandType == Hands.ThreeOfAKind)
		{
			int P1ThreeKindValue = 0;
			int P1ThreeKindSuits = 0;
			int P2ThreeKindValue = 0;
			int P2ThreeKindSuits = 0;
			int CheckAgainst = 0;
			
			for(int a = 0; a < _First.Length; a++)
			{
				for(int b = 0; b < _First.Length; b++)
				{
					if(_First[b] != _First[a] && _First[b].Value == _First[a].Value)
					{
						P1ThreeKindValue = (int) _First[a].Value;
						P1ThreeKindSuits += (int) _First[b].Suit;
						CheckAgainst++;
					}
				}
				if(CheckAgainst >= 2)
					break;
				else
				{
					CheckAgainst = 0; 
					P1ThreeKindValue = 0;
					P1ThreeKindSuits = 0;
				}
			}

			CheckAgainst = 0;

			for(int a = 0; a < _Second.Length; a++)
			{
				for(int b = 0; b < _Second.Length; b++)
				{
					if(_Second[b] != _Second[a] && _Second[b].Value == _Second[a].Value)
					{
						P2ThreeKindSuits += (int) _Second[b].Value;
						P2ThreeKindValue = (int) _Second[a].Value;
						CheckAgainst++;
					}
				}
				if(CheckAgainst >= 2)
				{
					break;
				}
				else
				{
					CheckAgainst = 0; 
					P2ThreeKindValue = 0; 
					P1ThreeKindSuits = 0;
				}
			}

			if(P1ThreeKindValue != P2ThreeKindValue)
				return P1ThreeKindValue > P2ThreeKindValue ? true : false;
			else
				return P1ThreeKindSuits > P2ThreeKindSuits ? true : false;
		}
		else if(P1HandType == Hands.Straight)
		{
			int P1StraightValue = 0;
			int P1StraightSuits = 0;
			int P2StraightValue = 0;
			int P2StraightSuits = 0;

			for(int i = 0; i < _First.Length; i++)
			{
				P1StraightValue += (int) _First[i].Value;
				P1StraightSuits += (int) _First[i].Suit;
				P2StraightValue += (int) _Second[i].Value;
				P2StraightSuits += (int) _Second[i].Suit;
			}

			if(P1StraightValue != P2StraightValue)
				return P1StraightValue > P2StraightValue ? true : false;
			else
				return P1StraightSuits > P2StraightSuits ? true : false;
		}
		else if(P1HandType == Hands.Flush)
		{
			if(_First[0].Suit != _Second[0].Suit)
				return (int) _First[0].Suit > (int) _Second[0].Suit ? true : false;

			int P1FlushValue = 0;
			int P2FlushValue = 0;

			for(int i = 0; i < _First.Length; i++)
			{
				P1FlushValue += _First[i].Value;
				P2FlushValue += _Second[i].Value;
			}

			return P1FlushValue > P2FlushValue ? true : false;
		}
		else if(P1HandType == Hands.FullHouse)
		{
			int P1FirstSetValue = 0;
			int P1FirstIndex = 0;
			int P1SecondIndex = 0;
			int P2FirstSetValue = 0;
			int P2FirstIndex = 0;
			int P2SecondIndex = 0;
			
			for(int a = 0; a < _First.Length; a++)
			{
				for(int b = 0; b < _First.Length; b++)
				{
					if(P1FirstSetValue != 0 && _First[b] != _First[a] && _First[b].Value == _First[a].Value)
					{
						P1FirstSetValue = (int) _First[a].Value;
						P1FirstIndex = a;
						P1SecondIndex = b;
					}
					else if(P2FirstSetValue != 0 && _Second[b] != _Second[a] && _Second[b].Value == _Second[a].Value)
					{
						P2FirstSetValue = (int) _Second[a].Value;
						P2FirstIndex = a;
						P2SecondIndex = b;
					}
				}
			}

			int P1ThreeValue = 0;
			int P1ThreeSuits = 0;
			int P2ThreeValue = 0;
			int P2ThreeSuits = 0;

			for(int a = 0; a < _First.Length; a++)
			{
				if(a != P1FirstIndex && a != P1SecondIndex)
				{
					P1ThreeSuits = (int) _First[a].Suit;
					P1ThreeValue = (int) _First[a].Value;
				}

				if(a != P2FirstIndex && a != P2SecondIndex)
				{
					P2ThreeSuits = (int) _Second[a].Suit;
					P2ThreeValue = (int) _Second[a].Value;
				}
			}

			if(P1ThreeValue != P2ThreeValue)
				return P1ThreeValue > P2ThreeValue ? true : false;

			return P1ThreeSuits > P2ThreeSuits ? true : false;
		}
		else if(P1HandType == Hands.FourOfAKind)
		{
			int P1FourValue = 0;
			int P1FourSuit = 0;
			int P1CheckAgainst = 0;
			int P2FourValue = 0;
			int P2FourSuit = 0;
			int P2CheckAgainst = 0;

			for(int a = 0; a < _First.Length; a++)
			{
				for(int b = 0; b < _First.Length; b++)
				{
					if(_First[b] != _First[a] && _First[b].Value == _First[a].Value)
					{
						P1FourValue = (int) _First[a].Value;
						P1FourSuit = (int) _First[a].Suit;
						P1CheckAgainst++;
					}

					if(_Second[b] != _Second[a] && _Second[b].Value == _Second[a].Value)
					{
						P2FourValue = (int) _Second[a].Value;
						P2FourSuit = (int) _Second[a].Suit;
						P2CheckAgainst++;
					}
				}
				if(P1CheckAgainst < 3){P1CheckAgainst = 0;}
				if(P2CheckAgainst < 3){P2CheckAgainst = 0;}
			}

			if(P1FourValue != P2FourValue){return P1FourValue > P2FourValue ? true : false;}
			return P1FourSuit > P2FourSuit ? true : false;
		}
		else if(P1HandType == Hands.StraightFlush)
		{
			int P1StraightValue = 0;
			int P1StraightSuits = (int) _First[0].Suit;
			int P2StraightValue = 0;
			int P2StraightSuits = (int) _Second[0].Suit;
			
			for(int i = 0; i < _First.Length; i++)
			{
				P1StraightValue += (int) _First[i].Value;
				P2StraightValue += (int) _Second[i].Value;
			}
			
			if(P1StraightValue != P2StraightValue){return P1StraightValue > P2StraightValue ? true : false;}
			else{return P1StraightSuits > P2StraightSuits ? true : false;}
		}
		else if(P1HandType == Hands.RoyalFlush)
		{
			int P1SuitValue = (int) _First[0].Suit;
			int P2SuitValue = (int) _Second[0].Suit;
			return P1SuitValue > P2SuitValue ? true : false;
		}

		return false;
	}

	public static Tier EvaluateHandTier(List<Card> _Hand, Hands _Type)
	{
		//Tier I being the lowest, Tier IV being the highest
		if(_Type == Hands.HighCard)
		{
			Values HighestValue = _Hand[0].Value;
			for(int i = 0; i < _Hand.Count; i++)
			{
				if((int)_Hand[i].Value > (int)HighestValue)
				{
					HighestValue = _Hand[i].Value;
				}
			}
			
			if(HighestValue == Values.Two || HighestValue == Values.Three || HighestValue == Values.Four)
			{return Tier.One;}
			if(HighestValue == Values.Five || HighestValue == Values.Six || HighestValue == Values.Seven)
			{return Tier.Two;}
			if(HighestValue == Values.Eight || HighestValue == Values.Nine || HighestValue == Values.Ten)
			{return Tier.Three;}
			if(HighestValue == Values.Jack || HighestValue == Values.Queen || HighestValue == Values.King || HighestValue == Values.Ace)
			{return Tier.Four;}
		}
		else if(_Type == Hands.OnePair)
		{
			Values PairValue = _Hand[0].Value;
			for(int a = 0; a < _Hand.Count; a++)
			{
				for(int b = 0; b < _Hand.Count; b++)
				{
					if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
					{
						PairValue = _Hand[b].Value;
						break;
					}
				}
			}
			
			if(PairValue == Values.Two || PairValue == Values.Three || PairValue == Values.Four)
			{return Tier.One;}
			if(PairValue == Values.Five || PairValue == Values.Six || PairValue == Values.Seven)
			{return Tier.Two;}
			if(PairValue == Values.Eight || PairValue == Values.Nine || PairValue == Values.Ten)
			{return Tier.Three;}
			if(PairValue == Values.Jack || PairValue == Values.Queen || PairValue == Values.King || PairValue == Values.Ace)
			{return Tier.Four;}
		}
		else if(_Type == Hands.TwoPair)
		{
			bool FirstPairCheck = false;
			bool SecondPairCheck = false;
			int FirstPairValue = 0;
			int SecondPairValue = 0;
			
			for(int a = 0; a < _Hand.Count; a++)
			{
				for(int b = 0; b < _Hand.Count; b++)
				{
					if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
					{
						if(FirstPairCheck == false && SecondPairCheck == false){FirstPairValue = (int) _Hand[a].Value; FirstPairCheck = true;}
						if((int) _Hand[a].Value != FirstPairValue && FirstPairCheck == true && SecondPairCheck == false){SecondPairValue = (int) _Hand[a].Value; SecondPairCheck = true;}
						break;
					}
				}
				if(FirstPairCheck == true && SecondPairCheck == true){break;}
			}
			
			int TotalValue = FirstPairValue + SecondPairValue;
			if(TotalValue > 0 && TotalValue < 10)
			{return Tier.One;}
			else if(TotalValue >= 10 && TotalValue < 16)
			{return Tier.Two;}
			else if(TotalValue >= 16 && TotalValue < 20)
			{return Tier.Three;}
			else if(TotalValue >= 20)
			{return Tier.Four;}
		}
		else if(_Type == Hands.ThreeOfAKind)
		{
			int CheckAgainst = 0;
			Values HighestValue = 0;
			
			for(int a = 0; a < _Hand.Count; a++)
			{
				for(int b = 0; b < _Hand.Count; b++)
				{
					if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
					{
						HighestValue = _Hand[b].Value;
						CheckAgainst++;
					}
				}
				if(CheckAgainst >= 2){break;}
				else{CheckAgainst = 0;}
			}
			
			if(HighestValue == Values.Two || HighestValue == Values.Three || HighestValue == Values.Four)
			{return Tier.One;}
			if(HighestValue == Values.Five || HighestValue == Values.Six || HighestValue == Values.Seven)
			{return Tier.Two;}
			if(HighestValue == Values.Eight || HighestValue == Values.Nine || HighestValue == Values.Ten)
			{return Tier.Three;}
			if(HighestValue == Values.Jack || HighestValue == Values.Queen || HighestValue == Values.King || HighestValue == Values.Ace)
			{return Tier.Four;}
		}
		else if(_Type == Hands.Straight)
		{
			Values LowestValue = _Hand[0].Value;
			
			for(int i = 0; i < _Hand.Count; i++)
			{
				if((int) _Hand[i].Value < (int) LowestValue)
				{
					LowestValue = _Hand[i].Value;
				}
			}
			
			if(LowestValue == Values.Two || LowestValue == Values.Three || LowestValue == Values.Four)
			{return Tier.One;}
			if(LowestValue == Values.Five || LowestValue == Values.Six)
			{return Tier.Two;}
			if(LowestValue == Values.Seven || LowestValue == Values.Eight)
			{return Tier.Three;}
			if(LowestValue == Values.Nine || LowestValue == Values.Ten)
			{return Tier.Four;}
		}
		else if(_Type == Hands.Flush)
		{
			Suits FlushSuit = _Hand[0].Suit;
			if(FlushSuit == Suits.Clubs)
			{return Tier.One;}
			else if(FlushSuit == Suits.Diamonds)
			{return Tier.Two;}
			else if(FlushSuit == Suits.Hearts)
			{return Tier.Three;}
			else if(FlushSuit == Suits.Spades)
			{return Tier.Four;}
		}
		else if(_Type == Hands.FullHouse)
		{
			int FirstSetValue = 0;
			int FSFirstIndex = 0;
			int FSSecondIndex = 0;
			Values FirstValue = _Hand[0].Value;
			for(int a = 0; a < _Hand.Count; a++)
			{
				for(int b = 0; b < _Hand.Count; b++)
				{
					if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
					{
						FirstSetValue = (int) _Hand[a].Value;
						FirstValue = _Hand[a].Value;
						FSFirstIndex = a;
						FSSecondIndex = b;
						break;
					}
				}
			}
			
			int IdenticalCounter = 0;
			int CheckValue = 0;
			Values SecondValue = _Hand[0].Value;
			for(int a = 0; a < _Hand.Count; a++)
			{
				if(a != FSFirstIndex && a != FSSecondIndex)
				{
					if(IdenticalCounter == 0)
					{
						CheckValue = (int) _Hand[a].Value;
						IdenticalCounter++;
					}
					else if(IdenticalCounter >= 1 && IdenticalCounter < 3)
					{
						if((int)_Hand[a].Value != CheckValue){break;}
						else{IdenticalCounter++; SecondValue = _Hand[a].Value;}
					}
				}
			}
			
			int TotalValue = ((int)FirstValue + 1) + ((int)SecondValue + 1);
			if(TotalValue > 0 && TotalValue < 12)
			{return Tier.One;}
			else if(TotalValue >= 12 && TotalValue < 16)
			{return Tier.Two;}
			else if(TotalValue >= 16 && TotalValue < 20)
			{return Tier.Three;}
			else if(TotalValue >= 20)
			{return Tier.Four;}
		}
		else if(_Type == Hands.FourOfAKind)
		{
			int CheckAgainst = 0;
			Values TypeValue = _Hand[0].Value;
			
			for(int a = 0; a < _Hand.Count; a++)
			{
				for(int b = 0; b < _Hand.Count; b++)
				{
					if(_Hand[b] != _Hand[a] && _Hand[b].Value == _Hand[a].Value)
					{
						CheckAgainst++;
					}
				}
				if(CheckAgainst >= 3){TypeValue = _Hand[a].Value; break;}
				else{CheckAgainst = 0;}
			}
			
			if(TypeValue == Values.Two || TypeValue == Values.Three || TypeValue == Values.Four)
			{return Tier.One;}
			if(TypeValue == Values.Five || TypeValue == Values.Six || TypeValue == Values.Seven)
			{return Tier.Two;}
			if(TypeValue == Values.Eight || TypeValue == Values.Nine || TypeValue == Values.Ten)
			{return Tier.Three;}
			if(TypeValue == Values.Jack || TypeValue == Values.Queen || TypeValue == Values.King || TypeValue == Values.Ace)
			{return Tier.Four;}
		}
		else if(_Type == Hands.StraightFlush)
		{
			Card LowestCard = _Hand[0];
			
			for(int i = 0; i < _Hand.Count; i++)
			{
				if((int) _Hand[i].Value < (int) LowestCard.Value)
				{
					LowestCard.Value = _Hand[i].Value;
				}
			}
			
			if(LowestCard.Suit == Suits.Clubs)
			{
				if(LowestCard.Value == Values.Two || LowestCard.Value == Values.Three || LowestCard.Value == Values.Four)
				{return Tier.One;}
				if(LowestCard.Value == Values.Five || LowestCard.Value == Values.Six)
				{return Tier.Two;}
				if(LowestCard.Value == Values.Seven || LowestCard.Value == Values.Eight)
				{return Tier.Three;}
				if(LowestCard.Value == Values.Nine || LowestCard.Value == Values.Ten)
				{return Tier.Three;}
			}
			else if(LowestCard.Suit == Suits.Diamonds)
			{
				if(LowestCard.Value == Values.Two || LowestCard.Value == Values.Three || LowestCard.Value == Values.Four)
				{return Tier.One;}
				if(LowestCard.Value == Values.Five || LowestCard.Value == Values.Six)
				{return Tier.Two;}
				if(LowestCard.Value == Values.Seven || LowestCard.Value == Values.Eight)
				{return Tier.Three;}
				if(LowestCard.Value == Values.Nine || LowestCard.Value == Values.Ten)
				{return Tier.Four;}
			}
			else if(LowestCard.Suit == Suits.Hearts)
			{
				if(LowestCard.Value == Values.Two || LowestCard.Value == Values.Three || LowestCard.Value == Values.Four)
				{return Tier.Two;}
				if(LowestCard.Value == Values.Five || LowestCard.Value == Values.Six)
				{return Tier.Two;}
				if(LowestCard.Value == Values.Seven || LowestCard.Value == Values.Eight)
				{return Tier.Three;}
				if(LowestCard.Value == Values.Nine || LowestCard.Value == Values.Ten)
				{return Tier.Four;}
			}
			else if(LowestCard.Suit == Suits.Spades)
			{
				if(LowestCard.Value == Values.Two || LowestCard.Value == Values.Three || LowestCard.Value == Values.Four)
				{return Tier.Two;}
				if(LowestCard.Value == Values.Five || LowestCard.Value == Values.Six)
				{return Tier.Three;}
				if(LowestCard.Value == Values.Seven || LowestCard.Value == Values.Eight)
				{return Tier.Four;}
				if(LowestCard.Value == Values.Nine || LowestCard.Value == Values.Ten)
				{return Tier.Four;}
			}
		}
		else if(_Type == Hands.RoyalFlush)
		{
			Suits FlushSuit = _Hand[0].Suit;
			if(FlushSuit == Suits.Clubs)
			{return Tier.One;}
			else if(FlushSuit == Suits.Diamonds)
			{return Tier.Two;}
			else if(FlushSuit == Suits.Hearts)
			{return Tier.Three;}
			else if(FlushSuit == Suits.Spades)
			{return Tier.Four;}
		}
		return Tier.One;
	}

	public static List<Card[]> DetermineRangeOfOpponent(Player _Player, Player _Opponent, bool _IncludePocket)
	{
		Deck SimulatedDeck = new Deck();

		for(int CardIndex = 0; CardIndex < _Player.Hand.Count; CardIndex++)
		{
			SimulatedDeck.RemoveSpecificCard(_Player.Hand[CardIndex]);
		}

		for(int PlayerIndex = 0; PlayerIndex < _Opponent.GManager.Players.Length; PlayerIndex++)
		{
			for(int CardIndex = 0; CardIndex < _Opponent.RevealedCards.Length; CardIndex++)
			{
				SimulatedDeck.RemoveSpecificCard(_Opponent.GManager.Players[PlayerIndex].RevealedCards[CardIndex]);
			}
		}

		List<Card[]> PossibleHands = new List<Card[]>();

		for(int FirstCardIndex = 0; FirstCardIndex < SimulatedDeck.Cards.Count - 1; FirstCardIndex++)
		{
			for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < SimulatedDeck.Cards.Count; SecondCardIndex++)
			{
				Card[] PossibleHand = new Card[5];
				PossibleHand[0] = _Opponent.RevealedCards[0];
				PossibleHand[1] = _Opponent.RevealedCards[1];
				PossibleHand[2] = _Opponent.RevealedCards[2];

				PossibleHand[3] = SimulatedDeck.Cards[FirstCardIndex];
				PossibleHand[4] = SimulatedDeck.Cards[SecondCardIndex];

				PossibleHands.Add(PossibleHand);
			}
		}

		if(_IncludePocket)
		{
			//Pass one card through
			List<Card[]> PossibleHandsWithPocket = new List<Card[]>();
//			Card[] PossibleHand = new Card[]

			if(_Opponent.Pocket.Count > 0)
			{
				for(int HandIndex = 0; HandIndex < PossibleHands.Count; HandIndex++)
				{
					for(int PocketIndex = 0; PocketIndex < _Opponent.Pocket.Count; PocketIndex++)
					{
						for(int CardIndex = 0; CardIndex < PossibleHands[HandIndex].Length; CardIndex++)
						{
							Card[] PossibleHand = new Card[PossibleHands[0].Length];
							PossibleHand[0] = PossibleHands[HandIndex][0];
							PossibleHand[1] = PossibleHands[HandIndex][1];
							PossibleHand[2] = PossibleHands[HandIndex][2];
							PossibleHand[3] = PossibleHands[HandIndex][3];
							PossibleHand[4] = PossibleHands[HandIndex][4];

							PossibleHand[CardIndex] = _Opponent.Pocket[PocketIndex];

							PossibleHandsWithPocket.Add(PossibleHand);
						}
					}	
				}
			}

			if(_Opponent.Pocket.Count > 1)
			{
				//Pass two cards through
				for(int HandIndex = 0; HandIndex < PossibleHands.Count; HandIndex++)
				{
					for(int FirstCardIndex = 0; FirstCardIndex < PossibleHands[HandIndex].Length - 1; FirstCardIndex++)
					{
						for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PossibleHands[HandIndex].Length; SecondCardIndex++)
						{
							Card[] PossibleHand = new Card[PossibleHands[0].Length];
							PossibleHand[0] = PossibleHands[HandIndex][0];
							PossibleHand[1] = PossibleHands[HandIndex][1];
							PossibleHand[2] = PossibleHands[HandIndex][2];
							PossibleHand[3] = PossibleHands[HandIndex][3];
							PossibleHand[4] = PossibleHands[HandIndex][4];

							PossibleHand[FirstCardIndex] = _Opponent.Pocket[0];
							PossibleHand[SecondCardIndex] = _Opponent.Pocket[1];

							PossibleHandsWithPocket.Add(PossibleHand);

							PossibleHand = new Card[PossibleHands[0].Length];
							PossibleHand[0] = PossibleHands[HandIndex][0];
							PossibleHand[1] = PossibleHands[HandIndex][1];
							PossibleHand[2] = PossibleHands[HandIndex][2];
							PossibleHand[3] = PossibleHands[HandIndex][3];
							PossibleHand[4] = PossibleHands[HandIndex][4];

							PossibleHand[FirstCardIndex] = _Opponent.Pocket[1];
							PossibleHand[SecondCardIndex] = _Opponent.Pocket[0];

							PossibleHandsWithPocket.Add(PossibleHand);
						}
					}
				}
			}

			if(_Opponent.Pocket.Count > 2)
			{
				//Pass three cards through
				for(int HandIndex = 0; HandIndex < PossibleHands.Count; HandIndex++)
				{
					for(int FirstCardIndex = 0; FirstCardIndex < PossibleHands[HandIndex].Length - 2; FirstCardIndex++)
					{
						for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PossibleHands[HandIndex].Length - 1; SecondCardIndex++)
						{
							for(int ThirdCardIndex = SecondCardIndex + 1; ThirdCardIndex < PossibleHands[HandIndex].Length; ThirdCardIndex++)
							{
								Card[] PossibleHand = new Card[PossibleHands[0].Length];
								PossibleHand[0] = PossibleHands[HandIndex][0];
								PossibleHand[1] = PossibleHands[HandIndex][1];
								PossibleHand[2] = PossibleHands[HandIndex][2];
								PossibleHand[3] = PossibleHands[HandIndex][3];
								PossibleHand[4] = PossibleHands[HandIndex][4];

								PossibleHand[FirstCardIndex] = _Opponent.Pocket[0];
								PossibleHand[SecondCardIndex] = _Opponent.Pocket[1];
								PossibleHand[ThirdCardIndex] = _Opponent.Pocket[2];

								PossibleHandsWithPocket.Add(PossibleHand);

								PossibleHand = new Card[PossibleHands[0].Length];
								PossibleHand[0] = PossibleHands[HandIndex][0];
								PossibleHand[1] = PossibleHands[HandIndex][1];
								PossibleHand[2] = PossibleHands[HandIndex][2];
								PossibleHand[3] = PossibleHands[HandIndex][3];
								PossibleHand[4] = PossibleHands[HandIndex][4];

								PossibleHand[FirstCardIndex] = _Opponent.Pocket[0];
								PossibleHand[SecondCardIndex] = _Opponent.Pocket[2];
								PossibleHand[ThirdCardIndex] = _Opponent.Pocket[1];

								PossibleHandsWithPocket.Add(PossibleHand);

								PossibleHand = new Card[PossibleHands[0].Length];
								PossibleHand[0] = PossibleHands[HandIndex][0];
								PossibleHand[1] = PossibleHands[HandIndex][1];
								PossibleHand[2] = PossibleHands[HandIndex][2];
								PossibleHand[3] = PossibleHands[HandIndex][3];
								PossibleHand[4] = PossibleHands[HandIndex][4];

								PossibleHand[FirstCardIndex] = _Opponent.Pocket[1];
								PossibleHand[SecondCardIndex] = _Opponent.Pocket[0];
								PossibleHand[ThirdCardIndex] = _Opponent.Pocket[2];

								PossibleHandsWithPocket.Add(PossibleHand);

								PossibleHand = new Card[PossibleHands[0].Length];
								PossibleHand[0] = PossibleHands[HandIndex][0];
								PossibleHand[1] = PossibleHands[HandIndex][1];
								PossibleHand[2] = PossibleHands[HandIndex][2];
								PossibleHand[3] = PossibleHands[HandIndex][3];
								PossibleHand[4] = PossibleHands[HandIndex][4];

								PossibleHand[FirstCardIndex] = _Opponent.Pocket[1];
								PossibleHand[SecondCardIndex] = _Opponent.Pocket[2];
								PossibleHand[ThirdCardIndex] = _Opponent.Pocket[0];

								PossibleHandsWithPocket.Add(PossibleHand);

								PossibleHand = new Card[PossibleHands[0].Length];
								PossibleHand[0] = PossibleHands[HandIndex][0];
								PossibleHand[1] = PossibleHands[HandIndex][1];
								PossibleHand[2] = PossibleHands[HandIndex][2];
								PossibleHand[3] = PossibleHands[HandIndex][3];
								PossibleHand[4] = PossibleHands[HandIndex][4];

								PossibleHand[FirstCardIndex] = _Opponent.Pocket[2];
								PossibleHand[SecondCardIndex] = _Opponent.Pocket[0];
								PossibleHand[ThirdCardIndex] = _Opponent.Pocket[1];

								PossibleHandsWithPocket.Add(PossibleHand);

								PossibleHand = new Card[PossibleHands[0].Length];
								PossibleHand[0] = PossibleHands[HandIndex][0];
								PossibleHand[1] = PossibleHands[HandIndex][1];
								PossibleHand[2] = PossibleHands[HandIndex][2];
								PossibleHand[3] = PossibleHands[HandIndex][3];
								PossibleHand[4] = PossibleHands[HandIndex][4];

								PossibleHand[FirstCardIndex] = _Opponent.Pocket[2];
								PossibleHand[SecondCardIndex] = _Opponent.Pocket[1];
								PossibleHand[ThirdCardIndex] = _Opponent.Pocket[0];

								PossibleHandsWithPocket.Add(PossibleHand);
							}
						}
					}
				}
			}

//			//Add the possible hands with pockets into the total possible hands
			for(int HandIndex = 0; HandIndex < PossibleHandsWithPocket.Count; HandIndex++)
				PossibleHands.Add(PossibleHandsWithPocket[HandIndex]);
		}

		return PossibleHands;
	}

	private static int CalculateTotalAmtOfPossibleHands(Player _Player, Deck _CurrentDeck, bool _IncludeActualHand, bool _IncludePocket)
	{
		/*CALCULATE AMT OF POSSIBLE HANDS:
		BASE HAND: 1
		WITH POCKET: 5  + (4 * (4 + 1)) / 2 * 2 + (3 * (3 + 1)) / 2 * 6 = 61
		CURRENT POSSIBLE HANDS: 1 + 61 = 62
		WITH DECK HANDS: (5 * 62 * SAMPLEDECK.COUNT) + (((4 * (4 + 1)) / 2) * CURRENT POSSIBLE HANDS * (((SAMPLEDECK.COUNT - 1) * ((SAMPLEDECK.COUNT - 1) + 1)) / 2)) + 
						 ((3 * (3 + 1)) / 2) * CURRENT POSSIBLE HANDS * (((SAMPLEDECK.COUNT - 2) * ((SAMPLEDECK.COUNT - 2) + 1)/2
		*/

		int TotalAmtOfPossibleHands = 1;

		if(_Player.Pocket.Count > 0)
			TotalAmtOfPossibleHands += 5;

		if(_Player.Pocket.Count > 1)
			TotalAmtOfPossibleHands += ( (4 * (4 + 1)) / 2 );

		if(_Player.Pocket.Count > 2)
			TotalAmtOfPossibleHands += ( (3 * (3 + 1)) / 2 );

		int BaseAmount = TotalAmtOfPossibleHands;

		TotalAmtOfPossibleHands += 5 * BaseAmount * _CurrentDeck.Cards.Count;
		TotalAmtOfPossibleHands += ( 4 + 3 + 2 + 1 ) * BaseAmount * ( ((_CurrentDeck.Cards.Count - 1) * (_CurrentDeck.Cards.Count - 1 + 1)) / 2 );
		TotalAmtOfPossibleHands += ( 3 + 2 + 1 ) * BaseAmount * ( ((_CurrentDeck.Cards.Count - 2) * (_CurrentDeck.Cards.Count - 2 + 1)) / 2 );

		return TotalAmtOfPossibleHands;
	}

	public static LinkedList<Card[]> DeterminePossibleHand(Player _Player, bool _IncludeActualHand,  bool _IncludePocket)
	{
		LinkedList<Card[]> PossibleHands = new LinkedList<Card[]>();
		Dictionary<Values,bool> ValueCheck = new Dictionary<Values, bool>();

		Card[] PlayerHand = _Player.Hand.ToArray();
		Card[] PlayerEffectiveHand = GetEffectiveHandWithPocket(_Player);
		Card[] PlayerPocket = _Player.Pocket.ToArray();
		Card[] PlayerCards = Utility.GetAllOfPlayerCards(_Player);
		Card[] SortedHand = Utility.SortHandByCards(PlayerHand,false);
		Card[] SortedEffectiveHand = Utility.SortHandByCards(PlayerEffectiveHand,false);
		Card[] SortedCards = Utility.SortHandByCards(PlayerCards,false);

		Hands PlayerHandType = EvaluatePlayerHandWithPocket(_Player);
		Hands HighestHandType = Hands.RoyalFlush;
		TurnPhase CurrentTurnPhase = _Player.GManager.Phase;
		AuctionPhase CurrentAuctionPhase = _Player.GManager.AuctionPhase;

		for(int ValueIndex = (int) Values.Two; ValueIndex <= (int) Values.Ace; ValueIndex++)
		{
			ValueCheck.Add((Values) ValueIndex, false);
		}

		for(int HandIndex = (int) HighestHandType; HandIndex >= (int) PlayerHandType; HandIndex--)
		{
			Deck SampleDeck = new Deck();
			
			#region Generate a sample deck to simulate the remaining cards in the current game
			SampleDeck.Cards.Clear();
			
			for(int CardIndex = 0; CardIndex < _Player.GManager.Deck.Cards.Count; CardIndex++)
			{
				SampleDeck.Cards.Add(_Player.GManager.Deck.Cards[CardIndex]);
			}
			
			Debug.Log("Remaining cards in deck: " + SampleDeck.Cards.Count);
			#endregion
			
			if((Hands) HandIndex == Hands.RoyalFlush)
			{
				if(PlayerHandType == Hands.RoyalFlush)
				{
					Debug.Log("No superior hands can be possible made from future cards");
					break;
				}

				List<Card> KeyCards = new List<Card>();
				Dictionary<Card,bool> RequiredCardsFound = new Dictionary<Card, bool>();

				Suits[] SuitRange = new Suits[4];
				SuitRange[0] = Suits.Clubs;
				SuitRange[1] = Suits.Diamonds;
				SuitRange[2] = Suits.Spades;
				SuitRange[3] = Suits.Hearts;
				Values[] ValueRange = new Values[5];
				ValueRange[0] = Values.Ten;
				ValueRange[1] = Values.Jack;
				ValueRange[2] = Values.Queen;
				ValueRange[3] = Values.King;
				ValueRange[4] = Values.Ace;

				#region Check whether the player's hand have the necessary cards needed to form a Royal Flush in later turns
				if(CurrentTurnPhase == TurnPhase.Betting)
				{
					KeyCards.AddRange(Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange));
					
					if((KeyCards.Count < 2) || (KeyCards.Count == 2 && KeyCards[0].Suit != KeyCards[1].Suit) || (KeyCards.Count == 3 && KeyCards[0].Suit != KeyCards[1].Suit 
					&& KeyCards[1].Suit != KeyCards[2].Suit && KeyCards[0].Suit != KeyCards[2].Suit))
						continue;

					for(int FirstCardIndex = 0; FirstCardIndex < KeyCards.Count - 1; FirstCardIndex++)
					{
						Suits ReferenceSuit = KeyCards[FirstCardIndex].Suit;

						if(Utility.CountSuitOccurance(KeyCards.ToArray(),ReferenceSuit) < 2)
							KeyCards.RemoveAt(FirstCardIndex);
					}

					if(KeyCards.Count < 2)
						continue;
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
				{
					KeyCards.AddRange(Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange));

					if((KeyCards.Count < 3) || (KeyCards.Count == 3 && (KeyCards[0].Suit != KeyCards[1].Suit || KeyCards[1].Suit != KeyCards[2].Suit || KeyCards[0].Suit != KeyCards[2].Suit)))
						continue;

					if(KeyCards.Count >= 4)
					{
						for(int FirstCardIndex = 0; FirstCardIndex < KeyCards.Count; FirstCardIndex++)
						{
							Suits ReferenceSuit = KeyCards[FirstCardIndex].Suit;

							if(Utility.CountSuitOccurance(KeyCards.ToArray(),ReferenceSuit) < 3)
								KeyCards.RemoveAt(FirstCardIndex);
						}
					}

					if(KeyCards.Count < 3)
						continue;
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
				{
					KeyCards.AddRange(Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange));

					if(KeyCards.Count < 4)
						continue;

					if(KeyCards.Count == 4)
					{
						bool Is4CardsSameSuit = false;

						for(int SuitIndex = (int) Suits.Clubs; SuitIndex < (int) Suits.Spades; SuitIndex++)
						{
							if(Utility.CountSuitOccurance(KeyCards.ToArray(),(Suits) SuitIndex) == 4)
							{
								Is4CardsSameSuit = true;
								break;
							}
						}

						if(!Is4CardsSameSuit)
							continue;
					}

					if(KeyCards.Count >= 5)
					{
						for(int CardIndex = 0; CardIndex < KeyCards.Count; CardIndex++)
						{
							if(Utility.CountSuitOccurance(KeyCards.ToArray(),KeyCards[CardIndex].Suit) < 4)
							{
								KeyCards.RemoveAt(CardIndex);
							}
						}
					}

					if(KeyCards.Count < 4)
						continue;
				}
				#endregion

				if(KeyCards.Count <= 0)
					continue;

				#region Go through each type of suits, check whether the player's hand combining with remaining cards in deck. If the player can form the hand, add that possible hand
				for(int SuitIndex = (int) Suits.Clubs; SuitIndex <= (int) Suits.Spades; SuitIndex++)
				{
					//Use a Dictionary to store a list of cards that need to exist in player's hand and deck. A boolean is also in each Dictionary entry to check whether the required card is fulfilled
					RequiredCardsFound.Clear();
					RequiredCardsFound.Add(new Card((Suits) SuitIndex, Values.Ten),false);
					RequiredCardsFound.Add(new Card((Suits) SuitIndex, Values.Jack),false);
					RequiredCardsFound.Add(new Card((Suits) SuitIndex, Values.Queen),false);
					RequiredCardsFound.Add(new Card((Suits) SuitIndex, Values.King),false);
					RequiredCardsFound.Add(new Card((Suits) SuitIndex, Values.Ace),false);

					//Go through player's hand and see whether the cards needed with that specific suit is available, and toggle the boolean in Dictionary's coressponding boolean
					//Go through remaining cards in deck and see whether the cards needed with that specific suit is available, and toggle the boolean in Dictionary's coressponding boolean
					foreach(KeyValuePair<Card,bool> RequiredCard in RequiredCardsFound)
					{
						if(RequiredCard.Value == false)
						{
							for(int CardIndex = 0; CardIndex < KeyCards.Count; CardIndex++)
							{
								if(KeyCards[CardIndex].Suit == (Suits) SuitIndex && KeyCards[CardIndex].Value == RequiredCard.Key.Value)
								{
									RequiredCardsFound[RequiredCard.Key] = true;
									break;
								}
							}
						}
						if(RequiredCard.Value == false)
						{
							for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
							{
								if(SampleDeck.Cards[CardIndex].Value == RequiredCard.Key.Value && SampleDeck.Cards[CardIndex].Suit == (Suits) SuitIndex)
								{
									RequiredCardsFound[RequiredCard.Key] = true;
									break;
								}
							}
						}
					}

					//Check whether all required cards are possible to enter player's hand
					bool RequirementClear = true;
					foreach(KeyValuePair<Card,bool> RequiredCard in RequiredCardsFound)
					{
						if(RequiredCard.Value == false)
						{
							RequirementClear = false;
							break;
						}
					}

					//If it is possible, add that hand to player's possible hand
					if(RequirementClear == true)
					{
						Card[] NewHand = new Card[5];
						NewHand[0] = new Card((Suits) SuitIndex, Values.Ten);
						NewHand[1] = new Card((Suits) SuitIndex, Values.Jack);
						NewHand[2] = new Card((Suits) SuitIndex, Values.Queen);
						NewHand[3] = new Card((Suits) SuitIndex, Values.King);
						NewHand[4] = new Card((Suits) SuitIndex, Values.Ace);

						PossibleHands.AddLast(NewHand);
					}
				}
				#endregion
			}

			else if((Hands) HandIndex == Hands.StraightFlush)
			{
				if(PlayerHandType == Hands.StraightFlush)
				{
					#region Identify the Highest and Lowest card of the straight aspect of the hand
					Card HighestCard = new Card(PlayerHand[0].Suit,Values.Two);
					Card LowestCard = new Card(PlayerHand[0].Suit,Values.Ace);

					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						if(PlayerHand[CardIndex].Value > HighestCard.Value)
							HighestCard = PlayerHand[CardIndex];

						if(PlayerHand[CardIndex].Value < LowestCard.Value)
							LowestCard = PlayerHand[CardIndex];
					}
					#endregion

					List<Card> CardToSearch = new List<Card>();

					#region Identify the cards that are needed to improve the current straight flush hand
					if(CurrentTurnPhase == TurnPhase.Betting)
					{
						if(HighestCard.Value == Values.Ace)
							continue;
						else if(HighestCard.Value == Values.King)
						{
							CardToSearch.Add(new Card(PlayerHand[0].Suit,Values.Ace));
						}
						else if(HighestCard.Value == Values.Queen)
						{
							CardToSearch.Add(new Card(PlayerHand[0].Suit,Values.King));
							CardToSearch.Add(new Card(PlayerHand[0].Suit,Values.Ace));
						}
						else
						{
							CardToSearch.Add(new Card(PlayerHand[0].Suit,HighestCard.Value + 1));
							CardToSearch.Add(new Card(PlayerHand[0].Suit,HighestCard.Value + 2));
							CardToSearch.Add(new Card(PlayerHand[0].Suit,HighestCard.Value + 3));
						}
					}
					else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
					{
						if(HighestCard.Value == Values.Ace)
							continue;
						else if(HighestCard.Value == Values.King)
						{
							CardToSearch.Add(new Card(PlayerHand[0].Suit, Values.Ace));
						}
						else
						{
							CardToSearch.Add(new Card(PlayerHand[0].Suit, HighestCard.Value + 1));
							CardToSearch.Add(new Card(PlayerHand[0].Suit, HighestCard.Value + 2));
						}
					}
					else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
					{
						if(HighestCard.Value == Values.Ace)
							continue;
						else
							CardToSearch.Add(new Card(PlayerHand[0].Suit, HighestCard.Value + 1));
					}
					#endregion

					if(CardToSearch.Count <= 0)
						continue;

					List<Card> CardFound = new List<Card>();

					#region Go through the remaining cards in the deck and determine whether it has the cards needed to improve the hand
					for(int SearchIndex = 0; SearchIndex < CardToSearch.Count; SearchIndex++)
					{
						for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
						{
							if(Utility.IsTwoCardsIdentical(CardToSearch[SearchIndex],SampleDeck.Cards[CardIndex]))
							{
								CardFound.Add(CardToSearch[SearchIndex]);
								break;
							}
						}
					}
					#endregion

					#region Check whether all the needed cards are in the deck. if they are, "Craft" the necessary hand and add it to the possible hand
					if(CardFound.Count == CardToSearch.Count)
					{
						Card[] PossibleHand = new Card[PlayerEffectiveHand.Length];

						if(CardFound.Count == 1)
						{
							PossibleHand[0] = PlayerEffectiveHand[0];
							PossibleHand[1] = PlayerEffectiveHand[1];
							PossibleHand[2] = PlayerEffectiveHand[2];
							PossibleHand[3] = PlayerEffectiveHand[3];
							PossibleHand[4] = CardToSearch[0];

							PossibleHands.AddLast(PossibleHand);
						}
						else if(CardFound.Count == 2)
						{
							PossibleHand[0] = PlayerEffectiveHand[0];
							PossibleHand[1] = PlayerEffectiveHand[1];
							PossibleHand[2] = PlayerEffectiveHand[2];
							PossibleHand[3] = CardToSearch[0];
							PossibleHand[4] = CardToSearch[1];

							PossibleHands.AddLast(PossibleHand);
						}
						else if(CardFound.Count == 3)
						{
							PossibleHand[0] = PlayerEffectiveHand[0];
							PossibleHand[1] = PlayerEffectiveHand[1];
							PossibleHand[2] = CardToSearch[0];
							PossibleHand[3] = CardToSearch[1];
							PossibleHand[4] = CardToSearch[2];

							PossibleHands.AddLast(PossibleHand);
						}
					}
					#endregion
				}

				List<Card> PossibleCombination = new List<Card>();
				List<Card[]> CombinationList = new List<Card[]>();

				for(int FirstCardIndex = 0; FirstCardIndex < SortedEffectiveHand.Length - 1; FirstCardIndex++)
				{
					for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < SortedEffectiveHand.Length; SecondCardIndex++)
					{
						if(SortedEffectiveHand[FirstCardIndex].Suit == SortedEffectiveHand[SecondCardIndex].Suit && ((int) SortedEffectiveHand[SecondCardIndex].Value - (int) SortedEffectiveHand[FirstCardIndex].Value) <= 4)
						{
							PossibleCombination.Clear();
							PossibleCombination.Add(SortedEffectiveHand[FirstCardIndex]);
							PossibleCombination.Add(SortedEffectiveHand[SecondCardIndex]);

							for(int SequentialIndex = SecondCardIndex + 1; SequentialIndex < SortedEffectiveHand.Length; SequentialIndex++)
							{
								if(SortedEffectiveHand[FirstCardIndex].Suit == SortedEffectiveHand[SequentialIndex].Suit)
								{
									if(((int) SortedEffectiveHand[SequentialIndex].Value - (int) SortedEffectiveHand[FirstCardIndex].Value) <= 4)
									{
										PossibleCombination.Add(SortedEffectiveHand[SequentialIndex]);
										continue;
									}

									break;
								}
							}

							CombinationList.Add(PossibleCombination.ToArray());
						}
					}	
				}

				for(int CombinationIndex = 0; CombinationIndex < CombinationList.Count; CombinationIndex++)
				{
					int AmtOfMissingCardsInCombo = 5 - CombinationList.Count;
					int MaximumMissingCard = 5;

					if(_Player.GManager.Phase == TurnPhase.Betting)
						MaximumMissingCard = 3;
					else if(_Player.GManager.Phase == TurnPhase.Auctioning && _Player.GManager.AuctionPhase == AuctionPhase.First)
						MaximumMissingCard = 2;
					else if(_Player.GManager.Phase == TurnPhase.Auctioning && (_Player.GManager.AuctionPhase == AuctionPhase.Second || _Player.GManager.AuctionPhase == AuctionPhase.Third))
						MaximumMissingCard = 1;

					if(AmtOfMissingCardsInCombo > MaximumMissingCard)
						continue;

					Card[] PossibleHand = new Card[5];
					
					if(AmtOfMissingCardsInCombo == 3)
					{
						Card RequiredCard_First = new Card(CombinationList[CombinationIndex][1].Suit,(Values)(CombinationList[CombinationIndex][1].Value + 1));
						Card RequiredCard_Second = new Card(RequiredCard_First.Suit,(Values)(RequiredCard_First.Value + 1));
						Card RequiredCard_Third = new Card(RequiredCard_Second.Suit,(Values)(RequiredCard_Second.Value + 1));

						if(Utility.IsCardInDeck(SampleDeck,RequiredCard_First) && Utility.IsCardInDeck(SampleDeck,RequiredCard_Second) && Utility.IsCardInDeck(SampleDeck,RequiredCard_Third))
						{
							PossibleHand[0] = CombinationList[CombinationIndex][0];
							PossibleHand[1] = CombinationList[CombinationIndex][1];
							PossibleHand[2] = new Card(PossibleHand[1].Suit,(Values)(PossibleHand[1].Value + 1));
							PossibleHand[3] = new Card(PossibleHand[2].Suit,(Values)(PossibleHand[2].Value + 1));
							PossibleHand[4] = new Card(PossibleHand[3].Suit,(Values)(PossibleHand[3].Value + 1));
							
							PossibleHands.AddLast(PossibleHand);
						}
					}
					else if(AmtOfMissingCardsInCombo == 2)
					{
						Card RequiredCard_First = new Card(CombinationList[CombinationIndex][2].Suit,(Values)(CombinationList[CombinationIndex][2].Value + 1));
						Card RequiredCard_Second = new Card(RequiredCard_First.Suit,(Values)(RequiredCard_First.Value + 1));

						if(Utility.IsCardInDeck(SampleDeck,RequiredCard_First) && Utility.IsCardInDeck(SampleDeck,RequiredCard_Second))
						{
							PossibleHand[0] = CombinationList[CombinationIndex][0];
							PossibleHand[1] = CombinationList[CombinationIndex][1];
							PossibleHand[2] = CombinationList[CombinationIndex][2];
							PossibleHand[3] = RequiredCard_First;
							PossibleHand[4] = RequiredCard_Second;
							
							PossibleHands.AddLast(PossibleHand);
						}
					}
					else if(AmtOfMissingCardsInCombo == 1)
					{
						Card RequiredCard = new Card(CombinationList[CombinationIndex][3].Suit,(Values)(CombinationList[CombinationIndex][3].Value + 1));

						if(Utility.IsCardInDeck(SampleDeck,RequiredCard))
						{
							PossibleHand[0] = CombinationList[CombinationIndex][0];
							PossibleHand[1] = CombinationList[CombinationIndex][1];
							PossibleHand[2] = CombinationList[CombinationIndex][2];
							PossibleHand[3] = CombinationList[CombinationIndex][3];
							PossibleHand[4] = RequiredCard;
							
							PossibleHands.AddLast(PossibleHand);
						}
					}
					else if(AmtOfMissingCardsInCombo == 0)
					{
						PossibleHand[0] = CombinationList[CombinationIndex][0];
						PossibleHand[1] = CombinationList[CombinationIndex][1];
						PossibleHand[2] = CombinationList[CombinationIndex][2];
						PossibleHand[3] = CombinationList[CombinationIndex][3];
						PossibleHand[4] = CombinationList[CombinationIndex][4];

						PossibleHands.AddLast(PossibleHand);
					}
				}
			}

			else if((Hands) HandIndex == Hands.FourOfAKind)
			{
				if(PlayerHandType == Hands.FourOfAKind)
				{
					if(CurrentTurnPhase != TurnPhase.Betting)
						continue;

					#region Determine whether the kicker card in player's hand has a higher value then their four of a kind
					Card OddCard = new Card(Suits.NULL,Values.NULL);
					int FourOfAKindIndex = 0;

					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						if(Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value) == 1)
						{
							OddCard.Suit = PlayerHand[CardIndex].Suit;
							OddCard.Value = PlayerHand[CardIndex].Value;
							break;
						}

						FourOfAKindIndex = CardIndex;
					}

					bool IsOddCardLarger = true;

					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						if(PlayerHand[CardIndex].Value > OddCard.Value)
						{
							IsOddCardLarger = false;
							break;
						}
					}

					//If the kicker card is larger than the four of a kind's value, the potential next 3 cards to be drawn may help to form that superior four of a kind with that kicker card
					if(!IsOddCardLarger)
						continue;
					#endregion

					//Determine whether the other suits of that kicker cards are all in the deck
					int OccuranceOfOddCardInDeck = Utility.CountValueOccurance(SampleDeck.Cards,OddCard.Value);

					//If they are, it means that a superior hand may be possibly drawn, so add that superior hand into the possiblehands
					if(OccuranceOfOddCardInDeck == 3)
					{
						Card[] PossibleHand = new Card[PlayerHand.Length];
						PossibleHand[0] = new Card(Suits.Spades, OddCard.Value);
						PossibleHand[1] = new Card(Suits.Spades, OddCard.Value);
						PossibleHand[2] = new Card(Suits.Spades, OddCard.Value);
						PossibleHand[3] = new Card(Suits.Spades, OddCard.Value);
						PossibleHand[4] = PlayerHand[FourOfAKindIndex];

						PossibleHands.AddLast(PossibleHand);
					}
				}

				if(CurrentTurnPhase == TurnPhase.Betting)
				{
					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						if(Utility.CountValueOccurance(SampleDeck.Cards,PlayerHand[CardIndex].Value) < 3)
							continue;

						Card PotentialKickerCard = new Card(Suits.NULL,Values.NULL);
						Values HighestValue = Values.Two;
						for(int CheckIndex = 0; CheckIndex < PlayerHand.Length; CheckIndex++)
						{
							if(!Utility.IsTwoCardsIdentical(PlayerHand[CardIndex], PlayerHand[CheckIndex]) && PlayerHand[CheckIndex].Value > HighestValue)
							{
								PotentialKickerCard = PlayerHand[CheckIndex];
								HighestValue = PlayerHand[CheckIndex].Value;
							}
						}

						Card[] PossibleHand = new Card[PlayerHand.Length];
						PossibleHand[0] = new Card(Suits.Spades,PlayerHand[CardIndex].Value);
						PossibleHand[1] = new Card(Suits.Hearts,PlayerHand[CardIndex].Value);
						PossibleHand[2] = new Card(Suits.Diamonds,PlayerHand[CardIndex].Value);
						PossibleHand[3] = new Card(Suits.Clubs,PlayerHand[CardIndex].Value);
						PossibleHand[4] = PotentialKickerCard;

						PossibleHands.AddLast(PossibleHand);
					}
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning)
				{
					for(int CardIndex = 0; CardIndex < PlayerCards.Length; CardIndex++)
					{
						if(CurrentAuctionPhase == AuctionPhase.First && Utility.CountValueOccurance(PlayerCards,PlayerCards[CardIndex].Value) < 2 
						&& Utility.CountValueOccurance(SampleDeck.Cards,PlayerCards[CardIndex].Value) > 2)
							continue;

						if((CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third) && Utility.CountValueOccurance(PlayerCards,PlayerCards[CardIndex].Value) < 3 
						&& Utility.CountValueOccurance(SampleDeck.Cards,PlayerCards[CardIndex].Value) > 1)
							continue;

						if(Utility.CountValueOccurance(PlayerCards,PlayerCards[CardIndex].Value) + Utility.CountValueOccurance(SampleDeck.Cards,PlayerCards[CardIndex].Value) != 4)
							continue;

						Card PotentialKickerCard = new Card(Suits.NULL,Values.NULL);
						Values HighestValue = Values.Two;
						for(int CheckIndex = 0; CheckIndex < PlayerCards.Length; CheckIndex++)
						{
							if(PlayerCards[CheckIndex].Value != PlayerCards[CardIndex].Value && PlayerCards[CheckIndex].Value > HighestValue)
							{
								PotentialKickerCard = PlayerCards[CheckIndex];
								HighestValue = PlayerCards[CheckIndex].Value;
							}
						}

						Card[] PossibleHand = new Card[PlayerHand.Length];
						PossibleHand[0] = new Card(Suits.Spades,PlayerCards[CardIndex].Value);
						PossibleHand[1] = new Card(Suits.Hearts,PlayerCards[CardIndex].Value);
						PossibleHand[2] = new Card(Suits.Diamonds,PlayerCards[CardIndex].Value);
						PossibleHand[3] = new Card(Suits.Clubs,PlayerCards[CardIndex].Value);
						PossibleHand[4] = PotentialKickerCard;

						PossibleHands.AddLast(PossibleHand);
					}
				}
			}

			else if((Hands) HandIndex == Hands.FullHouse)
			{
				if(PlayerHandType == Hands.FullHouse)
				{
					Values ThreeCardsValue = Values.Two;
					Values TwoCardsValue = Values.Two;

					#region Identify the value of the Three of a Kind and One Pair in the Full House Hand
					for(int CardIndex = 0; CardIndex < PlayerCards.Length; CardIndex++)
					{
						int ValueOccurance = Utility.CountValueOccurance(PlayerCards,PlayerCards[CardIndex].Value);

						if(ValueOccurance < 2)
							continue;

						if(ValueOccurance >= 3 && PlayerCards[CardIndex].Value > ThreeCardsValue)
							ThreeCardsValue = PlayerCards[CardIndex].Value;

						else if(ValueOccurance >= 3 && PlayerCards[CardIndex].Value < ThreeCardsValue && PlayerCards[CardIndex].Value > TwoCardsValue)
							TwoCardsValue = PlayerCards[CardIndex].Value;

						else if(ValueOccurance == 2 && PlayerCards[CardIndex].Value > TwoCardsValue)
							TwoCardsValue = PlayerCards[CardIndex].Value;
					}
					#endregion

					Values[] PairRange = new Values[1];
					Values[] ThreeOfAKindRange = new Values[1];
					Suits[] SuitRange = new Suits[4];
					Card[] PairCards = Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,PairRange);
					Card[] ThreeOfAKindCards = Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ThreeOfAKindRange);
					Card[] DeckCardsToImprove = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,PairRange);
					Dictionary<Values,bool> RequiredValuesFound = new Dictionary<Values,bool>();

					PairRange[0] = TwoCardsValue;

					ThreeOfAKindRange[0] = ThreeCardsValue;

					SuitRange[0] = Suits.Clubs;
					SuitRange[1] = Suits.Diamonds;
					SuitRange[2] = Suits.Hearts;
					SuitRange[3] = Suits.Spades;

					for(int ValuesIndex = (int) Values.Two; ValuesIndex <= (int) Values.Ace; ValuesIndex++)
					{
						RequiredValuesFound.Add((Values) ValuesIndex, false);
					}
					
					if(CurrentTurnPhase == TurnPhase.Betting)
					{
						Card[] PossibleHand = new Card[5];

						#region If the values of one pair is higher than the values of three of a kind, the one pair might have a chance to be a new superior three of kind in the hand
						if(TwoCardsValue > ThreeCardsValue)
						{
							if(Utility.CountValueOccurance(SampleDeck.Cards, TwoCardsValue) <= 0)
								continue;

							if(DeckCardsToImprove.Length <= 0)
								continue;

							PossibleHand = new Card[5];
							PossibleHand[0] = ThreeOfAKindCards[0];          
							PossibleHand[1] = ThreeOfAKindCards[1];
							PossibleHand[2] = DeckCardsToImprove[0];       
							PossibleHand[3] = PairCards[0]; 
							PossibleHand[4] = PairCards[2]; 

							PossibleHands.AddLast(PossibleHand);
						}
						#endregion

						List<Card[]> PairAndSetPotentialCombinations = new List<Card[]>();

						#region Find all the potential Sets and Pairs in the deck that are superior in terms of value than the Set and Pair in player's hand
						for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
						{
							if(!RequiredValuesFound[SampleDeck.Cards[CardIndex].Value] && SampleDeck.Cards[CardIndex].Value > ThreeCardsValue && Utility.CountValueOccurance(SampleDeck.Cards,SampleDeck.Cards[CardIndex].Value) >= 3)
							{
								ThreeOfAKindRange[0] = SampleDeck.Cards[CardIndex].Value;
								RequiredValuesFound[ThreeOfAKindRange[0]] = true;
								PairAndSetPotentialCombinations.Add(Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ThreeOfAKindRange));
							}
							else if(!RequiredValuesFound[SampleDeck.Cards[CardIndex].Value] && SampleDeck.Cards[CardIndex].Value > TwoCardsValue && Utility.CountValueOccurance(SampleDeck.Cards,SampleDeck.Cards[CardIndex].Value) >= 2)
							{
								PairRange[0] = SampleDeck.Cards[CardIndex].Value;
								RequiredValuesFound[PairRange[0]] = true;
								PairAndSetPotentialCombinations.Add(Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,PairRange));
							}
						}
						#endregion

						if(PairAndSetPotentialCombinations.Count <= 0)
							continue;

						#region Go through all the potential Sets and Pairs and generate the relative possible hands
						for(int CombinationIndex = 0; CombinationIndex < PairAndSetPotentialCombinations.Count; CombinationIndex++)
						{
							if(PairAndSetPotentialCombinations[CombinationIndex].Length == 2)
							{
								PossibleHand = new Card[5];
								PossibleHand[0] = ThreeOfAKindCards[0];
								PossibleHand[1] = ThreeOfAKindCards[1];
								PossibleHand[2] = ThreeOfAKindCards[2];
								PossibleHand[3] = PairAndSetPotentialCombinations[CombinationIndex][0];
								PossibleHand[4] = PairAndSetPotentialCombinations[CombinationIndex][1];

								PossibleHands.AddLast(PossibleHand);
							}
							else if(PairAndSetPotentialCombinations[CombinationIndex].Length == 3)
							{
								PossibleHand = new Card[5];
								PossibleHand[0] = PairCards[0];
								PossibleHand[1] = PairCards[1];
								PossibleHand[2] = PairAndSetPotentialCombinations[CombinationIndex][0];
								PossibleHand[3] = PairAndSetPotentialCombinations[CombinationIndex][1];
								PossibleHand[4] = PairAndSetPotentialCombinations[CombinationIndex][2];

								PossibleHands.AddLast(PossibleHand);
							}
						}
						#endregion
					}
					else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
					{
						Card[] PossibleHand = new Card[PlayerHand.Length];

						#region If the values of one pair is higher than the values of three of a kind, the one pair might have a chance to be a new superior three of kind in the hand
						if(TwoCardsValue > ThreeCardsValue)
						{
							if(Utility.CountValueOccurance(SampleDeck.Cards, TwoCardsValue) <= 0)
								continue;

							if(DeckCardsToImprove.Length <= 0)
								continue;

							PossibleHand = new Card[5];
							PossibleHand[0] = ThreeOfAKindCards[0];          
							PossibleHand[1] = ThreeOfAKindCards[1];
							PossibleHand[2] = DeckCardsToImprove[0];        
							PossibleHand[3] = PairCards[0]; 
							PossibleHand[4] = PairCards[2]; 

							PossibleHands.AddLast(PossibleHand);
						}
						#endregion

						List<Card[]> PairCombinations = new List<Card[]>();

						#region Find all potential pairs in the deck that are superior in terms of value than the pair in player's hand
						for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
						{
							if(!RequiredValuesFound[SampleDeck.Cards[CardIndex].Value] && SampleDeck.Cards[CardIndex].Value > TwoCardsValue && Utility.CountValueOccurance(SampleDeck.Cards,SampleDeck.Cards[CardIndex].Value) >= 2)
							{
								PairRange[0] = SampleDeck.Cards[CardIndex].Value;
								RequiredValuesFound[SampleDeck.Cards[CardIndex].Value] = true;
								PairCombinations.Add(Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,PairRange));
							}
						}
						#endregion

						if(PairCombinations.Count <= 0)
							continue;

						#region Go through all the potential pairs and generate the relative possible hands
						for(int CombinationIndex = 0; CombinationIndex < PairCombinations.Count; CombinationIndex++)
						{
							PossibleHand = new Card[5];
							PossibleHand[0] = ThreeOfAKindCards[0];
							PossibleHand[1] = ThreeOfAKindCards[1];
							PossibleHand[2] = ThreeOfAKindCards[2];
							PossibleHand[3] = PairCombinations[CombinationIndex][0];
							PossibleHand[4] = PairCombinations[CombinationIndex][1];

							PossibleHands.AddLast(PossibleHand);
						}
						#endregion
					}
					else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
						continue;
				}

				if(CurrentTurnPhase == TurnPhase.Betting)
				{
					if(EvaluateHand(PlayerHand) == Hands.FullHouse)
						continue;

					List<Card[]> KeyCombinations = new List<Card[]>();
					Card[] Combination = new Card[2];
					Values[] ValueRange = new Values[1];
					Suits[] SuitRange = new Suits[4];
					SuitRange[0] = Suits.Clubs;
					SuitRange[1] = Suits.Diamonds;
					SuitRange[2] = Suits.Hearts;
					SuitRange[3] = Suits.Spades;

					#region Get the component (Sets / One Pair) of forming a full house hand from player's hand
					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						int ValueOccurance = Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value);

						if(ValueOccurance >= 2)
						{
							ValueRange = new Values[1];
							ValueRange[0] = PlayerHand[CardIndex].Value;
							KeyCombinations.Add(Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange));
						}
					}
					#endregion

					#region Based on the component found in player's hand, determine the missing cards in that hand and check whether the deck has those cards. If so, form those possible hands.
					for(int CombinationIndex = 0; CombinationIndex < KeyCombinations.Count; CombinationIndex++)
					{
						if(KeyCombinations[CombinationIndex].Length == 2)
						{
							if(KeyCombinations[CombinationIndex][0].Value == KeyCombinations[CombinationIndex][1].Value)
							{
								for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
								{
									if(Utility.CountValueOccurance(SampleDeck.Cards,SampleDeck.Cards[CardIndex].Value) >= 3)
									{
										Card[] PossibleHand = new Card[5];
										PossibleHand[0] = new Card(Suits.Spades,SampleDeck.Cards[CardIndex].Value);
										PossibleHand[1] = new Card(Suits.Hearts,SampleDeck.Cards[CardIndex].Value);
										PossibleHand[2] = new Card(Suits.Diamonds,SampleDeck.Cards[CardIndex].Value);
										PossibleHand[3] = KeyCombinations[CombinationIndex][0];
										PossibleHand[4] = KeyCombinations[CombinationIndex][1];

										PossibleHands.AddLast(PossibleHand);
									}
								}	
							}
							else
							{
								if(Utility.CountValueOccurance(SampleDeck.Cards,KeyCombinations[CombinationIndex][0].Value) >= 2 && Utility.CountValueOccurance(SampleDeck.Cards,KeyCombinations[CombinationIndex][1].Value) >= 1)
								{
									ValueRange = new Values[1];
									ValueRange[0] = KeyCombinations[CombinationIndex][0].Value;
									Card[] SetCardsInDeck = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

									ValueRange = new Values[1];
									ValueRange[0] = KeyCombinations[CombinationIndex][1].Value;
									Card[] PairCardsInDeck = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

									Card[] PossibleHand = new Card[5];
									PossibleHand[0] = KeyCombinations[CombinationIndex][0];
									PossibleHand[1] = SetCardsInDeck[0];
									PossibleHand[2] = SetCardsInDeck[1];
									PossibleHand[3] = KeyCombinations[CombinationIndex][1];
									PossibleHand[4] = PairCardsInDeck[0];

									PossibleHands.AddLast(PossibleHand);
								}
								else if(Utility.CountValueOccurance(SampleDeck.Cards,KeyCombinations[CombinationIndex][0].Value) >= 1 && Utility.CountValueOccurance(SampleDeck.Cards,KeyCombinations[CombinationIndex][1].Value) >= 2)
								{
									ValueRange = new Values[1];
									ValueRange[0] = KeyCombinations[CombinationIndex][1].Value;
									Card[] SetCardsInDeck = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

									ValueRange = new Values[1];
									ValueRange[0] = KeyCombinations[CombinationIndex][0].Value;
									Card[] PairCardsInDeck = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

									Card[] PossibleHand = new Card[5];
									PossibleHand[0] = KeyCombinations[CombinationIndex][1];
									PossibleHand[1] = SetCardsInDeck[0];
									PossibleHand[2] = SetCardsInDeck[1];
									PossibleHand[3] = KeyCombinations[CombinationIndex][0];
									PossibleHand[4] = PairCardsInDeck[0];

									PossibleHands.AddLast(PossibleHand);
								}
							}
						}
						else if(KeyCombinations[CombinationIndex].Length == 3)
						{
							for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
							{
								Values CurrentCardValue = SampleDeck.Cards[CardIndex].Value;

								for(int CheckPairIndex = CardIndex + 1; CheckPairIndex < SampleDeck.Cards.Count; CheckPairIndex++)
								{
									if(SampleDeck.Cards[CheckPairIndex].Value == CurrentCardValue)
									{
										Card[] PossibleHand = new Card[5];
										PossibleHand[0] = KeyCombinations[CombinationIndex][0];
										PossibleHand[1] = KeyCombinations[CombinationIndex][1];
										PossibleHand[2] = KeyCombinations[CombinationIndex][2];
										PossibleHand[3] = SampleDeck.Cards[CardIndex];
										PossibleHand[4] = SampleDeck.Cards[CheckPairIndex];

										PossibleHands.AddLast(PossibleHand);
										break;
									}
								}
							}
						}
					}
					#endregion
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
				{
					Values ThreeCardsValue = Values.Two;
					Values TwoCardsValue = Values.Two;

					#region Identify the value of the Three of a Kind and One Pair in the Full House Hand
					for(int CardIndex = 0; CardIndex < PlayerCards.Length; CardIndex++)
					{
						int ValueOccurance = Utility.CountValueOccurance(PlayerCards,PlayerCards[CardIndex].Value);

						if(ValueOccurance < 2)
							continue;

						if(ValueOccurance >= 3 && PlayerCards[CardIndex].Value > ThreeCardsValue)
							ThreeCardsValue = PlayerCards[CardIndex].Value;

						else if(ValueOccurance >= 3 && PlayerCards[CardIndex].Value < ThreeCardsValue && PlayerCards[CardIndex].Value > TwoCardsValue)
							TwoCardsValue = PlayerCards[CardIndex].Value;

						else if(ValueOccurance == 2 && PlayerCards[CardIndex].Value > TwoCardsValue)
							TwoCardsValue = PlayerCards[CardIndex].Value;
					}
					#endregion

					Suits[] SuitRange = new Suits[4];
					SuitRange[0] = Suits.Clubs;
					SuitRange[1] = Suits.Diamonds;
					SuitRange[2] = Suits.Hearts;
					SuitRange[3] = Suits.Spades;
					Values[] ValueRange = new Values[1];
					ValueRange[0] = ThreeCardsValue;

					Card[] SetCards = Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange);

					#region Find any potential pairs in the remaining cards of the deck and form possible hands 
					for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count - 1; CardIndex++)
					{
						if(SampleDeck.Cards[CardIndex].Value > TwoCardsValue)
						{
							for(int SequentialIndex = CardIndex + 1; SequentialIndex < SampleDeck.Cards.Count; SequentialIndex++)
							{
								if(SampleDeck.Cards[SequentialIndex].Value == SampleDeck.Cards[CardIndex].Value)
								{
									Card[] PossibleHand = new Card[5];
									PossibleHand[0] = SetCards[0];
									PossibleHand[1] = SetCards[1];
									PossibleHand[2] = SetCards[2];
									PossibleHand[3] = SampleDeck.Cards[CardIndex];
									PossibleHand[4] = SampleDeck.Cards[SequentialIndex];

									PossibleHands.AddLast(PossibleHand);
								}
							}
						}
					}
					#endregion
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
					continue;
			}

			else if((Hands) HandIndex == Hands.Flush)
			{
				Suits[] SuitRange = new Suits[1];
				Values[] ValueRange = new Values[13];
				Card[] PossibleHand = new Card[5];

				if(PlayerHandType == Hands.Flush)
				{
					for(int CardIndex = PlayerEffectiveHand.Length - 1; CardIndex > 0; CardIndex--)
					{
						#region Calculate a range of values that are superior than the current card in player's effective hand
						ValueRange = new Values[(int) Values.Ace - ((int) PlayerEffectiveHand[CardIndex].Value + 1)];
						for(int ValueIndex = (int) PlayerEffectiveHand[CardIndex].Value + 1; ValueIndex <= (int) Values.Ace; ValueIndex++)
						{
							ValueRange[ValueIndex - ((int) PlayerEffectiveHand[CardIndex].Value + 1)] = (Values) ValueIndex;
						}
						#endregion

						SuitRange = new Suits[1];
						SuitRange[0] = PlayerEffectiveHand[0].Suit;

						#region Generate possible hands based on the range of value cards found in the deck
						Card[] KeyCards = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);
						PossibleHand[0] = PlayerEffectiveHand[0];
						PossibleHand[1] = PlayerEffectiveHand[1];
						PossibleHand[2] = PlayerEffectiveHand[2];
						PossibleHand[3] = PlayerEffectiveHand[3];
						PossibleHand[4] = PlayerEffectiveHand[4];

						for(int KeyIndex = 0; KeyIndex < KeyCards.Length; KeyIndex++)
						{
							PossibleHand[CardIndex] = KeyCards[KeyIndex];
							PossibleHands.AddLast(PossibleHand);
						}
						#endregion
					}	
				}

				for(int ValueIndex = (int) Values.Two; ValueIndex <= (int) Values.Ace; ValueIndex++)
				{
					ValueRange[ValueIndex] = (Values) ValueIndex;
				}

				if(CurrentTurnPhase == TurnPhase.Betting)
				{
					for(int SuitIndex = (int) Suits.Clubs; SuitIndex <= (int) Suits.Spades; SuitIndex++)
					{
						int AmtOfSuitCardsAvaliable = Utility.CountSuitOccurance(PlayerHand,(Suits) SuitIndex);

						if(AmtOfSuitCardsAvaliable < 2)
							continue;

						SuitRange = new Suits[1];
						SuitRange[0] = (Suits) SuitIndex;

						Card[] PlayerComponent = Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange);
						Card[] RemainingSuitedCardInDeck = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

						#region Depend on how many specific suit cards in player's hand and how many corresponding suit card in the deck, form various possible hands
						if(PlayerComponent.Length == 2)
						{
							for(int FirstCardIndex = 0; FirstCardIndex < RemainingSuitedCardInDeck.Length - 2; FirstCardIndex++)
							{
								for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < RemainingSuitedCardInDeck.Length - 1; SecondCardIndex++)
								{
									for(int ThirdCardIndex = SecondCardIndex + 1; ThirdCardIndex < RemainingSuitedCardInDeck.Length; ThirdCardIndex++)
									{
										PossibleHand = new Card[5];
										PossibleHand[0] = PlayerComponent[0];
										PossibleHand[1] = PlayerComponent[1];
										PossibleHand[2] = RemainingSuitedCardInDeck[FirstCardIndex];
										PossibleHand[3] = RemainingSuitedCardInDeck[SecondCardIndex];
										PossibleHand[4] = RemainingSuitedCardInDeck[ThirdCardIndex];

										PossibleHands.AddLast(PossibleHand);
									}
								}
							}
						}
						else if(PlayerComponent.Length == 3)
						{
							for(int FirstCardIndex = 0; FirstCardIndex < RemainingSuitedCardInDeck.Length - 2; FirstCardIndex++)
							{
								for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < RemainingSuitedCardInDeck.Length - 1; SecondCardIndex++)
								{
									PossibleHand = new Card[5];
									PossibleHand[0] = PlayerComponent[0];
									PossibleHand[1] = PlayerComponent[1];
									PossibleHand[2] = PlayerComponent[2];
									PossibleHand[3] = RemainingSuitedCardInDeck[FirstCardIndex];
									PossibleHand[4] = RemainingSuitedCardInDeck[SecondCardIndex];

									PossibleHands.AddLast(PossibleHand);
								}
							}
						}
						else if(PlayerComponent.Length == 4)
						{
							for(int CardIndex = 0; CardIndex < RemainingSuitedCardInDeck.Length - 2; CardIndex++)
							{
								PossibleHand = new Card[5];
								PossibleHand[0] = PlayerComponent[0];
								PossibleHand[1] = PlayerComponent[1];
								PossibleHand[2] = PlayerComponent[2];
								PossibleHand[3] = PlayerComponent[3];
								PossibleHand[4] = RemainingSuitedCardInDeck[CardIndex];

								PossibleHands.AddLast(PossibleHand);
							}
						}
						#endregion
					}
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
				{
					for(int SuitIndex = (int) Suits.Clubs; SuitIndex <= (int) Suits.Spades; SuitIndex++)
					{
						int AmtOfSuitCardsAvaliable = Utility.CountSuitOccurance(PlayerHand,(Suits) SuitIndex);

						if(AmtOfSuitCardsAvaliable < 3)
							continue;

						SuitRange = new Suits[1];
						SuitRange[0] = (Suits) SuitIndex;

						Card[] PlayerComponent = Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange);
						Card[] RemainingSuitedCardInDeck = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

						#region Depend on how many specific suit cards in player's hand and how many corresponding suit card in the deck, form various possible hands
						if(PlayerComponent.Length == 3)
						{
							for(int FirstCardIndex = 0; FirstCardIndex < RemainingSuitedCardInDeck.Length - 2; FirstCardIndex++)
							{
								for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < RemainingSuitedCardInDeck.Length - 1; SecondCardIndex++)
								{
									PossibleHand = new Card[5];
									PossibleHand[0] = PlayerComponent[0];
									PossibleHand[1] = PlayerComponent[1];
									PossibleHand[2] = PlayerComponent[2];
									PossibleHand[3] = RemainingSuitedCardInDeck[FirstCardIndex];
									PossibleHand[4] = RemainingSuitedCardInDeck[SecondCardIndex];

									PossibleHands.AddLast(PossibleHand);
								}
							}
						}
						else if(PlayerComponent.Length == 4)
						{
							for(int CardIndex = 0; CardIndex < RemainingSuitedCardInDeck.Length - 2; CardIndex++)
							{
								PossibleHand = new Card[5];
								PossibleHand[0] = PlayerComponent[0];
								PossibleHand[1] = PlayerComponent[1];
								PossibleHand[2] = PlayerComponent[2];
								PossibleHand[3] = PlayerComponent[3];
								PossibleHand[4] = RemainingSuitedCardInDeck[CardIndex];

								PossibleHands.AddLast(PossibleHand);
							}
						}
						#endregion
					}
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
				{
					for(int SuitIndex = (int) Suits.Clubs; SuitIndex <= (int) Suits.Spades; SuitIndex++)
					{
						int AmtOfSuitCardsAvaliable = Utility.CountSuitOccurance(PlayerHand,(Suits) SuitIndex);

						if(AmtOfSuitCardsAvaliable < 3)
							continue;

						SuitRange = new Suits[1];
						SuitRange[0] = (Suits) SuitIndex;

						Card[] PlayerComponent = Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange);
						Card[] RemainingSuitedCardInDeck = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

						#region Depend on how many specific suit cards in player's hand and how many corresponding suit card in the deck, form various possible hands
						if(PlayerComponent.Length == 4)
						{
							for(int CardIndex = 0; CardIndex < RemainingSuitedCardInDeck.Length - 2; CardIndex++)
							{
								PossibleHand = new Card[5];
								PossibleHand[0] = PlayerComponent[0];
								PossibleHand[1] = PlayerComponent[1];
								PossibleHand[2] = PlayerComponent[2];
								PossibleHand[3] = PlayerComponent[3];
								PossibleHand[4] = RemainingSuitedCardInDeck[CardIndex];

								PossibleHands.AddLast(PossibleHand);
							}
						}
						#endregion
					}
				}
			}

			else if((Hands) HandIndex == Hands.Straight)
			{
				if(PlayerHandType == Hands.Straight)
				{
					#region Identify the Highest and Lowest card of the straight aspect of the hand
					Card HighestCard = new Card(PlayerHand[0].Suit,Values.Two);
					Card LowestCard = new Card(PlayerHand[0].Suit,Values.Ace);

					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						if(PlayerHand[CardIndex].Value > HighestCard.Value)
							HighestCard = PlayerHand[CardIndex];

						if(PlayerHand[CardIndex].Value < LowestCard.Value)
							LowestCard = PlayerHand[CardIndex];
					}
					#endregion

					List<Card> CardToSearch = new List<Card>();

					#region Identify the cards that are needed to improve the current straight flush hand
					if(CurrentTurnPhase == TurnPhase.Betting)
					{
						if(HighestCard.Value == Values.Ace)
							continue;
						else if(HighestCard.Value == Values.King)
						{
							CardToSearch.Add(new Card(Suits.Clubs,Values.Ace));
							CardToSearch.Add(new Card(Suits.Diamonds,Values.Ace));
							CardToSearch.Add(new Card(Suits.Hearts,Values.Ace));
							CardToSearch.Add(new Card(Suits.Spades,Values.Ace));
						}
						else if(HighestCard.Value == Values.Queen)
						{
							CardToSearch.Add(new Card(Suits.Clubs,Values.King));
							CardToSearch.Add(new Card(Suits.Diamonds,Values.King));
							CardToSearch.Add(new Card(Suits.Hearts,Values.King));
							CardToSearch.Add(new Card(Suits.Spades,Values.King));
													  
							CardToSearch.Add(new Card(Suits.Clubs,Values.Ace));
							CardToSearch.Add(new Card(Suits.Diamonds,Values.Ace));
							CardToSearch.Add(new Card(Suits.Hearts,Values.Ace));
							CardToSearch.Add(new Card(Suits.Spades,Values.Ace));
						}
						else
						{
							CardToSearch.Add(new Card(Suits.Clubs,(Values) (HighestCard.Value + 1)));
							CardToSearch.Add(new Card(Suits.Diamonds,(Values) (HighestCard.Value + 1)));
							CardToSearch.Add(new Card(Suits.Hearts,(Values) (HighestCard.Value + 1)));
							CardToSearch.Add(new Card(Suits.Spades,(Values) (HighestCard.Value + 1)));

							CardToSearch.Add(new Card(Suits.Clubs,(Values) (HighestCard.Value + 2)));
							CardToSearch.Add(new Card(Suits.Diamonds,(Values) (HighestCard.Value + 2)));
							CardToSearch.Add(new Card(Suits.Hearts,(Values) (HighestCard.Value + 2)));
							CardToSearch.Add(new Card(Suits.Spades,(Values) (HighestCard.Value + 2)));

							CardToSearch.Add(new Card(Suits.Clubs,(Values) (HighestCard.Value + 3)));
							CardToSearch.Add(new Card(Suits.Diamonds,(Values) (HighestCard.Value + 3)));
							CardToSearch.Add(new Card(Suits.Hearts,(Values) (HighestCard.Value + 3)));
							CardToSearch.Add(new Card(Suits.Spades,(Values) (HighestCard.Value + 3)));
						}
					}
					else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
					{
						if(HighestCard.Value == Values.Ace)
							continue;
						else if(HighestCard.Value == Values.King)
						{
							CardToSearch.Add(new Card(Suits.Clubs, Values.Ace));
							CardToSearch.Add(new Card(Suits.Diamonds, Values.Ace));
							CardToSearch.Add(new Card(Suits.Hearts, Values.Ace));
							CardToSearch.Add(new Card(Suits.Spades, Values.Ace));
						}
						else
						{
							CardToSearch.Add(new Card(Suits.Clubs, HighestCard.Value + 1));
							CardToSearch.Add(new Card(Suits.Diamonds, HighestCard.Value + 1));
							CardToSearch.Add(new Card(Suits.Hearts, HighestCard.Value + 1));
							CardToSearch.Add(new Card(Suits.Spades, HighestCard.Value + 1));

							CardToSearch.Add(new Card(Suits.Clubs, HighestCard.Value + 2));
							CardToSearch.Add(new Card(Suits.Diamonds, HighestCard.Value + 2));
							CardToSearch.Add(new Card(Suits.Hearts, HighestCard.Value + 2));
							CardToSearch.Add(new Card(Suits.Spades, HighestCard.Value + 2));
						}
					}
					else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
					{
						if(HighestCard.Value == Values.Ace)
							continue;
						else
						{
							CardToSearch.Add(new Card(Suits.Clubs, HighestCard.Value + 1));
							CardToSearch.Add(new Card(Suits.Diamonds, HighestCard.Value + 1));
							CardToSearch.Add(new Card(Suits.Hearts, HighestCard.Value + 1));
							CardToSearch.Add(new Card(Suits.Spades, HighestCard.Value + 1));
						}
					}
					#endregion

					List<Card> CardFound = new List<Card>();

					#region Go through the remaining cards in the deck and determine whether it has the cards needed to improve the hand
					for(int SearchIndex = 0; SearchIndex < CardToSearch.Count; SearchIndex++)
					{
						for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
						{
							if(Utility.IsTwoCardsIdentical(CardToSearch[SearchIndex],SampleDeck.Cards[CardIndex]))
							{
								CardFound.Add(CardToSearch[SearchIndex]);
								break;
							}
						}
					}
					#endregion

					#region Check whether all the needed cards are in the deck. if they are, "Craft" the necessary hand and add it to the possible hand
					if(CardFound.Count == CardToSearch.Count)
					{
						Card[] PossibleHand = new Card[PlayerEffectiveHand.Length];

						if(CardFound.Count == 1)
						{
							PossibleHand[0] = PlayerEffectiveHand[0];
							PossibleHand[1] = PlayerEffectiveHand[1];
							PossibleHand[2] = PlayerEffectiveHand[2];
							PossibleHand[3] = PlayerEffectiveHand[3];
							PossibleHand[4] = CardToSearch[0];

							PossibleHands.AddLast(PossibleHand);
						}
						else if(CardFound.Count == 2)
						{
							PossibleHand[0] = PlayerEffectiveHand[0];
							PossibleHand[1] = PlayerEffectiveHand[1];
							PossibleHand[2] = PlayerEffectiveHand[2];
							PossibleHand[3] = CardToSearch[0];
							PossibleHand[4] = CardToSearch[1];

							PossibleHands.AddLast(PossibleHand);
						}
						else if(CardFound.Count == 3)
						{
							PossibleHand[0] = PlayerEffectiveHand[0];
							PossibleHand[1] = PlayerEffectiveHand[1];
							PossibleHand[2] = CardToSearch[0];
							PossibleHand[3] = CardToSearch[1];
							PossibleHand[4] = CardToSearch[2];

							PossibleHands.AddLast(PossibleHand);
						}
					}
					#endregion
				}

				List<Card> PossibleCombination = new List<Card>();
				List<Card[]> CombinationList = new List<Card[]>();

				#region Based a base card, check for rest of the cards in player's hand whether any other cards in hand is within value range ( 4 values ). If so, form the combination, add it to possible combination
				for(int FirstCardIndex = 0; FirstCardIndex < SortedEffectiveHand.Length - 1; FirstCardIndex++)
				{
					for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < SortedEffectiveHand.Length; SecondCardIndex++)
					{
						if(((int) SortedEffectiveHand[SecondCardIndex].Value - (int) SortedEffectiveHand[FirstCardIndex].Value) <= 4)
						{
							PossibleCombination.Clear();
							PossibleCombination.Add(SortedEffectiveHand[FirstCardIndex]);
							PossibleCombination.Add(SortedEffectiveHand[SecondCardIndex]);

							for(int SequentialIndex = SecondCardIndex + 1; SequentialIndex < SortedEffectiveHand.Length; SequentialIndex++)
							{
								if(((int) SortedEffectiveHand[SequentialIndex].Value - (int) SortedEffectiveHand[FirstCardIndex].Value) <= 4)
								{
									PossibleCombination.Add(SortedEffectiveHand[SequentialIndex]);
									continue;
								}

								break;
							}

							CombinationList.Add(PossibleCombination.ToArray());
						}
					}	
				}
				#endregion

				#region Go through all combinations found, identify the missing cards then form the possible hands
				for(int CombinationIndex = 0; CombinationIndex < CombinationList.Count; CombinationIndex++)
				{
					int AmtOfMissingCardsInCombo = 5 - CombinationList.Count;
					int MaximumMissingCard = 5;

					if(_Player.GManager.Phase == TurnPhase.Betting)
						MaximumMissingCard = 3;
					else if(_Player.GManager.Phase == TurnPhase.Auctioning && _Player.GManager.AuctionPhase == AuctionPhase.First)
						MaximumMissingCard = 2;
					else if(_Player.GManager.Phase == TurnPhase.Auctioning && (_Player.GManager.AuctionPhase == AuctionPhase.Second || _Player.GManager.AuctionPhase == AuctionPhase.Third))
						MaximumMissingCard = 1;

					if(AmtOfMissingCardsInCombo > MaximumMissingCard)
						continue;

					Card[] PossibleHand = new Card[5];
					
					if(AmtOfMissingCardsInCombo == 3)
					{
						Card RequiredCard_First = new Card(CombinationList[CombinationIndex][1].Suit,(Values)(CombinationList[CombinationIndex][1].Value + 1));
						Card RequiredCard_Second = new Card(RequiredCard_First.Suit,(Values)(RequiredCard_First.Value + 1));
						Card RequiredCard_Third = new Card(RequiredCard_Second.Suit,(Values)(RequiredCard_Second.Value + 1));

						if(Utility.IsCardInDeck(SampleDeck,RequiredCard_First) && Utility.IsCardInDeck(SampleDeck,RequiredCard_Second) && Utility.IsCardInDeck(SampleDeck,RequiredCard_Third))
						{
							PossibleHand[0] = CombinationList[CombinationIndex][0];
							PossibleHand[1] = CombinationList[CombinationIndex][1];
							PossibleHand[2] = RequiredCard_First;
							PossibleHand[3] = RequiredCard_Second;
							PossibleHand[4] = RequiredCard_Third;
							
							PossibleHands.AddLast(PossibleHand);
						}
					}
					else if(AmtOfMissingCardsInCombo == 2)
					{
						Card RequiredCard_First = new Card(CombinationList[CombinationIndex][2].Suit,(Values)(CombinationList[CombinationIndex][2].Value + 1));
						Card RequiredCard_Second = new Card(RequiredCard_First.Suit,(Values)(RequiredCard_First.Value + 1));

						if(Utility.IsCardInDeck(SampleDeck,RequiredCard_First) && Utility.IsCardInDeck(SampleDeck,RequiredCard_Second))
						{
							PossibleHand[0] = CombinationList[CombinationIndex][0];
							PossibleHand[1] = CombinationList[CombinationIndex][1];
							PossibleHand[2] = CombinationList[CombinationIndex][2];
							PossibleHand[3] = RequiredCard_First;
							PossibleHand[4] = RequiredCard_Second;
							
							PossibleHands.AddLast(PossibleHand);
						}
					}
					else if(AmtOfMissingCardsInCombo == 1)
					{
						Card RequiredCard = new Card(CombinationList[CombinationIndex][3].Suit,(Values)(CombinationList[CombinationIndex][3].Value + 1));

						if(Utility.IsCardInDeck(SampleDeck,RequiredCard))
						{
							PossibleHand[0] = CombinationList[CombinationIndex][0];
							PossibleHand[1] = CombinationList[CombinationIndex][1];
							PossibleHand[2] = CombinationList[CombinationIndex][2];
							PossibleHand[3] = CombinationList[CombinationIndex][3];
							PossibleHand[4] = RequiredCard;
							
							PossibleHands.AddLast(PossibleHand);
						}
					}
					else if(AmtOfMissingCardsInCombo == 0)
					{
						PossibleHand[0] = CombinationList[CombinationIndex][0];
						PossibleHand[1] = CombinationList[CombinationIndex][1];
						PossibleHand[2] = CombinationList[CombinationIndex][2];
						PossibleHand[3] = CombinationList[CombinationIndex][3];
						PossibleHand[4] = CombinationList[CombinationIndex][4];

						PossibleHands.AddLast(PossibleHand);
					}
				}
				#endregion
			}

			else if((Hands) HandIndex == Hands.ThreeOfAKind)
			{
				Suits[] SuitRange = new Suits[4];
				Values[] ValueRange = new Values[1];
				
				SuitRange[0] = Suits.Clubs;
				SuitRange[1] = Suits.Diamonds;
				SuitRange[2] = Suits.Hearts;
				SuitRange[3] = Suits.Spades;
				
				if(PlayerHandType == Hands.ThreeOfAKind)
				{
					if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third)) 
						continue;

					Values SetValue = Values.NULL;
					List<Card> MiscCards = new List<Card>();

					#region Identify the two cards in player's hand that is not the three of the kind
					for(int CardIndex = 0; CardIndex < PlayerEffectiveHand.Length; CardIndex++)
					{
						if(Utility.CountValueOccurance(PlayerEffectiveHand,PlayerEffectiveHand[CardIndex].Value) < 3)
							MiscCards.Add(PlayerEffectiveHand[CardIndex]);

						else
							SetValue = PlayerEffectiveHand[CardIndex].Value;
					}
					#endregion

					bool IsMiscCards = false;
					ValueRange[0] = SetValue;

					for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
					{
						IsMiscCards = false;

						for(int MiscCardIndex = 0; MiscCardIndex < MiscCards.Count; MiscCardIndex++)
						{
							if(Utility.IsTwoCardsIdentical(SampleDeck.Cards[CardIndex],MiscCards[MiscCardIndex]))
							{
								IsMiscCards = true;
								break;
							}
						}	

						if(CurrentTurnPhase == TurnPhase.Betting && !IsMiscCards && Utility.CountValueOccurance(SampleDeck.Cards,SampleDeck.Cards[CardIndex].Value) >= 3)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = PlayerHand[0];
							PossibleHand[1] = PlayerHand[1];
							PossibleHand[2] = PlayerHand[2];
							PossibleHand[3] = PlayerHand[3];
							PossibleHand[4] = PlayerHand[4];

							for(int HandCardIndex = 0; HandCardIndex < PossibleHand.Length; HandCardIndex++)
							{
								if(PossibleHand[HandCardIndex].Value == SetValue)
									PossibleHand[HandCardIndex].Value = SampleDeck.Cards[CardIndex].Value;
							}

							PossibleHands.AddLast(PossibleHand);
						}
						else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First && IsMiscCards 
						&& SampleDeck.Cards[CardIndex].Value > SetValue && Utility.CountValueOccurance(SampleDeck.Cards,SampleDeck.Cards[CardIndex].Value) >= 2)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = PlayerHand[0];
							PossibleHand[1] = PlayerHand[1];
							PossibleHand[2] = PlayerHand[2];
							PossibleHand[3] = PlayerHand[3];
							PossibleHand[4] = PlayerHand[4];

							for(int HandCardIndex = 0; HandCardIndex < PossibleHand.Length; HandCardIndex++)
							{
								if(PossibleHand[HandCardIndex].Value == SetValue)
									PossibleHand[HandCardIndex].Value = SampleDeck.Cards[CardIndex].Value;
							}

							PossibleHands.AddLast(PossibleHand);
						}
					}
				}

				if(CurrentTurnPhase == TurnPhase.Betting)
				{
					List<Card[]> KeyCombinations = new List<Card[]>();

					#region Check for cards in hand that can be made to Three Of a Kind then store the needed cards into an array which is then stored into a list
					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						if(Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value) == 1)
						{
							ValueRange = new Values[1];
							ValueRange[0] = PlayerHand[CardIndex].Value;

							Card[] KeyCards = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

							if(KeyCards.Length > 0)
								continue;

							Card[] PossibleCombination = new Card[2];
							PossibleCombination[0] = KeyCards[0];
							PossibleCombination[1] = KeyCards[1];

							KeyCombinations.Add(PossibleCombination);
						}
						else if(Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value) == 2)
						{
							ValueRange = new Values[1];
							ValueRange[0] = PlayerHand[CardIndex].Value;

							Card[] KeyCards = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

							if(KeyCards.Length > 0)
								continue;

							Card[] PossibleCombination = new Card[1];
							PossibleCombination[0] = KeyCards[0];

							KeyCombinations.Add(PossibleCombination);
						}
					}
					#endregion

					#region Go through the deck to see if there is any set within the deck, hence making it a possibility for player to draw them and gaining a three of a kind hand
					for(int FirstCardIndex = 0; FirstCardIndex < SampleDeck.Cards.Count - 2; FirstCardIndex++)
					{
						for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < SampleDeck.Cards.Count - 1; SecondCardIndex++)
						{
							for(int ThirdCardIndex = SecondCardIndex + 1; ThirdCardIndex < SampleDeck.Cards.Count; ThirdCardIndex++)
							{
								if(SampleDeck.Cards[FirstCardIndex].Value == SampleDeck.Cards[SecondCardIndex].Value &&
								SampleDeck.Cards[SecondCardIndex].Value == SampleDeck.Cards[ThirdCardIndex].Value &&
								SampleDeck.Cards[FirstCardIndex].Value == SampleDeck.Cards[ThirdCardIndex].Value)
								{
									Card[] PossibleCombination = new Card[3];
									PossibleCombination[0] = SampleDeck.Cards[FirstCardIndex];
									PossibleCombination[1] = SampleDeck.Cards[SecondCardIndex];
									PossibleCombination[2] = SampleDeck.Cards[ThirdCardIndex];

									KeyCombinations.Add(PossibleCombination);
								}
							}	
						}
					}
					#endregion

					Card[] PossibleHand = new Card[5];

					#region Go through all the possible combination of cards to made a Three Of A Kind and store those possible hands
					for(int CombinationIndex = 0; CombinationIndex < KeyCombinations.Count; CombinationIndex++)
					{
						if(KeyCombinations[CombinationIndex].Length == 1)
						{
							for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
							{
								if(PlayerHand[CardIndex].Value != PossibleHand[0].Value)
								{
									PossibleHand = new Card[5];
									PossibleHand[0] = PlayerHand[0];
									PossibleHand[1] = PlayerHand[1];
									PossibleHand[2] = PlayerHand[2];
									PossibleHand[3] = PlayerHand[3];
									PossibleHand[4] = PlayerHand[4];
									
									PossibleHand[CardIndex] = KeyCombinations[CombinationIndex][0];
									PossibleHands.AddLast(PossibleHand);
								}
							}
						}
						else if(KeyCombinations[CombinationIndex].Length == 2)
						{
							for(int FirstCardIndex = 0; FirstCardIndex < PlayerHand.Length - 1; FirstCardIndex++)
							{
								for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PlayerHand.Length; SecondCardIndex++)
								{
									if(PlayerHand[FirstCardIndex].Value != PossibleHand[0].Value && PlayerHand[SecondCardIndex].Value != PossibleHand[0].Value)
									{
										PossibleHand = new Card[5];
										PossibleHand[0] = PlayerHand[0];
										PossibleHand[1] = PlayerHand[1];
										PossibleHand[2] = PlayerHand[2];
										PossibleHand[3] = PlayerHand[3];
										PossibleHand[4] = PlayerHand[4];
										
										PossibleHand[FirstCardIndex] = KeyCombinations[CombinationIndex][0];
										PossibleHand[SecondCardIndex] = KeyCombinations[CombinationIndex][1];
										PossibleHands.AddLast(PossibleHand);
									}
								}
							}
						}
						else if(KeyCombinations[CombinationIndex].Length == 3)
						{
							for(int FirstCardIndex = 0; FirstCardIndex < PlayerHand.Length - 2; FirstCardIndex++)
							{
								for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PlayerHand.Length - 1; SecondCardIndex++)
								{
									for(int ThirdCardIndex = SecondCardIndex + 1; ThirdCardIndex < PlayerHand.Length; ThirdCardIndex++)
									{
										if(PlayerHand[FirstCardIndex].Value != PossibleHand[0].Value && PlayerHand[SecondCardIndex].Value != PossibleHand[0].Value && PlayerHand[ThirdCardIndex].Value != PossibleHand[0].Value)
										{
											PossibleHand = new Card[5];
											PossibleHand[0] = PlayerHand[0];
											PossibleHand[1] = PlayerHand[1];
											PossibleHand[2] = PlayerHand[2];
											PossibleHand[3] = PlayerHand[3];
											PossibleHand[4] = PlayerHand[4];
											
											PossibleHand[FirstCardIndex] = KeyCombinations[CombinationIndex][0];
											PossibleHand[SecondCardIndex] = KeyCombinations[CombinationIndex][1];
											PossibleHand[ThirdCardIndex] = KeyCombinations[CombinationIndex][1];
											PossibleHands.AddLast(PossibleHand);
										}
									}
								}
							}
						}
					}
					#endregion
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
				{
					List<Card[]> KeyCombinations = new List<Card[]>();

					#region Check for cards in hand that can be made to Three Of a Kind then store the needed cards into an array which is then stored into a list
					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						if(Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value) == 1)
						{
							ValueRange = new Values[1];
							ValueRange[0] = PlayerHand[CardIndex].Value;

							Card[] KeyCards = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

							if(KeyCards.Length > 0)
								continue;

							Card[] PossibleCombination = new Card[2];
							PossibleCombination[0] = KeyCards[0];
							PossibleCombination[1] = KeyCards[1];

							KeyCombinations.Add(PossibleCombination);
						}
						else if(Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value) == 2)
						{
							ValueRange = new Values[1];
							ValueRange[0] = PlayerHand[CardIndex].Value;

							Card[] KeyCards = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

							if(KeyCards.Length > 0)
								continue;

							Card[] PossibleCombination = new Card[1];
							PossibleCombination[0] = KeyCards[0];

							KeyCombinations.Add(PossibleCombination);
						}
					}
					#endregion

					Card[] PossibleHand = new Card[5];

					#region Go through all the possible combination of cards to made a Three Of A Kind and store those possible hands
					for(int CombinationIndex = 0; CombinationIndex < KeyCombinations.Count; CombinationIndex++)
					{
						if(KeyCombinations[CombinationIndex].Length == 1)
						{
							for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
							{
								if(PlayerHand[CardIndex].Value != PossibleHand[0].Value)
								{
									PossibleHand = new Card[5];
									PossibleHand[0] = PlayerHand[0];
									PossibleHand[1] = PlayerHand[1];
									PossibleHand[2] = PlayerHand[2];
									PossibleHand[3] = PlayerHand[3];
									PossibleHand[4] = PlayerHand[4];
									
									PossibleHand[CardIndex] = KeyCombinations[CombinationIndex][0];
									PossibleHands.AddLast(PossibleHand);
								}
							}
						}
						else if(KeyCombinations[CombinationIndex].Length == 2)
						{
							for(int FirstCardIndex = 0; FirstCardIndex < PlayerHand.Length - 1; FirstCardIndex++)
							{
								for(int SecondCardIndex = FirstCardIndex + 1; SecondCardIndex < PlayerHand.Length; SecondCardIndex++)
								{
									if(PlayerHand[FirstCardIndex].Value != PossibleHand[0].Value && PlayerHand[SecondCardIndex].Value != PossibleHand[0].Value)
									{
										PossibleHand = new Card[5];
										PossibleHand[0] = PlayerHand[0];
										PossibleHand[1] = PlayerHand[1];
										PossibleHand[2] = PlayerHand[2];
										PossibleHand[3] = PlayerHand[3];
										PossibleHand[4] = PlayerHand[4];
										
										PossibleHand[FirstCardIndex] = KeyCombinations[CombinationIndex][0];
										PossibleHand[SecondCardIndex] = KeyCombinations[CombinationIndex][1];
										PossibleHands.AddLast(PossibleHand);
									}
								}
							}
						}
					}
					#endregion
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
				{
					List<Card[]> KeyCombinations = new List<Card[]>();

					#region Check for cards in hand that can be made to Three Of a Kind then store the needed cards into an array which is then stored into a list
					for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
					{
						if(Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value) == 2)
						{
							ValueRange = new Values[1];
							ValueRange[0] = PlayerHand[CardIndex].Value;

							Card[] KeyCards = Utility.CaptureCardTypeFromDeck(SampleDeck,SuitRange,ValueRange);

							if(KeyCards.Length > 0)
								continue;

							Card[] PossibleCombination = new Card[1];
							PossibleCombination[0] = KeyCards[0];

							KeyCombinations.Add(PossibleCombination);
						}
					}
					#endregion

					Card[] PossibleHand = new Card[5];

					#region Go through all the possible combination of cards to made a Three Of A Kind and store those possible hands
					for(int CombinationIndex = 0; CombinationIndex < KeyCombinations.Count; CombinationIndex++)
					{
						if(KeyCombinations[CombinationIndex].Length == 1)
						{
							for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
							{
								if(PlayerHand[CardIndex].Value != PossibleHand[0].Value)
								{
									PossibleHand = new Card[5];
									PossibleHand[0] = PlayerHand[0];
									PossibleHand[1] = PlayerHand[1];
									PossibleHand[2] = PlayerHand[2];
									PossibleHand[3] = PlayerHand[3];
									PossibleHand[4] = PlayerHand[4];

									PossibleHand[CardIndex] = KeyCombinations[CombinationIndex][0];
									PossibleHands.AddLast(PossibleHand);
								}
							}
						}
					}
					#endregion
				}
			}

			else if((Hands) HandIndex == Hands.TwoPair)
			{
				if(PlayerHandType == Hands.TwoPair)
				{
					#region Identify the values of both pair in player's hand
					Suits Suit_First = Suits.NULL;
					Suits Suit_Second = Suits.NULL;
					Values PairValue_First = Values.NULL;
					Values PairValue_Second = Values.NULL;

					Suits[] SuitRange = new Suits[4];
					SuitRange[0] = Suits.Clubs;
					SuitRange[1] = Suits.Diamonds;
					SuitRange[2] = Suits.Hearts;
					SuitRange[3] = Suits.Spades;

					Values[] ValueRange = new Values[1];

					for(int CardIndex = 0; CardIndex < PlayerEffectiveHand.Length; CardIndex++)
					{
						if(PairValue_First == Values.NULL && PairValue_Second == Values.NULL && Utility.CountValueOccurance(PlayerEffectiveHand,PlayerEffectiveHand[CardIndex].Value) >= 2)
						{
							PairValue_First = PlayerEffectiveHand[CardIndex].Value;

							ValueRange = new Values[1];
							ValueRange[0] = PlayerEffectiveHand[CardIndex].Value;

							Card[] PairCards = Utility.CaptureCardTypeFromPlayer(_Player,SuitRange,ValueRange);
							Suit_First = PairCards[0].Suit;
							Suit_Second = PairCards[1].Suit;
						}
						else if(PairValue_First != Values.NULL && PairValue_Second == Values.NULL && Utility.CountValueOccurance(PlayerEffectiveHand,PlayerEffectiveHand[CardIndex].Value) >= 2)
						{
							PairValue_Second = PlayerEffectiveHand[CardIndex].Value;
							break;
						}
					}

					if(PairValue_First < PairValue_Second)
					{
						Values Temp = PairValue_First;
						PairValue_First = PairValue_Second;
						PairValue_Second = Temp;
					}
					#endregion

					Card KickerCard = new Card(Suits.NULL,Values.NULL);

					#region Find the kicker card of the current hand
					for(int CardIndex = 0; CardIndex < PlayerEffectiveHand.Length; CardIndex++)
					{
						if(PlayerEffectiveHand[CardIndex].Value != PairValue_First && PlayerEffectiveHand[CardIndex].Value != PairValue_Second)
						{
							KickerCard.Suit = PlayerEffectiveHand[CardIndex].Suit;
							KickerCard.Value = PlayerEffectiveHand[CardIndex].Value;
							break;
						}
					}
					#endregion

					List<Card[]> PairToReplaceFirst = new List<Card[]>();
					List<Card[]> PairToReplaceSecond = new List<Card[]>();

					Card[] FirstReferencePair = new Card[2];
					FirstReferencePair[0] = new Card(Suit_First,PairValue_First);
					FirstReferencePair[1] = new Card(Suit_Second,PairValue_First);

					PairToReplaceFirst.Add(FirstReferencePair);

					#region Find all superior pairs than the player's current pairs from player's cards and the deck, store them in a list
					for(int CardIndex = 0; CardIndex < PlayerCards.Length; CardIndex++)
					{
						if(PlayerCards[CardIndex].Value != PairValue_First && PlayerCards[CardIndex].Value != PairValue_Second)
						{
							for(int DeckCardIndex = 0; DeckCardIndex < SampleDeck.Cards.Count; DeckCardIndex++)
							{
								if(PlayerCards[CardIndex].Value == SampleDeck.Cards[DeckCardIndex].Value)
								{
									if(PlayerCards[CardIndex].Value > PairValue_First)
									{
										Card[] NewPair = new Card[2];
										NewPair[0] = PlayerCards[CardIndex];
										NewPair[1] = SampleDeck.Cards[DeckCardIndex];

										PairToReplaceFirst.Add(NewPair);
									}
									else if(PlayerCards[CardIndex].Value > PairValue_Second && PlayerCards[CardIndex].Value < PairValue_First)
									{
										Card[] NewPair = new Card[2];
										NewPair[0] = PlayerCards[CardIndex];
										NewPair[1] = SampleDeck.Cards[DeckCardIndex];

										PairToReplaceSecond.Add(NewPair);
									}
								}
							}
						}
					}
					#endregion

					List<Card> PossibleKickerCards = new List<Card>();

					#region Find all possible kicker cards and store them in a list
					for(int CardIndex = 0; CardIndex < PlayerCards.Length; CardIndex++)
					{
						if(PlayerCards[CardIndex].Value > KickerCard.Value && PlayerCards[CardIndex].Value != PairValue_First && PlayerCards[CardIndex].Value != PairValue_Second)
							PossibleKickerCards.Add(PlayerCards[CardIndex]);
					}

					for(int DeckCardIndex = 0; DeckCardIndex < SampleDeck.Cards.Count; DeckCardIndex++)
					{
						if(SampleDeck.Cards[DeckCardIndex].Value > KickerCard.Value)
							PossibleKickerCards.Add(SampleDeck.Cards[DeckCardIndex]);
					}
					#endregion

					#region Using possible pairs and range of kicker cards, form possible hands
					for(int FirstPairIndex = 0; FirstPairIndex < PairToReplaceFirst.Count; FirstPairIndex++)
					{
						for(int SecondPairIndex = 0; SecondPairIndex < PairToReplaceSecond.Count; SecondPairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = PairToReplaceFirst[FirstPairIndex][0];
							PossibleHand[1] = PairToReplaceFirst[FirstPairIndex][1];
							PossibleHand[2] = PairToReplaceFirst[SecondPairIndex][0];
							PossibleHand[3] = PairToReplaceFirst[SecondPairIndex][1];

							for(int KickerIndex = 0; KickerIndex < PossibleKickerCards.Count; KickerIndex++)
							{
								PossibleHand[4] = PossibleKickerCards[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}
					}
					#endregion
				}

				#region Determine the rest of the cards at hand that is not a pair
				List<Card> PairCards = new List<Card>(2);
				List<Card> MiscCards = new List<Card>(3);

				for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
				{
					if(Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value) >= 2)
						PairCards.Add(PlayerHand[CardIndex]);

					else
						MiscCards.Add(PlayerHand[CardIndex]);
				}
				#endregion

				foreach(KeyValuePair<Values,bool> Entry in ValueCheck)
				{
					ValueCheck[Entry.Key] = false;
				}

				List<Card[]> KeyPairsInDeck = new List<Card[]>();
				List<Card[]> KeyPairsInHandWithDeck = new List<Card[]>();
				List<Card> KeyKickers = new List<Card>();

				#region Determine possible pairs within the deck and store them in a list
				for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
				{
					if(!ValueCheck[SampleDeck.Cards[CardIndex].Value] && Utility.CountValueOccurance(SampleDeck.Cards, SampleDeck.Cards[CardIndex].Value) >= 2)
					{
						ValueCheck[SampleDeck.Cards[CardIndex].Value] = true;
					}
				}

				foreach(KeyValuePair<Values,bool> Entry in ValueCheck)
				{
					if(Entry.Value)
					{
						Card[] NewPair = new Card[2];
						NewPair[0] = new Card(Suits.Spades,Entry.Key);
						NewPair[1] = new Card(Suits.Hearts,Entry.Key);
						KeyPairsInDeck.Add(NewPair);
						ValueCheck[Entry.Key] = false;
					}
				}
				#endregion

				#region Based on those rest of the cards in hand, find pairs in the deck and store them in list
				for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
				{
					if(Utility.CountValueOccurance(PlayerHand,PlayerHand[CardIndex].Value) == 1)
					{
						for(int DeckCardIndex = 0; DeckCardIndex < SampleDeck.Cards.Count; DeckCardIndex++)
						{
							if(PlayerHand[CardIndex].Value == SampleDeck.Cards[DeckCardIndex].Value && !ValueCheck[SampleDeck.Cards[CardIndex].Value])
							{
								ValueCheck[SampleDeck.Cards[CardIndex].Value] = true;
								break;
							}
						}
					}
				}

				foreach(KeyValuePair<Values,bool> Entry in ValueCheck)
				{
					if(Entry.Value)
					{
						Card[] NewPair = new Card[2];
						NewPair[0] = new Card(Suits.Spades,Entry.Key);
						NewPair[1] = new Card(Suits.Hearts,Entry.Key);
						KeyPairsInHandWithDeck.Add(NewPair);
						ValueCheck[Entry.Key] = false;
					}
				}
				#endregion

				#region Determine the possible kicker card
				for(int CardIndex = 0; CardIndex < PlayerHand.Length; CardIndex++)
				{
					ValueCheck[PlayerHand[CardIndex].Value] = true;
				}

				for(int CardIndex = 0; CardIndex < SampleDeck.Cards.Count; CardIndex++)
				{
					ValueCheck[SampleDeck.Cards[CardIndex].Value] = true;
				}

				foreach(KeyValuePair<Values,bool> Entry in ValueCheck)
				{
					if(Entry.Value)
						KeyKickers.Add(new Card(Suits.Spades,Entry.Key));
				}
				#endregion

				if(CurrentTurnPhase == TurnPhase.Betting)
				{
					#region Form those possible hands and store them
					if(PlayerHandType == Hands.OnePair)
					{
						// HH DD H
						for(int DeckPairIndex = 0; DeckPairIndex < KeyPairsInDeck.Count; DeckPairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = PairCards[0];
							PossibleHand[1] = PairCards[1];
							PossibleHand[2] = KeyPairsInDeck[DeckPairIndex][0];
							PossibleHand[3] = KeyPairsInDeck[DeckPairIndex][1];
							
							for(int KickerIndex = 0; KickerIndex < MiscCards.Count; KickerIndex++)
							{
								PossibleHand[4] = MiscCards[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}
						
						// HH DD D
						for(int DeckPairIndex = 0; DeckPairIndex < KeyPairsInDeck.Count; DeckPairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = PairCards[0];
							PossibleHand[1] = PairCards[1];
							PossibleHand[2] = KeyPairsInDeck[DeckPairIndex][0];
							PossibleHand[3] = KeyPairsInDeck[DeckPairIndex][1];
							
							for(int KickerIndex = 0; KickerIndex < KeyKickers.Count; KickerIndex++)
							{
								PossibleHand[4] = KeyKickers[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}
						
						// HH HD H
						for(int PairIndex = 0; PairIndex < KeyPairsInHandWithDeck.Count; PairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = PairCards[0];
							PossibleHand[1] = PairCards[1];
							PossibleHand[2] = KeyPairsInHandWithDeck[PairIndex][0];
							PossibleHand[3] = KeyPairsInHandWithDeck[PairIndex][1];
							
							for(int KickerIndex = 0; KickerIndex < MiscCards.Count; KickerIndex++)
							{
								PossibleHand[4] = MiscCards[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}
						
						// HH HD D
						for(int PairIndex = 0; PairIndex < KeyPairsInHandWithDeck.Count; PairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = PairCards[0];
							PossibleHand[1] = PairCards[1];
							PossibleHand[2] = KeyPairsInHandWithDeck[PairIndex][0];
							PossibleHand[3] = KeyPairsInHandWithDeck[PairIndex][1];
							
							for(int KickerIndex = 0; KickerIndex < KeyKickers.Count; KickerIndex++)
							{
								PossibleHand[4] = KeyKickers[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}
						
					}

					// HD HD H
					for(int FirstPairIndex = 0; FirstPairIndex < KeyPairsInHandWithDeck.Count - 1; FirstPairIndex++)
					{
						for(int SecondPairIndex = FirstPairIndex + 1; SecondPairIndex < KeyPairsInHandWithDeck.Count; SecondPairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = KeyPairsInDeck[FirstPairIndex][0];
							PossibleHand[1] = KeyPairsInDeck[FirstPairIndex][1];
							PossibleHand[2] = KeyPairsInDeck[SecondPairIndex][0];
							PossibleHand[3] = KeyPairsInDeck[SecondPairIndex][1];

							for(int KickerIndex = 0; KickerIndex < MiscCards.Count; KickerIndex++)
							{
								PossibleHand[4] = MiscCards[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}	
					}

					// HD HD D
					for(int FirstPairIndex = 0; FirstPairIndex < KeyPairsInHandWithDeck.Count - 1; FirstPairIndex++)
					{
						for(int SecondPairIndex = FirstPairIndex + 1; SecondPairIndex < KeyPairsInHandWithDeck.Count; SecondPairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = KeyPairsInDeck[FirstPairIndex][0];
							PossibleHand[1] = KeyPairsInDeck[FirstPairIndex][1];
							PossibleHand[2] = KeyPairsInDeck[SecondPairIndex][0];
							PossibleHand[3] = KeyPairsInDeck[SecondPairIndex][1];

							for(int KickerIndex = 0; KickerIndex < KeyKickers.Count; KickerIndex++)
							{
								PossibleHand[4] = KeyKickers[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}	
					}

					// HD DD H
					for(int FirstPairIndex = 0; FirstPairIndex < KeyPairsInHandWithDeck.Count; FirstPairIndex++)
					{
						for(int SecondPairIndex = 0; SecondPairIndex < KeyPairsInDeck.Count; SecondPairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = KeyPairsInDeck[FirstPairIndex][0];
							PossibleHand[1] = KeyPairsInDeck[FirstPairIndex][1];
							PossibleHand[2] = KeyPairsInDeck[SecondPairIndex][0];
							PossibleHand[3] = KeyPairsInDeck[SecondPairIndex][1];

							for(int KickerIndex = 0; KickerIndex < MiscCards.Count; KickerIndex++)
							{
								PossibleHand[4] = MiscCards[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}	
					}
					#endregion
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
				{
					if(PlayerHandType == Hands.OnePair)
					{
						#region Form these possible hands and store them
						if(PlayerHandType == Hands.OnePair)
						{
							// HH DD H
							for(int DeckPairIndex = 0; DeckPairIndex < KeyPairsInDeck.Count; DeckPairIndex++)
							{
								Card[] PossibleHand = new Card[5];
								PossibleHand[0] = PairCards[0];
								PossibleHand[1] = PairCards[1];
								PossibleHand[2] = KeyPairsInDeck[DeckPairIndex][0];
								PossibleHand[3] = KeyPairsInDeck[DeckPairIndex][1];
								
								for(int KickerIndex = 0; KickerIndex < MiscCards.Count; KickerIndex++)
								{
									PossibleHand[4] = MiscCards[KickerIndex];
									PossibleHands.AddLast(PossibleHand);
								}
							}
							
							// HH HD D
							for(int PairIndex = 0; PairIndex < KeyPairsInHandWithDeck.Count; PairIndex++)
							{
								Card[] PossibleHand = new Card[5];
								PossibleHand[0] = PairCards[0];
								PossibleHand[1] = PairCards[1];
								PossibleHand[2] = KeyPairsInHandWithDeck[PairIndex][0];
								PossibleHand[3] = KeyPairsInHandWithDeck[PairIndex][1];
								
								for(int KickerIndex = 0; KickerIndex < KeyKickers.Count; KickerIndex++)
								{
									PossibleHand[4] = KeyKickers[KickerIndex];
									PossibleHands.AddLast(PossibleHand);
								}
							}

							// HH HD H
							for(int PairIndex = 0; PairIndex < KeyPairsInHandWithDeck.Count; PairIndex++)
							{
								Card[] PossibleHand = new Card[5];
								PossibleHand[0] = PairCards[0];
								PossibleHand[1] = PairCards[1];
								PossibleHand[2] = KeyPairsInHandWithDeck[PairIndex][0];
								PossibleHand[3] = KeyPairsInHandWithDeck[PairIndex][1];

								for(int KickerIndex = 0; KickerIndex < MiscCards.Count; KickerIndex++)
								{
									PossibleHand[4] = MiscCards[KickerIndex];
									PossibleHands.AddLast(PossibleHand);
								}
							}
						}
						#endregion
					}
					// HD HD H
					for(int FirstPairIndex = 0; FirstPairIndex < KeyPairsInHandWithDeck.Count - 1; FirstPairIndex++)
					{
						for(int SecondPairIndex = FirstPairIndex + 1; SecondPairIndex < KeyPairsInHandWithDeck.Count; SecondPairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = KeyPairsInHandWithDeck[FirstPairIndex][0];
							PossibleHand[1] = KeyPairsInHandWithDeck[FirstPairIndex][1];
							PossibleHand[2] = KeyPairsInHandWithDeck[SecondPairIndex][0];
							PossibleHand[3] = KeyPairsInHandWithDeck[SecondPairIndex][1];
							
							for(int KickerIndex = 0; KickerIndex < MiscCards.Count; KickerIndex++)
							{
								PossibleHand[4] = MiscCards[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}
					}
				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
				{
					if(PlayerHandType == Hands.OnePair)
					{
						// HH HD H
						for(int PairIndex = 0; PairIndex < KeyPairsInHandWithDeck.Count; PairIndex++)
						{
							Card[] PossibleHand = new Card[5];
							PossibleHand[0] = PairCards[0];
							PossibleHand[1] = PairCards[1];
							PossibleHand[2] = KeyPairsInHandWithDeck[PairIndex][0];
							PossibleHand[3] = KeyPairsInHandWithDeck[PairIndex][1];

							for(int KickerIndex = 0; KickerIndex < MiscCards.Count; KickerIndex++)
							{
								PossibleHand[4] = MiscCards[KickerIndex];
								PossibleHands.AddLast(PossibleHand);
							}
						}
					}
				}
			}

			else if((Hands) HandIndex == Hands.OnePair)
			{
				if(PlayerHandType == Hands.OnePair)
				{

					break;
				}

				if(CurrentTurnPhase == TurnPhase.Betting)
				{

				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
				{

				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
				{

				}
			}

			else if((Hands) HandIndex == Hands.HighCard)
			{
				if(PlayerHandType == Hands.HighCard)
				{

					break;
				}

				if(CurrentTurnPhase == TurnPhase.Betting)
				{

				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && CurrentAuctionPhase == AuctionPhase.First)
				{

				}
				else if(CurrentTurnPhase == TurnPhase.Auctioning && (CurrentAuctionPhase == AuctionPhase.Second || CurrentAuctionPhase == AuctionPhase.Third))
				{

				}
			}
		}

		return PossibleHands;
	}

	public static float CalculateEquityAgainstOpponent (Player _Player, Player _Opponent, List<Card[]> _EnemyRange, bool _IncludePocket)
	{
		Card[] PlayerHand = _Player.Hand.ToArray();

		if(_IncludePocket)
			PlayerHand = Utility.GetBestPostSwapHand(_Player);

		List<Card[]> OpponentWinningHands = new List<Card[]>();
		List<Card[]> OpponentLosingHands = new List<Card[]>();
		//NOTE: THE DETERMINING OF WINNING AND LOSING RANGE FOR OPPONENT MUST HAVE THE OPTION OF INCLUDING POCKET CARDS AS WELL
		for(int HandIndex = 0; HandIndex < _EnemyRange.Count; HandIndex++)
		{
			Card[] PossibleHand = _EnemyRange[HandIndex];

			if(IsFirstHandSuperiorOverSecond(PossibleHand,PlayerHand))
			{
				OpponentWinningHands.Add(PossibleHand);
				continue;
			}

			OpponentLosingHands.Add(PossibleHand);
		}

		float EnemyEquity = ((float) OpponentWinningHands.Count / (float) _EnemyRange.Count) * 100.0f;
//		Debug.Log("Opponent winning hand amount: " + OpponentWinningHands.Count);
//		Debug.Log("Total opponent hands: " + _EnemyRange.Count);
//		Debug.Log("Enemy equity: " + EnemyEquity);

		return 100.0f - EnemyEquity;
	}

	public static float CalculatePlayerAverageEquity(Player _Player)
	{
		float TotalEquity = 0.0f;
		int AmountOfValidPlayers = 0;

		for(int PlayerIndex = 0; PlayerIndex < _Player.GManager.Players.Length; PlayerIndex++)
		{
			if(PlayerIndex != _Player.Index - 1 && !_Player.GManager.Players[PlayerIndex].Busted && !_Player.GManager.Players[PlayerIndex].Fold)
			{
				List<Card[]> EnemyRange = DetermineRangeOfOpponent(_Player,_Player.GManager.Players[PlayerIndex],true);
				TotalEquity += CalculateEquityAgainstOpponent(_Player,_Player.GManager.Players[PlayerIndex],EnemyRange,true);
				AmountOfValidPlayers++;
			}
		}

		return TotalEquity / (float) AmountOfValidPlayers;
	}

	public static bool DoesPlayerHasPotOdd(Player _Player)
	{
		int MoneyUsedToCall = _Player.GManager.Table.GetHighestBet() - _Player.OnTheBet;

		float EquityRequired = (float) MoneyUsedToCall / (float) (MoneyUsedToCall + _Player.GManager.Table.Pot);

		return CalculatePlayerAverageEquity(_Player) >= EquityRequired ? true : false;
	}

	public static bool DoesPlayerHasImpliedOdd(Player _Player)
	{
		float MoneyRequired = MoneyNeededToGetImpliedOdd(_Player);
		float CurrentPot = (float) _Player.GManager.Table.Pot;

		float MoneyToMeetOdds = MoneyRequired - CurrentPot;
		int AmountOfValidPlayer = Utility.HowManyValidPlayersLeft(_Player.GManager);

		float PossibleAmountOfBet = 0.0f;

		for(int PlayerIndex = 0; PlayerIndex < _Player.GManager.Players.Length; PlayerIndex++)
		{
			if(_Player.GManager.Players[PlayerIndex].Busted || _Player.GManager.Players[PlayerIndex].Fold)
				continue;

			float MoneyEnemyAlreadyBet = _Player.GManager.Players[PlayerIndex].OnTheBet;

			float EnemyAverageEquity = CalculatePlayerAverageEquity(_Player.GManager.Players[PlayerIndex]);

			if(EnemyAverageEquity >= 50.0f)
			{
				float MinBet = -((EnemyAverageEquity * CurrentPot) / (EnemyAverageEquity * AmountOfValidPlayer - 1 + EnemyAverageEquity));
				PossibleAmountOfBet += MinBet;
			}
		}

		return PossibleAmountOfBet >= MoneyToMeetOdds ? true : false; 
	}

	public static float MoneyNeededToGetImpliedOdd(Player _Player)
	{
		float AverageEquity = CalculatePlayerAverageEquity(_Player);

		float Bet = _Player.GManager.Betting.CurrentBet;
		float RequiredMoney = 0.0f;

		if(AverageEquity >= 35.0f)
			RequiredMoney = 2.0f * Bet;
		else if(AverageEquity >= 25.0f && AverageEquity < 35.0f)
			RequiredMoney = 3.0f * Bet;
		else if(AverageEquity >= 20.0f && AverageEquity < 25.0f)
			RequiredMoney = 4.0f * Bet;
		else if(AverageEquity >= 15.0f && AverageEquity < 20.0f)
			RequiredMoney = 6.0f * Bet;
		else if(AverageEquity > 0.0f && AverageEquity < 15.0f)
			RequiredMoney = 9.0f * Bet;

		if((float) _Player.GManager.Table.Pot > RequiredMoney)
			return 0.0f;

		return RequiredMoney;
	}

	public static HandGrade DetermineHandGrade(Player _Player)
	{
		int Aggressiveness = _Player.EnemyAI.Aggressiveness;

		Hands HandType = EvaluateHand(_Player.Hand);

		if(HandType > Hands.OnePair)
			return HandGrade.Good;

		Values RepresentativeValue = Values.Two;

		if(HandType == Hands.HighCard)
		{
			Values HighestValue = Values.Two;

			for(int CardIndex = 0; CardIndex < _Player.Hand.Count; CardIndex++)
			{
				if(_Player.Hand[CardIndex].Value > HighestValue)
					HighestValue = _Player.Hand[CardIndex].Value;
			}

			RepresentativeValue = HighestValue;
		}
		else if(HandType == Hands.OnePair)
		{
			for(int FirstCardIndex = 0; FirstCardIndex < _Player.Hand.Count; FirstCardIndex++)
			{
				for(int SecondCardIndex = 0; SecondCardIndex < _Player.Hand.Count; SecondCardIndex++)
				{
					if(_Player.Hand[SecondCardIndex] != _Player.Hand[FirstCardIndex] && _Player.Hand[SecondCardIndex].Value == _Player.Hand[FirstCardIndex].Value)
					{
						RepresentativeValue = _Player.Hand[SecondCardIndex].Value;
						break;
					}
				}
			}
		}

		if(Aggressiveness >= 10)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Three && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;

			else if(HandType == Hands.OnePair && RepresentativeValue == Values.Two || HandType == Hands.HighCard && RepresentativeValue >= Values.Eight && RepresentativeValue <= Values.Ace)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Seven)
				return HandGrade.Bad;
		}
		else if(Aggressiveness >= 8 && Aggressiveness < 10)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Five && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;
			
			else if(HandType == Hands.OnePair && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Four || HandType == Hands.HighCard && RepresentativeValue >= Values.Ten && RepresentativeValue <= Values.Ace)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Nine)
				return HandGrade.Bad;
		}
		else if(Aggressiveness >= 5 && Aggressiveness < 8)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Seven && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;

			else if(HandType == Hands.OnePair && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Six || HandType == Hands.HighCard && RepresentativeValue >= Values.Queen && RepresentativeValue <= Values.Ace)
				return HandGrade.Average;
		
			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Jack)
				return HandGrade.Bad;
		}
		else if(Aggressiveness >= 2 && Aggressiveness < 5)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Nine && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;

			else if(HandType == Hands.OnePair && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Eight || HandType == Hands.HighCard && RepresentativeValue == Values.Ace)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.King)
				return HandGrade.Bad;
		}
		else if(Aggressiveness >= 0 && Aggressiveness < 2)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Jack && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;

			else if(HandType == Hands.OnePair && RepresentativeValue >= Values.Three && RepresentativeValue <= Values.Ten)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Ace || HandType == Hands.OnePair && RepresentativeValue == Values.Two)
				return HandGrade.Bad;
		}

		return HandGrade.Average;
	}

	public static HandGrade DetermineHandGradeByHand(Player _Player, Card[] _Hand)
	{
		int Aggressiveness = _Player.EnemyAI.Aggressiveness;

		Hands HandType = EvaluateHand(new List<Card>(_Hand));

		if(HandType > Hands.OnePair)
			return HandGrade.Good;

		Values RepresentativeValue = Values.Two;

		if(HandType == Hands.HighCard)
		{
			Values HighestValue = Values.Two;

			for(int CardIndex = 0; CardIndex < _Player.Hand.Count; CardIndex++)
			{
				if(_Player.Hand[CardIndex].Value > HighestValue)
					HighestValue = _Player.Hand[CardIndex].Value;
			}

			RepresentativeValue = HighestValue;
		}
		else if(HandType == Hands.OnePair)
		{
			for(int FirstCardIndex = 0; FirstCardIndex < _Player.Hand.Count; FirstCardIndex++)
			{
				for(int SecondCardIndex = 0; SecondCardIndex < _Player.Hand.Count; SecondCardIndex++)
				{
					if(_Player.Hand[SecondCardIndex] != _Player.Hand[FirstCardIndex] && _Player.Hand[SecondCardIndex].Value == _Player.Hand[FirstCardIndex].Value)
					{
						RepresentativeValue = _Player.Hand[SecondCardIndex].Value;
						break;
					}
				}
			}
		}

		if(Aggressiveness >= 10)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Three && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;

			else if(HandType == Hands.OnePair && RepresentativeValue == Values.Two || HandType == Hands.HighCard && RepresentativeValue >= Values.Eight && RepresentativeValue <= Values.Ace)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Seven)
				return HandGrade.Bad;
		}
		else if(Aggressiveness >= 8 && Aggressiveness < 10)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Five && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;
			
			else if(HandType == Hands.OnePair && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Four || HandType == Hands.HighCard && RepresentativeValue >= Values.Ten && RepresentativeValue <= Values.Ace)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Nine)
				return HandGrade.Bad;
		}
		else if(Aggressiveness >= 5 && Aggressiveness < 8)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Seven && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;

			else if(HandType == Hands.OnePair && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Six || HandType == Hands.HighCard && RepresentativeValue >= Values.Queen && RepresentativeValue <= Values.Ace)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Jack)
				return HandGrade.Bad;
		}
		else if(Aggressiveness >= 2 && Aggressiveness < 5)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Nine && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;

			else if(HandType == Hands.OnePair && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Eight || HandType == Hands.HighCard && RepresentativeValue == Values.Ace)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.King)
				return HandGrade.Bad;
		}
		else if(Aggressiveness >= 0 && Aggressiveness < 2)
		{
			if(HandType == Hands.OnePair && RepresentativeValue >= Values.Jack && RepresentativeValue <= Values.Ace)
				return HandGrade.Good;

			else if(HandType == Hands.OnePair && RepresentativeValue >= Values.Three && RepresentativeValue <= Values.Ten)
				return HandGrade.Average;

			else if(HandType == Hands.HighCard && RepresentativeValue >= Values.Two && RepresentativeValue <= Values.Ace || HandType == Hands.OnePair && RepresentativeValue == Values.Two)
				return HandGrade.Bad;
		}

		return HandGrade.Average;
	}

	public static PositionLevel DeterminePlayerPositionAdvantage(Player _Player)
	{
		TablePosition PlayerPosition = _Player.GManager.Betting.DeterminePlayerPosition(_Player);

		if(PlayerPosition == TablePosition.OnTheButton)
			return PositionLevel.Advantage;
		else if(PlayerPosition == TablePosition.UnderTheGun)
			return PositionLevel.Neutral;

		return PositionLevel.Disadvantage;
	}

	public static int DetermineHandLevelINT(Card[] _Hand)
	{
		if(_Hand == null || _Hand.Length < 5)
		{
			Debug.Log("HAND BEING PASS TO DETERMINE LEVEL IS NOT A COMPLETE/INVALID HAND");
			return 0;
		}

		Hands HandType = EvaluateHand(_Hand);
		if(HandType > Hands.OnePair)
			return 22;

		if(HandType == Hands.OnePair)
		{
			Values RepresentativeValue = Values.Two;

			for(int FirstCardIndex = 0; FirstCardIndex < _Hand.Length; FirstCardIndex++)
			{
				for(int SecondCardIndex = 0; SecondCardIndex < _Hand.Length; SecondCardIndex++)
				{
					if(_Hand[SecondCardIndex] != _Hand[FirstCardIndex] && _Hand[SecondCardIndex].Value == _Hand[FirstCardIndex].Value)
					{
						RepresentativeValue = _Hand[SecondCardIndex].Value;
						break;
					}
				}
			}

			return 9 + ((int) RepresentativeValue - (int) Values.Two);
		}
		else if(HandType == Hands.HighCard)
		{
			Values RepresentativeValue = Values.Two;

			for(int CardIndex = 0; CardIndex < _Hand.Length; CardIndex++)
			{
				if(_Hand[CardIndex].Value > RepresentativeValue)
					RepresentativeValue = _Hand[CardIndex].Value;
			}

			if(RepresentativeValue < Values.Seven)
				return 1;

			return 2 + ((int) RepresentativeValue - (int) Values.Seven);
		}

		return 1;
	}

	public static int DeterminePlayerHandLevelINT(Player _Player)
	{
		Hands HandType = EvaluateHand(_Player.Hand);
		if(HandType > Hands.OnePair)
			return 22;

		if(HandType == Hands.OnePair)
		{
			Values RepresentativeValue = Values.Two;

			for(int FirstCardIndex = 0; FirstCardIndex < _Player.Hand.Count; FirstCardIndex++)
			{
				for(int SecondCardIndex = 0; SecondCardIndex < _Player.Hand.Count; SecondCardIndex++)
				{
					if(_Player.Hand[SecondCardIndex] != _Player.Hand[FirstCardIndex] && _Player.Hand[SecondCardIndex].Value == _Player.Hand[FirstCardIndex].Value)
					{
						RepresentativeValue = _Player.Hand[SecondCardIndex].Value;
						break;
					}
				}
			}

			return 9 + ((int) RepresentativeValue - (int) Values.Two);
		}
		else if(HandType == Hands.HighCard)
		{
			Values RepresentativeValue = Values.Two;

			for(int CardIndex = 0; CardIndex < _Player.Hand.Count; CardIndex++)
			{
				if(_Player.Hand[CardIndex].Value > RepresentativeValue)
					RepresentativeValue = _Player.Hand[CardIndex].Value;
			}

			if(RepresentativeValue < Values.Seven)
				return 1;

			return 2 + ((int) RepresentativeValue - (int) Values.Seven);
		}

		return 1;
	}

	public static float FindPlayerProabilityofSpecificHandGrade (Player _Player, Player _Opponent,HandGrade _HandGrade)
	{
		List<Card[]> PlayerRange = DetermineRangeOfOpponent(_Player,_Opponent,true);

		int HandsInRange = 0;
		int HandsNotInRange = 0;

		for(int HandIndex = 0; HandIndex < PlayerRange.Count; HandIndex++)
		{
			if(DetermineHandGradeByHand(_Player,PlayerRange[HandIndex]) == _HandGrade)
				HandsInRange++;
			else
				HandsNotInRange++;
		}

		return ((float) HandsInRange / (float) PlayerRange.Count) * 100.0f;
	}

	public static StackSizing DeterminePlayerStackSize (Player _Player)
	{
		int PlayerStackInBB = _Player.Stack / _Player.GManager.Betting.BigBind;

		if(PlayerStackInBB >= 0 && PlayerStackInBB <= 40)
			return StackSizing.Short;

		else if(PlayerStackInBB > 40 && PlayerStackInBB <= 80)
			return StackSizing.Medium;

		else if(PlayerStackInBB > 80 && PlayerStackInBB <= 100)
			return StackSizing.Deep;

		return StackSizing.Short;
	}
}
