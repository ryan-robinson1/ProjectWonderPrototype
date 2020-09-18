using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HitByWeb : MonoBehaviourPunCallbacks
{
    public float webTimer = 5f;
    float webTimeTracker = float.PositiveInfinity;

    public float  frozenCooldownTimer = 6f;
    float frozenCooldownTimeTracker = float.PositiveInfinity;
    public bool freezePlayer = false;
    public Material webbedMaterial;
    public Material defaultMaterial;
    public Material coolDownMaterial; 
    public GameObject web;
    public bool frozen;
    float runSpeed;
    float walkSpeed;
    float crouchSpeed;
    PhotonView PV; 
   

    private void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }
    private void Update()
    {
        
        if(Time.time-frozenCooldownTimeTracker> frozenCooldownTimer)
        {
            frozenCooldownTimeTracker = float.PositiveInfinity;
            PV.RPC("changeToCooldownMaterial", RpcTarget.AllBuffered, false);
        }
        if(frozenCooldownTimeTracker != float.PositiveInfinity)
        {
            freezePlayer = false;
            PV.RPC("changeToCooldownMaterial", RpcTarget.AllBuffered, true);
        }
        else if (freezePlayer && !frozen)
        {
            Debug.Log(freezePlayer);
            webTimeTracker = Time.time;
            runSpeed = this.GetComponent<PlayerMovement>().runSpeed;
            walkSpeed = this.GetComponent<PlayerMovement>().walkSpeed;
            crouchSpeed = this.GetComponent<PlayerMovement>().crouchSpeed;

            PV.RPC("changeToWebMaterial", RpcTarget.AllBuffered, true);
            // freeze player when hit by web
            this.GetComponent<PlayerMovement>().runSpeed = 0;
            this.GetComponent<PlayerMovement>().walkSpeed = 0;
            this.GetComponent<PlayerMovement>().crouchSpeed = 0;
            freezePlayer = false;
            frozen = true; 
           
        }
        if (Time.time - webTimeTracker > webTimer)
        {
            // unfreeze player after time expires
            this.GetComponent<PlayerMovement>().runSpeed = runSpeed;
            this.GetComponent<PlayerMovement>().walkSpeed = walkSpeed;
            this.GetComponent<PlayerMovement>().crouchSpeed = crouchSpeed;
            PV.RPC("changeToWebMaterial", RpcTarget.AllBuffered, false);
            webTimeTracker = float.PositiveInfinity;
            frozen = false;
            PV.RPC("SetCooldownTimer", RpcTarget.AllBuffered);


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
    [PunRPC]
    void changeToCooldownMaterial(bool change)
    {
        if (change)
            this.GetComponent<Renderer>().material = coolDownMaterial;
        else
            this.GetComponent<Renderer>().material = defaultMaterial;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "WebProjectile" && GameObject.Find("Spider(Clone)").tag == "LocalHunter")
        {
            web = collision.gameObject;

            PV.RPC("DestroyObject", RpcTarget.All);



        }
    }
    [PunRPC]
    void DestroyObject()
    {
        freezePlayer = true;
        
        Destroy(web); 
    }
    [PunRPC]
    void SetCooldownTimer()
    {
        frozenCooldownTimeTracker = Time.time;
    }
   

}
