using UnityEngine;
using System.Collections;

public class SC_Kinect2Color : MonoBehaviour {


	void Start()
	{
        KinectManager manager = KinectManager.Instance;
        Debug.Log(manager);
        if (manager && manager.IsInitialized())
        {
            GetComponent<Renderer>().material.mainTexture = manager.GetUsersClrTex();
        }
	}
    
	void Update () 
	{
		
	}
    
}
