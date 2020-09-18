using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class TeleportPlayer : MonoBehaviourPunCallbacks
{
    public GameObject slot;
    public GameObject HUD;
    public PhotonView GameLogicPV;
   
    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //&& 
        if (other.gameObject.name == "SurvivorController(Clone)" )
        {
            setFreeView(other.gameObject);
            GameLogicPV.RPC("IncrementPlayersEscaped", RpcTarget.AllBuffered);
        }
        
    }
    public void setFreeView(GameObject player)
    {
        
        //HUD.SetActive(false);
        player.transform.GetChild(0).GetChild(0).GetChild(0).transform.position = new Vector3(50,50,-95);
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<GlobalMeshRemover>().enabled = true; 
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SceneView>().enabled = true;
        player.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<CameraMovement>().enabled = false;

    }
 
    
}
