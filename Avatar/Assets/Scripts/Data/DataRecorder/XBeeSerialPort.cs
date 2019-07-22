
using UnityEngine;
using System.Collections;
using System;
using System.IO.Ports;
using System.Text;

public class XBeeSerialPort : MonoBehaviour
{
		public string serialPortName;
		public string windowSerialPortName;
		public int baudRate;
		public bool autoStart;

		public bool Connected { get; private set; } // true when the device is connected
		public int packetProcessed = 0;
		private const int BATCH_PROCESS_BYTES_LIMIT = 64; // process no more than this many bytes per individual processInput call
		private const int MAX_DATA_BYTES = 4096; // make this larger if receiving very large sysex messages
		private const int minPacketSize = 16;
		private SerialPort _serialPort;
		private int delay = 100;
		private string xbdata = "";
		public float airFlow;
		public float skinVoltage;
        public float ECG;
        public float raw_ECG;
        public float BPM;
        public float IBI;

        public DataDrawer skinVoltageDataDrawer;
        public DataDrawer ECGDataDrawer;
        public DataDrawer airFlowDataDrawer;
        public DataDrawer raw_ECGDataDrawer;
        public DataDrawer BPMDataDrawer;
        public DataDrawer IBIDataDrawer;

		private bool can_poll_bytes_to_read = true; // store results of platform comparison...not available under all platforms (windows)

        private Byte[] inputByteArray = new Byte[1];

        public EHealthSensorDataCollection dataCollection = new EHealthSensorDataCollection();

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
	
		public void Start ()
		{
		
				if (serialPortName == null || serialPortName.Length == 0) {
						serialPortName = guessPortName ();
				}

				if (windowSerialPortName == null || windowSerialPortName.Length == 0) {
						windowSerialPortName = guessPortName ();
				}

				if (autoStart) {
						if (UnityEngine.Application.platform.ToString ().StartsWith ("Windows")) {
								connect (windowSerialPortName, baudRate, autoStart, delay);
						} else {
								connect (serialPortName, baudRate, autoStart, delay);
						}
				}

                skinVoltageDataDrawer.dataSource = dataCollection.skinVoltage;
                ECGDataDrawer.dataSource = dataCollection.ECG;
                airFlowDataDrawer.dataSource = dataCollection.airFlow;
                raw_ECGDataDrawer.dataSource = dataCollection.raw_ECG;
                BPMDataDrawer.dataSource = dataCollection.BPM;
                IBIDataDrawer.dataSource = dataCollection.IBI;



		}

        public void FixedUpdate()
		{
            if (_serialPort != null && _serialPort.IsOpen)
            {
				processInput ();
			}

            if (SensorRecorderManager.startRecordingSensorData)
            {
                dataCollection.airFlow.AddRecordedValue(airFlow);
                dataCollection.ECG.AddRecordedValue(ECG);
                dataCollection.skinVoltage.AddRecordedValue(skinVoltage);

                dataCollection.raw_ECG.AddRecordedValue(raw_ECG);
                dataCollection.BPM.AddRecordedValue(BPM);
                dataCollection.IBI.AddRecordedValue(IBI);
            }
		}

		public void Disconnect ()
		{
			Connected = false;
			Close ();
		}

		void OnDestroy ()
		{
			Disconnect ();
		}

		protected void connect (string serialPortName, int baudRate, bool autoStart, int delay)
		{
			_serialPort = new SerialPort (serialPortName, baudRate);
				
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
			if (UnityEngine.Application.platform.ToString ().StartsWith ("Windows")) {
					can_poll_bytes_to_read = false;
			}
		
			if (autoStart) {
					this.delay = delay;
					this.Open ();
			}
	    }


		/// <summary>
		/// Opens the serial port connection, should it be required. By default the port is
		/// opened when the object is first created.
		/// </summary>
		protected void Open ()
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
		protected void Close ()
		{
			if (_serialPort != null)
					_serialPort.Close ();
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
		public static string[] list ()
		{
				return SerialPort.GetPortNames ();
		}
	
		/// <summary>
		/// Poll and process present input directly. Must be called repeatedly 
		/// from same thread as created the object. (From the SerialPort documentation)
		/// Experimentally, polling from a separate thread works on OSX but *will die 
		/// horribly* on windows. Also, BytesToRead does not currently function under
		/// windows so each call will cost 1ms (ReadTimeout) if no data is present.
		// 
		/// </summary>
		void processInput (){
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

                            inputByteArray[0] = (Byte) _serialPort.ReadByte();
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
	
		public void parsePacket ()
		{

				int processed = 0;
				while (true /*processed < BATCH_PROCESS_BYTES_LIMIT*/) {
						processed++;

						// find a packet start
						int packetStart = xbdata.IndexOf ('{');
						int packetEnd = xbdata.IndexOf ('}');

						if (packetStart == -1) {
								return;
						}

						if (packetEnd < packetStart) {
								xbdata = xbdata.Substring (packetStart);
								packetStart = 0;
								packetEnd = xbdata.IndexOf ('}');
						}

                        if (packetEnd == -1)
                        {
                            return;
                        }

						string packet = null;
						string[] values = null;
						try {						
								packet = xbdata.Substring (packetStart + 1, packetEnd - packetStart - 1);
								values = packet.Split (new char[]{','});

								if (values.Length == 6) {

                                        airFlow = float.Parse(values[0]);
										skinVoltage = float.Parse (values [1]);
                                        ECG = float.Parse(values[2]);
                                        raw_ECG = float.Parse(values[3]);
                                        BPM = float.Parse(values[4]);
                                        IBI = float.Parse(values[5]);

										packetProcessed++;
								}
						} catch (Exception e) {
								Debug.LogError (e);
								Debug.Log (xbdata);
								Debug.Log (packet);
								Debug.Log (values);
								Debug.Log (packetStart);
								Debug.Log (packetEnd);
								xbdata = "";
								return;
						}
						xbdata = xbdata.Substring (packetEnd + 1);
				}
		}
	
		// Static Helpers	
		public static string guessPortName ()
		{		
				switch (Application.platform) {
				case RuntimePlatform.OSXPlayer:
				case RuntimePlatform.OSXEditor:
				case RuntimePlatform.OSXDashboardPlayer:
				case RuntimePlatform.LinuxPlayer:
						return guessPortNameUnix ();
			
				default: 
						return guessPortNameWindows ();
				}
		}
	
		public static string guessPortNameWindows ()
		{
				var devices = System.IO.Ports.SerialPort.GetPortNames ();
		
				if (devices.Length == 0) { //
						return "COM5"; // probably right 50% of the time		
				} else
						return devices [0];				
		}
	
		public static string guessPortNameUnix ()
		{			
				var devices = System.IO.Ports.SerialPort.GetPortNames ();
		
				if (devices.Length == 0) { // try manual enumeration
						devices = System.IO.Directory.GetFiles ("/dev/");		
				}
				string dev = "";
		
				foreach (var d in devices) {				
						if (d.StartsWith ("/dev/tty.usb") || d.StartsWith ("/dev/ttyUSB")) {
								dev = d;
								Debug.Log ("Guessing that device is " + dev);
								break;
						}
				}		
				return dev;		
		}
	
} // End Arduino class

