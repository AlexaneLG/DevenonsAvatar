using UnityEngine;
using System.Collections;

public class Gates_Wii : ScenarioItem
{
    public GatewayScript FirstGate;

    public int timeBeforeSymmetry = 60;

    // Use this for initialization
    override public void Start()
    {
        base.Start();

        FirstGate.EnableFirstGate();

        //StartCoroutine("EnableSymmetry");
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        if(GatewayScript.finishGatesWay == true)
        {
            controller.ActivateNextScenarioItem();
        }
    }

    IEnumerator EnableSymmetry()
    {
        yield return new WaitForSeconds(timeBeforeSymmetry);

        Symmetry.enableAvatarSymmetry = true;
    }
}

