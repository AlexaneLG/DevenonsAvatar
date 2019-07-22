using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PropertyAxis))]
public class PropertyCouple : MonoBehaviour {

	public PropertyAxis input, output;
	public AnimationCurve modCurve;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		output.currentValue = modCurve.Evaluate(input.currentValue);
		//output.currentValue = input.currentValue * modCurve.Evaluate((input.currentValue-input.min)/input.amplitude);
	}
}
