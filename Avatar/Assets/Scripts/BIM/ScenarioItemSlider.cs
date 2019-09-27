using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScenarioItemSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject userInterface;

    void Awake()
    {
        userInterface = GameObject.FindGameObjectWithTag("BIMUI");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        userInterface.GetComponent<SetTimeline>().DisplayLabel(this.transform.parent.parent.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        userInterface.GetComponent<SetTimeline>().HideLabel(this.transform.parent.parent.gameObject);
    }
}
