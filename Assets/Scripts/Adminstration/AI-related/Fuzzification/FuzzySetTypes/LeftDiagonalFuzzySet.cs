using UnityEngine;
using System.Collections;

public class LeftDiagonalFuzzySet : FuzzySet // " / " shape 
{
	private float m_PeakPoint;
	
	private float m_LeftOffSet;
	private float m_RightOffSet;
	
	public LeftDiagonalFuzzySet(float _PeakValue, float _LeftOffSet, float _RightOffSet) : base(_PeakValue)
	{
		m_PeakPoint = _PeakValue;
		m_RepresentativeValue = _PeakValue;
		
		m_LeftOffSet = _LeftOffSet;
		
		m_RightOffSet = _RightOffSet;
	}
	
	public override float CalculateDOM(float _PassValue)
	{
		if(_PassValue < (m_PeakPoint - m_LeftOffSet) || _PassValue > m_PeakPoint)
		{
			m_DOM = 0.0f;
			return m_DOM;
		}
		
		m_DOM = (_PassValue - (m_PeakPoint - m_LeftOffSet))/m_LeftOffSet;
		return m_DOM;
	
//		if(_PassValue < (m_PeakPoint - m_LeftOffSet) && _PassValue > (m_PeakPoint + m_RightOffSet))
//		{
//			m_DOM = 0.0f; 
//			return m_DOM;
//		}
//		
//		m_DOM = (_PassValue - (m_LeftOffSet - m_PeakPoint))/m_LeftOffSet;
////		m_DOM = _PassValue/m_RightOffSet;
//		return m_DOM;
	}
}
