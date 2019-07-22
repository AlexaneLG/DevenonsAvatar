using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonesPositionGetter : MonoBehaviour
{
	public List<Transform> kinectEmitter, avatarReceiver;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		for(int i=0; i<avatarReceiver.Count; i++)
		{

			if(i == 13)
			{
				Vector3 V3_EulerRotation = kinectEmitter[i].localEulerAngles;
				//if(V3_EulerRotation.x < 0) V3_EulerRotation.x += 360f;
				//V3_EulerRotation.x = 360f - V3_EulerRotation.x;

				//V3_EulerRotation += new Vector3(0, 180, 180);
				//if(V3_EulerRotation.y < 0) V3_EulerRotation.y += 360f;
				//if(V3_EulerRotation.z < 0) V3_EulerRotation.z += 360f;

				avatarReceiver[i].localEulerAngles = V3_EulerRotation;
			}
			else
				avatarReceiver[i].localRotation = kinectEmitter[i].localRotation;
		}
	}
}
