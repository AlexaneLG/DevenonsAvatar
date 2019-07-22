using UnityEngine;
using System.Collections;

public class SwitchToFPS : MonoBehaviour
{
	public Transform FPS, TPS, MID, self;
    public int positionRef = 0;
	private bool isFPS = false; 
    public bool switchPointOfVue = false;
    public bool automaticSwitch = false;

    public float cameraDistance = -4f;
    public float cameraAltitude = 1f;

    private float perc = 0;
    public float speed = 0;

	void Start ()
	{
		isFPS = false;

        TPS.localPosition = new Vector3(TPS.localPosition.x, cameraAltitude, cameraDistance);
        self.localPosition = TPS.localPosition;
	}
	
	void Update ()
	{
		if(Input.GetKeyDown (KeyCode.Tab))
		{
            switchPointOfVue = true;
		}

        if (switchPointOfVue && positionRef == 0)
        {
            if (perc < 1)
            {
                perc += Time.deltaTime * speed;
                self.localPosition = Vector3.Lerp(TPS.localPosition, MID.localPosition, perc);
                if (self.localPosition == MID.localPosition)
                {
                    switchPointOfVue = false;
                    positionRef = 1;
                    perc = 0;
                }
            }
        }

        else if (switchPointOfVue && positionRef == 1 && automaticSwitch)
        {
            if (perc < 1)
            {
                perc += Time.deltaTime * speed;
                self.localPosition = Vector3.Lerp(MID.localPosition, TPS.localPosition, perc);
                if (self.localPosition == TPS.localPosition)
                {
                    switchPointOfVue = false;
                    automaticSwitch = false;
                    positionRef = 0;
                    perc = 0;
                }
            }
        }

        else if (switchPointOfVue && positionRef == 1 && !automaticSwitch)
        {
            if (perc < 1)
            {
                perc += Time.deltaTime * speed;
                self.localPosition = Vector3.Lerp(MID.localPosition, FPS.localPosition, perc);
                if (self.localPosition == FPS.localPosition)
                {
                    switchPointOfVue = false;
                    positionRef = 2;
                    perc = 0;
                }
            }
        }

        else if (switchPointOfVue && positionRef == 2)
        {
            if (perc < 1)
            {
                perc += Time.deltaTime * speed;
                self.localPosition = Vector3.Lerp(FPS.localPosition, TPS.localPosition, perc);
                if (self.localPosition == TPS.localPosition)
                {
                    switchPointOfVue = false;
                    positionRef = 0;
                    perc = 0;
                }
            }
        }
	}
  
    public void AdjustCameraDistance(float newCameraDistance)
    {
        if(!isFPS)
        {
            TPS.localPosition = new Vector3(TPS.localPosition.x, TPS.localPosition.y, newCameraDistance);
            self.localPosition = TPS.localPosition;
        }
    }

    public void AdjustCameraAltitude(float newCameraAltitude)
    {
        if(!isFPS)
        {
            TPS.localPosition = new Vector3(TPS.localPosition.x, newCameraAltitude, TPS.localPosition.z);
            self.localPosition = TPS.localPosition;
        }
    }
}
