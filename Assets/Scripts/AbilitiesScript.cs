﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Claims;
using System.IO;
using JetBrains.Annotations;
using UnityEngine.UI;

public class AbilitiesScript : MonoBehaviourPunCallbacks
{
    


    
    public float distance = 2f;
    public Transform barrel;
    public GameObject webprojectile;
    GameObject observedPlayer;
    GameObject EPrompt;
    GameObject AnimEPrompt; 

    PhotonView PV;
    float timer = float.PositiveInfinity;
    public float webShootTimer = 0.4f;
    float webTimer = 0; 
    public float timeToHold = 2f; 

    private void Start()
    {
        PV = this.GetComponentInParent<PhotonView>();
        EPrompt = GameObject.FindGameObjectWithTag("ETag");
        AnimEPrompt = GameObject.FindGameObjectWithTag("AnimETag");

    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !this.GetComponentInParent<CreatureController>().disableMovement && Time.time - webTimer > webShootTimer) 
        {
            PV.RPC("ShootWeb", RpcTarget.AllBuffered);
            webShootTimer = 0.4f;
            webTimer = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.E) && CheckPlayer())
        {
            timer = Time.time;
            this.GetComponentInParent<CreatureController>().enabled = false;
            EPrompt.GetComponent<Image>().enabled = true;
            AnimEPrompt.GetComponent<Animator>().SetTrigger("PlayWheel");
            AnimEPrompt.GetComponent<Image>().enabled = true;
            

        }
        else if ((Input.GetKeyUp(KeyCode.E) && Time.time - timer < timeToHold && timer != float.PositiveInfinity )|| !CheckPlayer() && !this.GetComponentInParent<CreatureController>().disableMovement)
        {
            timer = float.PositiveInfinity;
            this.GetComponentInParent<CreatureController>().enabled = true;
            AnimEPrompt.GetComponent<Image>().enabled = false;
        }
        else if (Input.GetKey(KeyCode.E) && CheckPlayer() && !this.GetComponentInParent<CreatureController>().disableMovement)
        {
            Debug.Log(Time.time - timer);
            if (Time.time - timer > timeToHold)
            {
                timer = float.PositiveInfinity;
               observedPlayer.GetComponent<PhotonView>().RPC("TeleportPlayer", RpcTarget.AllBuffered,Random.value);
                this.GetComponentInParent<CreatureController>().enabled = true;
                AnimEPrompt.GetComponent<Image>().enabled = false;

            }


        }

    }
    private bool CheckPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 40f))
        {
         
           
            if ((hit.transform.tag == "Survivor" && hit.transform.GetComponent<HitByWeb>().frozen))
            {
                observedPlayer = hit.transform.gameObject;
                EPrompt.GetComponent<Image>().enabled = true;
                return true;
            }
            else
            {
                EPrompt.GetComponent<Image>().enabled = false;
            }
        }
        return false;
    }



    [PunRPC]
    private void ShootWeb()
    {
        GameObject clone;
        clone = Instantiate(webprojectile, barrel.position, barrel.rotation);
        clone.GetComponent<Rigidbody>().AddForce(barrel.transform.forward * 10000);


    }

    


}
