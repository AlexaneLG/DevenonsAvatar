using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dive_AXP_Avatar : AugmentedScenarioItem
{

    protected override void Awake()
    {
        //durationIncr = 4;

        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        dataManager.tmpSpeedFreeFly = dataManager.constSpeedDive; // Lerp to

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override IEnumerator DisplayScenarioItem()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(7f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
