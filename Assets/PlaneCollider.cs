using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 
public class PlaneCollider : MonoBehaviour
{
    public GameScript gameManager; 
    private void OnTriggerExit(Collider other)
    {
        gameManager.GetComponent<PhotonView>().RPC("DecrementPlayersJailed", RpcTarget.AllBuffered);
    }
}
