using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;


public class CollisionEventDataReplay : MonoBehaviour
{
    public SensorGeneric<float> collisionEvent = new SensorGeneric<float>("CollisionEvent");

    public List<int> collisionIndexList;

    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    void Awake ()
    {
        string path = Application.dataPath + "/StreamingAssets/dump.csv";
        string data = System.IO.File.ReadAllText(path);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        for (int i = 0; i < lines.Length -1; i++)
        {
            {
                var valuesTab = Regex.Split(lines[i], SPLIT_RE);

                if (valuesTab[0] == "CollisionEvent")
                {
                    collisionEvent.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        collisionEvent.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }
            }
        }
        for (int i = 0; i < collisionEvent.maxDataIndex; i++)
        {
            if(collisionEvent.values[i] == 1)
            {
                collisionIndexList.Add(i);
            }
        }
    }
}
