using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour {

    public Camera mainCamera;
    public Camera bimCamera;
    public GameObject modelCamera;

    private bool _switchCamera;

    public ChangeTimeScale timeScaler;

    public GameObject avatarSwitchButton;
    public GameObject infoButton;

    public RawImage cameraImage;
    public Texture cameraFullImage;
    public Texture cameraLineImage;

    public GameObject cameraNavigationButtons;

	// Use this for initialization
	void Start () {
        _switchCamera = false;
        bimCamera.gameObject.SetActive(false);
        avatarSwitchButton.gameObject.SetActive(false);
        cameraNavigationButtons.SetActive(false);

        modelCamera.gameObject.SetActive(false);
        modelCamera.transform.position = mainCamera.transform.position;
        modelCamera.transform.localScale = new Vector3(5, 5, 5);
        modelCamera.transform.rotation = Quaternion.Euler(-90, 90, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _switchCamera = !_switchCamera;
            bimCamera.gameObject.SetActive(_switchCamera);
            modelCamera.gameObject.SetActive(_switchCamera);
            avatarSwitchButton.gameObject.SetActive(_switchCamera);

            infoButton.gameObject.SetActive(!_switchCamera);
            mainCamera.gameObject.SetActive(!_switchCamera);
        }
    }

    public void SwitchToExternalCamera()
    {
        _switchCamera = !_switchCamera;
        bimCamera.gameObject.SetActive(_switchCamera);
        modelCamera.gameObject.SetActive(_switchCamera);
        avatarSwitchButton.gameObject.SetActive(_switchCamera);
        cameraNavigationButtons.SetActive(_switchCamera);

        infoButton.gameObject.SetActive(!_switchCamera);
        mainCamera.gameObject.SetActive(!_switchCamera);

        if (_switchCamera)
        {
            cameraImage.texture = cameraFullImage;
        } else
        {
            cameraImage.texture = cameraLineImage;
        }

        if (bimCamera.isActiveAndEnabled)
        {
            timeScaler.pauseGame();
        } else
        {
            timeScaler.playGame();
        }
    }
}
