using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectUser_AXP_Avatar : AugmentedScenarioItem
{
    public int samplesNumber;
    public GameObject heightBlock;

    private int idxSample;

    override protected void Awake()
    {
        durationIncr = 40;

        base.Awake();
    }

    // Use this for initialization
    override protected void Start()
    {
        gameObject.GetComponent<Animator>().speed = 0;

        transform.Find("RulerParent").gameObject.SetActive(false);

        if (dataManager != null && dataManager.isKinectActive)
        {
            transform.Find("RulerParent").gameObject.SetActive(true);

            idxSample = 0;
            if (samplesNumber == 0)
            {
                samplesNumber = 100;
            }

            if (heightBlock == null)
            {
                heightBlock = GameObject.FindGameObjectWithTag("HeightBlock");
                heightBlock.SetActive(false);
            }

            StartCoroutine(DisplayHeight());
        }

        base.Start();
        
    }

    // Update is called once per frame
    override protected void Update() {
        base.Update();
            
	}

    public override IEnumerator DisplayScenarioItem()
    {
        // Wait until the avatar is not visible
        ProjectUser_Avatar _currentSI = dataManager.CurrentScenarioItem as ProjectUser_Avatar;
        while (_currentSI.isAvatarVisible == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<Animator>().speed = 1;
    }

    public IEnumerator DisplayHeight()
    {
        while (true)
        {
            if (idxSample < samplesNumber)
            {
                dataManager.SetAvatarHeight();
                ++idxSample;
            }
            else
            {
                float height = Mathf.Round(DataManager.GetMean(dataManager.HeightSamples) * 100);
                GameObject.FindGameObjectWithTag("UserHeight").GetComponent<Text>().text = height.ToString();
                Debug.Log("Height : " + height);
                idxSample = 0;
            }
            yield return null;
        }
        //yield return null;
    }
}
