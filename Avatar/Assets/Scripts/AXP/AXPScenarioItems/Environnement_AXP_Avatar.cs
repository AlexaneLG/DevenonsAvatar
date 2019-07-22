using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environnement_AXP_Avatar : AugmentedScenarioItem {

    protected override void Awake()
    {
        durationIncr = 10;

        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        //dataManager.DisplayHUD(false); // hide HUD

        base.Start(); 
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public override IEnumerator DisplayScenarioItem()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            yield return new WaitForSeconds(2);
            transform.GetChild(i).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(4);
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<Animator>().SetTrigger("FadeOut");
        }

    }
}
