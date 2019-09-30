using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOff_AXP_Avatar : AugmentedScenarioItem
{

    protected override void Awake()
    {
        //durationIncr = 4;

        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        dataManager.DisplayHUD(true); // Display HUD

        transform.GetChild(0).gameObject.SetActive(false);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override IEnumerator DisplayScenarioItem()
    {
        while (dataManager.Altitude < 1)
        {
            yield return null;
        }

        StartCoroutine(LerpSpeedTakeOff());

        transform.GetChild(0).gameObject.SetActive(true); // Display text
        yield return new WaitForSeconds(4);

    }

    IEnumerator LerpSpeedTakeOff()
    {
        dataManager.tmpSpeedFreeFly = dataManager.constSpeedTakeOff; // Lerp to

        yield return new WaitForSeconds(2f);

        dataManager.tmpSpeedFreeFly = dataManager.constSpeedTakeOff / 2; // Lerp to
    }

}