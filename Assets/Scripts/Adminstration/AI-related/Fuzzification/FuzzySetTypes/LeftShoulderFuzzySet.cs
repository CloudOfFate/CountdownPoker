using UnityEngine;
using System.Collections;

public class LeftShoulderFuzzySet : FuzzySet {

	private float m_PeakPoint;
	
	private float m_LeftOffSet;
	private float m_RightOffSet;
	
	public LeftShoulderFuzzySet(float _PeakValue, float _LeftOffSet, float _RightOffSet):base(_PeakValue)
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
//			Debug.Log("LeftShoulderFuzzySet DOM calculated is 0");  
			return m_DOM;
		}
		
		if(_PassValue <= m_PeakPoint)
		{
			m_DOM = 1.0f;
//			Debug.Log("Left point: " + (m_PeakPoint - m_LeftOffSet) + " right point: " + (m_PeakPoint + m_RightOffSet));
//			Debug.Log("LeftShoulderFuzzySet DOM calculated is " + m_DOM);
		}
		else
		{
			m_DOM = 1.0f - ((_PassValue - m_PeakPoint) / (m_RightOffSet));
//			Debug.Log("Left point: " + (m_PeakPoint - m_LeftOffSet) + " right point: " + (m_PeakPoint + m_RightOffSet));
//			Debug.Log("passvalue - peakpoint: " + (_PassValue - m_PeakPoint) + " rightoffset - peakpoint: " + (m_RightOffSet));
//			Debug.Log("LeftShoulderFuzzySet DOM calculated is " + m_DOM);
		}

		return m_DOM;
	}
}
