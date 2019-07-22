using UnityEngine;
using System.Collections;

public class SwitchMonoStereo : MonoBehaviour {

    public bool userGender;
    public GameObject cameraMono, cameraStereo, skyCamera_1, skyCamera_2;
    public Camera cameraRight, cameraLeft;

    private Vector3 _cameraRightInitialPosition, _cameraLeftInitialPosition;

    // Use this for initialization
    void Start()
    {
        _cameraRightInitialPosition = cameraRight.transform.position;
        _cameraLeftInitialPosition = cameraLeft.transform.position;

        cameraLeft.pixelRect = new Rect(0, 0, Screen.width / 2, Screen.height);
        float ratioLeft = (cameraLeft.pixelWidth) / cameraLeft.pixelHeight * 2;
        cameraLeft.aspect = ratioLeft;
        cameraLeft.rect = new Rect(0.5F, 0, 0.25F, 1);

        //Debug.Log(string.Format("Current res = {0}x{1}, ratio of {2}.", cameraLeft.pixelWidth, cameraLeft.pixelHeight, nRatio));

        cameraRight.pixelRect = new Rect(0, 0, Screen.width / 2, Screen.height);
        float ratioRight = (cameraRight.pixelWidth) / cameraRight.pixelHeight * 2;
        cameraRight.aspect = ratioRight;
        cameraRight.rect = new Rect(0.75F, 0, 0.25F, 1);

        Camera camSky_1 = skyCamera_1.GetComponent<Camera>();
        camSky_1.pixelRect = new Rect(0, 0, Screen.width / 2, Screen.height);
        float ratioCamSky_1 = (camSky_1.pixelWidth) / camSky_1.pixelHeight * 2;
        camSky_1.aspect = ratioCamSky_1;
        camSky_1.rect = new Rect(0.5F, 0, 0.5F, 1);

        Camera camSky_2 = skyCamera_2.GetComponent<Camera>();
        camSky_2.pixelRect = new Rect(0, 0, Screen.width / 2, Screen.height);
        float ratioCamSky_2 = (camSky_2.pixelWidth) / camSky_2.pixelHeight * 2;
        camSky_2.aspect = ratioCamSky_2;
        camSky_2.rect = new Rect(0.75F, 0F, 0.25F, 1F);

        SetCameraPosition(!userGender);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            cameraRight.transform.position = new Vector3(cameraRight.transform.position.x + 0.000025F, cameraRight.transform.position.y, cameraRight.transform.position.z);
            cameraLeft.transform.position = new Vector3(cameraLeft.transform.position.x - 0.000025F, cameraLeft.transform.position.y, cameraLeft.transform.position.z);
        }

        else if (Input.GetKey(KeyCode.KeypadMinus))
        {
            cameraRight.transform.position = new Vector3(cameraRight.transform.position.x - 0.000025F, cameraRight.transform.position.y, cameraRight.transform.position.z);
            cameraLeft.transform.position = new Vector3(cameraLeft.transform.position.x + 0.000025F, cameraLeft.transform.position.y, cameraLeft.transform.position.z);
        }
    }

    void Update()
    {
        
    }

    private void SetCameraPosition(bool userGender)
    {        
        if (userGender)
        {
            // Ecartement iter-pupillaire homme
            cameraRight.transform.position = new Vector3(_cameraRightInitialPosition.x + 0.0326F, cameraRight.transform.position.y, cameraRight.transform.position.z);
            cameraLeft.transform.position = new Vector3(_cameraLeftInitialPosition.x - 0.0326F, cameraLeft.transform.position.y, cameraLeft.transform.position.z);
        }

        else if (!userGender)
        {
            // Ecartement iter-pupillaire femme
            cameraRight.transform.position = new Vector3(_cameraRightInitialPosition.x + 0.0307F, cameraRight.transform.position.y, cameraRight.transform.position.z);
            cameraLeft.transform.position = new Vector3(_cameraLeftInitialPosition.x - 0.0307F, cameraLeft.transform.position.y, cameraLeft.transform.position.z);
        }
    }

    public void SwitchMonoStrereo(bool switchMonoStereo)
    {
        if (!switchMonoStereo && !cameraMono.activeInHierarchy)
        {
            cameraMono.SetActive(true);
            cameraStereo.SetActive(false);

            skyCamera_1.GetComponent<Camera>().rect = new Rect(0.5F, 0, 0.5F, 1);

            skyCamera_2.SetActive(false);
        }

        else if (switchMonoStereo && cameraMono.activeInHierarchy)
        {
            cameraMono.SetActive(false);
            cameraStereo.SetActive(true);

            skyCamera_1.GetComponent<Camera>().rect = new Rect(0.5F, 0F, 0.25F, 1F);

            skyCamera_2.SetActive(true);
        }
    }

    public void SetUserGender(bool userGender)
    {
        SetCameraPosition(userGender);
    }
}
