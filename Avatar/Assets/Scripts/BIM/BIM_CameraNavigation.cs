using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIM_CameraNavigation : MonoBehaviour
{

    public Transform Center;
    private Transform _gravityCenter;
    private float orbitDistance = 5f;

    // Use this for initialization
    void Start()
    {
        _gravityCenter = new GameObject().transform;
        _gravityCenter.position = new Vector3(_gravityCenter.position.x, Center.position.y, _gravityCenter.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        /*_gravityCenter.position = new Vector3(_gravityCenter.position.x, Center.position.y, _gravityCenter.position.z);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(Center.position, Vector3.up, -0.5f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(Center.position, Vector3.up, 0.5f);
        }
        transform.LookAt(Center);*/
    }

    void LateUpdate()
    {
        //turnAround();
        transform.position = Center.position + (transform.position - Center.position).normalized * orbitDistance;
        transform.LookAt(Center);
    }

    private void turnAround()
    {
        transform.position = Center.position + (transform.position - Center.position).normalized * orbitDistance;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(Center.position, Vector3.up, -0.5f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(Center.position, Vector3.up, 0.5f);
        }
        //transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        transform.LookAt(Center);
    }

    public void TurnLeft()
    {
        transform.position = Center.position + (transform.position - Center.position).normalized * orbitDistance;
        transform.RotateAround(Center.position, Vector3.up, 0.5f);
        transform.LookAt(Center);
    }

    public void TurnRight()
    {
        transform.position = Center.position + (transform.position - Center.position).normalized * orbitDistance;
        transform.RotateAround(Center.position, Vector3.up, -0.5f);
        transform.LookAt(Center);
    }
}
