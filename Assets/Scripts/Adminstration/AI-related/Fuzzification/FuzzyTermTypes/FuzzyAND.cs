using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuzzyAND : FuzzyTerm 
{
	private List<FuzzyTerm> m_Terms;
	public List<FuzzyTerm> Terms{get{return m_Terms;} set{m_Terms = value;}}

	public FuzzyAND(FuzzyTerm _Term1, FuzzyTerm _Term2)
	{
		m_Terms = new List<FuzzyTerm>();
		m_Terms.Add(_Term1);
		m_Terms.Add(_Term2);
	}
	
	public FuzzyAND(FuzzyTerm _Term1, FuzzyTerm _Term2, FuzzyTerm _Term3)
	{
		m_Terms = new List<FuzzyTerm>();
		m_Terms.Add(_Term1);
		m_Terms.Add(_Term2);
		m_Terms.Add(_Term3);
	}
	
	public FuzzyAND(FuzzyTerm _Term1, FuzzyTerm _Term2, FuzzyTerm _Term3, FuzzyTerm _Term4)
	{
		m_Terms = new List<FuzzyTerm>();
		m_Terms.Add(_Term1);
		m_Terms.Add(_Term2);
		m_Terms.Add(_Term3);
		m_Terms.Add(_Term4);
	}
	
	public override float GetDOM()
	{
		float MinimumDOM = Mathf.Infinity;
		for(int i = 0; i < m_Terms.Count; i++)
		{
//			Debug.Log((m_Terms[i] as FuzzySET).Name + " DOM " + i + ": " + m_Terms[i].GetDOM());
			if(m_Terms[i].GetDOM() < MinimumDOM){MinimumDOM = m_Terms[i].GetDOM();}
		}
		
//		if(MinimumDOM != 0.0f)
//		{
//			if(m_Terms.Count == 3)	
//				Debug.Log("Set 1: " + (m_Terms[0]as FuzzySET).Name + " Set 2: " + (m_Terms[1] as FuzzySET).Name + " Set 3: " + (m_Terms[2] as FuzzySET).Name + " Minimum DOM: " + MinimumDOM);
//			else if(m_Terms.Count == 4)
//				Debug.Log("Set 1: " + (m_Terms[0]as FuzzySET).Name + " Set 2: " + (m_Terms[1] as FuzzySET).Name + " Set 3: " + (m_Terms[2] as FuzzySET).Name + " Set 4: " + (m_Terms[3] as FuzzySET).Name + " Minimum DOM: " + MinimumDOM);
//		}
		
		return MinimumDOM;
	}
	
	public override void ClearDOM()
	{
		for(int i = 0; i < m_Terms.Count; i++)
		{
			m_Terms[i].ClearDOM();
		}
	}
	
	public override void ORwithDOM(float _PassValue)
	{
		for(int i = 0; i < m_Terms.Count; i++)
		{
			m_Terms[i].ORwithDOM(_PassValue);
		}
	}
}
