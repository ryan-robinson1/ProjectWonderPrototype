using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GlobalMeshRemover : MonoBehaviourPunCallbacks
{

    void Start()
    {
        this.GetComponent<PhotonView>().RPC("DisableMeshGlobal", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void DisableMeshGlobal()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }


}
