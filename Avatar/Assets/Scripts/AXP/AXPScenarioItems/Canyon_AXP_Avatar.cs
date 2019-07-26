﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canyon_AXP_Avatar : AugmentedScenarioItem
{

    private Transform _canyon;
    private GameObject _canyonMesh;
    private float _canyonHeight;
    private Canyon_Avatar _canyonAvatarComponent;
    private float _distanceCameraAvatarValue;

    public GameObject distanceCameraAvatarText;
    public Camera mainCamera;
    public GameObject canyonTracker;
    public GameObject panel;
    public Transform canyonHeightUI;

    public UnityEngine.UI.Extensions.UILineRenderer LineRenderer;

    protected override void Awake()
    {
        //durationIncr = 4;

        // Get gameobjects tagged canyon
        GameObject[] canyons = GameObject.FindGameObjectsWithTag("Canyon");
        foreach (GameObject canyon in canyons)
        {
            if (canyon.GetComponent<MeshCollider>() != null)
            {
                _canyonMesh = canyon;
            }
            else
            {
                _canyon = canyon.transform;
            }
        }

        if (GameObject.Find("CanyonHeight") != null)
        {
            canyonHeightUI = GameObject.Find("CanyonHeight").transform;
            canyonHeightUI.gameObject.SetActive(false);
        }

        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        // Hide childs
        for (int i = 0; i < transform.childCount; ++i)
        {
            displayChild(i, false);
        }

        // Set main camera
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        // Get gameobjects tagged canyon
        /*GameObject[] canyons = GameObject.FindGameObjectsWithTag("Canyon");

        if (canyons.Length > 1)
        {
            _canyon = canyons[0].transform.position.y == 0 ? canyons[0].transform : canyons[1].transform;
        }
        else
        {
            _canyon = canyons[0].transform;
        }*/

        _canyonAvatarComponent = _canyon.parent.GetComponent<Canyon_Avatar>();
        _canyonHeight = dataManager.scaleData(_canyonMesh.GetComponent<MeshCollider>().bounds.size.y);
        dataManager.CanyonHeight = _canyonHeight;
        Debug.Log("Canyon height : " + _canyonHeight);


        AddNewPoint();


        base.Start();


    }

    // Update is called once per frame
    protected override void Update()
    {


        base.Update();
    }

    public override IEnumerator DisplayScenarioItem()
    {
        // while canyon is not visible
        while (_canyon.position.y < -1400)
        {
            yield return null;
        }

        displayChild(0, true); // display "appearance" bubble
        canyonTracker = GameObject.FindGameObjectWithTag("CanyonTracker");
        //yield return new WaitForSeconds(4);
        yield return StartCoroutine(TrackCanyon());
        displayChild(0, false);

        displayChild(2, true); // display "constraint" bubble
        yield return new WaitForSeconds(4);
        displayChild(2, false);

        StartCoroutine(DisplayCanyonHeight());

        yield return StartCoroutine(DisplayCameraAvatarDistance());
    }

    /// <summary>
    /// Display the distance between the camera and the avatar for few seconds.
    /// See TakeControl() in Canyon_Avatar
    /// </summary>
    public IEnumerator DisplayCameraAvatarDistance()
    {
        while (_canyonAvatarComponent.cropping == false)
        {
            yield return null;
        }

        // it takes 30 seconds to crop the image
        displayChild(1, true);
        initDistanceCameraAvatar();
        // update distance
        yield return updateDistanceCameraAvatar();
        displayChild(1, false);

        while (_canyonAvatarComponent.cropping)
        {
            yield return null;
        }
        Debug.Log("Cropping is false");

        // the image is cropped 20 seconds
        displayChild(1, true);
        // update distance
        yield return updateDistanceCameraAvatar();
        displayChild(1, false);

    }

    public float GetDistanceCameraAvatar()
    {
        return Vector3.Distance(mainCamera.transform.position, GameObject.FindGameObjectWithTag("Avatars").transform.position);
    }

    private void initDistanceCameraAvatar()
    {
        // Set distance
        if (distanceCameraAvatarText == null)
        {
            distanceCameraAvatarText = GameObject.FindGameObjectWithTag("DistanceCameraAvatar");
            _distanceCameraAvatarValue = GetDistanceCameraAvatar();
            distanceCameraAvatarText.GetComponent<Text>().text = _distanceCameraAvatarValue.ToString();
        }
    }

    private IEnumerator updateDistanceCameraAvatar()
    {
        // Create arrow
        DistanceArrow arrow = new DistanceArrow();
        Debug.Log("Distance cam-ava : " + _distanceCameraAvatarValue);
        arrow.Init();
        arrow.SetDistance(_distanceCameraAvatarValue);
        arrow.SetPositionReference(mainCamera.transform);
        arrow.SetRightHit(mainCamera.transform);
        arrow.SetDotsPosition();

        // Update distance
        int seconds = 7;
        int frames = (int)(1.0f / Time.smoothDeltaTime) * seconds; // how many frames
        do
        {
            _distanceCameraAvatarValue = GetDistanceCameraAvatar();
            distanceCameraAvatarText.GetComponent<Text>().text = dataManager.changeFloatFormatOneDecimal(dataManager.scaleData(_distanceCameraAvatarValue));
            --frames;
            yield return null;
        } while (frames > 0);
    }

    private void displayChild(int index, bool display)
    {
        transform.GetChild(index).gameObject.SetActive(display);
    }

    private IEnumerator TrackCanyon()
    {
        // Set Panel
        if (panel == null)
        {
            panel = GameObject.FindGameObjectWithTag("TrackingPanel");
        }
        float width = panel.GetComponent<RectTransform>().rect.width;

        int frame = 0;
        int frames = 60 * 3; // 3 seconds
        float offsetY = 0;
        float maxY = 950;
        float minX = 550;
        float maxX = 1380;
        Transform canyonRef = GameObject.Find("CanyonScreenPos").transform;
        if (canyonRef == null)
        {
            canyonRef = _canyon.transform;
        }

        while (frame < frames)
        {
            ++frame;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(canyonRef.position);
            float posX = canyonTracker.GetComponent<RectTransform>().anchoredPosition.x;
            float posY = canyonTracker.GetComponent<RectTransform>().anchoredPosition.y;
            //Debug.Log(canyonRef.name + " screenPos.y : " + screenPos.y);
            if (screenPos.y < maxY)
            {
                posY = screenPos.y;
            }
            if (screenPos.x - width > minX && screenPos.x - width < maxX)
            {
                posX = screenPos.x - width;
            }

            canyonTracker.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
            yield return null;
        }
    }

    private IEnumerator DisplayCanyonHeight()
    {
        canyonHeightUI.gameObject.SetActive(true);

        if (GameObject.FindGameObjectWithTag("CanyonHeight") != null)
        {
            GameObject.FindGameObjectWithTag("CanyonHeight").GetComponent<Text>().text = dataManager.changeFloatFormat(_canyonHeight);
        }

        Vector3 screenOffset = new Vector3(2880, 1000, 0);

        int frame = 0;
        int frames = 60 * 20; // 20 seconds

        while (frame < frames)
        {
            ++frame;
            canyonHeightUI.LookAt(mainCamera.transform);

            Vector3 worldScreenOffset = mainCamera.ScreenToWorldPoint(screenOffset);
            canyonHeightUI.position = new Vector3(canyonHeightUI.position.x, worldScreenOffset.y, canyonHeightUI.position.z);

            yield return null;
        }

        canyonHeightUI.gameObject.SetActive(false);
    }

    public void AddNewPoint()
    {
        var point = new Vector2(1, 1);
        var pointlist = new List<Vector2>(LineRenderer.Points);
        pointlist.Add(point);
        LineRenderer.Points = pointlist.ToArray();
    }
}
