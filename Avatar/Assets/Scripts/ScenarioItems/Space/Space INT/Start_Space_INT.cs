using UnityEngine;
using System.Collections;

public class Start_Space_INT : ScenarioItem
{
    public Material videoMaterial;

    public iTweenPath path;

    // Use this for initialization
    override public void Start()
    {
        base.Start();

        if (!SensorRecorderManager.instance.isRecording)
        {
            SensorRecorderManager.instance.startRecording();
        }

        if (videoMaterial)
        {
            videoMaterial.SetFloat("_fadeToBlack", 0);
        }

        if (path != null)
        {
            //var wp = path.transform.TransformPoint();
            CharacterControllerBasedOnAxis.instance.transform.position = path.nodes[0];
        }

    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }
}