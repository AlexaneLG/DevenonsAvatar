using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShowQ : MonoBehaviour {

    // Use this for initialization

    public Quaternion q;


	// Update is called once per frame
	void Update () {
        q = transform.localRotation;
	}
}
