using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Storm_Avatar : FreeFly_Avatar
{
    public Transform characterTransform;

    public List<GameObject> meteorList;
    public GameObject meteor;

    public int meteorCount;
    public float delay;
    public float thrust;
    public float meteorSize;
    public float maxSpeed;

    public override void OnEnable()
    {
        base.OnEnable();
        if (joystickController)
        {
            joystickController.isFlying = true;
        }
        InvokeMeteors();
    }

    public override void OnDisable()
    {
        foreach (GameObject go in meteorList)
            Destroy(go);

        base.OnDisable();
    }

    override public void Start()
    {
        base.Start();

        meteor.transform.localScale = new Vector3(meteorSize, meteorSize, meteorSize);
    }

    override public void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {
        var ct = Camera.main.transform;

        foreach (GameObject go in meteorList)
        {

            var p = go.transform.position;
            var cp = ct.InverseTransformPoint(p);

            if (cp.z < -100f)
            {
                Debug.Log("Respawn Meteor");
                go.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                Vector3 characterPos = characterTransform.position;
                Vector3 characterDirection = characterTransform.forward;
                Vector3 spawnPosition = characterPos + characterDirection * 3500;

                spawnPosition = new Vector3(spawnPosition.x, characterPos.y, spawnPosition.z);

                go.transform.localPosition = spawnPosition;
            }

            else
            {
                Vector3 meteorDirection = (characterTransform.position - go.transform.position).normalized;

                go.GetComponent<Rigidbody>().AddForce(meteorDirection * thrust);

                //Debug.DrawLine(go.transform.localPosition, characterTransform.localPosition);

                float speed = Vector3.Magnitude(go.GetComponent<Rigidbody>().velocity);

                if (speed > maxSpeed)
                {
                    float brakeSpeed = speed - maxSpeed;

                    Vector3 normalisedVelocity = go.GetComponent<Rigidbody>().velocity.normalized;
                    Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;

                    go.GetComponent<Rigidbody>().AddForce(-brakeVelocity);
                }
            }
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public void InvokeMeteors()
    {
        for (int i = 0; i < meteorCount; i++)
        {
            Invoke("LaunchMeteors", delay * i);
        }
    }

    public void LaunchMeteors()
    {
        Vector3 characterPos = characterTransform.position;
        Vector3 characterDirection = characterTransform.forward;
        Vector3 spawnPosition = characterPos + characterDirection * 3500;
        spawnPosition = new Vector3(spawnPosition.x, (Random.Range(100, 500)), spawnPosition.z);

        GameObject go = Instantiate(meteor, spawnPosition, Quaternion.identity) as GameObject;
        meteorList.Add(go);

        controller.objectsToReplace.Add(go.transform);
    }

    public void adjusteMeteorSize(float newMeteorSize)
    {
        meteorSize = newMeteorSize;
    }
}