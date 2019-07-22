using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_AXP_Avatar : AugmentedScenarioItem
{

    // Use this for initialization
    protected override void Start()
    {
        dataManager.DisplayHUD(false);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {


        base.Update();
    }

    public override IEnumerator DisplayScenarioItem()
    {
        yield return null;
    }
}
