using UnityEngine;
using System.Collections;

public class AltitudeDataCollection : SensorDataCollection {

    public SensorGeneric<float> Altitude = new SensorGeneric<float>("Altitude");

    public AltitudeDataCollection()
        : base("Altitude")
    {
        AddSensor(Altitude);
    }
}
