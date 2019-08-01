using UnityEngine;
using System.Collections;

public class BIM_KinectSensorDataCollection : SensorDataCollection {

    // BIM required datas
    public SensorGeneric<float> hand_Left_X = new SensorGeneric<float>("hand_Left_X");
    public SensorGeneric<float> hand_Left_Y = new SensorGeneric<float>("hand_Left_Y");
    public SensorGeneric<float> hand_Right_X = new SensorGeneric<float>("hand_Right_X");
    public SensorGeneric<float> hand_Right_Y = new SensorGeneric<float>("hand_Right_Y");
    public SensorGeneric<float> shoulder_center_X = new SensorGeneric<float>("hip_center_X");
    public SensorGeneric<float> shoulder_center_Y = new SensorGeneric<float>("hip_center_Y");

    public BIM_KinectSensorDataCollection()
        : base("Kinect")
    {
        // BIM required datas
        AddSensor(hand_Left_X);
        AddSensor(hand_Left_Y);
        AddSensor(hand_Right_X);
        AddSensor(hand_Right_Y);
        AddSensor(shoulder_center_X);
        AddSensor(shoulder_center_Y);
    }
}
