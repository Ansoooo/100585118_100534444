using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//References
//https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.udpclient?view=netframework-4.8
//https://forum.unity.com/threads/simple-udp-implementation-send-read-via-mono-c.15900/

public class Send : MonoBehaviour
{
    //Singleton
    private static Send _instance;
    public static Send instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }



    public string IP;
    public int port;

    public IPEndPoint remoteEndPoint;
    UdpClient client;

    public bool isClient;
    public string message = "";

    public GameObject player;
    public GameObject ball;
    public int score;

    public void changeIP(string input)
    {
        IP = input;
    }

    public void changeClient(bool input)
    {
        isClient = !input;
    }

    void Start()
    {
        //IP = "127.0.0.1"; // change?
        isClient = PlayerControl.instance.isClient;
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
    }

    void Update()
    {
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

        message = ""; //reset message
        if (isClient == true)
        {
            message += "!client"; // add who's sending

            message += score.ToString();
            message += "#";

            message += player.transform.position.x.ToString();
            message += "@";
            message += player.transform.position.z.ToString();

            message += "|";

            message += ball.transform.position.x.ToString();
            message += "@";
            message += ball.transform.position.z.ToString();
        }
        else
        {
            message += "!server";

            message += score.ToString();
            message += "#";

            message += player.transform.position.x.ToString();
            message += "@";
            message += player.transform.position.z.ToString();

            message += "|";

            message += ball.transform.position.x.ToString();
            message += "@";
            message += ball.transform.position.z.ToString();
        }

        sendString(message);
    }

    private void sendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            client.Send(data, data.Length, remoteEndPoint);
        }
        catch
        {
            Debug.Log("Invalid IP");
        }
    }
}