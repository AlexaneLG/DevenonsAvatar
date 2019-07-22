using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class InitStreamingVideo : MonoBehaviour {

	public VideoPlayer videoPlayer;
	public string VideoFileName = "VideoTransitionCasque.mp4";

	void Awake(){
		videoPlayer.url = System.IO.Path.Combine( Application.streamingAssetsPath, VideoFileName);
	}

}
