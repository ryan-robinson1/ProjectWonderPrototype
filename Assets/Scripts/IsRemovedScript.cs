using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class IsRemovedScript : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    public bool removed = false;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    void Update()
    {
        if(this.transform.parent == null || this.transform.parent.name != "CrystalHolder")
        {
            PV.RPC("Remove", RpcTarget.AllBuffered);
        }
     
    }

    [PunRPC]
   void Remove()
    {
        removed = true;
    }
}
