using UnityEngine;
using System.Collections;

public class CollisionEventDataCollection : SensorDataCollection {

    public SensorGeneric<int> CollisionEvent = new SensorGeneric<int>("CollisionEvent");


    public CollisionEventDataCollection()
        : base("CollisionEvent")
    {
        AddSensor(CollisionEvent);
    }
}
