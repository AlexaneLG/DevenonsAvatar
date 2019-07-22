using UnityEngine;
using System.Collections;

public class FreeFly_Wii : ScenarioItem
{

    public bool needUser = true;

    public static bool enableFreeFly;

    // Use this for initialization
    override public void Start()
    {
        base.Start();

        WiiController.enableMovement = true;

        enableFreeFly = true;
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

    }
}

