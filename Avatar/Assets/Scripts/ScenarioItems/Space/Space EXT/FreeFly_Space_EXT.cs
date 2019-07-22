using UnityEngine;
using System.Collections;

public class FreeFly_Space_EXT : ScenarioItem
{
    // Use this for initialization
    override public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        if(SatelliteObjectif.isCollideByPlayer == true)
        {
            Debug.Log("collideStation");
            controller.ActivateNextScenarioItem ();
        }
    }
}