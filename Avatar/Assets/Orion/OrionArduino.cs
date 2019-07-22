
using UnityEngine;
using System.Collections;
using System;
using System.IO.Ports;
using System.Text;


public class OrionDataCollection : SensorDataCollection
{

    public SensorGeneric<float> d0 = new SensorGeneric<float>("Orion D0");
    public SensorGeneric<float> d1 = new SensorGeneric<float>("Orion D1");
    public SensorGeneric<float> d2 = new SensorGeneric<float>("Orion D2");
    public SensorGeneric<float> d3 = new SensorGeneric<float>("Orion D3");
    public SensorGeneric<float> d4 = new SensorGeneric<float>("Orion D4");
    public SensorGeneric<float> d5 = new SensorGeneric<float>("Orion D5");
    public SensorGeneric<float> d6 = new SensorGeneric<float>("Orion D6");
    public SensorGeneric<float> d7 = new SensorGeneric<float>("Orion D7");
    public SensorGeneric<float> d8 = new SensorGeneric<float>("Orion D8");
    public SensorGeneric<float> d9 = new SensorGeneric<float>("Orion D9");
    public SensorGeneric<float> d10 = new SensorGeneric<float>("Orion D10");
    public SensorGeneric<float> d11 = new SensorGeneric<float>("Orion D11");
    public SensorGeneric<float> d12 = new SensorGeneric<float>("Orion D12");
    public SensorGeneric<float> d13 = new SensorGeneric<float>("Orion D13");
    public SensorGeneric<float> d14 = new SensorGeneric<float>("Orion D14");
    public SensorGeneric<float> d15 = new SensorGeneric<float>("Orion D15");

    public OrionDataCollection()
        : base("Orion")
    {
        AddSensor(d0);
        AddSensor(d1);
        AddSensor(d2);
        AddSensor(d3);
        AddSensor(d4);
        AddSensor(d5);
        AddSensor(d6);
        AddSensor(d7);
        AddSensor(d8);
        AddSensor(d9);
        AddSensor(d10);
        AddSensor(d11);
        AddSensor(d12);
        AddSensor(d13);
        AddSensor(d14);
        AddSensor(d15);
    }
}

public class OrionArduino : MonoBehaviour
{
    public string serialPortName;
    public int baudRate;
    public bool autoStart;

    public bool Connected { get; private set; } // true when the device is connected

    public int packetProcessed = 0;
    public float APPS = 0.0f;
    public int maxval = 0;

    private const int BATCH_PROCESS_BYTES_LIMIT = 64*1024; // process no more than this many bytes per individual processInput call
    private const int minPacketSize = 512;
    private SerialPort _serialPort;
    private int delay = 100;
    private string xbdata = "";

    public float d0, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15;
    public DataDrawer dd0, dd1, dd2, dd3, dd4, dd5, dd6, dd7, dd8, dd9, dd10, dd11, dd12, dd13, dd14, dd15;


    private bool can_poll_bytes_to_read = true; // store results of platform comparison...not available under all platforms (windows)

    private Byte[] inputByteArray = new Byte[1];

    public OrionDataCollection dataCollection = new OrionDataCollection();

    private const float INV1023 = 1.0f / 1023.0f;


    /// <summary>
    /// Construct with explicit parameters
    /// </summary>
    /// <param name="serialPortName">String specifying the name of the serial port. eg COM4</param>
    /// <param name="baudRate">The baud rate of the communication. Note that the default firmata firmware sketches communicate at 57600 by default.</param>
    /// <param name="autoStart">Determines whether the serial port should be opened automatically.
    ///                     use the Open() method to open the connection manually.</param>
    /// <param name="delay">Time delay that may be required to allow some arduino models
    ///                     to reboot after opening a serial connection. The delay will only activate
    ///                     when autoStart is true.</param>

    public void Start()
    {

        if (serialPortName == null || serialPortName.Length == 0)
        {
            serialPortName = guessPortName();
        }


        if (autoStart)
        {
            connect(serialPortName, baudRate, autoStart, delay);
        }

        if (dd0 != null) dd0.dataSource = dataCollection.d0;
        if (dd1 != null) dd1.dataSource = dataCollection.d1;
        if (dd2 != null) dd2.dataSource = dataCollection.d2;
        if (dd3 != null) dd3.dataSource = dataCollection.d3;
        if (dd4 != null) dd4.dataSource = dataCollection.d4;
        if (dd5 != null) dd5.dataSource = dataCollection.d5;
        if (dd6 != null) dd6.dataSource = dataCollection.d6;
        if (dd7 != null) dd7.dataSource = dataCollection.d7;
        if (dd8 != null) dd8.dataSource = dataCollection.d8;
        if (dd9 != null) dd9.dataSource = dataCollection.d9;

        if (dd10 != null) dd10.dataSource = dataCollection.d10;
        if (dd11 != null) dd11.dataSource = dataCollection.d11;
        if (dd12 != null) dd12.dataSource = dataCollection.d12;
        if (dd13 != null) dd13.dataSource = dataCollection.d13;
        if (dd14 != null) dd14.dataSource = dataCollection.d14;
        if (dd15 != null) dd15.dataSource = dataCollection.d15;




    }

    public void FixedUpdate()
    {

        APPS = (float)packetProcessed / Time.timeSinceLevelLoad;

        if (_serialPort != null && _serialPort.IsOpen)
        {
            processInput();
        }

        if (SensorRecorderManager.startRecordingSensorData)
        {
            dataCollection.d0.AddRecordedValue(d0);
            dataCollection.d1.AddRecordedValue(d1);
            dataCollection.d2.AddRecordedValue(d2);
            dataCollection.d3.AddRecordedValue(d3);
            dataCollection.d4.AddRecordedValue(d4);
            dataCollection.d5.AddRecordedValue(d5);
            dataCollection.d6.AddRecordedValue(d6);
            dataCollection.d7.AddRecordedValue(d7);
            dataCollection.d8.AddRecordedValue(d8);
            dataCollection.d9.AddRecordedValue(d9);
            dataCollection.d10.AddRecordedValue(d10);
            dataCollection.d11.AddRecordedValue(d11);
            dataCollection.d12.AddRecordedValue(d12);
            dataCollection.d13.AddRecordedValue(d13);
            dataCollection.d14.AddRecordedValue(d14);
            dataCollection.d15.AddRecordedValue(d15);
        }
    }

    public void Disconnect()
    {
        Connected = false;
        Close();
    }

    void OnDestroy()
    {
        Disconnect();
    }

    protected void connect(string serialPortName, int baudRate, bool autoStart, int delay)
    {
        _serialPort = new SerialPort(serialPortName, baudRate);

        _serialPort.DtrEnable = true; // win32 hack to try to get DataReceived event to fire
        _serialPort.RtsEnable = true;
        _serialPort.PortName = serialPortName;
        _serialPort.BaudRate = baudRate;

        _serialPort.DataBits = 8;
        _serialPort.Parity = Parity.None;
        _serialPort.StopBits = StopBits.One;
        _serialPort.ReadTimeout = 1; // since on windows we *cannot* have a separate read thread
        _serialPort.WriteTimeout = 1000;


        // HAX: cant use compile time flags here, so cache result in a variable
        if (UnityEngine.Application.platform.ToString().StartsWith("Windows"))
        {
            can_poll_bytes_to_read = false;
        }

        if (autoStart)
        {
            this.delay = delay;
            this.Open();
        }
    }


    /// <summary>
    /// Opens the serial port connection, should it be required. By default the port is
    /// opened when the object is first created.
    /// </summary>
    protected void Open()
    {
        if (_serialPort != null)
        {
            try
            {
                _serialPort.Open();
                Debug.Log("Serial port " + _serialPort.PortName + " opened");
            }
            catch (Exception ex)
            {
                Debug.Log("Serial port " + _serialPort.PortName + "Open() error: " + ex);
            }
        }
    }



    /// <summary>
    /// Closes the serial port.
    /// </summary>
    protected void Close()
    {
        if (_serialPort != null)
            _serialPort.Close();
    }

    /// <summary>
    /// True if this instance has an open serial port; does not imply that what we are actually connected
    /// to a properly configured Arduino running firmata.
    /// </summary>
    public bool IsOpen { get { return _serialPort != null && _serialPort.IsOpen; } }

    /// <summary>
    /// Lists all available serial ports on current system by calling SerialPort.GetPortNames(). This may not reliable across all platforms and usb serial capable devices.
    /// </summary>
    /// <returns>An array of strings containing all available serial ports.</returns>
    public static string[] list()
    {
        return SerialPort.GetPortNames();
    }

    /// <summary>
    /// Poll and process present input directly. Must be called repeatedly 
    /// from same thread as created the object. (From the SerialPort documentation)
    /// Experimentally, polling from a separate thread works on OSX but *will die 
    /// horribly* on windows. Also, BytesToRead does not currently function under
    /// windows so each call will cost 1ms (ReadTimeout) if no data is present.
    // 
    /// </summary>
    void processInput()
    {
        // dont let this loop block the entire thread if the input is coming in faster than we can read it
        int processed = 0;
        while (processed < BATCH_PROCESS_BYTES_LIMIT)
        {
            processed++;

            if (can_poll_bytes_to_read && _serialPort.BytesToRead == 0)
                return;

            //if (_serialPort.BytesToRead > 0) // windows fail
            {
                try
                {

                    // int inputData = _serialPort.ReadByte();
                    // xbdata += inputData.ToString();

                    inputByteArray[0] = (Byte)_serialPort.ReadByte();
                    xbdata += System.Text.Encoding.ASCII.GetString(inputByteArray);

                }
                catch (Exception e)
                { // swallow read exceptions
                    //Log(e.GetType().Name + ": "+e.Message);	
                    if (e.GetType() == typeof(TimeoutException))
                    {
                        //Debug.Log ("TimeoutException");
                        return;
                    }
                    else
                    {
                        Debug.Log(e);
                    }
                }
            }

            if (xbdata.Length > minPacketSize)
            {
                parsePacket();
            }

        }
    }

    public void parsePacket()
    {

        int processed = 0;
        while (true /*processed < BATCH_PROCESS_BYTES_LIMIT*/)
        {
            processed++;

            // find a packet start
            int packetStart = xbdata.IndexOf('{');
            int packetEnd = xbdata.IndexOf('}');

            if (packetStart == -1)
            {
                return;
            }

            if (packetEnd < packetStart)
            {
                xbdata = xbdata.Substring(packetStart);
                packetStart = 0;
                packetEnd = xbdata.IndexOf('}');
            }

            if (packetEnd == -1)
            {
                return;
            }

            string packet = null;
            string[] values = null;
            try
            {
                packet = xbdata.Substring(packetStart + 1, packetEnd - packetStart - 1);
                values = packet.Split(new char[] { ',' });

                if (values.Length == 16)
                {
                    
                    int i0 = int.Parse(values[0]); maxval = Mathf.Max(maxval, i0);
                    int i1 = int.Parse(values[1]); 
                    int i2 = int.Parse(values[2]); 
                    int i3 = int.Parse(values[3]); 
                    int i4 = int.Parse(values[4]); 
                    int i5 = int.Parse(values[5]); 
                    int i6 = int.Parse(values[6]); 
                    int i7 = int.Parse(values[7]); 
                    int i8 = int.Parse(values[8]); 
                    int i9 = int.Parse(values[9]); 

                    int i10 = int.Parse(values[10]); 
                    int i11 = int.Parse(values[11]); 
                    int i12 = int.Parse(values[12]); 
                    int i13 = int.Parse(values[13]); 
                    int i14 = int.Parse(values[14]); 
                    int i15 = int.Parse(values[15]); 

                    d0 = INV1023 * (float)i0;
                    d1 = INV1023 * (float)i1;
                    d2 = INV1023 * (float)i2;
                    d3 = INV1023 * (float)i3;
                    d4 = INV1023 * (float)i4;
                    d5 = INV1023 * (float)i5;
                    d6 = INV1023 * (float)i6;
                    d7 = INV1023 * (float)i7;
                    d8 = INV1023 * (float)i8;
                    d9 = INV1023 * (float)i9;

                    d10 = INV1023 * (float)i10;
                    d11 = INV1023 * (float)i11;
                    d12 = INV1023 * (float)i12;
                    d13 = INV1023 * (float)i13;
                    d14 = INV1023 * (float)i14;
                    d15 = INV1023 * (float)i15;

                    packetProcessed++;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e + " " + xbdata + " " + packet + " " + values + " " + packetStart + " " + packetEnd);
                xbdata = "";
                return;
            }
            xbdata = xbdata.Substring(packetEnd + 1);
        }
    }

    // Static Helpers	
    public static string guessPortName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXDashboardPlayer:
            case RuntimePlatform.LinuxPlayer:
                return guessPortNameUnix();

            default:
                return guessPortNameWindows();
        }
    }

    public static string guessPortNameWindows()
    {
        var devices = System.IO.Ports.SerialPort.GetPortNames();

        if (devices.Length == 0)
        { //
            return "COM5"; // probably right 50% of the time		
        }
        else
            return devices[0];
    }

    public static string guessPortNameUnix()
    {
        var devices = System.IO.Ports.SerialPort.GetPortNames();

        if (devices.Length == 0)
        { // try manual enumeration
            devices = System.IO.Directory.GetFiles("/dev/");
        }
        string dev = "";

        foreach (var d in devices)
        {
            if (d.StartsWith("/dev/tty.usb") || d.StartsWith("/dev/ttyUSB"))
            {
                dev = d;
                Debug.Log("Guessing that device is " + dev);
                break;
            }
        }
        return dev;
    }

} // End Arduino class

