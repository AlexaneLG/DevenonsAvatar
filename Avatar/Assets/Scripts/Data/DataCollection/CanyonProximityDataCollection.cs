using UnityEngine;
using System.Collections;

public class CanyonProximityDataCollection : SensorDataCollection {

    public SensorGeneric<float> canyonProximity = new SensorGeneric<float>("CanyonProximity");


    public CanyonProximityDataCollection()
        : base("CanyonProximity")
    {
        AddSensor(canyonProximity);
    }
}
