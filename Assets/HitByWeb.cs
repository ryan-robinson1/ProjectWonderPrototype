using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HitByWeb : MonoBehaviourPunCallbacks
{
    public float webTimer = 4f;
    float webTimeTracker = float.PositiveInfinity;
    public bool freezePlayer = false;
    public Material webbedMaterial;
    public Material defaultMaterial;
    public GameObject web;
    public bool frozen;
    PhotonView PV; 
    float sprintSpeed;
    float walkSpeed;

    private void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }
    private void Update()
    {
        
        if (freezePlayer)
        {
            Debug.Log(freezePlayer);
            webTimeTracker = Time.time;
            Debug.Log("Player is frozen");

            //We will change this once the updated player controller is implemented, player should freeze
 /*           sprintSpeed = this.GetComponent<PlayerController>().sprintSpeed;
            walkSpeed = this.GetComponent<PlayerController>().walkSpeed;*/


            PV.RPC("changeToWebMaterial", RpcTarget.AllBuffered, true);
      /*      this.GetComponent<PlayerController>().sprintSpeed = 0;
            this.GetComponent<PlayerController>().walkSpeed = 0;*/

            if(PV.IsMine)
                this.GetComponent<Rigidbody>().isKinematic = true; 
            
            freezePlayer = false;
            frozen = true; 
           
        }
        if (Time.time - webTimeTracker > webTimer)
        {
            Debug.Log("Player is unfrozen");
 /*           this.GetComponent<PlayerController>().sprintSpeed = sprintSpeed;
            this.GetComponent<PlayerController>().walkSpeed = walkSpeed;*/
            PV.RPC("changeToWebMaterial", RpcTarget.AllBuffered, false);
            webTimeTracker = float.PositiveInfinity;
            frozen = false;
            if (PV.IsMine)
                this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    [PunRPC]
    void changeToWebMaterial(bool change)
    {
        if(change)
            this.GetComponent<Renderer>().material = webbedMaterial;
        else
            this.GetComponent<Renderer>().material = defaultMaterial;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "WebProjectile")
        {
            web = collision.gameObject;
            freezePlayer = true;
    
            PV.RPC("DestroyObject", RpcTarget.AllBuffered);



        }
    }
    [PunRPC]
    void DestroyObject()
    {
        Destroy(web); 
    }

}
