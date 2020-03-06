using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Singleton
    private static PlayerControl _instance;
    public static PlayerControl instance
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



    public bool isClient;
    public GameObject UDPClient;
    public GameObject Canvas;
    Rigidbody rb;
    //Vector3 moveTowards;
    Vector3 mouse;


    public void changeClient(bool input)
    {
        isClient = !input;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Screen.fullScreen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (UDPClient.activeSelf == true)
        {
            if (Receive.instance.connection)
            {
                mouse = Input.mousePosition;
                mouse = Camera.main.ScreenToWorldPoint(mouse);

                if (mouse.x > 0f)
                {
                    mouse.x = 0f;
                }

                rb.MovePosition(new Vector3(mouse.x, 0.0f, mouse.z));
            }
        }

        if(Input.GetKey(KeyCode.Return))
        {
            Canvas.SetActive(false);
            UDPClient.SetActive(true);
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
