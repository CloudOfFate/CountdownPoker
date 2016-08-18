using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using VariableMap = System.Collections.Generic.Dictionary<string,FuzzyVariable>;
using RuleList = System.Collections.Generic.List<FuzzyRule>;

public class FuzzyModule 
{
	private VariableMap m_Variables;
	private RuleList m_Rules;
	public VariableMap Variables{get{return m_Variables;} set{m_Variables = value;}}
	public RuleList Rules{get{return m_Rules;} set{m_Rules = value;}}
	
	public FuzzyModule()
	{
		m_Variables = new VariableMap();
		m_Rules = new RuleList();
	}
	
	public FuzzyVariable CreateFuzzyVariable(string _VariableName)
	{
		m_Variables.Add(_VariableName,new FuzzyVariable(this));
		return m_Variables[_VariableName];
	}
	
	public void AddRule(FuzzyTerm _Causes, FuzzyTerm _Consequences)
	{
		m_Rules.Add(new FuzzyRule(_Causes,_Consequences));
	}
	
	public void Fuzzify(string _VariableName, float _Value)
	//Fuzzify the variables using the input value 
	{
//		Debug.Log("VariableName: " + _VariableName + " Value: " + _Value);
		m_Variables[_VariableName].Fuzzify(_Value);
	}
	
	public float Defuzzify(string _VariableName)
	//Make use of the desirability calculated to form a single crisp value to represent the decision desirability
	{
		SetConfidencesOfConsequentsToZero();
		
		//Execute the If Then rules onto the fuzzy variables to obtain the desirability
//		Debug.Log("Avaliable amount of rules: " + m_Rules.Count);
		for(int i = 0; i < m_Rules.Count; i++)
		{
//			Debug.Log("Rule calculation");
			m_Rules[i].Calculate();
		}
		
		return m_Variables[_VariableName].Defuzzify();
	}
	
	private void SetConfidencesOfConsequentsToZero()
	{
		for(int i = 0; i < m_Rules.Count; i++)
		{
			m_Rules[i].Results.ClearDOM();
		}
	}
}
