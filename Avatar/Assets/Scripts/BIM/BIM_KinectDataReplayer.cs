using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;


public class BIM_KinectDataReplayer : MonoBehaviour
{

    // BIM required datas
    public SensorGeneric<float> hand_Left_X = new SensorGeneric<float>("hand_Left_X");
    public SensorGeneric<float> hand_Left_Y = new SensorGeneric<float>("hand_Left_Y");
    public SensorGeneric<float> hand_Right_X = new SensorGeneric<float>("hand_Right_X");
    public SensorGeneric<float> hand_Right_Y = new SensorGeneric<float>("hand_Right_Y");
    public SensorGeneric<float> shoulder_center_X = new SensorGeneric<float>("shoulder_center_X");
    public SensorGeneric<float> shoulder_center_Y = new SensorGeneric<float>("shoulder_center_Y");

    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    void Start()
    {
        Debug.Log("Start kinect replay");
        string path = Application.dataPath + "/BIM_Data/dump.csv";
        string data = System.IO.File.ReadAllText(path);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            {
                var valuesTab = Regex.Split(lines[i], SPLIT_RE);
                //Debug.Log("Values tab 0 :" + valuesTab[0] + " ; lenght : " + valuesTab.Length);

                if (valuesTab[0] == "hand_Left_X")
                {
                    hand_Left_X.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        hand_Left_X.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "hand_Left_Y")
                {
                    hand_Left_Y.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        hand_Left_Y.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "hand_Right_X")
                {
                    hand_Right_X.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        hand_Right_X.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "hand_Right_Y")
                {
                    hand_Right_Y.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        hand_Right_Y.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "hip_center_X")
                {
                    shoulder_center_X.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        shoulder_center_X.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "hip_center_Y")
                {
                    shoulder_center_Y.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        shoulder_center_Y.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }
            }
        }

        // Debug
        //Debug.Log("Display kinect datas, size : " + shoulder_center_X.values.Count);

    }
}
