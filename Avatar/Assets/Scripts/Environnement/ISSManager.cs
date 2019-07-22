using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ISSManager : MonoBehaviour {

    public Slider speed;
    public Slider altitudeFactor;

    public float proximitySpeed;
    public float proximityAltitudeFactor;

    private float initialSpeed;
    private float initialAltitudeFactor;

    void Start()
    {
        initialSpeed = speed.value;
        initialAltitudeFactor = altitudeFactor.value;
    }

    void OnTriggerEnter(Collider other)
    {     
        speed.value = proximitySpeed;
        altitudeFactor.value = proximityAltitudeFactor;
    }

    void OnTriggerExit(Collider other)
    {
        speed.value = initialSpeed;
        altitudeFactor.value = initialAltitudeFactor;
    }
}
