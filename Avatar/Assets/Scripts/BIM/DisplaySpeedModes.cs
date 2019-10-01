using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplaySpeedModes : MonoBehaviour, IPointerEnterHandler
{
    private bool _displayButtons = false;
    public Transform buttons;

	// Update is called once per frame
	void Update () {
        for (int i=0; i < buttons.childCount; ++i)
        {
            buttons.GetChild(i).gameObject.SetActive(_displayButtons);
        }
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        _displayButtons = !_displayButtons;
    }
}
