using UnityEngine;
using System.Collections;

public class TriangularFuzzySet : FuzzySet 
{
	private float m_PeakPoint;
	
	//Amount of offset to left/right in terms of x value
	private float m_LeftOffSet;
	private float m_RightOffSet;
	
	public TriangularFuzzySet(float _PeakValue, float _LeftOffSet, float _RightOffSet):base(_PeakValue)
	{
		m_PeakPoint = _PeakValue;
		m_RepresentativeValue = _PeakValue;
		
		m_LeftOffSet = _LeftOffSet;
		
		m_RightOffSet = _RightOffSet;
	}
	
	public override float CalculateDOM(float _PassValue)
	{
		if(_PassValue > (m_PeakPoint + m_RightOffSet) || _PassValue < (m_PeakPoint - m_LeftOffSet))
		{
			m_DOM = 0.0f; 
//			Debug.Log("TriangularFuzzySet DOM calculated is " + m_DOM);
			return m_DOM;
		}
		
		if(_PassValue == m_PeakPoint)
		{
			m_DOM = 1.0f; 
//			Debug.Log("TriangularFuzzySet DOM calculated is " + m_DOM);
			return m_DOM;
		}
		
		if(_PassValue > m_PeakPoint)
			m_DOM = (_PassValue - m_PeakPoint) / m_RightOffSet;
		else if(_PassValue < m_PeakPoint)
			m_DOM = (_PassValue - (m_PeakPoint - m_LeftOffSet))/(m_LeftOffSet);
			
//		Debug.Log("TriangularFuzzySet DOM calculated is " + m_DOM);
		return m_DOM;
	}
}
