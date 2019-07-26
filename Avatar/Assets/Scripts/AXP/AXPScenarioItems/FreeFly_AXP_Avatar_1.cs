using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeFly_AXP_Avatar_1 : AugmentedScenarioItem
{
    private float _highestAltitude;
    private float _lowestAltitude;
    private bool _hasReachedHighestAltitude;
    private bool _hasReachedLowestAltitude;
    private float _altitudeOffset;

    private bool _hasDisplayLowText;
    private bool _hasDisplayHighText;

    public Transform texts;
    public Transform bubble;
    public GameObject[] bubbles;

    protected override void Awake()
    {
        //durationIncr = 4;
        bubbles = GameObject.FindGameObjectsWithTag("BubbleUserAvatar");

        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        // by observation +/- 5
        _highestAltitude = 479;
        _lowestAltitude = 66;
        _hasReachedHighestAltitude = false;
        _hasReachedLowestAltitude = false;
        _altitudeOffset = 6;

        _hasDisplayLowText = false;
        _hasDisplayHighText = false;
        texts = transform.GetChild(0).GetChild(2);
        bubble = transform.GetChild(0);

        // Hide texts
        bubble.gameObject.SetActive(false);
        for (int i = 0; i < texts.childCount; ++i)
        {
            texts.GetChild(i).gameObject.SetActive(false);
        }

        foreach (GameObject bubble in bubbles)
        {
            bubble.SetActive(false);
        }

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {


        base.Update();
    }

    public override IEnumerator DisplayScenarioItem()
    {
        StartCoroutine(DisplayAltitudes());

        yield return new WaitForSeconds(6f); // wait for dive bubble to end
        yield return new WaitForSeconds(4f);
        if (bubbles.Length > 0)
        {
            foreach (GameObject bubble in bubbles)
            {
                yield return StartCoroutine(DisplayBubble(bubble));
                yield return new WaitForSeconds(4f);
            }
        }

        yield return null;
    }

    private IEnumerator DisplayBubble(GameObject bubble)
    {
        bubble.SetActive(true);
        yield return new WaitForSeconds(7f);
        bubble.SetActive(false);
    }

    private IEnumerator DisplayAltitudes()
    {
        while (_hasReachedHighestAltitude == false || _hasReachedLowestAltitude == false)
        {
            if (_hasDisplayHighText == false && dataManager.Altitude < _highestAltitude && dataManager.Altitude > _highestAltitude - _altitudeOffset)
            {
                _hasReachedHighestAltitude = true;
                _hasDisplayHighText = true;
                Debug.Log("Reached highest altitude");
                yield return DisplayText(1);
            }

            if (_hasDisplayLowText == false && dataManager.Altitude > _lowestAltitude && dataManager.Altitude < _lowestAltitude + _altitudeOffset)
            {
                _hasReachedLowestAltitude = true;
                _hasDisplayLowText = true;
                Debug.Log("Reached lowest altitude");
                yield return DisplayText(0);
            }

            yield return null;
        }

        Debug.Log("User has reached highest and lowest altitudes");
    }

    private IEnumerator DisplayText(int index)
    {
        Color greenColor = new Color(0, 1, 0, 1);
        Color whiteColor = new Color(1, 1, 1, 1);
        GameObject tmp = new GameObject();
        Text hudText = tmp.AddComponent<Text>();
        if (index == 0)
        {
            if (GameObject.Find("Text-MinAltitude-Value"))
            {
                hudText = GameObject.Find("Text-MinAltitude-Value").GetComponent<Text>();
            }
        }
        else
        {
            if (GameObject.Find("Text-MaxAltitude-Value"))
            {
                hudText = GameObject.Find("Text-MaxAltitude-Value").GetComponent<Text>();
            }
        }

        Text altitudeValueText = hudText;
        Text altitudeUnitText = hudText;
        if (GameObject.Find("Text-Altitude-Value") && GameObject.Find("Text-Altitude-Unit"))
        {
            altitudeValueText = GameObject.Find("Text-Altitude-Value").GetComponent<Text>();
            altitudeUnitText = GameObject.Find("Text-Altitude-Unit").GetComponent<Text>();
        }

        bubble.gameObject.SetActive(true);
        texts.GetChild(index).gameObject.SetActive(true);
        hudText.color = greenColor;
        altitudeValueText.color = greenColor;
        altitudeUnitText.color = greenColor;
        yield return new WaitForSeconds(5);
        texts.GetChild(index).gameObject.SetActive(false);
        bubble.gameObject.SetActive(false);
        hudText.color = whiteColor;
        altitudeValueText.color = whiteColor;
        altitudeUnitText.color = whiteColor;
    }
}