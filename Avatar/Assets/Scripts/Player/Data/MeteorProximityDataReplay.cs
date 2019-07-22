using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MeteorProximityDataReplay : MonoBehaviour {

    public List<SensorGeneric<float>> meteorProximity = new List<SensorGeneric<float>>();   

    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    public int meteor_Index = 0;

    void Awake()
    {
        string path = Application.dataPath + "/StreamingAssets/dump.csv";
        string data = System.IO.File.ReadAllText(path);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            {
                var valuesTab = Regex.Split(lines[i], SPLIT_RE);

                if (valuesTab[0].Substring(0, 3) == "Met")
                {
                    meteorProximity.Add(new SensorGeneric<float>(valuesTab[0].ToString()));

                    meteorProximity[meteor_Index].maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        meteorProximity[meteor_Index].AddRecordedValue(float.Parse(valuesTab[y].ToString()));

                    meteor_Index++;
                }
            }
        }
    }
}
