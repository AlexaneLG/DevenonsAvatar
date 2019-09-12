using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class BIM_SC_Kinect2Color : MonoBehaviour {

    public VideoClip videoClip;
    public VideoPlayer videoPlayer;

	void Start()
	{
        KinectManager manager = KinectManager.Instance;
        Debug.Log(manager);
        if (manager && manager.IsInitialized())
        {
            //GetComponent<Renderer>().material.mainTexture = manager.GetUsersClrTex();

            videoPlayer = gameObject.GetComponent<VideoPlayer>();
            videoPlayer.clip = videoClip;
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
            videoPlayer.targetMaterialProperty = "_MainTex";
            videoPlayer.Play();

            //movieTexture = Resources.Load("bim_video");
            //GetComponent<Renderer>().material.mainTexture = movieTexture; // video in a texture

        }
	}
    
	void Update () 
	{
		
	}
    
}
