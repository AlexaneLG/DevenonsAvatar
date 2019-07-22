using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class TimeDataRecorder : MonoBehaviour
{
    public TimeDataCollection dataCollection = new TimeDataCollection();
    public double currentTime;
    Stopwatch _stopWatch;

    public void StartRecording()
    {
        _stopWatch = Stopwatch.StartNew();
    }
        
    void FixedUpdate()
    {
        if (SensorRecorderManager.startRecordingSensorData)
        {
            //currentTime = _stopWatch.ElapsedMilliseconds * 0.001;
            currentTime = (double) _stopWatch.ElapsedTicks / (double)Stopwatch.Frequency;
            dataCollection.TimeData.AddRecordedValue(currentTime);

        }
    }

}
