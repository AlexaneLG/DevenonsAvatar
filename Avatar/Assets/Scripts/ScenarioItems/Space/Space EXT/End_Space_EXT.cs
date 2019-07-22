using UnityEngine;
using System.Collections;

public class End_Space_EXT : ScenarioItem
{
    public GameObject endQuad;
    public GameObject greenScreen;

    public override void OnEnable()
    {
        base.OnEnable();
        
        endQuad.SetActive(true);
        Color c = endQuad.GetComponent<Renderer>().sharedMaterial.GetColor("_TintColor");
        c.a = 0;
        endQuad.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", c);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //endQuad.SetActive(false);
        if (SensorRecorderManager.instance.isRecording)
        {
            SensorRecorderManager.instance.endRecording();
        }
    }


    // Use this for initialization
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        Color c = endQuad.GetComponent<Renderer>().sharedMaterial.GetColor("_TintColor");
        c.a = progressionQuad;
        endQuad.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor",c);
    }
}