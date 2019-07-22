using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AugmentedScenarioItem : MonoBehaviour {
    
    public int durationIncr; // extend duration of scenario item

    public DataManager dataManager;

    protected virtual void Awake()
    {
        // Get Data Manager
        dataManager = GameObject.FindWithTag("DataManager").GetComponent<DataManager>();
    }

    // Use this for initialization
    protected virtual void Start () {

        if (durationIncr > 0)
        {
            ExtendDuration(durationIncr);
        }

        StartCoroutine(DisplayScenarioItem());
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    public abstract IEnumerator DisplayScenarioItem();

    public void ExtendDuration(int incr)
    {
        dataManager.CurrentScenarioItem.duration += incr;
    }
    
}
