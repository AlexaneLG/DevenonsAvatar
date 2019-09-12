using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIM_CameraNavigation : MonoBehaviour {

    public Transform Center;
    private Transform _gravityCenter;

    // Use this for initialization
    void Start()
    {
        _gravityCenter = new GameObject().transform;
        Debug.Log("Center pos y : " + Center.position.y);
        _gravityCenter.position = new Vector3(_gravityCenter.position.x, Center.position.y, _gravityCenter.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(_gravityCenter.position, Vector3.up, -0.5f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(_gravityCenter.position, Vector3.up, 0.5f);
        }
        transform.LookAt(Center);
    }
}
