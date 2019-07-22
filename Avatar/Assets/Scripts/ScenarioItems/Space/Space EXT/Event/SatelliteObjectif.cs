using UnityEngine;
using System.Collections;

public class SatelliteObjectif : MonoBehaviour {

    public static bool isCollideByPlayer;

	public Light leftLight;
	public Light rightLight;
	public Transform target;

	private Color color1 = Color.red;
	private Color color2 = Color.black;
	private Transform myTransform;

	[Range (0f, 5f)]
	public float colorSwitchDuration = 0.5f;

	[Range (-2f, 0f)]
	public float satelliteSpeed = -0.75f;


	void Start () 
	{
		myTransform = transform;
	}


	void Update () 
	{
		// Ping Pong between two colors
		float lerp = Mathf.PingPong (Time.time, colorSwitchDuration) / colorSwitchDuration;
		leftLight.color = Color.Lerp (color1, color2, lerp);
		rightLight.color = Color.Lerp (color1, color2, lerp);

		// Déplacement du satellite
		myTransform.RotateAround (target.position, Vector3.up, Time.deltaTime * satelliteSpeed);
		myTransform.LookAt (target.position);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isCollideByPlayer = true;
        }
    }
}
