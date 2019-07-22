using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonesPositionGetter2 : MonoBehaviour
{
    public Transform T_KinectEmitter, T_AvatarReceiver;
    private List<Transform> Li_T_Emitter, Li_T_Receiver;

    void Awake()
    {
        Li_T_Emitter = new List<Transform>();
        Li_T_Receiver = new List<Transform>();

        Transform[] _Emmiters = T_KinectEmitter.GetComponentsInChildren<Transform>();
        for (int i = 0; i < _Emmiters.Length; i++)
        {
            Li_T_Emitter.Add(_Emmiters[i]);
        }

        Transform[] _Receivers = T_AvatarReceiver.GetComponentsInChildren<Transform>();
        for (int i = 0; i < _Receivers.Length; i++)
        {
            Li_T_Receiver.Add(_Receivers[i]);
        }
        Update();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Li_T_Receiver.Count; i++)
        {

            if (i == 13)
            {
                Vector3 V3_EulerRotation = Li_T_Emitter[i].localEulerAngles;
                //if(V3_EulerRotation.x < 0) V3_EulerRotation.x += 360f;
                //V3_EulerRotation.x = 360f - V3_EulerRotation.x;

                //V3_EulerRotation += new Vector3(0, 180, 180);
                //if(V3_EulerRotation.y < 0) V3_EulerRotation.y += 360f;
                //if(V3_EulerRotation.z < 0) V3_EulerRotation.z += 360f;

                Li_T_Receiver[i].localEulerAngles = V3_EulerRotation;
            }
            else
                Li_T_Receiver[i].localRotation = Li_T_Emitter[i].localRotation;
        }
    }
}
