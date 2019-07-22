using UnityEngine;
using System.Collections;

using DG.Tweening;

public class SpaceController : MonoBehaviour
{
    public Navigation_Space_INT pathScript;

    public Transform pointManHead;
    [Range(0, 1)]
    public float rollingEffectStrength = 1.0f;

    [Header("WiiBB Control")]
    public bool useWii = true;
    public Transform root;
    public WiiBBSocket wii;

    public float MaxDistance = 1;

    public float speedDiv = 100.0f;

    public bool invert = true;

    public float multiplier = 1f;

    [Header("Kinect Control")]
    public bool useFlexion = true;
    private bool isFlexing = false;
    public float flexFactor = 0.985f;
    public float flexionSpeed = 1.2f;
    private Rigidbody myRigidbody;
    private Transform myTransform;
    private float initialHeadPosition;
    private float heightVariator;
    private bool userCalibration;



    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myTransform = transform;

    }

    void Update()
    {
        var newRot = transform.localEulerAngles;

        newRot.y = rollingEffectStrength * Mathf.Sin(Time.time * 1) * 7f;
        newRot.z = rollingEffectStrength * Mathf.Sin(Time.time * 1) * 7f;

        transform.localEulerAngles = newRot;

        if (useWii)
        {
            // Use Wii to move   
            float x = wii.X_CP * multiplier;

            var v = root.localPosition;
            float rx = v.x;
            rx += (invert ? -1f : 1f) * x / speedDiv;
            rx = Mathf.Clamp(rx, -MaxDistance, MaxDistance);

            v.x = Mathf.Lerp(v.x, rx, Time.deltaTime);

            root.localPosition = v;

            var delta = Mathf.Abs(v.x / MaxDistance);

            //            Debug.Log("Speed up ?: " + delta);

            if (delta > 0.9f)
            {
                pathScript.pathSpeed = flexionSpeed;
            }

        }

        if (KinectManager.Instance != null)
        {
            if (KinectManager.Instance.GetUsersCount() > 0)
            {
                if (!userCalibration)
                {
                    Invoke("setBodyInitialPosition", 2);
                    userCalibration = true;
                }

                heightVariator = pointManHead.position.y / initialHeadPosition;

                if (useFlexion)
                {
                    if (heightVariator < flexFactor && !isFlexing)
                    {
                        isFlexing = true;
                    }

                    if (heightVariator > flexFactor && isFlexing == true)
                    {
                        Debug.Log("Flex !!!");
                        pathScript.pathSpeed = flexionSpeed;
                        isFlexing = false;
                    }
                }
            }
        }
    }

    public void setBodyInitialPosition()
    {
        initialHeadPosition = pointManHead.position.y;
    }

    public void adjustFlexionSpeed(float newFlexionSpeed)
    {
        flexionSpeed = newFlexionSpeed;
    }
}
