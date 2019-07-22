using UnityEngine;
using System.Collections;

public class ISS : FreeFly_Avatar
{
    public Transform target;

    [Range(0.0f, 1000.0f)]
    public float attraction;

    [Range(0.0f, 25.0f)]
    public float directionAttraction;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }


    // Use this for initialization
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        Transform ct = controller.transform;

        ArmController arm = controller.GetComponent<ArmController>() as ArmController;

        if (arm)
        {
            arm.handRotationSpeed = controller.armRegularRotationSpeed;
        }

        AttractTo(ct, target);
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
}