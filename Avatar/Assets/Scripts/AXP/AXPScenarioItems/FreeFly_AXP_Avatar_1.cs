using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Awake()
    {
        //durationIncr = 4;

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

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {


        base.Update();
    }

    public override IEnumerator DisplayScenarioItem()
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
        bubble.gameObject.SetActive(true);
        texts.GetChild(index).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        texts.GetChild(index).gameObject.SetActive(false);
        bubble.gameObject.SetActive(false);
    }
}