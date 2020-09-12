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
        
        if (other.gameObject.name == "SurvivorController(Clone)" && slot.transform.childCount > 0)
        {
            setFreeView(other.gameObject);
            GameLogicPV.RPC("IncrementPlayersEscaped", RpcTarget.AllBuffered);
        }
        
    }
    public void setFreeView(GameObject player)
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        HUD.SetActive(false);
        player.transform.position = new Vector3(50,50,-95);
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<GlobalMeshRemover>().enabled = true; 
        player.GetComponent<PlayerController>().enabled = false;
        player.transform.Find("CameraHolder").Find("Camera").GetComponent<SceneView>().enabled = true;

    }
 
    
}
