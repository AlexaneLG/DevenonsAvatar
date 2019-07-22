using UnityEngine;
using System.Collections;
using Vectrosity;

//[ExecuteInEditMode]
public class AvatarUI : MonoBehaviour
{

    void Start()
    {
        VectorLine.SetCamera(GetComponent<Camera>());
    }

    public void StartApplication()
    {
        CharacterControllerBasedOnAxis.instance.StartScenario();
    }

    public void RestartApplication()
    {
        try
        {
            SensorRecorderManager.instance.endRecording();
        }
        catch
        {

        }
        Application.LoadLevel(0);
    }

    public void dumpAndQuitApplication()
    {
        SensorRecorderManager.instance.endRecording();
        Invoke("quitApplication", 1F);
    }

    public void Dumping()
    {
        SensorRecorderManager.instance.Dump();
        SensorRecorderManager.instance.DumpColumns();

    }

    public void SaveUserName(string newSubject)
    {
        SensorRecorderManager.instance.subjectName = newSubject;
    }

    public void quitApplication()
    {
        Application.Quit();
    }
}
