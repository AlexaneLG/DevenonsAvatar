using UnityEngine;
using System.Collections;

public class KinectSensorDataCollection : SensorDataCollection {

    public SensorGeneric<float> arm_Forearm_R = new SensorGeneric<float>("arm_Forearm_R");
    public SensorGeneric<float> arm_Forearm_L = new SensorGeneric<float>("arm_Forearm_L");
    public SensorGeneric<float> arm_Torso_R = new SensorGeneric<float>("arm_Torso_R");
    public SensorGeneric<float> arm_Torso_L = new SensorGeneric<float>("arm_Torso_L");
    public SensorGeneric<float> thigh_Calf_R = new SensorGeneric<float>("thigh_Calf_R");
    public SensorGeneric<float> thigh_Calf_L = new SensorGeneric<float>("thigh_Calf_L");
    public SensorGeneric<float> torso_Tighs = new SensorGeneric<float>("torso_Tighs");
   
    public SensorGeneric<float> shouldersVerticalRotation = new SensorGeneric<float>("shouldersVerticalRotation");
    public SensorGeneric<float> shouldersLateralRotation = new SensorGeneric<float>("shouldersLateralRotation");
    public SensorGeneric<float> hipRotation = new SensorGeneric<float>("hipRotation");

    // BIM required datas
    public SensorGeneric<float> hand_Left_X = new SensorGeneric<float>("hand_Left_X");
    public SensorGeneric<float> hand_Left_Y = new SensorGeneric<float>("hand_Left_Y");
    public SensorGeneric<float> hand_Right_X = new SensorGeneric<float>("hand_Right_X");
    public SensorGeneric<float> hand_Right_Y = new SensorGeneric<float>("hand_Right_Y");
    public SensorGeneric<float> shoulder_center_X = new SensorGeneric<float>("hip_center_X");
    public SensorGeneric<float> shoulder_center_Y = new SensorGeneric<float>("hip_center_Y");

    public KinectSensorDataCollection()
        : base("Kinect")
    {
        AddSensor(arm_Forearm_R);
        AddSensor(arm_Forearm_L);

        AddSensor(arm_Torso_R);
        AddSensor(arm_Torso_L);

        AddSensor(thigh_Calf_R);
        AddSensor(thigh_Calf_L);

        AddSensor(torso_Tighs);

        AddSensor(shouldersVerticalRotation);
        AddSensor(shouldersLateralRotation);

        AddSensor(hipRotation);

        // BIM
        AddSensor(hand_Left_X);
        AddSensor(hand_Left_Y);
        AddSensor(hand_Right_X);
        AddSensor(hand_Right_Y);
        AddSensor(shoulder_center_X);
        AddSensor(shoulder_center_Y);
    }
}
