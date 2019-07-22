using UnityEngine;
using System.Collections;

public class MeteorProximityDataCollection : SensorDataCollection{

    public SensorGeneric<float> meteorProximity_1 = new SensorGeneric<float>("MeteorProximity_1");
    public SensorGeneric<float> meteorProximity_2 = new SensorGeneric<float>("MeteorProximity_2");
    public SensorGeneric<float> meteorProximity_3 = new SensorGeneric<float>("MeteorProximity_3");

    public MeteorProximityDataCollection()
        : base("MeteorProximity")
    {
        AddSensor(meteorProximity_1);
        AddSensor(meteorProximity_2);
        AddSensor(meteorProximity_3);
    }
}
