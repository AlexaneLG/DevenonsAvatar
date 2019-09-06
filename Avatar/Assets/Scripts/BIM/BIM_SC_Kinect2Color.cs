using UnityEngine;
using System.Collections;

public class BIM_SC_Kinect2Color : MonoBehaviour {


	void Start()
	{
        KinectManager manager = KinectManager.Instance;
        Debug.Log(manager);
        if (manager && manager.IsInitialized())
        {
            //GetComponent<Renderer>().material.mainTexture = manager.GetUsersClrTex();
            GetComponent<Renderer>().material.mainTexture = Resources.Load("bim_video"); // video in a texture
        }
	}
    
	void Update () 
	{
		
	}
    
}
