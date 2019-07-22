using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;
using System.Threading;

public class JSocketClient : JMonoBehaviour
{
    public string hostname = "127.0.0.1";
    public int port = 8888;

    int bufferSize = 2048;

    public SocketClientState State { get { return _state; } }
    private SocketClientState _state = SocketClientState.Disconnected;

    private NetworkStream _stream;
    private StreamWriter _writer;
    private StreamReader _reader;
    private Thread _readThread;
    private TcpClient _client;
    private Queue<SocketEvent> _events;
    private Queue<string> _messages;

    enum SocketEvent { DataReceived, Connected, Disconnected, Connecting };
    public enum SocketClientState
    {
        Connecting = 0,
        Connected = 1,
        Disconnected = 2,
    }

    #region Events

    public event EventHandler<StringEventArgs> OnMessageReceived;
    public event EventHandler<EventArgs> OnConnected;
    public event EventHandler<EventArgs> OnDisconnected;

    #endregion

    #region UnityLoop

    void Update()
    {
        if (_state == SocketClientState.Connected)
        {
            while (_events.Count > 0)
            {
                SocketEvent SocketEvent = _events.Dequeue();
                JLog("event received: " + SocketEvent.ToString());
            }
            while (_messages.Count > 0)
            {
                string message = _messages.Dequeue();
                if (OnMessageReceived != null)
                    OnMessageReceived(this, new StringEventArgs(message));
                JLog("message received: " + message);
            }
        }
    }

    void OnDestroy()
    {
        Disconnect();
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }
    
    #endregion

    #region Connections / Disconnection

    public void Connect(string hostname, int port)
    {
        this.hostname = hostname;
        this.port = port;
        Connect();
    }

    public void Connect()
    {
        if (_state == SocketClientState.Disconnected)
        {
            _messages = new Queue<string>();
            _events = new Queue<SocketEvent>();
            _client = new TcpClient();
            _client.BeginConnect(hostname, port, new AsyncCallback(ConnectCallback), (object)_client);
        }

    }

    public void Disconnect()
    {
        _state = SocketClientState.Disconnected;
        try
        {
            if (_reader != null)
                _reader.Close();
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
        try
        {
            if (_writer != null)
                _writer.Close();
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
        try
        {
            if (_client == null)
                return;
            _client.Close();
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }


    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            tcpClient.EndConnect(ar);
            SetTcpClient(tcpClient);
        }
        catch (Exception ex)
        {
            Debug.LogError((object)("Connect Exception: " + ex.Message));
        }
    }

    private void SetTcpClient(TcpClient tcpClient)
    {
        _client = tcpClient;
        if (_client.Connected)
        {
            _stream = _client.GetStream();
            _reader = new StreamReader((Stream)_stream);
            _writer = new StreamWriter((Stream)_stream);
            _state = SocketClientState.Connected;
            _events.Enqueue(SocketEvent.Connected);
            _readThread = new Thread(new ThreadStart(ReadData));
            _readThread.IsBackground = true;
            _readThread.Start();

            if (OnConnected != null)
                OnConnected(this, new EventArgs());
        }
        else
            _state = SocketClientState.Disconnected;
    }

    #endregion

    #region Send / Receive

    public void Send(string message)
    {
        if (_state != SocketClientState.Connected)
            return;

        byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message+"$");
        _stream.Write(outStream, 0, outStream.Length);
        _stream.Flush();
    }


    private void ReadData()
    {
        bool flag = false;
        while (!flag)
        {
            //receive byte data
            byte[] inStream = new byte[bufferSize];
            int bytesCount = _stream.Read(inStream, 0, bufferSize);
            if (bytesCount == 0)
            {
                flag = true;
            }
            else
            {
                lock (_events)
                {
                    _events.Enqueue(SocketEvent.DataReceived);
                }

                //decode byte stream to text
                lock (_messages)
                {
                    _messages.Enqueue(System.Text.Encoding.ASCII.GetString(inStream));
                }
            };
        }
        _state = SocketClientState.Disconnected;
        _client.Close();
        _events.Enqueue(SocketEvent.Disconnected);
    }

    #endregion
}
