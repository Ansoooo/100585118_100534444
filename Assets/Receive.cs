//Anson Cheng 100585118    Zhijun Yang 100534444
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

public class Receive : MonoBehaviour
{
    //Singleton
    private static Receive _instance;
    public static Receive instance
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



    Thread receiver;
    UdpClient client;

    public int port;

    public string message = "";

    public GameObject player2;
    Rigidbody playerRB;
    public GameObject ball;
    public GameObject ball2;

    public float Px, Py, Bx, By;
    public int score;
    public bool connection = false;

    void Start()
    {
        receiver = new Thread(new ThreadStart(ReceiveData));
        receiver.IsBackground = true;
        receiver.Start();

        Px = -player2.transform.position.x;
        Py = player2.transform.position.z;

        playerRB = player2.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (connection)
        {
            playerRB.MovePosition(new Vector3(-Px, 0.0f, Py));      

            if (Send.instance.isClient == true)
            {
                ball.SetActive(false);
                ball2.SetActive(true);
                ball2.transform.position = new Vector3(-Bx, 0.0f, By);
                PlayerControl.instance.score = -score;
            }
        }
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);

        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                message = text;
                interpretMessage();
                connection = true;
            }
            catch
            {
                Debug.Log("Waiting for Connection");
                connection = false;
            }
        }
    }

    public void interpretMessage()
    {

        //!client score# NUMBER@NUMBER|NUMBER@NUMBER
        string identity = message.Substring(0, 7);
        string info = message.Substring(7); //everything after identity
        string[] infoSet = info.Split(char.Parse("#")); //0 is the score, 1 is the positions
        string[] infoSetPosi = infoSet[1].Split(char.Parse("|")); //0 is the player position, 1 is the puck position

        string[] playerPosi = infoSetPosi[0].Split(char.Parse("@")); //0 is the X, 1 is the Y
        string[] puckPosi = infoSetPosi[1].Split(char.Parse("@")); //0 is the X, 1 is the Y

        score = int.Parse(infoSet[0]);

        Px = float.Parse(playerPosi[0]);
        Py = float.Parse(playerPosi[1]);

        Bx = float.Parse(puckPosi[0]);
        By = float.Parse(puckPosi[1]);
    }
}