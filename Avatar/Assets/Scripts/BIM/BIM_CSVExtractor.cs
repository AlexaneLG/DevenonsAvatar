﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BIM_CSVExtractor : MonoBehaviour {

    public TimeDataReplay timeData;
    public ScenarioDataReplay scenarioItemData;
    public KinectDataReplayer kinectData;

    private bool firstExtraction = true;
    public InputField extractDurationField;
    public float extractDuration = 0f;

    private string baseFolder;
    int fileIndex = 0;
    public string CSVfileName = null;

    public Slider extractionSlider;
    public InputField extractionInputField;
    public InputField CSVFileNameInputField;

    public void ExtractCSVFile()
    {
        if(firstExtraction)
        {
            string prefix = System.DateTime.Now.ToString().Replace('/', '-');
            prefix = prefix.Replace(':', '-');
            prefix = prefix.Replace(' ', '_');

            baseFolder = System.IO.Path.Combine(Application.dataPath + "/StreamingAssets", "extract_folder" + "-" + prefix);
            System.IO.Directory.CreateDirectory(baseFolder);

            firstExtraction = false;
        }

        string fileRef;

        if (CSVfileName != "")
        {
            if(fileIndex == 0)
                fileRef = CSVfileName + ".csv";

            else
                fileRef = CSVfileName + "_" + fileIndex + ".csv";
        }
        else
        {
            fileRef = "extract_" + fileIndex + ".csv";
        }

        var fileName = System.IO.Path.Combine(baseFolder, fileRef);
        System.IO.StreamWriter stream = new System.IO.StreamWriter(fileName);

        fileIndex++;

        // Time sensor data
        ExtractRecordings(stream, timeData.timeData_Replay, timeData.timeData_Replay.sensorName);

        // Scenario sensor data
        ExtractRecordings(stream, scenarioItemData.scenarioData_Replay, scenarioItemData.scenarioData_Replay.sensorName);

        // Kinect sensor data
        ExtractRecordings(stream, kinectData.arm_Forearm_R, kinectData.arm_Forearm_R.sensorName);
        ExtractRecordings(stream, kinectData.arm_Forearm_L, kinectData.arm_Forearm_L.sensorName);
        ExtractRecordings(stream, kinectData.arm_Torso_R, kinectData.arm_Torso_R.sensorName);
        ExtractRecordings(stream, kinectData.arm_Torso_L, kinectData.arm_Torso_L.sensorName);
        ExtractRecordings(stream, kinectData.thigh_Calf_R, kinectData.thigh_Calf_R.sensorName);
        ExtractRecordings(stream, kinectData.thigh_Calf_L, kinectData.thigh_Calf_L.sensorName);
        ExtractRecordings(stream, kinectData.torso_Tighs, kinectData.torso_Tighs.sensorName);

        ExtractRecordings(stream, kinectData.shouldersVerticalRotation, kinectData.shouldersVerticalRotation.sensorName);
        ExtractRecordings(stream, kinectData.shouldersLateralRotation, kinectData.shouldersLateralRotation.sensorName);
        ExtractRecordings(stream, kinectData.hipRotation, kinectData.hipRotation.sensorName);

        stream.Close();

        Debug.Log(fileName);
    }

    public void ExtractRecordings(StreamWriter writer, SensorGeneric<float> sensor, string sensorName)
    {
        writer.Write(sensorName);

        int extractDurationIndex = (int)(Mathf.Round(extractDuration * (1 / Time.fixedDeltaTime)));

        for (int i = TimeDataReplay.dataTimeIdx - (extractDurationIndex / 2); i <= TimeDataReplay.dataTimeIdx + (extractDurationIndex / 2); i++)
        {
            if (i >= 0 && i < sensor.maxDataIndex)
            {
                writer.Write(",");
                writer.Write(sensor.values[i].ToString());
            }

            else
            {
                writer.Write(",");
                writer.Write("0");
            }
        }
        writer.Write(writer.NewLine);
    }

    public void adjustExtractionDuration (float newExtractionDuration)
    {
        extractDuration = newExtractionDuration;
        extractionInputField.text = extractDuration.ToString();
    }

    public void adjustExtractionDuration_STR(string newExtractionDuration)
    {
        extractDuration = float.Parse(newExtractionDuration);
        extractionSlider.value = extractDuration;
    }

    public void changeCSVFileName_STR(string newCSVFileName)
    {
        CSVfileName = newCSVFileName;
        fileIndex = 0;
    }
}