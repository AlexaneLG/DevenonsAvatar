using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storm_AXP_Avatar : AugmentedScenarioItem
{
    public Camera mainCamera;
    public Image pointerImage; // anchor has to be in the bottom left corner of the panel
    public GameObject meteor;
    public GameObject panel;
    public float offset;

    public GameObject unknownObjectBubble;
    public GameObject meteorRainBubble;

    private int _currentMeteor;
    List<GameObject> _meteorList;

    // Use this for initialization
    protected override void Start()
    {
        unknownObjectBubble = transform.GetChild(0).gameObject;
        unknownObjectBubble.gameObject.SetActive(false);

        meteorRainBubble = transform.GetChild(1).gameObject;
        meteorRainBubble.gameObject.SetActive(false);

        base.Start();

        // Set offset
        if (offset == 0f)
        {
            offset = 150f;
        }
        // Set Main Camera
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        // Set Image
        if (pointerImage == null)
        {
            pointerImage = GameObject.FindGameObjectWithTag("TrackerImage").GetComponent<Image>();
        }
        // Set Panel
        if (panel == null)
        {
            panel = GameObject.FindGameObjectWithTag("TrackingPanel");
        }

        // Set current meteor
        _currentMeteor = 0;

        _meteorList = dataManager.CurrentScenarioItem.GetComponent<Storm_Avatar>().meteorList;
    }

    private void FixedUpdate()
    {
        DisplayCurrentMeteorSpeed();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (meteor == null
            || mainCamera.transform.InverseTransformPoint(meteor.transform.position).z < -100f // same condition in Storm_Avatar
            || pointerImage.GetComponent<RectTransform>().anchoredPosition.y > panel.GetComponent<RectTransform>().rect.height - offset
            || pointerImage.GetComponent<RectTransform>().anchoredPosition.y < 0 + offset
            || pointerImage.GetComponent<RectTransform>().anchoredPosition.x > panel.GetComponent<RectTransform>().rect.width - offset
            || pointerImage.GetComponent<RectTransform>().anchoredPosition.x < 0 + offset)
        {
            pointerImage.enabled = false;
            pointerImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset+1, offset+1);
            meteor = GetNextMeteor();
        }
        else
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(meteor.transform.position);
            pointerImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(screenPos.x - panel.GetComponent<RectTransform>().rect.width, screenPos.y);

            if (!pointerImage.enabled)
            {
                pointerImage.enabled = true;
            }
        }
    }

    public GameObject GetNextMeteor()
    {
        int nextMeteor = _currentMeteor + 1;
        nextMeteor = nextMeteor > (_meteorList.Count - 1) ? 0 : nextMeteor;
        _currentMeteor = nextMeteor;
        return _meteorList[_currentMeteor];
    }

    public float GetCurrentMeteorSpeed()
    {
        return Vector3.Magnitude(_meteorList[_currentMeteor].GetComponent<Rigidbody>().velocity);
    }

    private void DisplayCurrentMeteorSpeed()
    {
        GameObject.FindGameObjectWithTag("MeteorSpeed").GetComponent<Text>().text = Mathf.Round(GetCurrentMeteorSpeed()).ToString();
    }

    public override IEnumerator DisplayScenarioItem()
    {
        unknownObjectBubble.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        unknownObjectBubble.gameObject.SetActive(false);

        meteorRainBubble.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        meteorRainBubble.gameObject.SetActive(false);
    }
}
