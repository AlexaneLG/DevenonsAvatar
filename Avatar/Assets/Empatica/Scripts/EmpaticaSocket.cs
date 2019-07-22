
using System.Collections;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using MsgPack.Serialization;

public enum EmpaticaDataID
{
	HIHELLO = 11,
	BATTERY = 31,
	ACCELERATION,
	BVP,
	IBI,
	GSR,
	TEMPERATURE,
    TAG,
};

public class EmpaticaData
{
	public EmpaticaDataID command;
    public double timestamp;
	public float d0;
	public float d2;
	public float d1;
}

public class EmpaticaSocket : MonoBehaviour
{
    public string ip = "192.168.1.70";
	public int port = 4567;

	public double timeStamp;
	public float battery;
	public int x,y,z;
	public float bvp,gsr,ibi,bpm,temperature;
	public double ttag;

	private TcpListener tcpListener; 
	private TcpClient tcpClient;
	private NetworkStream stream;

	System.Threading.Thread readingThread;

    public EmpaticaSensorDataCollection dataCollection = new EmpaticaSensorDataCollection();
    public DataDrawer EmpaticaBVPDrawer;
    public DataDrawer EmpaticaGSRDrawer;

    private IPAddress LocalIPAddress()
    {
        IPHostEntry host;
        IPAddress localIP = null;
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip;
                break;
            }
        }
        return localIP;
    }

	public void Start() 
	{
        EmpaticaBVPDrawer.dataSource = dataCollection.BVP;
        EmpaticaGSRDrawer.dataSource = dataCollection.GSR;

        IPAddress ipAddress = IPAddress.Parse(ip);// LocalIPAddress();

        try
        {
		    tcpListener =  new TcpListener(ipAddress,port ); 
		    tcpListener.Start();
        }
        catch(System.Exception){
            tcpListener = null;
            throw;
        }

		Debug.Log("I am listening for connections on " + ipAddress + " port " + port);
               
	}

	public void OnDisable()
	{
		if(stream != null){
			stream.Close();
			stream = null;
		}

        readingThread = null;

	}

    public void FixedUpdate()
    {

        if (tcpListener != null && tcpListener.Pending())
        {

			//Accept the pending client connection and return a TcpClient object initialized for communication.
			tcpClient = tcpListener.AcceptTcpClient();
			// Using the RemoteEndPoint property.
			Debug.Log("Connections on " + 
			                  IPAddress.Parse(((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString()) +
			                  " on port number " + ((IPEndPoint)tcpListener.LocalEndpoint).Port.ToString());
	
			stream = tcpClient.GetStream();

		byte[] buf = System.Text.Encoding.UTF8.GetBytes("Welcome");
			Debug.Log("Sending " + buf.Length + " bytes");
			stream.Write(buf,0,buf.Length);

			if(readingThread == null)
			{		

				Debug.Log("Starting Network thread...");
                readingThread = new System.Threading.Thread(ReadDataFunc);
				readingThread.Start();
			}
		}

        if (SensorRecorderManager.startRecordingSensorData)
        {
            dataCollection.Battery.AddRecordedValue(battery);
            dataCollection.BVP.AddRecordedValue(bvp);
            dataCollection.IBI.AddRecordedValue(ibi);
            dataCollection.GSR.AddRecordedValue(gsr);
            dataCollection.Temperature.AddRecordedValue(temperature);
            dataCollection.Acceleration.AddRecordedValue(new Vector3(x, y, z));
        }
	}

	void ReadDataFunc()
	{
		Debug.Log("Netwok thread Started");
		while(stream != null){
			if(stream.CanRead)
			{
				// Incoming message may be larger than the buffer size. 
				if(stream.DataAvailable)
				{
					var serializer = MessagePackSerializer.Get<string>();
					var deserializedObject = serializer.Unpack( stream );
					EmpaticaData d = Newtonsoft.Json.JsonConvert.DeserializeObject<EmpaticaData>(deserializedObject);
					//Debug.Log ("EmpaticaData : " + d.command + " " + d.timestamp + " " + d.d0 + " " + d.d1 + " " + d.d2);
	                HandleData(d);                
	            }
	        }
		}
    }
    
    void HandleData(EmpaticaData d)
	{
		switch(d.command){
			case EmpaticaDataID.HIHELLO:
				Debug.Log("Phone Connected");
				break;
			case EmpaticaDataID.BATTERY:
				battery = d.d0;
				break;
            
			case EmpaticaDataID.ACCELERATION:
				x = (int)d.d0;
				y = (int)d.d1;
				z = (int)d.d2;
				break;
                               
            case EmpaticaDataID.BVP:
				bvp = d.d0;
	            break;
            
			case EmpaticaDataID.GSR:
				gsr = d.d0;
				break;

			case EmpaticaDataID.IBI:
				ibi = d.d0;
				bpm = 60.0f/ibi;
				break;

			case EmpaticaDataID.TEMPERATURE:
				temperature = d.d0;
				break;

			case EmpaticaDataID.TAG:
				ttag = d.timestamp;
				break;
	                
        }
        
    }
    
   
}
