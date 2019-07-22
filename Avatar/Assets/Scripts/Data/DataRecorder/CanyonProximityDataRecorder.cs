using UnityEngine;
using System.Collections;

public class CanyonProximityDataRecorder : MonoBehaviour {

    public CanyonProximityDataCollection dataCollection = new CanyonProximityDataCollection();

    public CollisionManager collisionManager;

    void FixedUpdate ()
    {
        dataCollection.canyonProximity.AddRecordedValue(collisionManager.canyonDistance);
    }
}
