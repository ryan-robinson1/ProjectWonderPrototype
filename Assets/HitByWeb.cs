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
    float runSpeed;
    float walkSpeed;
    float crouchSpeed;

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
            runSpeed = this.GetComponent<PlayerMovement>().runSpeed;
            walkSpeed = this.GetComponent<PlayerMovement>().walkSpeed;
            crouchSpeed = this.GetComponent<PlayerMovement>().crouchSpeed;


            PV.RPC("changeToWebMaterial", RpcTarget.AllBuffered, true);
            this.GetComponent<PlayerMovement>().runSpeed = 0;
            this.GetComponent<PlayerMovement>().walkSpeed = 0;
            this.GetComponent<PlayerMovement>().crouchSpeed = 0;

            
            
            freezePlayer = false;
            frozen = true; 
           
        }
        if (Time.time - webTimeTracker > webTimer)
        {
            Debug.Log("Player is unfrozen");
            this.GetComponent<PlayerMovement>().runSpeed = runSpeed;
            this.GetComponent<PlayerMovement>().walkSpeed = walkSpeed;
            this.GetComponent<PlayerMovement>().crouchSpeed = crouchSpeed;
            PV.RPC("changeToWebMaterial", RpcTarget.AllBuffered, false);
            webTimeTracker = float.PositiveInfinity;
            frozen = false;
            
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
