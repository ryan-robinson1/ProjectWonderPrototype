﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
   
   

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController(); 
        }
    }
    void CreateController()
    {
        Debug.Log("Instantiated Player Controller");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SurvivorController"), new Vector3(-22, 2, -5), Quaternion.identity);
    }
}
