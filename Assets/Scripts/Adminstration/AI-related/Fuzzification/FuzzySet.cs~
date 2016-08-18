using UnityEngine;
using System.Collections;

public class FuzzySet 
{
	//Degree of membership for this set when given a specific value
	protected float m_DOM;
	public float DOM{get{return m_DOM;} set{m_DOM = value;}}
	//The highest value in the set A.K.A the peak point of the graph
	protected float m_RepresentativeValue;
	public float RepresentativeValue{get{return m_RepresentativeValue;} set{m_RepresentativeValue = value;}}
	
	public FuzzySet(float _RepresentativeValue)
	{
		m_DOM = 0.0f;
		m_RepresentativeValue = _RepresentativeValue;
	}
	
	//A virtual function that is referenced by the different types of fuzzy set to calculate their own DOM
	public virtual float CalculateDOM(float _PassValue){return 0.0f;}
	
	public void ResetDOM(){m_DOM = 0.0f;}
	
	public void SetDOM(float _PassValue){m_DOM = _PassValue;}
	
	public void ORwithDOM(float _PassValue)
	{
//		Debug.Log("pass: " + _PassValue + " original: " + m_DOM);
		m_DOM = _PassValue > m_DOM ? _PassValue : m_DOM;
	}
}
