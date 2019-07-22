using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

using RockVR.Video;

public class BlackMagic : MonoBehaviour
{

    VideoCapture vc = null;
    VideoCaptureCtrl vcc = null;

    void Awake()
    {

        var camera = Camera.main;


        if (camera.GetComponent<VideoCapture>() == null)
        {
            vc = camera.gameObject.AddComponent<VideoCapture>();

            vc.isDedicated = false;
            vc.captureGUI = false;
            vc.customPath = true;

            vc.eventDelegate.OnComplete += SensorRecorderManager.instance.OnCaptureComplete;
        }

        if (GetComponent<VideoCaptureCtrl>() == null)
        {
            vcc = gameObject.AddComponent<VideoCaptureCtrl>();
            vcc.videoCaptures = new VideoCapture[1];

            vcc.videoCaptures[0] = vc;
        }


    }


    // Use this for initialization
    void Start()
    {

    }

    void OnDisable()
    {
        //vcc.StopCapture();
    }


    public void SetRecord(bool record)
    {
        if (record)
        {
            Debug.Log("Start Video Capture");
            vcc.StartCapture();
        }
        else
        {
            Debug.Log("Stop Video Capture");
            vcc.StopCapture();

        }
    }

    public void SetFileDestination(string file)
    {
        file = file.Replace('/', '\\');

        var path = System.IO.Path.GetDirectoryName(file) + System.IO.Path.DirectorySeparatorChar;
        PathConfig.SaveFolder = path;
        Debug.Log("Saving video to : " + path);

    }

}
