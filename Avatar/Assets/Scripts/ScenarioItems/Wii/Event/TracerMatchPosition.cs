using UnityEngine;
using System.Collections;

public class TracerMatchPosition : MonoBehaviour {

	public Transform handTarget;

	// Update is called once per frame
	void Update () 
	{
		transform.position = handTarget.position;
	}
}
