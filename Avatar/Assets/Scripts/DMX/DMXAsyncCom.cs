using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using UnityEngine;


#if true
    public class DMXAsyncCom
    {
        private const int dataSize = 513;

        public static byte[] buffer = new byte[4 + dataSize + 1];
        public static bool done = false;
        public static uint bytesWritten = 0;

        public static System.IO.Ports.SerialPort _port;
                
        public static void start(string portName)
        {
            buffer[0] = 0x7E; //start code
            buffer[1] = 6; //DMX TX
            buffer[2] = dataSize & 0xFF; //pack length logical and with max packet size
            buffer[3] = dataSize >> 8; //packet length shifted by byte length? DMX standard idk
            buffer[4] = 0;

            _port = new System.IO.Ports.SerialPort();
            _port.Encoding = System.Text.Encoding.UTF8;
            _port.PortName = portName;
            _port.Open();
            
            Thread thread = new Thread(new ThreadStart(threadFunc));
            thread.Start();

        }

        public static void stop()
        {

            done = true;
            
        }
      

        public static void setDmxValue(int channel, byte value)
        {
            if (buffer != null)
            {
                buffer[4 + channel + 1] = value;
            }
        }

        public static void threadFunc()
        {
            Debug.Log("DMX Thread Started");

            setDmxValue(0, 0);
            setDmxValue(1, 0);
            setDmxValue(2, 0);
            setDmxValue(3, 0);
            setDmxValue(4, 0);
            setDmxValue(5, 0);

            _write();
            
   
            while (!done)
            {
                _write();
                Thread.Sleep(20);
            }
            
            setDmxValue(0, 0);
            setDmxValue(1, 0);
            setDmxValue(2, 0);
            setDmxValue(3, 0);
            setDmxValue(4, 0);
            setDmxValue(5, 0);

            _write();
            Thread.Sleep(20);

            _port.Close();
            Debug.Log("DMX Stopped");
        
        }

        static void _write()
        {
            _port.Write(buffer, 0, buffer.Length);           
        }
    }
#endif