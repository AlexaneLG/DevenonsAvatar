﻿using UnityEngine;
using System.Collections;

public class SphereRotate : MonoBehaviour {

	public float rotateSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
	
	}
}
