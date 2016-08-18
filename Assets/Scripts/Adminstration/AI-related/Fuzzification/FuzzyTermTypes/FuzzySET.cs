using UnityEngine;
using System.Collections;

public class FuzzySET : FuzzyTerm 
{
	private FuzzySet m_Set;
	private string m_SetName;
	public string Name {get{return m_SetName;}}
	
	public FuzzySET(string _SetName, ref FuzzySet _Set)
	{
		m_SetName = _SetName;
		m_Set = _Set;
	}
	
	public FuzzySET(FuzzySet _Set)
	{
		m_Set = _Set;
	}
	
	public override float GetDOM()
	{
		return m_Set.DOM;
	}
	
	public override void ClearDOM()
	{
		m_Set.ResetDOM();
	}
	
	public override void ORwithDOM(float _PassValue)
	{
		m_Set.ORwithDOM(_PassValue);
	}
}
