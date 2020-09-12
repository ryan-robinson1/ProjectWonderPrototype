using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class ItemManager : MonoBehaviourPunCallbacks
{
    


    void Start()
    {
        /*if (PhotonNetwork.IsMasterClient)
        {
            CreateCrystals();
        }*/
       
    }
    void CreateCrystals()
    {
        Debug.Log("Instantiated 4 Crystal Keys");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BlueCrystal"), new Vector3(-22, 2, -5),Quaternion.identity);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "RedCrystal"), new Vector3(-22, 2, -5), Quaternion.identity);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "RedCrystal"), new Vector3(-22, 2, -5), Quaternion.identity);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GreenCrystal"), new Vector3(-22, 2, -5), Quaternion.identity);
    }
}
