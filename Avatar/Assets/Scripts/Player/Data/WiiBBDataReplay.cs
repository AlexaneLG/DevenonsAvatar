using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class WiiBBDataReplay : MonoBehaviour
{
    public SensorGeneric<float> X_CP_DataReplay = new SensorGeneric<float>("X_CP");
    public SensorGeneric<float> Y_CP_DataReplay = new SensorGeneric<float>("Y_CP");

    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    public Text X_CP_Text;
    public Text Y_CP_Text;

    public Transform visualPressionCenter;

    void Awake ()
    {
        string path = Application.dataPath + "/StreamingAssets/dump.csv";
        string data = System.IO.File.ReadAllText(path);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            {
                var valuesTab = Regex.Split(lines[i], SPLIT_RE);

                if (valuesTab[0] == "X CP")
                {
                    X_CP_DataReplay.maxDataIndex = valuesTab.Length - 1;

                    for (var y = 1; y < valuesTab.Length; y++)
                        X_CP_DataReplay.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "Y CP")
                {
                    Y_CP_DataReplay.maxDataIndex = valuesTab.Length - 1;

                    for (var y = 1; y < valuesTab.Length; y++)
                        Y_CP_DataReplay.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }
            }
        }
    }

    void Update ()
    {
        if ((TimeDataReplay.dataTimeIdx) >= 0 && (TimeDataReplay.dataTimeIdx) < X_CP_DataReplay.maxDataIndex && (TimeDataReplay.dataTimeIdx) < Y_CP_DataReplay.maxDataIndex)
        {
            X_CP_Text.text = X_CP_DataReplay.values[TimeDataReplay.dataTimeIdx].ToString();
            Y_CP_Text.text = Y_CP_DataReplay.values[TimeDataReplay.dataTimeIdx].ToString();

            visualPressionCenter.localPosition = new Vector3(X_CP_DataReplay.values[TimeDataReplay.dataTimeIdx], Y_CP_DataReplay.values[TimeDataReplay.dataTimeIdx], 0.0f);
        }

        else
        {
            X_CP_Text.text = "0";
            Y_CP_Text.text = "0";
        }
    }
}
