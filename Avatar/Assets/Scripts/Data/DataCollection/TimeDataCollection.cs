using UnityEngine;
using System.Collections;

public class TimeDataCollection : SensorDataCollection
{
    public SensorGeneric<double> TimeData = new SensorGeneric<double>("TimeIdx");


    public TimeDataCollection()
        : base("TimeIdx")
    {
        AddSensor(TimeData);
    }
}
