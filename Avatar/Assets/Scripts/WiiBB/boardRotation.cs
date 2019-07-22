using UnityEngine;
using System.Collections;

public class boardRotation : MonoBehaviour
{

	private float perc;
	private float perc_2;
	public float speed;
	private Transform backAngle;
	private bool hasTurn;
	public float _angleRotate;
	private 
		
	void Update () 
	{
		
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			backAngle = this.transform;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			backAngle = this.transform;
		}
         
		if (perc < 1 && Input.GetKey(KeyCode.LeftArrow))
        {
			perc += Time.deltaTime * speed;
			transform.localRotation = Quaternion.Lerp(backAngle.localRotation,Quaternion.Euler (0,0, 1 * _angleRotate) , perc);
		}
		
		if (perc < 1 && Input.GetKey(KeyCode.RightArrow))
		{ 
			perc += Time.deltaTime * speed;
			transform.localRotation = Quaternion.Lerp(backAngle.localRotation, Quaternion.Euler (0,0,-1 * _angleRotate), perc);
		}
		
		else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
		{
			hasTurn = true;
			perc = 0;
			perc_2 = 0;
			backAngle = this.transform;
		}
		
		if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && hasTurn && perc_2 < 1) 
		{
			perc_2 += Time.deltaTime * speed;
			transform.localRotation = Quaternion.Lerp(backAngle.localRotation, Quaternion.Euler(0,0,0), perc_2);
		}
	}
}
