using UnityEngine;
using System.Collections;

[ExecuteInEditMode]

public class Dayswitcher : MonoBehaviour {

    public Light _directionnalLight;
    public bool _switchDay;
    public float _lightIntensityDay;
    public float _lightIntensityNight;
    public Material _daySkybox;
    public Material _nightSkybox;
    public Light _firstSpotLight;
    public Light _secondSpotLight;
    public Color _fogDayColor;
    public Color _fogNightColor;
    public Color _ambientDayLightColor;
    public Color _ambientNightLightColor;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            if (_switchDay == true)
            {
                _switchDay = false;
            }
            else
            {
                _switchDay = true;
            }

        }
        if (_switchDay == true)
        {
            _directionnalLight.GetComponent<Light>().intensity = _lightIntensityDay;
            RenderSettings.skybox = _daySkybox;
            _firstSpotLight.enabled = false;
            _secondSpotLight.enabled = false;
            RenderSettings.ambientLight = _ambientDayLightColor;
            RenderSettings.fogColor = _fogDayColor;
        }
        if (_switchDay == false)
        {
            _directionnalLight.GetComponent<Light>().intensity = _lightIntensityNight;
            RenderSettings.skybox = _nightSkybox;
            _firstSpotLight.enabled = true;
            _secondSpotLight.enabled = true;
            RenderSettings.ambientLight = _ambientNightLightColor;
            RenderSettings.fogColor = _fogNightColor;
        }
    }
}
