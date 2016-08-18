using UnityEngine;
using System.Collections;

public class RightDiagonalFuzzySet : FuzzySet // " \ " shape
{
	private float m_PeakPoint;
	
	private float m_LeftOffSet;
	private float m_RightOffSet;
	
	public RightDiagonalFuzzySet(float _PeakValue, float _LeftOffSet, float _RightOffSet):base(_PeakValue)
	{
		m_PeakPoint = _PeakValue;
		m_RepresentativeValue = _PeakValue;
		
		m_LeftOffSet = _LeftOffSet;//left offset and peak value should be same and left offset should be 0 in x axis
		
		m_RightOffSet = _RightOffSet;
	}
	
	public override float CalculateDOM(float _PassValue)
	{
		if(_PassValue < (m_PeakPoint - m_LeftOffSet) || _PassValue > (m_PeakPoint + m_RightOffSet))
		{
			m_DOM = 0.0f; 
			return m_DOM;
		}
		
		m_DOM = 1.0f - ((_PassValue - m_PeakPoint)/m_RightOffSet);
		return m_DOM;
	}
}
