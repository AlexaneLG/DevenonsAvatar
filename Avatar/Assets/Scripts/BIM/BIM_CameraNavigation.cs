using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIM_CameraNavigation : MonoBehaviour
{

    public Transform Center;
    private Transform _gravityCenter;
    private float orbitDistance = 5f;
    private float _maxOrbitDistance = 10f;
    private float _minOrbitDistance = 3f;

    private bool _turningLeft = false;
    private bool _turningRight = false;
    private bool _zoomingIn = false;
    private bool _zoomingOut = false;

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

        if (_turningRight)
        {
            transform.RotateAround(Center.position, Vector3.up, -0.5f);
        }

        if (_turningLeft)
        {
            transform.RotateAround(Center.position, Vector3.up, 0.5f);
        }

        if (_zoomingIn)
        {
            if (orbitDistance > _minOrbitDistance)
            {
                orbitDistance -= 0.25f;
            }
        }

        if (_zoomingOut)
        {
            if (orbitDistance < _maxOrbitDistance)
            {
                orbitDistance += 0.25f;
            }
        }

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
        _turningLeft = true;
    }

    public void StopTurningLeft()
    {
        _turningLeft = false;
    }

    public void TurnRight()
    {
        _turningRight = true;
    }

    public void StopTurningRight()
    {
        _turningRight = false;
    }

    public void ZoomIn()
    {
        _zoomingIn = true;
    }

    public void StopZoomIn()
    {
        _zoomingIn = false;
    }

    public void ZoomOut()
    {
        _zoomingOut = true;
    }

    public void StopZoomOut()
    {
        _zoomingOut = false;

    }
}
