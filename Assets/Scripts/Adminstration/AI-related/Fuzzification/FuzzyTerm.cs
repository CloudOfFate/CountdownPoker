using UnityEngine;
using System.Collections;

public class FuzzyTerm 
{
	public virtual float GetDOM(){return 0.0f;}
	public virtual void ClearDOM(){}
	public virtual void ORwithDOM(float _PassValue){}
}
