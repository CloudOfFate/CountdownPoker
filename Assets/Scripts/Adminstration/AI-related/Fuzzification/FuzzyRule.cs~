using UnityEngine;
using System.Collections;

public class FuzzyRule //AND operator takes the minimum of the two DOM && OR operator takes the maximum of the two DOM
{
	private FuzzyTerm m_Causes;
	public FuzzyTerm Causes {get{return m_Causes;} set{m_Causes = value;}}
	private FuzzyTerm m_Results;
	public FuzzyTerm Results {get{return m_Results;} set{m_Results = value;}}
	
	public FuzzyRule(FuzzyTerm _Causes, FuzzyTerm _Results)
	{
		m_Causes = _Causes;
		m_Results = _Results;
	}
	
	private void SetResultsDOMToZero()
	{
		m_Results.ClearDOM();
	}
	
	public void Calculate()
	{
		float causeDOm = m_Causes.GetDOM();
//		if(causeDOm != 0.0f){Debug.Log("Defuzzify desirability level: " + (m_Results as FuzzySET).Name);}
		m_Results.ORwithDOM(causeDOm);
	}
}
