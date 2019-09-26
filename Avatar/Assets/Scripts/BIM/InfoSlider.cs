using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject userInterface;

    void Awake()
    {
        userInterface = GameObject.FindGameObjectWithTag("BIMUI");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse is over GameObject.");
        userInterface.GetComponent<SetTimeline>().DisplayLabel(this.transform.parent.parent.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse is no longer on GameObject.");
        userInterface.GetComponent<SetTimeline>().HideLabel(this.transform.parent.parent.gameObject);
    }

}
