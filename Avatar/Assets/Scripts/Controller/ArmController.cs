using UnityEngine;
using System.Collections;

public class ArmController : AvatarFlightController
{

    public Transform leftHand, rightHand, shouldreCenter;

    [HideInInspector]
    public float handRotationSpeed = 2f;
    public float handMagnitude, handDirection;

    public float altitudeFactor = 500f;
    private float tolerance = 0.2f;
    public float maxAltitude = 500f;
    public float minAltitude = 35f;

    public bool needsKinect = true;

    public override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        if (KinectManager.Instance != null || !needsKinect)
        {
            if (!needsKinect || KinectManager.Instance.GetUsersCount() > 0)
            {

                Vector3 pl = leftHand.position;
                Vector3 pr = rightHand.position;
                Vector3 pc = shouldreCenter.position;

                Vector3 d = pr - pl;
                handMagnitude = d.magnitude;
                d.Normalize();
                handDirection = -d.y;

                if (handMagnitude > 0.5f)
                {
                    controller.direction = Mathf.Lerp(controller.direction, handDirection * handRotationSpeed, Time.deltaTime);
                }

                if (CharacterControllerBasedOnAxis.currentScenarioItem > 3)
                {
                    if (pr.y > (pc.y + tolerance) && pl.y > (pc.y + tolerance))
                        controller.altitude = Mathf.Clamp(controller.altitude, minAltitude, maxAltitude) + altitudeFactor * Time.deltaTime;

                    else if (pr.y < (pc.y - tolerance) && pl.y < (pc.y - tolerance))
                        controller.altitude = Mathf.Clamp(controller.altitude, minAltitude, maxAltitude) - altitudeFactor * Time.deltaTime;
                }
            }
        }
    }

    public void adjustAltitudeFactor(float newAltitudeFactor)
    {
        altitudeFactor = newAltitudeFactor;
    }
}
