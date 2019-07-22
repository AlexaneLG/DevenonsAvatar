using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {
    public Transform ControllerCamera;


	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = ControllerCamera.rotation;

	
	}
}
