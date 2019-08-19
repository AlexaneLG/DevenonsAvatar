using UnityEngine;
using System.Collections;

public class BIM_KinectSensorRecorder : MonoBehaviour {

    public BIM_KinectSensorDataCollection dataCollection = new BIM_KinectSensorDataCollection();

    // BIM required datas
    public Transform handLeft, handRight, shoulderCenter;
    public float hand_Left_X, hand_Left_Y, hand_Right_X, hand_Right_Y, shoulder_center_X, shoulder_center_Y;

    void FixedUpdate()
    {
        KinectManager manager = KinectManager.Instance;

        if (manager == null)
        {
            return;
        }

        if (manager.GetUsersCount() > 0)
        {
            // BIM
            hand_Left_X = handLeft.position.x;
            hand_Left_Y = handLeft.position.y;
            hand_Right_X = handRight.position.x;
            hand_Right_Y = handRight.position.y;
            shoulder_center_X = shoulderCenter.position.x;
            shoulder_center_Y = shoulderCenter.position.y;
            

            if (SensorRecorderManager.startRecordingSensorData)
            {
                // BIM
                dataCollection.hand_Left_X.AddRecordedValue(hand_Left_X);
                dataCollection.hand_Left_Y.AddRecordedValue(hand_Left_Y);
                dataCollection.hand_Right_X.AddRecordedValue(hand_Right_X);
                dataCollection.hand_Right_Y.AddRecordedValue(hand_Right_Y);
                dataCollection.shoulder_center_X.AddRecordedValue(shoulder_center_X);
                dataCollection.shoulder_center_Y.AddRecordedValue(shoulder_center_Y);
            } 
        }
        else
        {
            if (SensorRecorderManager.startRecordingSensorData)
            {
                // BIM
                dataCollection.hand_Left_X.AddRecordedValue(0);
                dataCollection.hand_Left_Y.AddRecordedValue(0);
                dataCollection.hand_Right_X.AddRecordedValue(0);
                dataCollection.hand_Right_Y.AddRecordedValue(0);
                dataCollection.shoulder_center_X.AddRecordedValue(0);
                dataCollection.shoulder_center_Y.AddRecordedValue(0);
            }
        }
    }
}
