using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Threading;

public class WebScript : MonoBehaviourPunCallbacks
{
    PhotonView PV; 
    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    private void OnCollisionEnter(Collision collision)
    {
       /* if(collision.gameObject.tag == "web" && PhotonNetwork.IsMasterClient)
        {
            PV.RPC("DestroyWeb", RpcTarget.AllBuffered);
        }*/
        if (collision.gameObject.tag == "Ground" && PhotonNetwork.IsMasterClient)
        {

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Web"), this.transform.position, Quaternion.identity);
            
        }
        
    }
    [PunRPC]
    void DestroyWeb()
    {
        Destroy(gameObject);
    }
}
