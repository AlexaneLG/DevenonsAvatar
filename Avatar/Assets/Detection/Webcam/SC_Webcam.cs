using UnityEngine;
using System.Collections;

public class SC_Webcam : MonoBehaviour {

	WebCamTexture webcamTexture;

	// Use this for initialization
	void Start () 
	{
		WebCamDevice[] devices = WebCamTexture.devices;
		webcamTexture = new WebCamTexture();
		webcamTexture.deviceName = devices[0].name;
		//webcamTexture.deviceName = "Logitech HD Pro Webcam C920";
        webcamTexture.deviceName = devices[0].name;
		Debug.Log( devices[0].name );
		webcamTexture.requestedWidth = 1280;
		webcamTexture.requestedHeight = 720;

		GetComponent<Renderer>().material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}

}
