using UnityEngine;
using System.Collections;

public class SC_TriggerHead : MonoBehaviour 
{
	private bool b_Head;

	public float result;

	// Use this for initialization
	void Start () {
		result = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(b_Head)
		{
			//Debug.Log("Head Detected");
			result = 1f;
//			SC_CharacterController.b_IsGoingUp = true;
		}
		else
		{
			//Debug.Log("Head Not Detected");
			//result = 0f;
//			SC_CharacterController.b_IsGoingUp = false;
		}


	}

	void OnTriggerEnter(Collider other)
	{
		if(other.name == "02_Shoulder_Center")
			b_Head = true;
	}

	void OnTriggerExit(Collider other)
	{
		if(other.name == "02_Shoulder_Center")
			b_Head = false;
	}
}
