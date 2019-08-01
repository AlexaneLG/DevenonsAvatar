using UnityEngine;
using System.Collections;

public class KinectSensorRecorder : MonoBehaviour {

    public KinectSensorDataCollection dataCollection = new KinectSensorDataCollection();

    public Transform shoulderSpine, shoulderRight, shoulderLeft, elbowRight, elbowLeft, wirstRight, wirstLeft, hipCenter, kneeRight, kneeLeft, ankleRight, ankleLeft;
    public Transform kneeCenter;

    public int arm_Forearm_R, arm_Forearm_L, arm_Torso_R, arm_Torso_L, thigh_Calf_R, thigh_Calf_L, torso_Tighs;
    public int shouldersVerticalRotation, shouldersLateralRotation, hipRotation;

    void FixedUpdate()
    {
        KinectManager manager = KinectManager.Instance;

        if (manager == null)
        {
            return;
        }

        if (manager.GetUsersCount() > 0)
        {
            arm_Forearm_R = angleCalculator(shoulderRight, elbowRight, wirstRight);
            arm_Forearm_L = angleCalculator(shoulderLeft, elbowLeft, wirstLeft);
            arm_Torso_R = angleCalculator(hipCenter, shoulderRight, elbowRight);
            arm_Torso_L = angleCalculator(hipCenter, shoulderLeft, elbowLeft);
            thigh_Calf_R = angleCalculator(hipCenter, kneeRight, ankleRight);
            thigh_Calf_L = angleCalculator(hipCenter, kneeLeft, ankleLeft);

            if (kneeRight != null && kneeLeft != null)
            {
                kneeCenter.localPosition = (kneeRight.localPosition + kneeLeft.localPosition) / 2;
                torso_Tighs = angleCalculator(shoulderSpine, hipCenter, kneeCenter);
            }

            else
            {
                torso_Tighs = 0;
            }
                

            shouldersVerticalRotation = (int)shoulderSpine.localEulerAngles.y;
            shouldersLateralRotation = (int)shoulderSpine.localEulerAngles.z;
            hipRotation = (int)hipCenter.localEulerAngles.y;

            if (SensorRecorderManager.startRecordingSensorData)
            {
                dataCollection.arm_Forearm_R.AddRecordedValue(arm_Forearm_R);
                dataCollection.arm_Forearm_L.AddRecordedValue(arm_Forearm_L);
                dataCollection.arm_Torso_R.AddRecordedValue(arm_Torso_R);
                dataCollection.arm_Torso_L.AddRecordedValue(arm_Torso_L);
                dataCollection.thigh_Calf_R.AddRecordedValue(thigh_Calf_R);
                dataCollection.thigh_Calf_L.AddRecordedValue(thigh_Calf_L);
                dataCollection.torso_Tighs.AddRecordedValue(torso_Tighs);

                dataCollection.shouldersVerticalRotation.AddRecordedValue(shouldersVerticalRotation);
                dataCollection.shouldersLateralRotation.AddRecordedValue(shouldersLateralRotation);
                dataCollection.hipRotation.AddRecordedValue(hipRotation);
            } 
        }
        else
        {
            if (SensorRecorderManager.startRecordingSensorData)
            {
                dataCollection.arm_Forearm_R.AddRecordedValue(0);
                dataCollection.arm_Forearm_L.AddRecordedValue(0);
                dataCollection.arm_Torso_R.AddRecordedValue(0);
                dataCollection.arm_Torso_L.AddRecordedValue(0);
                dataCollection.thigh_Calf_R.AddRecordedValue(0);
                dataCollection.thigh_Calf_L.AddRecordedValue(0);
                dataCollection.torso_Tighs.AddRecordedValue(0);

                dataCollection.shouldersVerticalRotation.AddRecordedValue(0);
                dataCollection.shouldersLateralRotation.AddRecordedValue(0);
                dataCollection.hipRotation.AddRecordedValue(0);
            }
        }
    }

    public int angleCalculator(Transform pA, Transform pB, Transform pC)
    {
        Vector3 vBA = new Vector3(pA.localPosition.x - pB.localPosition.x, pA.localPosition.y - pB.localPosition.y, pA.localPosition.z - pB.localPosition.z);
        Vector3 vBC = new Vector3(pC.localPosition.x - pB.localPosition.x, pC.localPosition.y - pB.localPosition.y, pC.localPosition.z - pB.localPosition.z);

        float vBA_Mag = Mathf.Sqrt(vBA.x * vBA.x + vBA.y * vBA.y + vBA.z * vBA.z);
        Vector3 vBA_Norm = new Vector3(vBA.x / vBA_Mag, vBA.y / vBA_Mag, vBA.z / vBA_Mag);

        float vBC_Mag = Mathf.Sqrt(vBC.x * vBC.x + vBC.y * vBC.y + vBC.z * vBC.z);
        Vector3 vBC_Norm = new Vector3(vBC.x / vBC_Mag, vBC.y / vBC_Mag, vBC.z / vBC_Mag);

        float scal = vBA_Norm.x * vBC_Norm.x + vBA_Norm.y * vBC_Norm.y + vBA_Norm.z * vBC_Norm.z;

        float radAngle = Mathf.Acos(scal);
        int degAngle = (int)((radAngle * 180) / Mathf.PI);

        return (degAngle);
    }
}
