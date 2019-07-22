using UnityEngine;
using System.Collections;

public class Canyon_Avatar : FreeFly_Avatar
{

    public Transform Canyon;
    public Transform FalaiseEnter;
    public Transform FalaiseExit;

    public iTweenPathLocal path;
    public Transform debugPath;

    public SwitchToFPS switchToFPS;

    public float spawnDistance = 10000.0f;
    public float spawnAltitude = -1000.0f;
    public float targetAltitude = -100.0f;

    [Range(0.0f, 1000.0f)]
    public float attraction;

    [Range(0.0f, 25.0f)]
    public float directionAttraction;

    [Range(0.0f, 1.0f)]
    public float pathProgression = 0f;

    [Range(0.0f, 0.2f)]
    public float advance = 0f;

    public float chuteDuration = 4f;
    //private float chuteTimer = 0f;

    public float elevationDuration = 6f;
    private float elevationTimer = 0f;

    public bool cropping = false;


    public override void OnEnable()
    {
        base.OnEnable();

        controller.objectsToReplace.Add(Canyon);

    }

    public override void OnDisable()
    {
        base.OnDisable();
        controller.objectsToReplace.Remove(Canyon);
    }


    // Use this for initialization
    override public void Start()
    {
        base.Start();

        Transform target = controller.avatar.transform;

        Vector3 pos = target.TransformPoint(new Vector3(0f, 0f, spawnDistance));
        pos.y = spawnAltitude;

        Vector3 lookPos = target.position;
        lookPos.y = spawnAltitude;
        Canyon.position = pos;
        Canyon.LookAt(lookPos);
        Canyon.Rotate(0, 180, 0);

        path.UpdateWorldNodes();
        FalaiseEnter.position = path.nodesWorld[0];
        FalaiseExit.position = path.nodesWorld[path.nodeCount - 1];

        //cruisingAltitude = controller.altitude;

        elevationTimer = 0f;

        StartCoroutine("TakeControl");
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        float p = 8f * progressionQuad;

        if (p < 1f)
        {

            Vector3 spos = Canyon.position;
            Vector3 tpos = spos;

            spos.y = spawnAltitude;
            tpos.y = targetAltitude;

            Vector3 pos = Vector3.Lerp(spos, tpos, p);
            Canyon.position = pos;
        }

        Transform ct = controller.transform;

        Vector3 entranceAvatarLocalPos = FalaiseEnter.InverseTransformPoint(ct.position);
        Vector3 entranceExitLocalPos = FalaiseEnter.InverseTransformPoint(FalaiseExit.position);

        Vector3 exitAvatarLocalPos = FalaiseExit.InverseTransformPoint(ct.position);

        ArmController arm = controller.GetComponent<ArmController>() as ArmController;

        if (entranceAvatarLocalPos.z < 0.0f)
        {
            // Avatar is not yet in the canyon
            debugPath.position = FalaiseEnter.position;
            AttractTo(ct, FalaiseEnter);

            if (arm)
            {
                arm.handRotationSpeed = controller.armRegularRotationSpeed;
            }

            //controller.altitude = cruisingAltitude;
        }
        else
            if (exitAvatarLocalPos.z > 0.0f)
        {
            //Avatar is after the canyon exit 
            if (arm)
            {
                arm.handRotationSpeed = controller.armRegularRotationSpeed;
            }

            //controller.altitude = Interpolate.EaseInOutQuad(altitude, cruisingAltitude, Mathf.Clamp01(chuteTimer/chuteDuration),1f);
            //chuteTimer += Time.deltaTime;

            //controller.altitude = cruisingAltitude;
        }
        else
        {
            // Avatar is in the canyon
            if (arm)
            {
                arm.handRotationSpeed = controller.armSlowRotationSpeed;
            }
            pathProgression = Mathf.Clamp01(advance + entranceAvatarLocalPos.z / entranceExitLocalPos.z);

            Vector3 sp = iTween.PointOnPath(path.nodes.ToArray(), pathProgression);
            debugPath.localPosition = sp;

            AttractTo(ct, debugPath);

            //controller.altitude = Interpolate.EaseInOutQuad(cruisingAltitude, altitude, Mathf.Clamp01(elevationTimer/elevationDuration), 1f);
            elevationTimer += Time.deltaTime;
        }

    }

    void AttractTo(Transform ct, Transform target)
    {

        Vector3 targetVector = target.position - ct.position;
        targetVector.y = 0f;
        targetVector.Normalize();
        targetVector.z = 0;
        translation += targetVector * Time.deltaTime * attraction;

        Vector3 r = self.InverseTransformPoint(target.position);
        r.Normalize();

        float direction = controller.direction;

        direction += r.x * directionAttraction * Time.deltaTime;
        controller.direction = direction;

    }

    /*public IEnumerator TakeControl()
    {
        
        yield return new WaitForSeconds(30f);
        cropping = true;

        switchToFPS.switchPointOfVue = true;
        Debug.Log("Switch POV : Début Recadrage");

        yield return new WaitForSeconds(20.0f);
        cropping = false;

        switchToFPS.automaticSwitch = true;
        switchToFPS.switchPointOfVue = true;
        Debug.Log("Switch POV : Fin Recadrage");
        
    }*/

    public IEnumerator TakeControl()
    {

        yield return new WaitForSeconds(28f);
        cropping = true;
        yield return new WaitForSeconds(2f);

        switchToFPS.switchPointOfVue = true;

        yield return new WaitForSeconds(18f);
        cropping = false;
        yield return new WaitForSeconds(2f);

        switchToFPS.automaticSwitch = true;
        switchToFPS.switchPointOfVue = true;
    }
}