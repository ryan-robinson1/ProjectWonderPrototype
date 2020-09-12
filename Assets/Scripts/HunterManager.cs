using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 
using System.IO;

public class HunterManager : MonoBehaviour
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
        Debug.Log("Instantiated Hunter Controller");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Spider"), new Vector3(-55, 6, -71), Quaternion.identity);
        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
    }
}
