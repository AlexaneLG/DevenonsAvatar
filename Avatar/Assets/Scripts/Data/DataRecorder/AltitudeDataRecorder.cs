using UnityEngine;
using System.Collections;

public class AltitudeDataRecorder : MonoBehaviour {

    public AltitudeDataCollection dataCollection = new AltitudeDataCollection();

    public Transform characterTransform;

    void FixedUpdate()
    {
        if (SensorRecorderManager.startRecordingSensorData && characterTransform != null)
        {
            float userAltitude = Mathf.Round(characterTransform.transform.position.y * 100f) / 100f;
            dataCollection.Altitude.AddRecordedValue(userAltitude);
        }
    }
}