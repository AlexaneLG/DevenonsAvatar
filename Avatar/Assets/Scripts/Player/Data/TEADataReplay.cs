using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;


public class TEADataReplay : MonoBehaviour {

    public List<SensorGeneric<float>> EMG_DataReplay = new List<SensorGeneric<float>>();   
    public SensorGeneric<float> ECG_DataReplay = new SensorGeneric<float>("ECG");
    public SensorGeneric<float> RESP_DataReplay = new SensorGeneric<float>("RESP");
    public SensorGeneric<float> GSR_DataReplay = new SensorGeneric<float>("GSR");

    public DataDrawerReplay[] EMG_Drawer;
    public DataDrawerReplay ECG_Drawer;
    public DataDrawerReplay RESP_Drawer;
    public DataDrawerReplay GSR_Drawer;

    public Text[] EMG_SensorName;
    public Text ECG_SensorName;
    public Text RESP_SensorName;
    public Text GSR_SensorName;

    public int EMG_Index = 0;

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

                if (valuesTab[0] == "ECG")
                {
                    ECG_SensorName.text = valuesTab[0].ToString();
                    ECG_DataReplay.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        ECG_DataReplay.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "GSR")
                {
                    GSR_SensorName.text = valuesTab[0].ToString();
                    GSR_DataReplay.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        GSR_DataReplay.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0] == "RESP")
                {
                    RESP_SensorName.text = valuesTab[0].ToString();
                    RESP_DataReplay.maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        RESP_DataReplay.AddRecordedValue(float.Parse(valuesTab[y].ToString()));
                }

                else if (valuesTab[0].Substring(0, 3) == "EMG")
                {
                    EMG_DataReplay.Add(new SensorGeneric<float>(valuesTab[0].ToString()));

                    if(EMG_Index < EMG_SensorName.Length)
                        EMG_SensorName[EMG_Index].text = valuesTab[0].ToString();

                    EMG_DataReplay[EMG_Index].maxDataIndex = valuesTab.Length - 1;
                    for (var y = 1; y < valuesTab.Length; y++)
                        EMG_DataReplay[EMG_Index].AddRecordedValue(float.Parse(valuesTab[y].ToString()));    

                    EMG_Index++;
                }
            }
        }
    }

	void Start () 
    {
        for (int i = 0; i < EMG_Index ; i++)
        {
            EMG_Drawer[i].dataSource = EMG_DataReplay[i];
        }

        ECG_Drawer.dataSource = ECG_DataReplay;
        GSR_Drawer.dataSource = GSR_DataReplay;
        RESP_Drawer.dataSource = RESP_DataReplay;
	} 
}
