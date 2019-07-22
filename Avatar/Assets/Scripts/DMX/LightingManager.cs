using UnityEngine;
using System.Collections;

/// <summary>
/// An example lighting manager using the DMX class to interface with an Enttec DMX USB Pro.
/// Requires one RGB 3-channel fixture connected as device 1 (i.e. channels 1 through 3).
/// </summary>
/// <remarks>
/// Author: Bryan Maher (bm3n@andrew.cmu.edu) 26-Jan-2015
/// 
/// Feel free to use this example code as starting point for your own project.
/// </remarks>
public class LightingManager : MonoBehaviour {

    public string DmxComNumber;

    public float LightIntensity;
    public float LightTemperature;

    void Start()
    {
        LightIntensity = 128.0f;
        LightTemperature = 128.0f;
    }

    void OnEnable()
    {
        DMXAsyncCom.start(DmxComNumber);
    }

    void OnDisable()
    {
    }

	// Use this for initialization
    void Update()
    {
        DMXAsyncCom.setDmxValue(0, (byte)(Mathf.FloorToInt(LightIntensity)));
        //DMXAsyncCom.setDmxValue(1, 0);
        DMXAsyncCom.setDmxValue(2, (byte)(Mathf.FloorToInt(LightTemperature)));
    }

    public void adjustLightIntensity(float newLightIntensity)
    {
        LightIntensity = newLightIntensity;
    }

    public void adjustLightTemperature(float newLightTemperature)
    {
        LightTemperature = newLightTemperature;
    }
}
