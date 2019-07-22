using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class EmpaticaSensorDataCollection : SensorDataCollection {

    public SensorGeneric<float> Battery = new SensorGeneric<float>("Empatica Battery");
    public SensorGeneric<Vector3> Acceleration = new SensorGeneric<Vector3>("Empatica Acceleration");
    public SensorGeneric<float> BVP = new SensorGeneric<float>("Empatica BVP");
    public SensorGeneric<float> IBI = new SensorGeneric<float>("Empatica IBI");
    public SensorGeneric<float> GSR = new SensorGeneric<float>("Empatica GSR");
    public SensorGeneric<float> Temperature = new SensorGeneric<float>("Empatica Temperature");

    public EmpaticaSensorDataCollection()
        : base("Empatica")
	{
        AddSensor(Battery);
        AddSensor(Acceleration);
        AddSensor(BVP);
        AddSensor(IBI);
        AddSensor(GSR);
        AddSensor(Temperature);
	}
}
