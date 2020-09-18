using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CatchPlayerIfTooLow : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (this.transform.position.y < -250)
        {
            this.GetComponent<PlayerMovement>().enabled = false;
            this.GetComponent<CharacterController>().enabled = false;
            
            this.GetComponent<PhotonView>().RPC("TeleportPlayer", RpcTarget.AllBuffered, Random.value);
            
            Debug.Log("Teleported");

            this.GetComponent<PlayerMovement>().enabled = true;
            this.GetComponent<CharacterController>().enabled = true;
        }
    }
}
