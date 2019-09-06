using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class BIM_ScenarioItemDataReplayer : MonoBehaviour {

    public int currentScenarioItem;
    public BIM_TimeDataReplayer timeDataReplayer;

    public SensorGeneric<float> scenarioData_Replay = new SensorGeneric<float>("Scenario");

    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    void Awake ()
    {
        string path = Application.dataPath + "/BIM_Data/dump.csv";
        string data = System.IO.File.ReadAllText(path);

        var lines = Regex.Split(data, LINE_SPLIT_RE);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            {
                var valuesTab = Regex.Split(lines[i], SPLIT_RE);

                if (valuesTab[0] == "Scenario")
                {
                    scenarioData_Replay.maxDataIndex = valuesTab.Length - 1;

                    for (var y = 1; y < valuesTab.Length; y++)
                        scenarioData_Replay.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }
            }  
        }
    }

    private void Start()
    {
        if (timeDataReplayer == null && GameObject.Find("TimeDataReplayer"))
        {
            timeDataReplayer = GameObject.Find("TimeDataReplayer").GetComponent<BIM_TimeDataReplayer>();
        }
        else
        {
            Debug.Log("Can not find TimeDataReplayer");
        }
    }

    void Update ()
    {
        if(timeDataReplayer.dataTimeIdx > -1)
        {
            currentScenarioItem = (int)scenarioData_Replay.values[timeDataReplayer.dataTimeIdx];
        }
    }
}
