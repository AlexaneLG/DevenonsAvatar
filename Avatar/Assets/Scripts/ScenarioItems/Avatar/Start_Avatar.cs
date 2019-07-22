using UnityEngine;
using System.Collections;

public class Start_Avatar : ScenarioItem
{
    public AudioSource WindSound;

    // Use this for initialization
    override public void Start()
    {
        base.Start();

        if (!SensorRecorderManager.instance.isRecording)
        {
            SensorRecorderManager.instance.startRecording();
        }

        WindSound.enabled = true;
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }

}