using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using MsgPack.Serialization;

namespace WiiBalanceWalker
{
    public class ProgramServer
    {
        public static int tcpPort = 1409;


        static WiiBBServer server; 
        public static void LaunchServer()
        {
            Console.WriteLine("Test lauchning server");
            server = new WiiBBServer();            
            server.init(tcpPort);
            server.Run();
            
        }
    }

    public class WiiBBData
    {
        public float TLSensorValue;
        public float TRSensorValue;
        public float BLSensorValue;
        public float BRSensorValue;

        public double timeStamp;
    }

    class WiiBBServer
    {
        
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private NetworkStream stream;

        private Thread WiiBBThread;
        private bool _rec;

        private Thread readingThread;

        public void init(int tcpPort)
        {

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(ipAddress, tcpPort);
            try
            {
                tcpListener.Start();
            }
            catch (Exception)
            {
                throw;
            }

            Console.WriteLine("I am listening for connections on " + ipAddress + " port " + tcpPort);
        }

        public void StartRecording()
        {
            Console.WriteLine("StartRecording");

            _rec = true;

            WiiBBThread = new Thread(WiiBBThreadFunc);
            WiiBBThread.Start();
        }


        float f = 0.0f;

        void WiiBBThreadFunc()
        {
            Console.WriteLine("WiiBBThreadFunc started");

            do
            {
                if (stream != null) { 
                    WiiBBData data = new WiiBBData();

                    data.TLSensorValue = FormMain.owTLTempSensorValue;
                    data.TRSensorValue = FormMain.owTRTempSensorValue; 
                    data.BLSensorValue = FormMain.owBLTempSensorValue; 
                    data.BRSensorValue = FormMain.owBRTempSensorValue; 
                
                    MessagePackSerializer<WiiBBData> serializer = MessagePackSerializer.Get<WiiBBData>();
                    try
                    {
                        serializer.Pack(stream, data);
                    }
                    catch(Exception )
                    {
                        stream = null;
                    }
                }
                Thread.Sleep(10);
            } 
            while (_rec);
        }
        
        public void Run()
        {

         
            while (true)
            {
                Thread.Sleep(10);

                if (tcpListener.Pending())
                {

                    //Accept the pending client connection and return a TcpClient object initialized for communication.
                    tcpClient = tcpListener.AcceptTcpClient();
                    // Using the RemoteEndPoint property.
                    Console.WriteLine("Connections on " +
                                      IPAddress.Parse(((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString()) +
                                      " on port number " + ((IPEndPoint)tcpListener.LocalEndpoint).Port.ToString());

                    stream = tcpClient.GetStream();

                   
                    
                    if (readingThread == null)
                    {
                        Console.WriteLine("Starting Network thread...");
                        readingThread = new System.Threading.Thread(StartRecording);
                        readingThread.Start();
                    }
                    
                }
                
            }
        }
    }
}
