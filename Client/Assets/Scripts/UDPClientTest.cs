using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;


using System.Threading;

public class UDPClientTest : MonoBehaviour
{

    public string message = "Hello World";
    public int port = 5000;
    public String ipAddress = "127.0.0.1";

    private UdpClient udpClient;

    void Start()
    {
        udpClient = new UdpClient();
        udpClient.Connect(ipAddress, port);
    }

    void OnGUI()
    {
        message = GUI.TextField(new Rect(10, 10, 200, 20), message, 25);

        if (GUI.Button(new Rect(50, 50, 150, 100), "Send"))
        {
            Byte[] sendBytes = Encoding.ASCII.GetBytes(message);
            udpClient.Send(sendBytes, sendBytes.Length);

            print("Sent!");

        }
    }
}