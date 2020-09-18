using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

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
    float timeToEscape = 60f;
    public GameObject powerCrystalSlot1, powerCrystalSlot2;
    public GameObject teleporter;
    public Text timer;
    public Animator anim1;
    public Animator anim2;
    PhotonView PV;
    bool inRoom = true;
    bool gatesOpened = false; 

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
            timer.enabled = false;
        }
        else if(gameState == "Escape")
        {
            timer.enabled = true;
            timeToEscape -= Time.deltaTime;
            TimeSpan t = TimeSpan.FromSeconds(timeToEscape);
            string timeLeft = string.Format("{0:D2}:{1:D2}",
                t.Minutes,
                t.Seconds
                );
            timer.text = timeLeft.Substring(1);
            
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
        if((powerCrystalSlot1.transform.childCount > 0 || powerCrystalSlot2.transform.childCount > 0) && gatesOpened == false)
        {
            PV.RPC("OpenExitGates", RpcTarget.AllBuffered);
            gatesOpened = true; 
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
    [PunRPC]
    public void OpenExitGates()
    {
        Debug.Log("Opened Gates");
        anim1.SetTrigger("Open");
        anim2.SetTrigger("Open");
    }
}
