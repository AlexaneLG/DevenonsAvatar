
//#define DUMP_REAL_TIME

using System.Collections;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using MsgPack.Serialization;

using System.IO;
using System.Globalization;

using UnityEngine.UI;

public struct TEAData
{
    public long packetIndex;
    public int sensorID;
    public string sensorName;
    public double[] timeStamps;
    public float[][] values;

}

public enum teaCommandEnum
{
    START = 0x1234,
    STOP,
    TRIGGER,
}

public class teaCommand
{
    public teaCommandEnum command;
    public float data;
}


public class TEASensorDataCollection : SensorDataCollection
{
    public SensorGeneric<float> EMG_1 = new SensorGeneric<float>("EMG 1");
    public SensorGeneric<float> EMG_2 = new SensorGeneric<float>("EMG 2");
    public SensorGeneric<float> EMG_3 = new SensorGeneric<float>("EMG 3");
    public SensorGeneric<float> EMG_4 = new SensorGeneric<float>("EMG 4");
    public SensorGeneric<float> EMG_5 = new SensorGeneric<float>("EMG 5");
    public SensorGeneric<float> EMG_6 = new SensorGeneric<float>("EMG 6");
    public SensorGeneric<float> EMG_7 = new SensorGeneric<float>("EMG 7");
    public SensorGeneric<float> EMG_8 = new SensorGeneric<float>("EMG 8");

    public SensorGeneric<double> EMG_1_Time = new SensorGeneric<double>("EMG 1 Time");
    public SensorGeneric<double> EMG_2_Time = new SensorGeneric<double>("EMG 2 Time");
    public SensorGeneric<double> EMG_3_Time = new SensorGeneric<double>("EMG 3 Time");
    public SensorGeneric<double> EMG_4_Time = new SensorGeneric<double>("EMG 4 Time");
    public SensorGeneric<double> EMG_5_Time = new SensorGeneric<double>("EMG 5 Time");
    public SensorGeneric<double> EMG_6_Time = new SensorGeneric<double>("EMG 6 Time");
    public SensorGeneric<double> EMG_7_Time = new SensorGeneric<double>("EMG 7 Time");
    public SensorGeneric<double> EMG_8_Time = new SensorGeneric<double>("EMG 8 Time");


    public SensorGeneric<float> ECG = new SensorGeneric<float>("ECG");
    public SensorGeneric<float> GSR = new SensorGeneric<float>("GSR");
    public SensorGeneric<float> RESP = new SensorGeneric<float>("RESP");

    public SensorGeneric<double> ECG_Time = new SensorGeneric<double>("ECG Time");
    public SensorGeneric<double> GSR_Time = new SensorGeneric<double>("GSR Time");
    public SensorGeneric<double> RESP_Time = new SensorGeneric<double>("RESP Time");


    public SensorGeneric<Vector3> MOTION_1 = new SensorGeneric<Vector3>("MOTION 1");
    public SensorGeneric<Vector3> MOTION_2 = new SensorGeneric<Vector3>("MOTION 2");

    public SensorGeneric<double> MOTION_1_Time = new SensorGeneric<double>("MOTION 1 Time");
    public SensorGeneric<double> MOTION_2_Time = new SensorGeneric<double>("MOTION 2 Time");

    public SensorGeneric<int> electroMagnet = new SensorGeneric<int>("ElectroMagnet");


    public TEASensorDataCollection()
        : base("TEA")
    {

        MOTION_1.defaultValue = Vector3.zero.ToString();
        MOTION_2.defaultValue = Vector3.zero.ToString();

        AddSensor(EMG_1_Time);
        AddSensor(EMG_1);
        AddSensor(EMG_2_Time);
        AddSensor(EMG_2);
        AddSensor(EMG_3_Time);
        AddSensor(EMG_3);
        AddSensor(EMG_4_Time);
        AddSensor(EMG_4);
        AddSensor(EMG_5_Time);
        AddSensor(EMG_5);
        AddSensor(EMG_6_Time);
        AddSensor(EMG_6);
        AddSensor(EMG_7_Time);
        AddSensor(EMG_7);
        AddSensor(EMG_8_Time);
        AddSensor(EMG_8);

        AddSensor(ECG_Time);
        AddSensor(ECG);

        AddSensor(GSR_Time);
        AddSensor(GSR);

        AddSensor(RESP_Time);
        AddSensor(RESP);

        AddSensor(MOTION_1_Time);
        AddSensor(MOTION_1);

        AddSensor(MOTION_2_Time);
        AddSensor(MOTION_2);

        AddSensor(electroMagnet);
    }

    public void changeEMGSensorName(string newSensorName, int EMG_idx)
    {
        _values[EMG_idx].sensorName = "EMG_" + newSensorName;
    }

}

public class TeaSocket : MonoBehaviour
{
    public string ip = "127.0.0.1";
    public int port = 1408;

    public int packetProcessed = 0;
    public float APPS = 0.0f;
    public int packetsInQueue;
    public double timeStamp;
    public float emg_1, emg_2, emg_3, emg_4, emg_5, emg_6, emg_7, emg_8, ecg, resp, gsr;

    private NetworkStream stream;
    private long packetIndex = -1;


    TcpClient tcpClient;
    System.Threading.Thread readingThread;

    public TEASensorDataCollection dataCollection = new TEASensorDataCollection();

    public DataDrawer EMG1Drawer, EMG2Drawer, EMG3Drawer, EMG4Drawer, EMG5Drawer, EMG6Drawer, EMG7Drawer, EMG8Drawer;
    public DataDrawer ECGDrawer;
    public DataDrawer RESPDrawer;
    public DataDrawer GSRDrawer;

    public Transform motion1, motion2;

    private List<TEAData> _packets = new List<TEAData>(16);

    public void Start()
    {

        EMG1Drawer.dataSource = dataCollection.EMG_1;
        EMG2Drawer.dataSource = dataCollection.EMG_2;
        EMG3Drawer.dataSource = dataCollection.EMG_3;
        EMG4Drawer.dataSource = dataCollection.EMG_4;
        EMG5Drawer.dataSource = dataCollection.EMG_5;
        EMG6Drawer.dataSource = dataCollection.EMG_6;
        EMG7Drawer.dataSource = dataCollection.EMG_7;
        EMG8Drawer.dataSource = dataCollection.EMG_8;
        ECGDrawer.dataSource = dataCollection.ECG;
        GSRDrawer.dataSource = dataCollection.GSR;
        RESPDrawer.dataSource = dataCollection.RESP;

        ActivateDataDrawers(false);


    }

    public void StopRecording()
    {
        CloseSocket();
        ActivateDataDrawers(false);

    }


    public void StartRecording()
    {
        // Connect to TeaServer
        ActivateDataDrawers(false);

        try
        {
            tcpClient = new TcpClient(ip, port);
            stream = tcpClient.GetStream();

            packetIndex = -1;
            SendCommand(teaCommandEnum.START);

        }

        catch (SocketException se)
        {
            Debug.Log("Cannot connect to TEA, disabling all related Data Drawers: " + se.ToString());

        }
    }

    public void ActivateDataDrawers(bool active)
    {
        EMG1Drawer.isSensorActive = active;
        EMG2Drawer.isSensorActive = active;
        EMG3Drawer.isSensorActive = active;
        EMG4Drawer.isSensorActive = active;
        EMG5Drawer.isSensorActive = active;
        EMG6Drawer.isSensorActive = active;
        EMG7Drawer.isSensorActive = active;
        EMG8Drawer.isSensorActive = active;
        ECGDrawer.isSensorActive = active;
        GSRDrawer.isSensorActive = active;
        RESPDrawer.isSensorActive = active;
    }

    void SendCommand(teaCommandEnum command)
    {
        if (stream != null)
        {
            teaCommand c = new teaCommand();
            c.command = command;
            MessagePackSerializer<teaCommand> serializer = MessagePackSerializer.Get<teaCommand>();
            serializer.Pack(stream, c);
        }
    }

    public void OnDisable()
    {
        CloseSocket();
    }


    void CloseSocket()
    {
        if (stream != null)
        {
            SendCommand(teaCommandEnum.STOP);
            System.Threading.Thread.Sleep(500);
            stream.Close();
            stream = null;
        }
        readingThread = null;
    }

    public void Update()
    {
        APPS = (float)packetProcessed / Time.timeSinceLevelLoad;


        if (SensorRecorderManager.startRecordingSensorData)
        {
            if (stream != null && stream.DataAvailable)
            {
                ReadDataFuncSync();
            }

        }
    }


    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SendCommand(teaCommandEnum.TRIGGER);
            dataCollection.electroMagnet.AddRecordedValue(1);
        }
        else
        {
            dataCollection.electroMagnet.AddRecordedValue(0);
        }

    }

    void ReadDataFuncSync()
    {
        packetProcessed++;

        while (stream.DataAvailable)
        {
            var serializer = MessagePackSerializer.Get<TEAData>();
            TEAData d = serializer.Unpack(stream);
            ProcessPacket(d);
        }
    }


    void ProcessPacket(TEAData d)
    {

        switch (d.sensorName)
        {

            case "ECG":
                ECGDrawer.UpdateWithValues256(d.values[0]);
                dataCollection.ECG.AddRecordedValues256(d.values[0]);
                dataCollection.ECG_Time.AddRecordedValues256(d.timeStamps);
                ecg = dataCollection.ECG.lastValue;
                ECGDrawer.isSensorActive = true;
                break;

            case "EMG 1":
                EMG1Drawer.UpdateWithValues128(d.values[0]);
                dataCollection.EMG_1.AddRecordedValues128(d.values[0]);
                dataCollection.EMG_1_Time.AddRecordedValues128(d.timeStamps);
                emg_1 = dataCollection.EMG_1.lastValue;
                EMG1Drawer.isSensorActive = true;
                break;

            case "EMG 2":
                EMG2Drawer.UpdateWithValues128(d.values[0]);
                dataCollection.EMG_2.AddRecordedValues128(d.values[0]);
                dataCollection.EMG_2_Time.AddRecordedValues128(d.timeStamps);
                emg_2 = dataCollection.EMG_2.lastValue;
                EMG2Drawer.isSensorActive = true;
                break;

            case "EMG 3":
                EMG3Drawer.UpdateWithValues128(d.values[0]);
                dataCollection.EMG_3.AddRecordedValues128(d.values[0]);
                dataCollection.EMG_3_Time.AddRecordedValues128(d.timeStamps);
                emg_3 = dataCollection.EMG_3.lastValue;
                EMG3Drawer.isSensorActive = true;
                break;

            case "EMG 4":
                EMG4Drawer.UpdateWithValues128(d.values[0]);
                dataCollection.EMG_4.AddRecordedValues128(d.values[0]);
                dataCollection.EMG_4_Time.AddRecordedValues128(d.timeStamps);
                emg_4 = dataCollection.EMG_4.lastValue;
                EMG4Drawer.isSensorActive = true;
                break;

            case "EMG 5":
                EMG5Drawer.UpdateWithValues128(d.values[0]);
                dataCollection.EMG_5.AddRecordedValues128(d.values[0]);
                dataCollection.EMG_5_Time.AddRecordedValues128(d.timeStamps);
                emg_5 = dataCollection.EMG_5.lastValue;
                EMG5Drawer.isSensorActive = true;
                break;

            case "EMG 6":
                EMG6Drawer.UpdateWithValues128(d.values[0]);
                dataCollection.EMG_6.AddRecordedValues128(d.values[0]);
                dataCollection.EMG_6_Time.AddRecordedValues128(d.timeStamps);
                emg_6 = dataCollection.EMG_6.lastValue;
                EMG6Drawer.isSensorActive = true;
                break;

            case "EMG 7":
                EMG7Drawer.UpdateWithValues128(d.values[0]);
                dataCollection.EMG_7.AddRecordedValues128(d.values[0]);
                dataCollection.EMG_7_Time.AddRecordedValues128(d.timeStamps);
                emg_7 = dataCollection.EMG_7.lastValue;
                EMG7Drawer.isSensorActive = true;
                break;

            case "EMG 8":
                EMG8Drawer.UpdateWithValues128(d.values[0]);
                dataCollection.EMG_8.AddRecordedValues128(d.values[0]);
                dataCollection.EMG_8_Time.AddRecordedValues128(d.timeStamps);
                emg_8 = dataCollection.EMG_8.lastValue;
                EMG8Drawer.isSensorActive = true;
                break;


            case "GSR":
                GSRDrawer.UpdateWithValues32(d.values[0]);
                dataCollection.GSR.AddRecordedValues32(d.values[0]);
                dataCollection.GSR_Time.AddRecordedValues32(d.timeStamps);
                gsr = dataCollection.GSR.lastValue;
                GSRDrawer.isSensorActive = true;
                break;

            case "RESP":
                RESPDrawer.UpdateWithValues32(d.values[0]);
                dataCollection.RESP.AddRecordedValues32(d.values[0]);
                dataCollection.RESP_Time.AddRecordedValues32(d.timeStamps);
                resp = dataCollection.RESP.lastValue;
                RESPDrawer.isSensorActive = true;
                break;

            case "MOTION 1":
                {
                    dataCollection.MOTION_1.AddRecordedValues64(d.values);
                    dataCollection.MOTION_1_Time.AddRecordedValues64(d.timeStamps);
                    var v = dataCollection.MOTION_1.lastValue;
                    if (motion1)
                    {
                        Quaternion q = Quaternion.Euler(v);
                        motion1.localRotation = q;
                    }
                }
                break;

            case "MOTION 2":
                {
                    dataCollection.MOTION_2.AddRecordedValues64(d.values);
                    dataCollection.MOTION_2_Time.AddRecordedValues64(d.timeStamps);
                    if (motion2)
                    {
                        var v = dataCollection.MOTION_2.lastValue;
                        Quaternion q = Quaternion.Euler(v);
                        motion2.localRotation = q;
                    }
                }
                break;

        }

    }

    public void changeEMGSensorName(InputField uiif)
    {
        EMGIndex emg = uiif.GetComponent<EMGIndex>();
        int idx = emg.idx;
        dataCollection.changeEMGSensorName(uiif.text, idx);
    }
}
