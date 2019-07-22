using UnityEngine;
using System.Collections;

public class ResetScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp (KeyCode.Keypad9)) Application.LoadLevel(Application.loadedLevel);
	}
}
