using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine.AI;

public class GestionElectroAimant : MonoBehaviour
{
    private static string x;
    private static string[] data;

    int baudRate = 9600;
    SerialPort currentPort;
    
    Thread myThread;

    private bool StopThread = false;

    bool CoupurelectroAimant = false;


    void Start()
    {
        SetComPort();
        myThread = new Thread(new ThreadStart(GetArduino));
        myThread.IsBackground = true;
        myThread.Start();
    }

    
    private void GetArduino()
    {
        while (myThread.IsAlive && currentPort != null && !StopThread)
        {

            if (CoupurelectroAimant) 
            {
                Debug.Log("CoupurelectroAimant = true");

                currentPort.Write("1");
                CoupurelectroAimant = false;
            }
            /*try
            {
                x = currentPort.ReadLine();
                //data = x.Split(';');

                RPM = int.Parse(data[0]);
                bike.speed = ((float)(RPM)) / 5.0f;
            }
            catch
            {
            }*/
        }
    }
    void Update()
    {
        if (Input.GetKeyDown("space")) { 
            CoupurelectroAimant = true;
            //currentPort.Write("1");
            Debug.Log("CoupurelectroAimant = true");            
        }

        if (Input.GetKeyUp("space"))
        {
            CoupurelectroAimant = false;
        }
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    IEnumerator Pause()
    {
        yield return new WaitForSeconds(3);
    }
    
    void OnApplicationQuit()
    {
        StopThread = true;
        Pause();
        if (currentPort != null)
            currentPort.Close();

        
        if (myThread.IsAlive)
        {
            myThread.Abort();
            Debug.Log(myThread.IsAlive); //true (must be false)
        }
        else
        {
            Debug.Log("Aborting thread failed");
        }
        
    }

    void OnDestroy()
    {
        StopThread = true;
        Pause();
        if (myThread.IsAlive)
        {
            myThread.Abort();
            Debug.Log(myThread.IsAlive); //true (must be false)
        }
        else
        {
            Debug.Log("Aborting thread failed");
        }
    }
    

    private void SetComPort()
    {
        try
        {
            //string[] ports = SerialPort.GetPortNames();
            //foreach (string port in ports)
            {

                //currentPort = new SerialPort(@"\\.\" + port, baudRate, Parity.None, 8, StopBits.One);
                currentPort = new SerialPort("COM4", baudRate, Parity.None, 8, StopBits.One);
                Debug.Log("[GestionElectroAimant] Test du port : " + currentPort.PortName);
                if (currentPort != null)
                {
                    if (currentPort.IsOpen)
                    {
                        currentPort.Close();
                        Debug.Log("Closing port, because it was already open!");
                    }
                    else
                    {
                        currentPort.Open();  // opens the connection
                        currentPort.ReadTimeout = 25;  // sets the timeout value before reporting error
                        Debug.Log("Port Opened! : " + currentPort.PortName);
                    }
                }
                else
                {
                    if (currentPort.IsOpen)
                    {
                        Debug.Log("Port is already open");
                    }
                    else
                    {
                        Debug.Log("Port == null");
                    }
                }
                Debug.Log("Open Connection finished running");
            }
        }
        catch (Exception e)
        {

        }


    }
}