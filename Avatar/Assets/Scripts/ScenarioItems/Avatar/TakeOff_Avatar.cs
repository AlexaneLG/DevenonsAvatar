using UnityEngine;
using System.Collections;

public class TakeOff_Avatar : ScenarioItem
{

    public bool decollageAuto = false;

    public bool needsKinect = true;

    private bool decollageStarted = false;
    private float decollageInitalAltitude = 0f;

    public Transform handLeft, handRight, spine;

    // Use this for initialization
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {

        if (decollageAuto || decollageStarted)
        {

            base.Update();

            float d = decollageInitalAltitude;
            float u = controller.altitude;

            float py = Interpolate.EaseInOutQuad(d, u, timer, duration);

            Vector3 p = controller.transform.position;
            p.y = py;
            controller.transform.position = p;

            // if (!controller.avatar.GetComponent<Animation>().isPlaying)
            // {
            //     controller.avatar.GetComponent<Animation>().Play();
            // }
        }
        else
        {
            if (!needsKinect || KinectManager.Instance.GetUsersCount() > 0)
            {
                if (handLeft.transform.position.y > spine.transform.position.y && handRight.transform.position.y > spine.transform.position.y)
                {
                    decollageStarted = true;
                    decollageInitalAltitude = controller.transform.position.y;

                }
            }
        }
    }
}
