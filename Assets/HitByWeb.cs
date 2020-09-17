using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HitByWeb : MonoBehaviourPunCallbacks
{
    public float webTimer = 5f;
    float webTimeTracker = float.PositiveInfinity;
    public bool freezePlayer = false;
    public Material webbedMaterial;
    public Material defaultMaterial;
    public GameObject web;
    public bool frozen;
    PhotonView PV; 
   

    private void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }
    private void Update()
    {
        
        if (freezePlayer && !frozen)
        {
            Debug.Log(freezePlayer);
            webTimeTracker = Time.time;

            PV.RPC("changeToWebMaterial", RpcTarget.AllBuffered, true);
            // freeze player when hit by web
            this.GetComponent<PlayerMovement>().controller.enabled = false;
            freezePlayer = false;
            frozen = true; 
           
        }
        if (Time.time - webTimeTracker > webTimer)
        {
            // unfreeze player after time expires
            this.GetComponent<PlayerMovement>().controller.enabled = true;
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
