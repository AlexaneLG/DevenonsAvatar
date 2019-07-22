using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_TriggerPositions : MonoBehaviour 
{
	public List<string> s_Bones;
	private List<bool> b_Bones;
	private bool b_IsIn;


	// Use this for initialization
	void Start () 
	{

		b_Bones = new List<bool>();
		for(int i = 0; i< s_Bones.Count; i++)
		{
			b_Bones.Add(false);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i =0; i< b_Bones.Count; i++)
		{
			if(!b_Bones[i])
			{
				b_IsIn = false;
				break;
			}
			b_IsIn = true;
		}

		if(b_IsIn)
		{
			Debug.Log("Head Detected");
//			SC_CharacterController.b_IsGoingUp = true;
		}
		else
		{
			Debug.Log("Head Not Detected");
//			SC_CharacterController.b_IsGoingUp = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		for(int i =0; i< s_Bones.Count; i++)
		{
			if(other.name == s_Bones[i])
			{
				b_Bones[i] = true;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		for(int i =0; i< s_Bones.Count; i++)
		{
			if(other.name == s_Bones[i])
			{
				b_Bones[i] = false;
			}
		}
	}
}
