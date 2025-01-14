﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public GameObject characterManager;
    public CharacterControllerBasedOnAxis characterController;
    public GameObject avatarsController;
    public CollisionManager collisionManager;
    public GameObject cubeMan;
    public GameObject HUD;

    private ScenarioItem _currentScenarioItem;
    private int _currentScenarioItemIdx;
    public int numberOfScenarioItems;
    public bool isKinectActive;

    private float _altitude;
    private float _startAltitude;
    private float _attitude; // assiette
    private float _speed;
    private float _rotation;
    private float _avatarHeight;
    private List<float> _heightSamples = new List<float>();
    private int _i;


    public float Altitude
    {
        get
        {
            return _altitude;
        }

        set
        {
            _altitude = value;
        }
    }

    public float Attitude
    {
        get
        {
            return _attitude;
        }

        set
        {
            _attitude = value;
        }
    }

    public float Speed
    {
        get
        {
            return _speed;
        }

        set
        {
            _speed = value;
        }
    }

    public float LocalRotation
    {
        get
        {
            return _rotation;
        }

        set
        {
            _rotation = value;
        }
    }

    public float AvatarHeight
    {
        get
        {
            return _avatarHeight;
        }

        set
        {
            _avatarHeight = value;
        }
    }

    public ScenarioItem CurrentScenarioItem
    {
        get
        {
            return _currentScenarioItem;
        }
    }

    public int CurrentScenarioItemIdx
    {
        get
        {
            return _currentScenarioItemIdx;
        }
    }

    public List<float> HeightSamples
    {
        get
        {
            return _heightSamples;
        }

        set
        {
            _heightSamples = value;
        }
    }

    private void Awake()
    {
        isKinectActive = (KinectManager.Instance && KinectManager.Instance.GetUsersCount() > 0) ? true : false;
    }

    // Use this for initialization
    public virtual void Start () {
        if (characterManager == null)
        {
            characterManager = GameObject.FindGameObjectWithTag("CharacterManager");
        }
        if (characterManager != null)
        {
            characterController = characterManager.GetComponent<CharacterControllerBasedOnAxis>();
            // Set datas
            _speed = characterController.speed;
            _attitude = characterController.direction;
            _startAltitude = characterController.transform.position.y;
            Debug.Log("Start altitude : " + _startAltitude);
            _altitude = 0;
            _rotation = characterController.transform.rotation.eulerAngles.y;
        }
        if (cubeMan == null)
        {
            cubeMan = GameObject.FindGameObjectWithTag("CubeMan");
        }
        numberOfScenarioItems = characterController.ScenarioItems.Length;
        _i = 0;
        if (avatarsController == null)
        {
            avatarsController = GameObject.FindGameObjectWithTag("Avatars");
        }
        if (collisionManager == null)
        {
            collisionManager = avatarsController.GetComponent<CollisionManager>();
        }
        if (HUD == null)
        {
            HUD = GameObject.FindGameObjectWithTag("HUD");
        }
        
    }

    // Update is called once per frame
    public virtual void Update () {
        _speed = characterController.speed;
        _attitude = characterController.direction;
        _altitude = (characterController.transform.position.y - _startAltitude);
        _rotation = characterController.transform.rotation.eulerAngles.y;

        /*if (Input.GetKeyDown(KeyCode.H))
        {
            if (_i < 10)
            {
                SetAvatarHeight();
                ++ _i;
            }
            if(_i > 5)
            {
                Debug.Log("Mean height : " + GetMean(_heightSamples));
                _i = 0;
            }
            
        }*/

        /*if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Constant speed : " + _speed + "; Velocity : " + Mathf.Round(Vector3.Magnitude(avatarsController.GetComponent<Rigidbody>().velocity)));
        }*/
    }

    public void SetScenarioItemOnChange(ScenarioItem item, int index)
    {
        Debug.Log("___________________________________________________________ Current Scenario Item : " + item.name);
        _currentScenarioItem = item;
        _currentScenarioItemIdx = index;
        GameObject.FindGameObjectWithTag("ContextualLayer").GetComponent<ContextualUIController>().CanDisplay = true;
    }

    #region AvatarHeight

    public void SetAvatarHeight()
    {
        //cubeMan.GetComponent<CubemanController>();

        List<GameObject> joints = new List<GameObject>();
        joints.Add(cubeMan.GetComponent<CubemanController>().Head);
        joints.Add(cubeMan.GetComponent<CubemanController>().Neck);
        joints.Add(cubeMan.GetComponent<CubemanController>().Spine);
        joints.Add(cubeMan.GetComponent<CubemanController>().Hip_Center);
        joints.Add(cubeMan.GetComponent<CubemanController>().Hip_Left);
        joints.Add(cubeMan.GetComponent<CubemanController>().Knee_Left);
        joints.Add(cubeMan.GetComponent<CubemanController>().Ankle_Left);

        float height = Length(joints);
        _heightSamples.Add(height);
        //Debug.Log("Height : " + height);
    }

    public static float GetMean(List<float> list)
    {
        float sum = 0;

        foreach (float f in list)
        {
            sum += f;
        }
        return sum / list.Count;
    }

    public static float Length(GameObject p1, GameObject p2)
    {
        return Mathf.Sqrt(
            Mathf.Pow(p1.transform.position.x - p2.transform.position.x, 2) +
            Mathf.Pow(p1.transform.position.y - p2.transform.position.y, 2) +
            Mathf.Pow(p1.transform.position.z - p2.transform.position.z, 2));
    }

    public static float Length(List<GameObject> joints)
    {
        float length = 0;

        for (int index = 0; index < joints.Count - 1; index++)
        {
            length += Length(joints[index], joints[index + 1]);
        }

        return length;
    }

    public void DisplayHUD(bool display)
    {
        HUD.SetActive(display);
    }
    
    #endregion
}
