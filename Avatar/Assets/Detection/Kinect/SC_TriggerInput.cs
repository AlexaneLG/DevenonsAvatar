using UnityEngine;
using System.Collections;

// brouillon - à compléter si on a le temps pour des inputs plus flexibles/cohérents

public class SC_KinectInputManager : MonoBehaviour {

	private bool b_finger;
	private bool b_hand;
	private bool b_wrist;
	private bool b_elbow;
	private bool b_finger2;
	private bool b_hand2;
	private bool b_wrist2;
	private bool b_elbow2;
	private bool b_head;

	public float result;

	// Use this for initialization
	public virtual void Start () {
		result = 0f;
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}
}
