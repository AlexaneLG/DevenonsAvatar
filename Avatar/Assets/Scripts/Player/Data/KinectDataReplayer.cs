using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;


public class KinectDataReplayer : MonoBehaviour {

    public SensorGeneric<float> arm_Forearm_R = new SensorGeneric<float>("arm_Forearm_R");
    public SensorGeneric<float> arm_Forearm_L = new SensorGeneric<float>("arm_Forearm_L");
    public SensorGeneric<float> arm_Torso_R = new SensorGeneric<float>("arm_Torso_R");
    public SensorGeneric<float> arm_Torso_L = new SensorGeneric<float>("arm_Torso_L");
    public SensorGeneric<float> thigh_Calf_R = new SensorGeneric<float>("thigh_Calf_R");
    public SensorGeneric<float> thigh_Calf_L = new SensorGeneric<float>("thigh_Calf_L");
    public SensorGeneric<float> torso_Tighs = new SensorGeneric<float>("torso_Tighs");

    public SensorGeneric<float> shouldersVerticalRotation = new SensorGeneric<float>("shouldersVerticalRotation");
    public SensorGeneric<float> shouldersLateralRotation = new SensorGeneric<float>("shouldersLateralRotation");
    public SensorGeneric<float> hipRotation = new SensorGeneric<float>("hipRotation");

    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

	void Start () 
    {
        string path = Application.dataPath + "/StreamingAssets/dump.csv";
        string data = System.IO.File.ReadAllText(path);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            {
                var valuesTab = Regex.Split(lines[i], SPLIT_RE);

                if (valuesTab[0] == "arm_Forearm_R")
                {
                    arm_Forearm_R.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        arm_Forearm_R.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "arm_Forearm_L")
                {
                    arm_Forearm_L.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        arm_Forearm_L.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "arm_Torso_R")
                {
                    arm_Torso_R.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        arm_Torso_R.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "arm_Torso_L")
                {
                    arm_Torso_L.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        arm_Torso_L.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "thigh_Calf_R")
                {
                    thigh_Calf_R.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        thigh_Calf_R.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "thigh_Calf_L")
                {
                    thigh_Calf_L.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        thigh_Calf_L.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "torso_Tighs")
                {
                    torso_Tighs.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        torso_Tighs.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "shouldersVerticalRotation")
                {
                    shouldersVerticalRotation.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        shouldersVerticalRotation.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "shouldersLateralRotation")
                {
                    shouldersLateralRotation.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        shouldersLateralRotation.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "hipRotation")
                {
                    hipRotation.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        hipRotation.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }
            }
        }
    }
}
