using UnityEngine;
using System.Collections;

public class KinectSensorRecorder : MonoBehaviour
{

    public KinectSensorDataCollection dataCollection = new KinectSensorDataCollection();

    public Transform shoulderSpine, shoulderRight, shoulderLeft, elbowRight, elbowLeft, wirstRight, wirstLeft, hipCenter, kneeRight, kneeLeft, ankleRight, ankleLeft;
    public Transform kneeCenter;

    public int arm_Forearm_R, arm_Forearm_L, arm_Torso_R, arm_Torso_L, thigh_Calf_R, thigh_Calf_L, torso_Tighs;
    public int shouldersVerticalRotation, shouldersLateralRotation, hipRotation;

    // BIM required datas
    public Transform handLeft, handRight, shoulderCenter;
    public float hand_Left_X, hand_Left_Y, hand_Left_Z, hand_Right_X, hand_Right_Y, hand_Right_Z, shoulder_center_X, shoulder_center_Y, shoulder_center_Z;
    public bool recordBIMData = false;

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

            if (recordBIMData)
            {
                // BIM
                hand_Left_X = handLeft.position.x;
                hand_Left_Y = handLeft.position.y;
                hand_Left_Z = handLeft.position.z;
                hand_Right_X = handRight.position.x;
                hand_Right_Y = handRight.position.y;
                hand_Right_Z = handRight.position.z;
                shoulder_center_X = shoulderCenter.position.x;
                shoulder_center_Y = shoulderCenter.position.y;
                shoulder_center_Z = shoulderCenter.position.z;
            }

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

                if (recordBIMData)
                {
                    // BIM
                    dataCollection.hand_Left_X.AddRecordedValue(hand_Left_X);
                    dataCollection.hand_Left_Y.AddRecordedValue(hand_Left_Y);
                    dataCollection.hand_Left_Z.AddRecordedValue(hand_Left_Z);
                    dataCollection.hand_Right_X.AddRecordedValue(hand_Right_X);
                    dataCollection.hand_Right_Y.AddRecordedValue(hand_Right_Y);
                    dataCollection.hand_Right_Z.AddRecordedValue(hand_Right_Z);
                    dataCollection.shoulder_center_X.AddRecordedValue(shoulder_center_X);
                    dataCollection.shoulder_center_Y.AddRecordedValue(shoulder_center_Y);
                    dataCollection.shoulder_center_Z.AddRecordedValue(shoulder_center_Z);
                }
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

                if (recordBIMData)
                {
                    // BIM
                    dataCollection.hand_Left_X.AddRecordedValue(0);
                    dataCollection.hand_Left_Y.AddRecordedValue(0);
                    dataCollection.hand_Left_Z.AddRecordedValue(0);
                    dataCollection.hand_Right_X.AddRecordedValue(0);
                    dataCollection.hand_Right_Y.AddRecordedValue(0);
                    dataCollection.hand_Right_Z.AddRecordedValue(0);
                    dataCollection.shoulder_center_X.AddRecordedValue(0);
                    dataCollection.shoulder_center_Y.AddRecordedValue(0);
                    dataCollection.shoulder_center_Z.AddRecordedValue(0);
                }
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
