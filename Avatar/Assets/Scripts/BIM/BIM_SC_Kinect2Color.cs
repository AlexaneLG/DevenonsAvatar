using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class BIM_SC_Kinect2Color : MonoBehaviour
{

    public VideoClip videoClip;
    public VideoPlayer videoPlayer;
    public Slider videoSlider;

    void Start()
    {
        KinectManager manager = KinectManager.Instance;
        Debug.Log(manager);
        if (manager && manager.IsInitialized())
        {

            videoPlayer = gameObject.GetComponent<VideoPlayer>();
            videoPlayer.clip = videoClip;
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
            videoPlayer.targetMaterialProperty = "_MainTex";
            videoPlayer.Play();
            videoSlider.maxValue = (float)videoClip.length;
            videoSlider.value = (float)videoPlayer.time;

        }
    }

    void Update()
    {
        videoSlider.value = (float)videoPlayer.time;
    }

}
