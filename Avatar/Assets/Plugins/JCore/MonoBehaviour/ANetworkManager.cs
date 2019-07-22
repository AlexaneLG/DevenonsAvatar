using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Abstract class describing a manager that creates a multi player server, or connects to an existing one
/// </summary>
public abstract class ANetworkManager : JMonoBehaviour
{
    public string remoteIP = "192.168.0.3";
    public int remotePort = 25000;
    public int listenPort = 25000;

    public bool debug = true;

    public enum State { Disconnected, Server, ClientConnected, ClientConnecting }



    #region Properties

    protected State _currentState = State.Disconnected;
    public State CurrentState { get { return _currentState; } }

    public bool IsServer
    {
        get
        {
            return Network.isServer;
        }
    }

    public bool IsClient
    {
        get
        {
            return Network.isClient;
        }
    }

    public bool IsConnected { get { return (Network.peerType == NetworkPeerType.Client || Network.peerType == NetworkPeerType.Server); } }

    public bool ConnexionLost { get { return (Network.peerType == NetworkPeerType.Disconnected && _clientId > 0); } }

    //after the session is properly set up, we set this to TRUE, so that if we lose connexion we'll know we're trying to reconnect for a simple network failure and not a crash (meaing the scene assets are still in RAM)
    public bool LostNetworkDuringSession { get { return _lostNetworkDuringSession; } }
    protected bool _lostNetworkDuringSession = false;

    protected int _serverSessionId = -1;
    public int ServerSessionId { get { return _serverSessionId; } }

    //when clientID is not -1, it means a session is in progress
    protected int _clientId = -1;
    public int ClientId { get { return _clientId; } }

    protected string _clientName = "";
    public string ClientName { get { return _clientName; } set { _clientName = value; } }

    #endregion

    //methods for managing sessions (create/join)
    public abstract void CreateServer();
    public abstract void StopServer();

    public abstract void DisconnectFromServer(bool iDefinitiveQuitSession);
    public abstract void Reconnect();
    public abstract void AbortReconnect();
    public abstract void Connect();
}
