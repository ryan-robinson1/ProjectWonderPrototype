using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; 
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
   
    Vector3 LocalSpawn;
    System.Random random = new System.Random();
    ArrayList spawnPoints = new ArrayList();
    Vector3 SpawnPoint1 = new Vector3(513, 35, -287);
    Vector3 SpawnPoint2 = new Vector3(518, 17, -477);
    Vector3 SpawnPoint3 = new Vector3(56, 37, -354);
    Vector3 SpawnPoint4 = new Vector3(92, 5, -158);
    Vector3 SpawnPoint5 = new Vector3(327, 5, -233);
   



    void Awake()
    {
        SpawnPoint1.x += random.Next(4);
        SpawnPoint2.x += random.Next(4);
        SpawnPoint3.x += random.Next(4);
        SpawnPoint4.x += random.Next(4);
        SpawnPoint5.x += random.Next(4);

        spawnPoints.Add(SpawnPoint1);
        spawnPoints.Add(SpawnPoint2);
        spawnPoints.Add(SpawnPoint3);
        spawnPoints.Add(SpawnPoint4);
        spawnPoints.Add(SpawnPoint5);
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
        setSpawnPoint(); 
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SurvivorController"), LocalSpawn, Quaternion.identity);
    }

   
   void setSpawnPoint()
    {

        LocalSpawn = (Vector3)spawnPoints[random.Next(4)];
    }

}
