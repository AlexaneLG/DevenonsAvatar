
//#define MANUAL

//#define DUMP_ONLY
//#define DUMP_REAL_TIME


using System;
using System.Globalization;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using MsgPack.Serialization;

using TUSB_Controller;

using Newtonsoft.Json;



namespace CF_TEAServer
{
    public class Config
    {
        public int comPort = 6;
        public int tcpPort = 1408;
        public int carryingWaveFrequency = 37;
        public int triggerIndex = 0;
        public int triggerImpulseDuration = 255;
        public string[] mapping;
    }

    class Program
    {

      
        
        static Config config = new Config();
        

   
        static TEAServer server; 
        static void Main(string[] args)
        {

            string json = File.ReadAllText("config.json");
            config  = JsonConvert.DeserializeObject<Config>(json);

            server = new TEAServer();            
            server.init(config);
            server.Run();
            
        }
    }

    public struct TEACData
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

    class TEAServer
    {
        
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private Controller tdac = new Controller();

        private Thread teaThread;
        private bool _rec;
        private Config _config;
        private Thread readingThread;

        private long packetIndex = 0;
        //        private long prevPacketIndex = -1;

#if DUMP_REAL_TIME
        private System.IO.StreamWriter _file;
#endif

        //Debug        
        int[] sensor_packet_count = new int[16];

        public void init(Config config )
        {

            _config = config;

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(ipAddress, config.tcpPort);
            try
            {
                tcpListener.Start();
            }
            catch(Exception )
            {
                throw;
            }

            Console.WriteLine("I am listening for connections on " + ipAddress + " port " + config.tcpPort);

            if (tdac.setPortCom((int)config.comPort) == 1)
            {
                Console.WriteLine("T-Dac Found on com port : " + config.comPort);

                int[] ts = new int[tdac.MAX_TSENS];
                int ndTsens;
                string[] tmpstr = new string[2];
                tdac.setCarryingWaveFrequency(config.carryingWaveFrequency);
                ndTsens = tdac.scan(ref ts);

                
                for (int i = 0; i < tdac.MAX_TSENS; i++)
                {
                    if (ts[i] == 1)
                    {

                        int stype = tdac.getTypeTSens(i + 1);
                        Console.WriteLine("T-Sens @" + Convert.ToString(i + 1) + "(" + tdac.getNameTSens((int)(i + 1)) + ") connected" + " ( Type = " + stype + ")" + " -> " + config.mapping[i]);
                        for (int j = 1; j <= tdac.getNbSensors(i + 1); j++)
                        {
                            Console.WriteLine("    " + Convert.ToString(i) + "-" + Convert.ToString(j), tdac.getNameSensor(i + 1, j));
                        }
                    }
                    else
                    {
                       //Console.WriteLine("T-Sens @" + Convert.ToString(i + 1) + " not connected");
                    }
                }

                if (ndTsens <= 0){
                    throw new Exception("No Sensors connected to the T-DAC");   
                }
               
            }
            else
            {
                Console.WriteLine("T-Dac not Found");
                throw new System.Exception("T-Dac not Found");     
            }

#if DUMP_REAL_TIME
            _file = new System.IO.StreamWriter("C:\\Users\\NICOLAS\\Desktop\\dump.csv");
#endif

#if DUMP_ONLY
            StartRecording();

            Utils.ShutdownEventCatcher.Shutdown += (p) => { 
                StopRecording();
            };
#endif
        }

        void ReadDataFunc()
        {
            Console.WriteLine("Network read thread Started");
            while (stream != null)
            {
                if (stream.CanRead)
                {
                    // Incoming message may be larger than the buffer size. 
                    if (stream.DataAvailable)
                    {
                        var serializer = MessagePackSerializer.Get<teaCommand>();
                        var command = serializer.Unpack(stream);
                        HandleCommand(command);
                    }
                }
            }
        }

        void HandleCommand(teaCommand command){

            switch (command.command)
            {
                case teaCommandEnum.START:
                    StartRecording();
                    break;

                case teaCommandEnum.STOP:
                    StopRecording();
                    break;

                case teaCommandEnum.TRIGGER:
                    Trigger();
                    break;
            }

        }

        public void Trigger()
        {
            tdac.sendTrigger((byte)_config.triggerIndex, (byte)_config.triggerImpulseDuration);
        }
        

        public void StartRecording()
        {
            if (teaThread == null)
            {
                this.sensor_packet_count = new int[16];
                Console.WriteLine("StartRecording");
                packetIndex = 0;
                tdac.recConfig(1, 0, "", 0);
                tdac.startRec();
                _rec = true;
                teaThread = new Thread(teaThreadFunc);
                teaThread.Start();
            }
        }
 


        CultureInfo _cu = CultureInfo.CreateSpecificCulture("en-US");

        void teaThreadFunc()
        {
            Console.WriteLine("teaThreadFunc started");
            MessagePackSerializer<TEACData> serializer = MessagePackSerializer.Get<TEACData>();

            //double[] vals = new double[1];           
            do
            {
               
                // for each Sensors
                for (int sensorIndex = 1; sensorIndex < tdac.MAX_TSENS+1; sensorIndex++)
                {
                    if (tdac.IsTSensConnected(sensorIndex) == 1)
                    {
#if MANUAL
                        tdac.getValue(i, ref vals);
                        //Console.WriteLine("pi:" + packetIndex++ + " value:" + vals[0]);
                        
                        TEACData data = new TEACData();
                        data.packetIndex = packetIndex++;
                        data.timeStamp = 0;
                        data.sensorID = i;
                        data.channelID = 0;
                        data.value = (float) vals[0];
                        serializer.Pack(stream, data);
#else
                        int nbdata = tdac.isDataRTAvailable(sensorIndex);
                        if (nbdata > 0) 
                        {
                            int channelCount = tdac.getNbSensors(sensorIndex);
                            int sensorType = tdac.getTypeTSens(sensorIndex);

                            float scale = (sensorType == 19) ? 0.5f : 1f;

                            double[,] TSdata = new double[channelCount, nbdata];
                            double[] TSTime = new double[nbdata];

                            tdac.getDataRT(sensorIndex, ref TSdata, ref TSTime, nbdata);

                            TEACData data = new TEACData();

                            data.sensorName = _config.mapping[sensorIndex-1];

                            data.sensorID = sensorIndex;
                            data.timeStamps = TSTime;
                            data.values = new float[channelCount][];
                            data.packetIndex = packetIndex++;

                           
                            for (int k = 0; k < channelCount; k++)
                            {
                                var d = new float[nbdata];

                                data.values[k] = d;

                                for (int j = 0; j < nbdata; j++)
                                {
                                    float val = (float)TSdata[k, j];
                                   d[j] = val;

                                }

                                Array.Sort(data.timeStamps, d);

                                for (int i = 0; i < data.timeStamps.Length;i++ )
                                {
                                    data.timeStamps[i] = data.timeStamps[i] * scale;
#if DUMP_REAL_TIME
                                    _file.Write(d[i].ToString(_cu) + "\n");
#endif
                                }


                            }

                            if (stream != null)
                            {
#if true
                                sensor_packet_count[sensorIndex] += TSTime.Length;

                                byte[] bdata = serializer.PackSingleObject(data);
                                stream.Write(bdata, 0, bdata.Length);
                                //Console.WriteLine("pi:" + packetIndex + " pv:" + prevPacketIndex);
                                //prevPacketIndex = packetIndex;
#else
                                    serializer.Pack(stream,data);
#endif
                            }
                        }
#endif
                    }
                }

#if MANUAL
             Thread.Sleep(1000/128);
#endif
                //Console.Write(Console.ReadKey().KeyChar);
                //Trigger();

            } while (_rec);

        }

        public void StopRecording()
        {
            if (teaThread != null)
            {
                Console.WriteLine("StopRecording");
                tdac.stopRec();
                _rec = false;
                teaThread.Join();
                teaThread = null;

                foreach (var k in sensor_packet_count)
                {
                    Console.Write(k + " ");
                }
                Console.WriteLine();
            }
        }

        public static void SaveArrayAsCSV<T>(T[] arrayToSave, string fileName)
        {
           
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.Write("Data\n");
                foreach (T item in arrayToSave)
                {
                    file.Write(item + "\n");
                }
            }
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
                        readingThread = new System.Threading.Thread(ReadDataFunc);
                        readingThread.Start();
                    }                    
                }                
            }
        }

        public IPAddress GetLocalIPv4(NetworkInterfaceType _type)
        {
            IPAddress output = null;
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address;
                        }
                    }
                }
            }
            return output;
        }
    }
}
