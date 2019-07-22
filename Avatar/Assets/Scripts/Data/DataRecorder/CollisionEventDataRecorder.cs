using UnityEngine;
using System.Collections;

public class CollisionEventDataRecorder : MonoBehaviour {

    public CollisionEventDataCollection dataCollection = new CollisionEventDataCollection();

    [HideInInspector]
    public int collisionSensor = 0;

    void FixedUpdate()
    {
        if (SensorRecorderManager.startRecordingSensorData)
        {
            dataCollection.CollisionEvent.AddRecordedValue(collisionSensor);
        }

        collisionSensor = 0;
    }
}
