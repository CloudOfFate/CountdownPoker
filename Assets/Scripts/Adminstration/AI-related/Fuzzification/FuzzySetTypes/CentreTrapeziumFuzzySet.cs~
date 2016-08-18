using UnityEngine;
using System.Collections;

public class CentreTrapeziumFuzzySet : FuzzySet 
{
	private float m_FirstPeakPoint;
	private float m_SecondPeakPoint;
	
	private float m_LeftOffSet;
	private float m_RightOffSet;
	
	public CentreTrapeziumFuzzySet(float _FirstPeak, float _SecondPeak, float _MinValue, float _MaxValue):base(_FirstPeak)
	{
		m_FirstPeakPoint = _FirstPeak;
		m_SecondPeakPoint = _SecondPeak;
		m_LeftOffSet = m_FirstPeakPoint - _MinValue;
		m_RightOffSet = _MaxValue - m_SecondPeakPoint;
	}
	
	public override float CalculateDOM (float _PassValue)
	{
		
		if(_PassValue < (m_FirstPeakPoint - m_LeftOffSet) || _PassValue > (m_SecondPeakPoint + m_RightOffSet))
		{
			m_DOM = 0.0f; 
			return m_DOM;
		}
		
		if(_PassValue >= (m_FirstPeakPoint - m_LeftOffSet) && _PassValue <= m_FirstPeakPoint)
			m_DOM = (_PassValue - (m_FirstPeakPoint - m_LeftOffSet))/m_LeftOffSet;
		else if(_PassValue > m_FirstPeakPoint && _PassValue < m_SecondPeakPoint)
			m_DOM = 1.0f;
		else if(_PassValue >= m_SecondPeakPoint && _PassValue <= (m_SecondPeakPoint + m_RightOffSet))
			m_DOM = 1.0f - (_PassValue - m_SecondPeakPoint)/m_RightOffSet;
		
		return m_DOM;
	}
}
