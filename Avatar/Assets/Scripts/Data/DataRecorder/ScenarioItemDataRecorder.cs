using UnityEngine;
using System.Collections;

public class ScenarioItemDataRecorder : MonoBehaviour
{
    public ScenarioItemDataCollection dataCollection = new ScenarioItemDataCollection();

    void FixedUpdate()
    {
        if (SensorRecorderManager.startRecordingSensorData)
        {
            dataCollection.ScenarioItem.AddRecordedValue(CharacterControllerBasedOnAxis.currentScenarioItem);
        }
    }
}

