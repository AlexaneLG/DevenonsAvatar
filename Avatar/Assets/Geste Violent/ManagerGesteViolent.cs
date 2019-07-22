using UnityEngine;
using System.Collections;

public class ManagerGesteViolent : MonoBehaviour
{
    public string userName;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartApplication()
    {
        if (!SensorRecorderManager.instance.isRecording)
        {
            SensorRecorderManager.instance.startRecording();
        }
    }

    public void RestartApplication()
    {
        Application.LoadLevel(0);
    }

    public void Dumping()
    {
        SensorRecorderManager.instance.Dump();
        SensorRecorderManager.instance.DumpColumns();
    }
}
