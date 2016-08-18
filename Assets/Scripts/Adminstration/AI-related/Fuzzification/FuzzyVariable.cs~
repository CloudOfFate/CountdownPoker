using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MemberSets = System.Collections.Generic.Dictionary<string,FuzzySet>;

public class FuzzyVariable 
{
	private FuzzyModule m_Module;
	public FuzzyModule Module{get{return m_Module;} set{m_Module = value;}}
	private MemberSets m_MemberSets;
	public MemberSets Sets{get{return m_MemberSets;} set{m_MemberSets = value;}}
	
	private float m_MinimumRange;
	private float m_MaximumRange;
	
	public FuzzyVariable(FuzzyModule _Module)
	{
		m_MinimumRange = 0.0f;
		m_MaximumRange = 0.0f;
		m_Module = _Module;
		m_MemberSets = new MemberSets();
	}
	
	private void AdjustRangeToFit(float _Min, float _Max)
	{
		if(_Min < m_MinimumRange) 
			m_MinimumRange = _Min;
		if(_Max > m_MaximumRange)
			m_MaximumRange = _Max;
//		if((m_MinimumRange == 0.0f) || (m_MinimumRange != 0.0f && _Min < m_MinimumRange)) 
//			m_MinimumRange = _Min;
//		if((m_MaximumRange == 0.0f) || (m_MaximumRange != 0.0f && _Max > m_MaximumRange))
//			m_MaximumRange = _Max;
	}
	
	public FuzzySET AddCentreTrapeziumSet(string _SetName, float _Peak1, float _Peak2, float _MinRange, float _MaxRange)
	{
		CentreTrapeziumFuzzySet Trapezium = new CentreTrapeziumFuzzySet(_Peak1,_Peak2,_MinRange,_MaxRange);
		m_MemberSets.Add(_SetName,Trapezium);
		AdjustRangeToFit(_MinRange,_MaxRange);
		return new FuzzySET(Trapezium);
	}
	
	public FuzzySET AddLeftShoulderSet(string _SetName, float _PeakPoint, float _MinRange, float _MaxRange)
	{
		LeftShoulderFuzzySet leftShoulder = new LeftShoulderFuzzySet(_PeakPoint, _PeakPoint - _MinRange, _MaxRange - _PeakPoint);
		m_MemberSets.Add(_SetName,leftShoulder);
		AdjustRangeToFit(_MinRange,_MaxRange);
		return new FuzzySET(leftShoulder);
	}
	
	public FuzzySET AddRightShoulderSet(string _SetName, float _PeakPoint, float _MinRange, float _MaxRange)
	{
		RightShoulderFuzzySet rightShoulder = new RightShoulderFuzzySet(_PeakPoint,_PeakPoint - _MinRange, _MaxRange - _PeakPoint);
		m_MemberSets.Add(_SetName,rightShoulder);
		AdjustRangeToFit(_MinRange,_MaxRange);
		return new FuzzySET(rightShoulder);
	}
	
	public FuzzySET AddLeftDiagonalSet(string _SetName, float _LeftPoint, float _RightPoint)
	{
		LeftDiagonalFuzzySet leftDiagonal = new LeftDiagonalFuzzySet(_RightPoint,_RightPoint - _LeftPoint,0.0f); 
		m_MemberSets.Add(_SetName,leftDiagonal);
		AdjustRangeToFit(_LeftPoint,_RightPoint);
		return new FuzzySET(leftDiagonal);
	}
	
	public FuzzySET AddRightDiagonalSet(string _SetName, float _LeftPoint, float _RightPoint)
	{
		RightDiagonalFuzzySet rightDiagonal = new RightDiagonalFuzzySet(_LeftPoint,0.0f,_RightPoint - _LeftPoint);
		m_MemberSets.Add(_SetName, rightDiagonal);
		AdjustRangeToFit(_LeftPoint,_RightPoint);
		return new FuzzySET(rightDiagonal);
	}
	
	public FuzzySET AddTriangularSet(string _SetName, float _PeakPoint, float _LeftPoint, float _RightPoint)
	{
		TriangularFuzzySet triangular = new TriangularFuzzySet(_PeakPoint,_PeakPoint - _LeftPoint,_RightPoint - _PeakPoint);
		m_MemberSets.Add(_SetName, triangular);
		AdjustRangeToFit(_LeftPoint,_RightPoint);
		return new FuzzySET(triangular);
	}
	
	public void Fuzzify(float _PassValue) // Calculate the DOM of all the sets in the variable and fuzzify the variable value
	{
//		Debug.Log("Pass value: " + _PassValue + " Min: " + m_MinimumRange + " Max: " + m_MaximumRange);
		if(_PassValue < m_MinimumRange || _PassValue > m_MaximumRange)
		{
			Debug.Log("Pass value is out of this variable's range: Current value: " + _PassValue + " Min value: " + m_MinimumRange + " Max value: " + m_MaximumRange); 
			return;
		}
		
		foreach(string name in m_MemberSets.Keys)
		{
			m_MemberSets[name].DOM = m_MemberSets[name].CalculateDOM(_PassValue);
//			Debug.Log("Set: " + name + " DOM: " + m_MemberSets[name].DOM);
		}
	}
	
	public float Defuzzify()// Defuzzifying using max average method
	{
		float SumOfRepresentativeValueNConfidence = 0.0f;
		float SumOfConfidence = 0.0f;
		
		foreach(string name in m_MemberSets.Keys)
		{
//			Debug.Log("Desirability: " + name + " Representative value: " + m_MemberSets[name].RepresentativeValue + " DOM: " + m_MemberSets[name].DOM);
			SumOfRepresentativeValueNConfidence += m_MemberSets[name].RepresentativeValue * m_MemberSets[name].DOM;
			SumOfConfidence += m_MemberSets[name].DOM;
		}
		
		if(SumOfConfidence <= 0.0f) 
		{
//			Debug.Log("Defuzzified Desirability: " + SumOfRepresentativeValueNConfidence + " / " + SumOfConfidence + " = " + 0.0f);
			return 0.0f;
		}
		
//		Debug.Log("Defuzzified Desirability: " + SumOfRepresentativeValueNConfidence + " / " + SumOfConfidence + " = " + SumOfRepresentativeValueNConfidence/SumOfConfidence);
		
		return SumOfRepresentativeValueNConfidence/SumOfConfidence;
	}
}
