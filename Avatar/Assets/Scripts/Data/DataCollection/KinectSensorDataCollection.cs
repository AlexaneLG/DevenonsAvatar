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
    }
}
