using UnityEngine;
using System.Collections;

public class Symmetry : MonoBehaviour {

	static public bool enableAvatarSymmetry;

    public float timeBeforeDisableSymmetry = 15;
    private bool disableAvatarSymmetry;

	private float finalScale;
	private float currentScale;
	public float scaleSpeed;

	void Start()
	{
		finalScale =  transform.localScale.x * -1;
		currentScale = transform.localScale.x ;
	}

	void Update()
	{
        if (enableAvatarSymmetry)
		{
            WiiController.directionSwitch = -1;

			if (currentScale > finalScale)
			{
				currentScale -= scaleSpeed * Time .deltaTime;
				transform.localScale = new Vector3 (currentScale, transform.localScale.y, transform.localScale.z);  
			}

            else if (currentScale <= finalScale)
            {
                StartCoroutine("DisableSymmetry");
                enableAvatarSymmetry = false;
            }
		}

        if (currentScale < (finalScale * -1) && disableAvatarSymmetry)
        {
            currentScale += scaleSpeed * Time.deltaTime;
            transform.localScale = new Vector3(currentScale, transform.localScale.y, transform.localScale.z);

            WiiController.directionSwitch = 1;
        }
    }

    IEnumerator DisableSymmetry()
    {
        yield return new WaitForSeconds(timeBeforeDisableSymmetry);

        disableAvatarSymmetry = true;
    }
}
