using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameScript : MonoBehaviourPunCallbacks
{
    //Imported Objects
    public GameObject PowerCrystal;
    public GameObject Lock;
    
    //Game State Variables
    public string gameState;
    public int keysPlaced;
    public int playersInJail;
    public int playersEscaped = 0;
    public float timeToEscape = 10f;
    public GameObject powerCrystalSlot1, powerCrystalSlot2;
    public GameObject teleporter; 
    PhotonView PV;
    bool inRoom = true;

    bool switchedToEscape = false; 
    void Start()
    {
        PV = this.GetComponent<PhotonView>();
        gameState = "Capture";
        keysPlaced = 0;
        playersInJail = 0; 
    }

   
    void Update()
    {
       
        if (PowerCrystal.GetComponent<IsRemovedScript>().removed && !switchedToEscape)
        {
            gameState = "Escape";
            switchedToEscape = true; 
        }
        else if(gameState == "Escape")
        {
            timeToEscape -= Time.deltaTime;
            Debug.Log(timeToEscape);
            powerCrystalSlot1.tag = "CanPlace";
            powerCrystalSlot2.tag = "CanPlace";
        }
        if (timeToEscape < 0 && PhotonNetwork.IsMasterClient && inRoom)
        {
            inRoom = false;
            PV.RPC("LeaveRoom", RpcTarget.AllBuffered);


        }
        if (PhotonNetwork.PlayerList.Length - 1 <= playersEscaped && inRoom &&PhotonNetwork.PlayerList.Length !=1)
        {
            inRoom = false;
            PV.RPC("LeaveRoom", RpcTarget.AllBuffered);
        }

        keysPlaced = Lock.GetComponent<KeyDetector>().getKeysPlaced();
    }
   
    
    [PunRPC]
    public void LeaveRoom()
    {
        Destroy(RoomManager.Instance.gameObject);
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
    }
    [PunRPC]
    public void IncrementPlayersEscaped()
    {
        playersEscaped++;
    }
}
