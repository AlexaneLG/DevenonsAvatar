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

    public CubemanController cubeMan;
    public AvatarControllerClassic refAvatar;

    // Arm Controller attributs

    [HideInInspector]
    public float handRotationSpeed = 2f;
    public float handMagnitude, handDirection;

    public float altitudeFactor = 500f;
    private float tolerance = 0.2f;
    public float maxAltitude = 500f;
    public float minAltitude = 35f;

    public string csvPath;

    public Vector3 recordedHandRight;
    public Vector3 recordedHandLeft;
    public Vector3 recordedShoulderCenter;

    // Use this for initialization
    public override void Start()
    {

        // Find replayers

        if (kinectDataReplayer == null && GameObject.Find("KinectDataReplayer"))
        {
            kinectDataReplayer = GameObject.Find("KinectDataReplayer").GetComponent<BIM_KinectDataReplayer>();
        }

        if (scenarioItemDataReplayer == null && GameObject.Find("ScenarioItemDataReplayer"))
        {
            scenarioItemDataReplayer = GameObject.Find("ScenarioItemDataReplayer").GetComponent<BIM_ScenarioItemDataReplayer>();
        }

        if (timeDataReplayer == null && GameObject.Find("TimeDataReplayer"))
        {
            timeDataReplayer = GameObject.Find("TimeDataReplayer").GetComponent<BIM_TimeDataReplayer>();
        }

        _deltaT = 0;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("CSV : Scenario item n°" + scenarioItemDataReplayer.currentScenarioItem);

        recordedHandLeft = new Vector3(kinectDataReplayer.hand_Left_X.values[_deltaT], kinectDataReplayer.hand_Left_Y.values[_deltaT], kinectDataReplayer.hand_Left_Z.values[_deltaT]);
        recordedHandRight = new Vector3(kinectDataReplayer.hand_Right_X.values[_deltaT], kinectDataReplayer.hand_Right_Y.values[_deltaT], kinectDataReplayer.hand_Right_Z.values[_deltaT]);
        recordedShoulderCenter = new Vector3(kinectDataReplayer.shoulder_center_X.values[_deltaT], kinectDataReplayer.shoulder_center_Y.values[_deltaT], kinectDataReplayer.shoulder_center_Z.values[_deltaT]);

        cubeMan.Hand_Left.transform.position = recordedHandLeft;
        cubeMan.Hand_Right.transform.position = recordedHandRight;
        cubeMan.Neck.transform.position = recordedShoulderCenter;

        /*refAvatar.HandLeft.transform.position = recordedHandLeft;
        refAvatar.HandRight.transform.position = recordedHandRight;
        refAvatar.Neck.transform.position = recordedShoulderCenter;*/

        Vector2 pl = cubeMan.Hand_Left.transform.position;
        Vector2 pr = cubeMan.Hand_Right.transform.position;
        Vector2 pc = cubeMan.Neck.transform.position;

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

        if (scenarioItemDataReplayer.currentScenarioItem > -1)
        {
            //++_deltaT;
            _deltaT += 5;
        }
    }

    public void adjustAltitudeFactor(float newAltitudeFactor)
    {
        altitudeFactor = newAltitudeFactor;
    }
}
