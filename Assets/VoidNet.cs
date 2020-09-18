using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidNet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SurvivorController(Clone)")
        {
            other.gameObject.GetComponent<PhotonView>().RPC("TeleportPlayer", RpcTarget.AllBuffered, Random.value);
            Debug.Log("Hit Net");
        }
        
    }
}
