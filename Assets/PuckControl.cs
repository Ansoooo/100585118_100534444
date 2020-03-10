//Anson Cheng 100585118    Zhijun Yang 100534444
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckControl : MonoBehaviour
{
    Vector3 startPosPla;
    Vector3 startPosOpp;
    Vector3 startVel;
    // Start is called before the first frame update
    void Start()
    {
        startPosPla = new Vector3(-3f, 0f, 0f);
        startPosOpp = new Vector3(3f, 0f, 0f);
        startVel = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > 9.5f)
        {
            PlayerControl.instance.score += 1;
            gameObject.transform.position = startPosOpp;
            gameObject.GetComponent<Rigidbody>().velocity = startVel;
        }
        else if (gameObject.transform.position.x < -9.5)
        {
            PlayerControl.instance.score -= 1;
            gameObject.transform.position = startPosPla;
            gameObject.GetComponent<Rigidbody>().velocity = startVel;
        }
    }
}
