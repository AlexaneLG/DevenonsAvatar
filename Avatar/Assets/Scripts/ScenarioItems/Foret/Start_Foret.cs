using UnityEngine;
using System.Collections;

public class Start_Foret : ScenarioItem
{
    // Use this for initialization

    override public void Start()
    {
        base.Start();

        if (!SensorRecorderManager.instance.isRecording)
        {
            SensorRecorderManager.instance.startRecording();
        }

    }

}