﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class TimeDataReplay : MonoBehaviour {

    public Slider videoSlider;
    public static bool usingSlider = false;

    public static int dataTimeIdx = -1;
    public float timeOffset = 0.55f;

    public SensorGeneric<float> timeData_Replay = new SensorGeneric<float>("TimeIdx");

    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    void Awake ()
    {
        string path = Application.dataPath + "/StreamingAssets/dump.csv";
        string data = System.IO.File.ReadAllText(path);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            {
                var valuesTab = Regex.Split(lines[i], SPLIT_RE);

                if (valuesTab[0] == "TimeIdx")
                {
                    timeData_Replay.maxDataIndex = valuesTab.Length - 1;

                    for (var y = 1; y < valuesTab.Length; y++)
                        timeData_Replay.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }
            }
        }
    }

    void Update ()
    {
        if (videoSlider.value >= timeData_Replay.values[timeData_Replay.maxDataIndex - 1])
            dataTimeIdx = -1;

        else
        {
            for (var i = 1; i < timeData_Replay.maxDataIndex; i++)
            {
                if (videoSlider.value + timeOffset >= timeData_Replay.values[i])
                {
                    dataTimeIdx = i;
                }
            } 
        }
        
    }

    public void OnPointerDown()
    {
        usingSlider = true;
    }

    public void OnPointerUp()
    {
        usingSlider = false;
    }

    public void adjustTimeOffset(string newTimeOffset)
    {
        timeOffset = float.Parse(newTimeOffset);
    }
}
