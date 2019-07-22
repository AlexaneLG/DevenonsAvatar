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
        yield return new WaitForSeconds(5);
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
