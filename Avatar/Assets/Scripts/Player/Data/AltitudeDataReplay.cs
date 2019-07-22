using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AltitudeDataReplay : MonoBehaviour {

    public SensorGeneric<float> altitude_Replay = new SensorGeneric<float>("Altitude");

    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    void Awake()
    {
        string path = Application.dataPath + "/StreamingAssets/dump.csv";
        string data = System.IO.File.ReadAllText(path);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            {
                var valuesTab = Regex.Split(lines[i], SPLIT_RE);

                if (valuesTab[0] == "Altitude")
                {
                    altitude_Replay.maxDataIndex = valuesTab.Length - 1;

                    for (var y = 1; y < valuesTab.Length; y++)
                        altitude_Replay.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }
            }
        }
    }
}
