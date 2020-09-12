using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading;

public class WebCaptureScript : MonoBehaviourPunCallbacks
{
    float destroyTimerLog = float.PositiveInfinity;
    float destroyTimer =6f;
    float colTimer = 0.2f;

    void Start()
    {
        destroyTimerLog = Time.time;   
    }
    private void Update()
    {
        if (Time.time - destroyTimerLog > destroyTimer)
        {
            Destroy(gameObject);

        }
    }
   


}
