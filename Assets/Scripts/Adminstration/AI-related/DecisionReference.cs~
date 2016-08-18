using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DecisionReference
{
	public static HandValue[] HandValuesReference;
	public static Calling[] CallingsReference;
	public static Raising[] RaisingsReference;
	public static RaisingType[] RaisingTypesReference;
	public static Folding[] FoldingReference;
	public static Bidding[] BiddingsReference;
	public static Forfeiting[] ForfeitingReference;
	public static Purchasing[] PurchasingReference;
	public static Bluffing[] BluffingReference;

	public DecisionReference()
	{
		HandValuesReference = HandValuesDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/HandValuesXML.xml")).HandValues.ToArray();
		CallingsReference = CallingDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/WorthCallingXML.xml")).Callings.ToArray();
		RaisingsReference = RaisingDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/WorthRaisingXML.xml")).Raisings.ToArray();
		RaisingTypesReference = RaisingTypeDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/RaisingTypesXML.xml")).RaiseTypes.ToArray();
		FoldingReference = FoldingDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/WorthFoldingXML.xml")).Foldings.ToArray();
		BiddingsReference = BiddingDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/WorthBiddingXML.xml")).Biddings.ToArray();
		ForfeitingReference = ForfeitingDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/WorthForfeitingXML.xml")).Forfeitings.ToArray();
		PurchasingReference = PurchasingDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/WorthPurchasingXML.xml")).Purchasings.ToArray();
		BluffingReference = BluffingDatabase.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/WorthBluffingXML.xml")).Bluffings.ToArray();
	}
	
	public static int GetValueOfCurrentHand(List<Card> _Hand)
	{
		Hands type = Evaluator.EvaluateHand(_Hand);
		Tier tier = Evaluator.EvaluateHandTier(_Hand,type);
	
		for(int i = 0; i < HandValuesReference.Length; i++)
		{
			if(HandValuesReference[i].HandType == type)
			{
				if(tier == Tier.One){return HandValuesReference[i].Tier1;}
				else if(tier == Tier.Two){return HandValuesReference[i].Tier2;}
				else if(tier == Tier.Three){return HandValuesReference[i].Tier3;}
				else if(tier == Tier.Four){return HandValuesReference[i].Tier4;}
			}
		}
		return HandValuesReference[0].Tier1;
	}
	
	public static int GetValueOfHandAndPocket(Player _Player)
	{
		Card[] Hand          = _Player.Hand.ToArray();
		Card[] Pocket        = _Player.Pocket.ToArray();
		
		Card[] PossibleHand  = (Card[]) Hand.Clone();
		Card[] HighestHand   = (Card[]) Hand.Clone();
		
		int HighestHandValue = GetValueOfCurrentHand(new List<Card>(HighestHand));
		
		if(Pocket.Length > 0)
		{
			for(int PocketSlot = 0; PocketSlot < Pocket.Length; PocketSlot++)
			{
				Card PocketCard = Pocket[PocketSlot];
				
				for(int HandSlot = 0; HandSlot < Hand.Length; HandSlot++)
				{
					PossibleHand = (Card[]) Hand.Clone();
					PossibleHand[HandSlot] = PocketCard;
					
					int PossibleHandValue = GetValueOfCurrentHand(new List<Card> (PossibleHand));
					
					if(PossibleHandValue > HighestHandValue)
					{
						HighestHand = (Card[]) PossibleHand.Clone();
						HighestHandValue = PossibleHandValue;
					}
					else if(PossibleHandValue == HighestHandValue)
					{
						int PossibleHandScore = 0;
						int HighestHandScore  = 0;
						
						for(int PossibleHandSlot = 0; PossibleHandSlot < PossibleHand.Length; PossibleHandSlot++)
						{
							PossibleHandScore += (int) PossibleHand[PossibleHandSlot].Suit + (int) PossibleHand[PossibleHandSlot].Value;
						}
						
						for(int HighestHandSlot = 0; HighestHandSlot < HighestHand.Length; HighestHandSlot++)
						{
							HighestHandScore += (int) HighestHand[HighestHandSlot].Suit + (int) HighestHand[HighestHandSlot].Value;
						}
						
						if(PossibleHandScore > HighestHandScore)
						{
							HighestHand = (Card[]) PossibleHand.Clone();
							HighestHandValue = PossibleHandValue;
						}
					}
				}
			}
			
			return GetValueOfCurrentHand(new List<Card> (HighestHand));
		}
		
		return GetValueOfCurrentHand(new List<Card>(Hand));
	}
	
//	public static RaiseType GetPlayerApproachToRaise(Player _Player)
//	{
//		float DefensiveLevel = _Player.EnemyAI.RaisingModule.Variables["Aggressiveness"].Sets["Aggressiveness_Defensive"].DOM;
//		float NeutralLevel = _Player.EnemyAI.RaisingModule.Variables["Aggressiveness"].Sets["Aggressiveness_Neutral"].DOM;
//		float AggressivenessLevel = _Player.EnemyAI.RaisingModule.Variables["Aggressiveness"].Sets["Aggressiveness_Aggressive"].DOM;
//		
//		AggressiveLevel Aggressiveness = AggressiveLevel.Neutral;
//		if(DefensiveLevel > NeutralLevel && DefensiveLevel > AggressivenessLevel)
//			Aggressiveness = AggressiveLevel.Defensive;
//		else 
//			Aggressiveness = NeutralLevel > AggressivenessLevel ? AggressiveLevel.Neutral : AggressiveLevel.Aggressive;
//		
//		float HandLowLevel = _Player.EnemyAI.RaisingModule.Variables["HandValue"].Sets["HandValue_Low"].DOM;
//		float HandMediumLevel = _Player.EnemyAI.RaisingModule.Variables["HandValue"].Sets["HandValue_Medium"].DOM;
//		float HandHighLevel = _Player.EnemyAI.RaisingModule.Variables["HandValue"].Sets["HandValue_High"].DOM;
//		
//		ValueOfHand HandValues = ValueOfHand.Low;
//		if(HandLowLevel > HandMediumLevel && HandLowLevel > HandHighLevel)
//			HandValues = ValueOfHand.Low;
//		else 
//			HandValues = HandMediumLevel > HandHighLevel ? ValueOfHand.Medium : ValueOfHand.High;
//		
//		float EarningLowLevel = _Player.EnemyAI.RaisingModule.Variables["Earnings"].Sets["Earnings_Low"].DOM;
//		float EarningMediumLevel = _Player.EnemyAI.RaisingModule.Variables["Earnings"].Sets["Earnings_Medium"].DOM;
//		float EarningHighLevel = _Player.EnemyAI.RaisingModule.Variables["Earnings"].Sets["Earnings_High"].DOM;
//		
//		Earning EarningLevel = Earning.Low;
//		if(EarningLowLevel > EarningMediumLevel && EarningLowLevel > EarningHighLevel)
//			EarningLevel = Earning.Low;
//		else
//			EarningLevel = EarningMediumLevel > EarningHighLevel ? Earning.Medium : Earning.High;
//		
//		for(int i = 0; i < RaisingTypesReference.Length; i++)
//		{
//			if(RaisingTypesReference[i].Aggressiveness == Aggressiveness && RaisingTypesReference[i].HandValue == HandValues && RaisingTypesReference[i].Earning == EarningLevel)
//				return RaisingTypesReference[i].TypeOfRaise;
//		}
//		return RaiseType.RaiseToFold;
//	}

//	#region Printing function for XML Tables inputted
//	public void PrintHandValueReference()
//	{
//		Debug.Log("///////////////HandValue REFERENCE///////////////////");
//		for(int i = 0; i < HandValuesReference.Length; i++)
//		{
//			Debug.Log("Hand Type: " + HandValuesReference[i].HandType + " Tier1: " + HandValuesReference[i].Tier1 + " Tier2: " + HandValuesReference[i].Tier2 + " Tier3: " + HandValuesReference[i].Tier3 + " Tier4: " + HandValuesReference[i].Tier4);
//		}
//	}
//	
//	public void PrintCallingsReference()
//	{
//		Debug.Log("///////////////CALLINGS REFERENCE///////////////////");
//		for(int i = 0; i < CallingsReference.Length; i++)
//		{
//			Debug.Log("Aggressiveness: " + CallingsReference[i].Aggressiveness + " Desirability: " + CallingsReference[i].Desirability + " HandValue: " + CallingsReference[i].HandValue + " MoneyInPot: " + CallingsReference[i].MoneyInPot + " MoneyRequiredToCall: " + CallingsReference[i].MoneyRequiredToCall);
//		}
//	}
//	
//	public void PrintRaisingsReference()
//	{
//		Debug.Log("///////////////RAISINGS REFERENCE///////////////////");
//		for(int i = 0; i < RaisingsReference.Length; i++)
//		{
//			Debug.Log("Aggressiveness: " + RaisingsReference[i].Aggressiveness + " Desirability: " + RaisingsReference[i].Desirability + " HandValue: " + RaisingsReference[i].HandValue + " MoneyInPot: " + RaisingsReference[i].MoneyInPot + " PlayerMoney: " + RaisingsReference[i].PlayerMoney);
//		}
//	}
//	
//	public void PrintBiddingsReference()
//	{
//		Debug.Log("///////////////BIDDINGS REFERENCE///////////////////");
//		for(int i = 0; i < BiddingsReference.Length; i++)
//		{
//			Debug.Log("Aggressiveness: " + BiddingsReference[i].Aggressiveness + " Desirability: " + BiddingsReference[i].Desirability + " Earning: " + BiddingsReference[i].Earning + " ProjectedHandValue: " + BiddingsReference[i].ProjectedHandValue + " PlayerMoney: " + BiddingsReference[i].PlayerMoney);
//		}
//	}
//	
//	public void PrintPurchasingReference()
//	{
//		Debug.Log("///////////////PURCHASING REFERENCE///////////////////");
//		for(int i = 0; i < PurchasingReference.Length; i++)
//		{
//			Debug.Log("Aggressiveness: " + PurchasingReference[i].Aggressiveness + " Desirability: " + PurchasingReference[i].Desirability + " Earning: " + PurchasingReference[i].Earning + " ProjectedHandValue: " + PurchasingReference[i].ProjectedHandValue + " PlayerMoney: " + PurchasingReference[i].PlayerMoney);
//		}
//	}
//	#endregion
}
