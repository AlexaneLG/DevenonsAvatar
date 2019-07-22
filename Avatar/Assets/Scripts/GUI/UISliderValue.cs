using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISliderValue : MonoBehaviour {

    Text sliderValue;
    float truncatedSliderValue;

    void Awake ()
    {
        sliderValue = GetComponent<Text>();
    }
	
	void Update () 
    {
        truncatedSliderValue = Mathf.Round(GetComponentInParent<Slider>().value * 100) / 100;
        sliderValue.text = truncatedSliderValue.ToString();
	}
}
