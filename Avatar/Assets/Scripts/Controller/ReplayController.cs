using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayController : AvatarFlightController
{

    // take replayers datas 
    // then apply the transform positions of the hands and the neck

    public BIM_KinectDataReplayer kinectDataReplayer;
    public BIM_ScenarioItemDataReplayer scenarioItemDataReplayer;
    public BIM_TimeDataReplayer timeDataReplayer;
    private int _deltaT;

    public TakeOff_Avatar takeOffItem;

    // Arm Controller attributs

    [HideInInspector]
    public float handRotationSpeed = 2f;
    public float handMagnitude, handDirection;

    public float altitudeFactor = 500f;
    private float tolerance = 0.2f;
    public float maxAltitude = 500f;
    public float minAltitude = 35f;

    // Use this for initialization
    public override void Start () {

        // Find replayers

        if (kinectDataReplayer == null && GameObject.Find("KinectDataReplayer"))
        {
            kinectDataReplayer = GameObject.Find("KinectDataReplayer").GetComponent<BIM_KinectDataReplayer>();
        } else
        {
            Debug.Log("Can not find KinectDataReplayer");
        }


        if (scenarioItemDataReplayer == null && GameObject.Find("ScenarioItemDataReplayer"))
        {
            scenarioItemDataReplayer = GameObject.Find("ScenarioItemDataReplayer").GetComponent<BIM_ScenarioItemDataReplayer>();
        }
        else
        {
            Debug.Log("Can not find ScenarioItemDataReplayer");
        }

        if (timeDataReplayer == null && GameObject.Find("TimeDataReplayer"))
        {
            timeDataReplayer = GameObject.Find("TimeDataReplayer").GetComponent<BIM_TimeDataReplayer>();
        }
        else
        {
            Debug.Log("Can not find TimeDataReplayer");
        }

        _deltaT = 0;

        base.Start();
    }
	
	// Update is called once per frame
	void Update () {

        if (scenarioItemDataReplayer.currentScenarioItem == 2)
        {
            takeOffItem.decollageAuto = true;
        }

        Vector2 pl = new Vector2(kinectDataReplayer.hand_Left_X.values[_deltaT], kinectDataReplayer.hand_Left_Y.values[_deltaT]);
        Vector2 pr = new Vector2(kinectDataReplayer.hand_Right_X.values[_deltaT], kinectDataReplayer.hand_Right_Y.values[_deltaT]);
        Vector2 pc = new Vector2(kinectDataReplayer.shoulder_center_X.values[_deltaT], kinectDataReplayer.shoulder_center_Y.values[_deltaT]);

        Vector2 d = pr - pl;
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

        ++_deltaT;

    }

    public void adjustAltitudeFactor(float newAltitudeFactor)
    {
        altitudeFactor = newAltitudeFactor;
    }
}
