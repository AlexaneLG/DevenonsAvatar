using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollisionManager : MonoBehaviour
{

    public GameObject controller;

    public CharacterControllerBasedOnAxis characterController;

    public SC_AvatarSwitch avatarSwitch;
    public CanyonProximityDataRecorder canyonProximityDataRecorder;
    public CollisionEventDataRecorder collisionEventDataRecorder;

    public Toggle Kinect;
    public Toggle Joystick;

    public Transform characterTransform;
    public GameObject canyonScenarioItem;

    public int avatarSwitchRef = 0;

    [HideInInspector]
    public float canyonDistance = -1;

    static public float shakeMagnitude = 20;
    static public float shakeRoughness = 5;
    static public float shakeFade = 2;

    private bool isCrashing;
    private bool enableLerpAfterCrash;

    public float crashMoveDistance;
    public float canyonHitDistance;

    private float lerpTime = 0.75f;
    private float currentLerpTime = 0;

    // private float initPosX, endPosX;

    private Transform initPos, endPos;

    private bool rightCanyonCollision;

    EZCameraShake.CameraShaker _shaker;

    void Start()
    {
        initPos = new GameObject().transform;
        endPos = new GameObject().transform;

        characterController.objectsToReplace.Add(initPos);
        characterController.objectsToReplace.Add(endPos);

        if (avatarSwitch == null)
        {
            _shaker = Camera.main.gameObject.AddComponent<EZCameraShake.CameraShaker>();
        }



    }

    void FixedUpdate()
    {
        if (SensorRecorderManager.instance != null)
        {
            if (SensorRecorderManager.startRecordingSensorData)
            {
                if (characterTransform != null && canyonScenarioItem.activeInHierarchy == true && !isCrashing)
                {
                    RaycastHit leftHit;
                    Ray leftRay = new Ray(characterTransform.position, transform.right * -1);

                    RaycastHit rightHit;
                    Ray rightRay = new Ray(characterTransform.position, transform.right);

                    if (Physics.Raycast(leftRay, out leftHit) && (Physics.Raycast(rightRay, out rightHit)))
                    {
                        Debug.DrawLine(characterTransform.position, leftHit.point, Color.red);
                        Debug.DrawLine(characterTransform.position, rightHit.point, Color.cyan);

                        if (leftHit.distance <= rightHit.distance && !isCrashing)
                        {
                            canyonDistance = Mathf.Round(leftHit.distance * 100f) / 100f;

                            if (canyonDistance < canyonHitDistance && canyonDistance >= 0 && !isCrashing)
                            {
                                rightCanyonCollision = false;
                                CanyonCrash(rightCanyonCollision);
                            }
                        }

                        else if (leftHit.distance > rightHit.distance && !isCrashing)
                        {
                            canyonDistance = Mathf.Round(rightHit.distance * 100f) / 100f;

                            if (canyonDistance < canyonHitDistance && canyonDistance >= 0)
                            {
                                rightCanyonCollision = true;
                                CanyonCrash(rightCanyonCollision);
                            }
                        }
                    }
                    else
                        canyonDistance = -1;
                }
                else
                    canyonDistance = -1;
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.K))
        {
            Shake();
        }

        if (enableLerpAfterCrash)
        {
            if (currentLerpTime > lerpTime)
                currentLerpTime = lerpTime;

            float t = currentLerpTime / lerpTime;
            //"ease out" diminution de la vitesse en fin de déplacement
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            //controller.transform.position = Vector3.Lerp(new Vector3(initPosX, controller.transform.position.y, controller.transform.position.z), new Vector3(endPosX, controller.transform.position.y, controller.transform.position.z), t);

            controller.transform.position = Vector3.Lerp(initPos.position, endPos.position, t);

            currentLerpTime += Time.deltaTime;

            if (Vector3.Distance(controller.transform.position, endPos.position) < 0.1f)
            {
                enableLerpAfterCrash = false;
                currentLerpTime = 0;
            }
        }
    }

    public void CanyonCrash(bool rightCanyonCrash)
    {
        DisplayCollisionLayer();

        isCrashing = true;

        collisionEventDataRecorder.collisionSensor = 1;

        //initPosX = controller.transform.position.x;
        initPos.position = controller.transform.position;

        Vector3 replaceDelta = controller.transform.TransformVector(new Vector3(crashMoveDistance / 2, 0f, 0f));

        if (rightCanyonCrash)
        {
            //endPosX = controller.transform.position.x - crashMoveDistance;
            endPos.position = controller.transform.position - replaceDelta;
        }
        else
        {
            //endPosX = controller.transform.position.x + crashMoveDistance;
            endPos.position = controller.transform.position + replaceDelta;
        }

        if (avatarSwitch)
        {
            avatarSwitch.SetAvatar(avatarSwitch.defaultAvatar);
        }
        else
        {
            // Shake Camera 
            Shake();
        }

        Kinect.isOn = false;
        Joystick.isOn = false;

        StartCoroutine("TakeControl");

        enableLerpAfterCrash = true;
    }

    public void MeteorCrash()
    {
        DisplayCollisionLayer();

        if (avatarSwitch)
        {
            avatarSwitch.SetAvatar(avatarSwitch.defaultAvatar);
        }
        else
        {
            Shake();
        }

        collisionEventDataRecorder.collisionSensor = 1;

        StartCoroutine("TakeControl");
    }


    public void Shake()
    {
        _shaker.ShakeOnce(shakeMagnitude, shakeRoughness, 0, shakeFade);

    }

    public IEnumerator TakeControl()
    {
        yield return new WaitForSeconds(0.75f);

        Kinect.isOn = true;
        Joystick.isOn = true;


        if (avatarSwitch)
        {
            avatarSwitchRef = avatarSwitchRef != avatarSwitch.currentAvatar ? avatarSwitch.currentAvatar : avatarSwitchRef;
            avatarSwitch.SetAvatar(avatarSwitchRef);
        }


        isCrashing = false;

        yield return null;
    }

    public void DisplayCollisionLayer()
    {
        if (GameObject.FindGameObjectWithTag("CollisionLayer") != null)
        {
            GameObject.FindGameObjectWithTag("CollisionLayer").GetComponent<CollisionLayer>().DisplayCollisionLayer();
        }
    }
}
