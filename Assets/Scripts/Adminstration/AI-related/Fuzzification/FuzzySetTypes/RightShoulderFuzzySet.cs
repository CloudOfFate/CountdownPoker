using UnityEngine;
using System.Collections;

public class RightShoulderFuzzySet : FuzzySet 
{
	private float m_PeakPoint;
	
	private float m_LeftOffSet;
	private float m_RightOffSet;
	
	public RightShoulderFuzzySet(float _PeakValue, float _LeftOffSet, float _RightOffSet):base(_PeakValue)
	{
		m_PeakPoint = _PeakValue;
		m_RepresentativeValue = _PeakValue;
		
		m_LeftOffSet = _LeftOffSet;
		
		m_RightOffSet = _RightOffSet;
	}
	
	public override float CalculateDOM(float _PassValue)
	{
		if(_PassValue < (m_PeakPoint - m_LeftOffSet) || _PassValue > (m_PeakPoint + m_RightOffSet))
		{ 
			m_DOM = 0.0f; 
//			Debug.Log("RightShoulderFuzzySet DOM calculated is 0"); 
			return m_DOM;
		}
	
		if(_PassValue >= m_PeakPoint)
			m_DOM = 1.0f;
		else
			m_DOM = (_PassValue - (m_PeakPoint - m_LeftOffSet))/m_LeftOffSet;
			
//		Debug.Log("RightShoulderFuzzySet DOM calculated is " + m_DOM);
		return m_DOM;
	}
}
