using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UDPNetwork : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;
    public Text number;

    public Text name;

    public Text currentName;

    public static bool nameSet = false;

    public GameObject setNameGroup;

    public GameObject gameUIGroup;

    public GameObject instructionsGroup;

    UdpClient client;

    public InputField portInput;
    public InputField ipInput;

    int port;
    String ipAddress;

    private UdpClient udpClient;


    public String lastRecvMessage;
    public String[] allRecvMessage;

    void Start()
    {
        ipInput.text = "127.0.0.1";
        portInput.text = "5000";
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.Mouse0)) && nameSet && instructionsGroup.activeSelf)
        {
            instructionsGroup.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    static void Main()
    {
        UDPNetwork receiveObj = new UDPNetwork();
        receiveObj.init();
    }

    void init()
    {
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    public void SetNameButton()
    {
        if (name.text.Length > 3)
        {
            ipAddress = ipInput.text;
            port = Convert.ToInt32(portInput.text);
            udpClient = new UdpClient();
            udpClient.Connect(ipAddress, port);
            init();

            Byte[] sendBytes = Encoding.ASCII.GetBytes(name.text);
            udpClient.Send(sendBytes, sendBytes.Length);
            currentName.text = "Welcome " + name.text;
            setNameGroup.SetActive(false);
            gameUIGroup.SetActive(true);
            nameSet = true;
            TextController.text = "Username must be longer than 3 characters!";
        }
        else
        {
            TextController.text = "Username must be longer than 3 characters!";
        }
    }

    public void GuessNumberButton()
    {
        if (number.text != null)
        {
            Byte[] sendBytes = Encoding.ASCII.GetBytes(number.text);
            udpClient.Send(sendBytes, sendBytes.Length);
        }
    }

    void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);

                TextController.text = text;

                lastRecvMessage = text;

                if (text.Length >= 60)
                {
                    TextController.leaderboardText = text;
                }

                if (text.Length >= 16 && text.Length < 60)
                {
                    TextController.winnerText = text;
                    TextController.gameReset = true;
                }
            }
            catch (Exception err)
            {
                
            }
        }
    }

    void OnApplicationQuit()
    {
        if (ipAddress != null)
        {
            Byte[] sendBytes = Encoding.ASCII.GetBytes("User is quitting client application");
            udpClient.Send(sendBytes, sendBytes.Length);
            receiveThread.Abort();
        }
    }
}