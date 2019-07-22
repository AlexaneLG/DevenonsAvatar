using UnityEngine;
using System.Collections;

public class Start_Space_EXT : ScenarioItem
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

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }
}