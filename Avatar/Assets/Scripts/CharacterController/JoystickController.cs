using UnityEngine;
using System.Collections;

public class JoystickController : AvatarFlightController

{

    public float Amplitude = 70f;
    public float Neutral = -35f;

    [Range(-1f, 1f)]

    public float Horizontal = 0f;
    [Range(-1f, 1f)]
    public float Vertical = 0f;
    [Range(-1f, 1f)]
    public float Horizontal1 = 0f;
    [Range(-1f, 1f)]
    public float Vertical1 = 0f;

    public Transform[] leftShoulder, rightShoulder;

    bool isStarted, isStarted1;
    public bool isFlying = false;

    void Awake()
    {
        foreach (var item in leftShoulder)
        {
            item.transform.rotation = Quaternion.Euler(0f, 0f, Neutral);
        }
        foreach (var item in rightShoulder)
        {
            item.transform.rotation = Quaternion.Euler(180f, 0f, Neutral);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        Horizontal1 = Input.GetAxis("Horizontal1");
        Vertical1 = Input.GetAxis("Vertical1");

        if (Vertical < -0.8f)
        {
            isStarted = true;
        }

        if (Vertical1 < -0.8f)
        {
            isStarted1 = true;
        }

        //controller.direction = Mathf.Lerp(controller.direction, Horizontal * strength, Time.deltaTime);
        if (isStarted || isStarted1)
        {
            if (!isFlying)
            {
                if (isStarted)
                    foreach (var item in leftShoulder)
                    {
                        item.transform.rotation = Quaternion.Slerp(item.transform.rotation, Quaternion.Euler(0f, 0f, 0), 2 * Time.deltaTime);
                        //item.transform.rotation = Quaternion.Euler(0f, 0f, 0);
                    }

                if (isStarted1)
                    foreach (var item in rightShoulder)
                    {
                        item.transform.rotation = Quaternion.Slerp(item.transform.rotation, Quaternion.Euler(180f, 0f, 0f), 2 * Time.deltaTime);
                        //item.transform.rotation = Quaternion.Euler(180f, 0f, 0);
                    }
            }
            else
            {
                foreach (var item in leftShoulder)
                {
                    item.transform.rotation = Quaternion.Euler(0f, 0f, Vertical * Amplitude);
                }

                foreach (var item in rightShoulder)
                {
                    item.transform.rotation = Quaternion.Euler(180f, 0f, Vertical1 * Amplitude);
                }
            }
        }
        else
        {
            foreach (var item in leftShoulder)
            {
                item.transform.rotation = Quaternion.Euler(0f, 0f, Neutral);
            }
            foreach (var item in rightShoulder)
            {
                item.transform.rotation = Quaternion.Euler(180f, 0f, Neutral);
            }
        }


    }
}
