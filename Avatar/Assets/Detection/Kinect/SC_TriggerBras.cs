using UnityEngine;
using System.Collections;

public class SC_TriggerBras : MonoBehaviour 
{
	private bool b_finger;
	private bool b_hand;
	private bool b_wrist;
	private bool b_elbow;
	private bool b_finger2;
	private bool b_hand2;
	private bool b_wrist2;
	private bool b_elbow2;

	public Transform self;
	public float resultSpread, resultAngle;

    public bool readyToFly = false;

	// cheat codes pour x1.1 et x0.9

	// Use this for initialization
	void Start () {
		resultSpread = 0;
		resultAngle = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if((b_finger && b_hand && b_wrist && b_elbow && b_finger2 && b_hand2 && b_wrist2 && b_elbow2) || Input.GetKey(KeyCode.UpArrow))
		{
            readyToFly = true;
			//Debug.Log("YouFly");
//			Debug.Log(transform.rotation.eulerAngles);
//			SC_CharacterController.b_IsGoingUp = true;
			resultSpread = 1f;
			if(Input.GetAxis ("Horizontal") == 0)
			{
				Vector2 selfRight = new Vector2(self.right.x, self.right.y);
				resultAngle = Vector2.Angle(selfRight, Vector2.right);
				Vector3 cross = Vector3.Cross(selfRight, Vector2.right);
				if (cross.z < 0) resultAngle *= -1;
			}
			else
			{
				resultAngle = Input.GetAxis ("Horizontal") * 50;
			}
		}
		else
		{
			//Debug.Log("YouDont");
//			SC_CharacterController.b_IsGoingUp = false;
			resultSpread = 0;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.name == "07_Hand_Left")
			b_finger = true;
		else if(other.name == "06_Wrist_Left")
			b_hand = true;
		else if(other.name == "05_Elbow_Left")
			b_wrist = true;
		else if(other.name == "04_Shoulder_Left")
			b_elbow = true;
		else if(other.name == "11_Hand_Right")
			b_finger2 = true;
		else if(other.name == "10_Wrist_Right")
			b_hand2 = true;
		else if(other.name == "09_Elbow_Right")
			b_wrist2 = true;
		else if(other.name == "08_Shoulder_Right")
			b_elbow2 = true;
	}

	void OnTriggerExit(Collider other)
	{
		if(other.name == "07_Hand_Left")
			b_finger = false;
		else if(other.name == "06_Wrist_Left")
			b_hand = false;
		else if(other.name == "05_Elbow_Left")
			b_wrist = false;
		else if(other.name == "04_Shoulder_Left")
			b_elbow = false;
		else if(other.name == "11_Hand_Right")
			b_finger2 = false;
		else if(other.name == "10_Wrist_Right")
			b_hand2 = false;
		else if(other.name == "09_Elbow_Right")
			b_wrist2 = false;
		else if(other.name == "08_Shoulder_Right")
			b_elbow2 = false;
	}
}
