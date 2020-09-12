using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTransformUpdater : MonoBehaviour
{
    public Camera FPCamera;
    PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            transform.rotation = FPCamera.transform.rotation;
        }
      
    }
  
}
