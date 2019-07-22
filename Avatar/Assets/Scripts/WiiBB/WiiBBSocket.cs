
using System.Collections;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using MsgPack.Serialization;
using UnityEngine.UI;

public struct WiiBBData
{
    public float TLSensorValue;
    public float TRSensorValue;
    public float BLSensorValue;
    public float BRSensorValue;

    public double timeStamp;
}

public class WiiBBSensorDataCollection : SensorDataCollection
{

    public SensorGeneric<float> X_CP = new SensorGeneric<float>("X CP");
    public SensorGeneric<float> Y_CP = new SensorGeneric<float>("Y CP");

  
    public WiiBBSensorDataCollection()
        : base("WiiBB")
    {
        
        AddSensor(X_CP);
        AddSensor(Y_CP); 
    }
}

public class WiiBBSocket : MonoBehaviour
{
    public string ip = "127.0.0.1";
    public int port = 1409;

    public volatile int packetProcessed = 0;
    public volatile float APPS = 0.0f;

    public volatile float maxPacketDeltaTime;
    
    public double timeStamp;

    public volatile float TL, TR, BL, BR;

    public volatile float X_CP;
    public volatile float Y_CP;

    public Text X_CP_Text;
    public Text Y_CP_Text; 

    private NetworkStream stream;

    TcpClient tcpClient;
    System.Threading.Thread readingThread;

   
    public WiiBBSensorDataCollection dataCollection = new WiiBBSensorDataCollection();

    public void Start()
    {   
        // Connect to WiiBBServer
        packetProcessed = 0;

        try
        {
            tcpClient = new TcpClient(ip, port);
            stream = tcpClient.GetStream();
            
            Debug.Log("Starting Network thread...");
            readingThread = new System.Threading.Thread(ReadDataFunc);
            readingThread.Start();
            
        }

        catch(SocketException se)
        {
            Debug.Log("[WiiBBSensorDataCollection] SocketException : " + se.ToString());
        }
    }

    public void OnDisable()
    {
        if (stream != null)
        {
            System.Threading.Thread.Sleep(500);
            stream.Close();
            stream = null;
        }
        readingThread = null;
    }

    static int frameIndex = 0;

    public void FixedUpdate()
    {

        //ReadDataFuncSync();

        //lock (this)
        { 
            
            GravityCenterCalculator(TL, TR, BL, BR);

            dataCollection.X_CP.AddRecordedValue(X_CP);
            dataCollection.Y_CP.AddRecordedValue(Y_CP);
            
            X_CP_Text.text = X_CP.ToString();
            Y_CP_Text.text = Y_CP.ToString();

           // APPS = (float) packetProcessed / Time.timeSinceLevelLoad;
       }
   
    }
    

    void ReadDataFunc()
    {
        long last = System.DateTime.Now.Ticks;
        var serializer = MessagePackSerializer.Get<WiiBBData>();
                   
        
        Debug.Log("Netwok thread Started");
        while (stream != null)
        {
            if (stream.CanRead)
            {
                // Incoming message may be larger than the buffer size. 
                if (stream.DataAvailable)
                {
                    WiiBBData d = serializer.Unpack(stream);
                    //lock (this)
                    {
                        packetProcessed++;
                        HandleData(d);

                        long dt = System.DateTime.Now.Ticks - last;

                        float dtf = dt / 1e7f;

                        maxPacketDeltaTime = Mathf.Max(maxPacketDeltaTime,dtf);
                        last = System.DateTime.Now.Ticks;
                    }
                }
            }
        }
    }
    
    long last = -1;

        
   /* void ReadDataFuncSync()
    {
        if(last == -1)
        {
            last = System.DateTime.Now.Ticks;
        }

        var serializer = MessagePackSerializer.Get<WiiBBData>();

        if (stream != null && stream.DataAvailable)
        {

            while (stream.DataAvailable)
            {
                WiiBBData d = serializer.Unpack(stream);

                packetProcessed++;
                HandleData(d);
            }
        }
    }*/

    void HandleData(WiiBBData d)
    {
        //if (d.timeStamp >= timeStamp)
        if(SensorRecorderManager.startRecordingSensorData)
        {
            timeStamp = d.timeStamp;

            TL = d.TLSensorValue;
            TR = d.TRSensorValue;
            BL = d.BLSensorValue;
            BR = d.BRSensorValue;
        }
    }
  
    public void GravityCenterCalculator(float TLSensorValue, float TRSensorValue, float BLSensorValue, float BRSensorValue)
    {
        X_CP = ((TRSensorValue + BRSensorValue) - (TLSensorValue + BLSensorValue));
        //X_CP = Mathf.Round(X_CP * 100) / 100;

        Y_CP = ((TLSensorValue + TRSensorValue) - (BLSensorValue + BRSensorValue));
        //Y_CP = Mathf.Round(Y_CP * 100) / 100;
    }
}
