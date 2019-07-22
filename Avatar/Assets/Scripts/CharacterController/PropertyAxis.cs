using UnityEngine;
using System.Collections;

public class PropertyAxis : MonoBehaviour {

	public float min, max, startValue;
	
    public float currentValue, amplitude;

	public bool debug;
	public float debugValue;

	public SC_TriggerHead masterHead;
	public SC_TriggerBras masterArms, masterSide;

	public KeyCode keyValueUp, keyValueDown, keyEndShortcut;
	public bool keyboardOverride;
	public float keyCheatValue;

	void Start ()
	{
		currentValue = startValue;
		amplitude = max-min;
		keyboardOverride = false;
	}
	
	void Update ()
	{
		if(!keyboardOverride)
		{
			if(masterHead != null) currentValue = masterHead.result;
			if(masterArms != null) currentValue = masterArms.resultSpread;
			if(masterSide != null) currentValue = masterSide.resultAngle / 15;

			if(currentValue > max) currentValue = max;
			if(currentValue < min) currentValue = min;
		}
		if(debug) currentValue = debugValue;

		return; // comment this for getting the cheatkeys back
        /*
		if(Input.GetKeyDown (keyValueUp)) currentValue += keyCheatValue;
		if(Input.GetKeyDown (keyValueDown)) currentValue -= keyCheatValue;
		if(Input.GetKeyDown (keyValueDown) || Input.GetKeyDown (keyValueUp)) keyboardOverride = true;
		if(Input.GetKeyDown (keyEndShortcut)) keyboardOverride = false;
        */
	}

    public void AdjustCurrentValue(float newCurrentValue)
    {
        currentValue = newCurrentValue;
    }
}