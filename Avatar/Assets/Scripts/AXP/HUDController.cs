using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    public DataManager dataManager;

    public Camera mainCamera;

    public GameObject altitudeText;
    public GameObject attitudeText;
    public GameObject speedText;
    //public GameObject distanceCameraAvatarText;
    public GameObject canyonWidthText;
    public GameObject bottomFOVText;
    public GameObject upFOVText;

    public GameObject attitudeImage;
    public GameObject compassImage;

    private float _attitudeValue;
    private float _altitudeValue;
    private float _speedValue;
    private float _rotationValue;
    //private float _distanceCameraAvatarValue;
    private float _canyonWidthValue;
    private float _bottomFOVValue = 0;
    private float _upFOVValue = 0;

    private Vector3 _previousPosition;
    private float _previousTime;

    // Use this for initialization
    void Start () {
        // Get Data Manager
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        // Set main camera
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        // Set attitude
        if (attitudeText == null)
        {
            attitudeText = GameObject.FindGameObjectWithTag("Attitude");
        }
        if (attitudeImage == null)
        {
            attitudeImage = GameObject.FindGameObjectWithTag("AttitudeNeedle");
        }
        // Set altitude
        if (altitudeText == null)
        {
            altitudeText = GameObject.FindGameObjectWithTag("Altitude");
        }
        // Set speed
        if (speedText == null)
        {
            speedText = GameObject.FindGameObjectWithTag("Speed");
        }
        // Set rotation
        if (compassImage == null)
        {
            compassImage = GameObject.FindGameObjectWithTag("CompassNeedle");
        }
        // Set distance
        /*if (distanceCameraAvatarText == null)
        {
            distanceCameraAvatarText = GameObject.FindGameObjectWithTag("DistanceCameraAvatar");
            _distanceCameraAvatarValue = GetDistanceCameraAvatar();
            distanceCameraAvatarText.GetComponent<Text>().text = _distanceCameraAvatarValue.ToString(); // HERE
        }*/
        // Set camera ray
        if (canyonWidthText == null)
        {
            canyonWidthText = GameObject.FindGameObjectWithTag("CanyonWidth");
            _canyonWidthValue = 0;
        }
        // Set fov
        if(bottomFOVText == null)
        {
            bottomFOVText = GameObject.FindGameObjectWithTag("BottomFOV");
        }
        if (upFOVText == null)
        {
            upFOVText = GameObject.FindGameObjectWithTag("UpFOV");
        }
        // Set previous position
        _previousPosition = Vector3.zero;
        _previousTime = 0f;

        StartCoroutine(DisplayDatas());
}

    // Update is called once per frame
    void Update () {
        // Update attitude
        _attitudeValue = Mathf.Round(dataManager.Attitude * 30);
        attitudeText.GetComponent<Text>().text = _attitudeValue.ToString();
        attitudeImage.GetComponent<RectTransform>().rotation = Quaternion.Euler(
            attitudeImage.transform.rotation.x,
            attitudeImage.transform.rotation.y,
            _attitudeValue * (-1));
        // Update altitude
        _altitudeValue = Mathf.Round(dataManager.Altitude);
        altitudeText.GetComponent<Text>().text = _altitudeValue.ToString();
        // Update speed
        //_speedValue = Mathf.Round(dataManager.Speed);
        //_speedValue = GetAvatarSpeed();
        //speedText.GetComponent<Text>().text = _speedValue.ToString();
        // Update rotation
        _rotationValue = dataManager.LocalRotation;
        compassImage.GetComponent<RectTransform>().rotation = Quaternion.Euler(
            compassImage.transform.rotation.x,
            compassImage.transform.rotation.y,
            _rotationValue);
        // Update distance
        /*_distanceCameraAvatarValue = GetDistanceCameraAvatar();
        distanceCameraAvatarText.GetComponent<Text>().text = _distanceCameraAvatarValue.ToString(); // HERE*/
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Update distance in canyon
            DisplayCanyonWidth();
            // Update FOV
            DisplayFieldOfVision();
        }
            
    }

    /*public float GetDistanceCameraAvatar()
    {
        return Vector3.Distance(mainCamera.transform.position, GameObject.FindGameObjectWithTag("Avatars").transform.position);
    }*/

    public void DisplayCanyonWidth()
    {
        LayerMask mask = LayerMask.GetMask("Environment");
        RaycastHit rightHit;
        RaycastHit leftHit;
        float rightDistance = 0;
        float leftDistance = 0;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.right, out rightHit, Mathf.Infinity, mask)
            && Physics.Raycast(mainCamera.transform.position, -mainCamera.transform.right, out leftHit, Mathf.Infinity, mask))
        {
            if (rightHit.collider.gameObject.tag == "Canyon"
                && leftHit.collider.gameObject.tag == "Canyon")
            {
                rightDistance = Mathf.Round(rightHit.distance);
                leftDistance = Mathf.Round(leftHit.distance);
                _canyonWidthValue = rightDistance + leftDistance;
                canyonWidthText.GetComponent<Text>().text = _canyonWidthValue.ToString();
            }
        } else
        {
            canyonWidthText.GetComponent<Text>().text = "-";
        }
    }

    public void DisplayFieldOfVision()
    {
        float offsetX = 100;
        float offsetY = 55;
        float offsetZ = 100;
        float offsetUp = -25;

        Quaternion cameraRotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles);

        Vector3 bottomRight = cameraRotation * new Vector3(offsetX, -offsetY, offsetZ).normalized;
        Vector3 bottomLeft = cameraRotation * new Vector3(-offsetX, -offsetY, offsetZ).normalized;
        Vector3 upRight = cameraRotation * new Vector3(offsetX, offsetY + offsetUp, offsetZ).normalized;
        Vector3 upLeft = cameraRotation * new Vector3(-offsetX, offsetY + offsetUp, offsetZ).normalized;

        /*Debug.DrawRay(mainCamera.transform.position, bottomRight, Color.red);
        Debug.DrawRay(mainCamera.transform.position, bottomLeft, Color.green);
        Debug.DrawRay(mainCamera.transform.position, upRight, Color.blue);
        Debug.DrawRay(mainCamera.transform.position, upLeft, Color.yellow);*/

        LayerMask mask = LayerMask.GetMask("Environment");

        RaycastHit bottomRightHit;
        RaycastHit bottomLeftHit;
        RaycastHit upRightHit;
        RaycastHit upLeftHit;

        // raycasts hit a terrain clone so have to check the parent tag

        // bottom fov
        if (Physics.Raycast(mainCamera.transform.position, bottomRight, out bottomRightHit, Mathf.Infinity, mask)
            && Physics.Raycast(mainCamera.transform.position, bottomLeft, out bottomLeftHit, Mathf.Infinity, mask))
        {
            Debug.Log("Nom parent : " + bottomRightHit.collider.gameObject.transform.parent.gameObject.name);
            if (bottomRightHit.collider.gameObject.transform.parent.gameObject.tag == "Terrain"
                && bottomLeftHit.collider.gameObject.transform.parent.gameObject.tag == "Terrain")
            {
                _bottomFOVValue = Mathf.Round(bottomRightHit.distance + bottomLeftHit.distance);
                bottomFOVText.GetComponent<Text>().text = _bottomFOVValue.ToString();
            }
            Debug.Log("Bottom raycast");
        }
        else
        {
            bottomFOVText.GetComponent<Text>().text = "-";
        }

        // up fov
        if (Physics.Raycast(mainCamera.transform.position, upRight, out upRightHit, Mathf.Infinity, mask)
            && Physics.Raycast(mainCamera.transform.position, upLeft, out upLeftHit, Mathf.Infinity, mask))
        {
            if (upRightHit.collider.gameObject.transform.parent.gameObject.tag == "Terrain"
                && upLeftHit.collider.gameObject.transform.parent.gameObject.tag == "Terrain")
            {
                _upFOVValue = Mathf.Round(upRightHit.distance + upLeftHit.distance);
                upFOVText.GetComponent<Text>().text = _upFOVValue.ToString();
            }
            Debug.Log("Up raycast");
        }
        else
        {
            upFOVText.GetComponent<Text>().text = "-";
        }
    }

    private IEnumerator DisplayDatas()
    {
        while (true)
        {
            // Display avatar's speed
            _speedValue = GetAvatarSpeed();
            speedText.GetComponent<Text>().text = _speedValue.ToString();
            yield return new WaitForSeconds(0.4f);
        }
    }

    public float GetAvatarSpeed()
    {
        if (dataManager != null && dataManager.characterController != null)
        {
            Vector3 currentPosition = dataManager.characterController.transform.position;
            float deltaDistance = Vector3.Distance(_previousPosition, currentPosition);
            float deltaTime = Time.time - _previousTime;
            _previousPosition = currentPosition;
            _previousTime = Time.time;
            float speed = Mathf.Round(deltaDistance / deltaTime);
            return speed;
        } else
        {
            return 0f;
        }
    }
}
