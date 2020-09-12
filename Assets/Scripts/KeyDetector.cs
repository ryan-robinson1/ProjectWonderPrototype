using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class KeyDetector : MonoBehaviourPunCallbacks
{

    int slot = 1;
    public bool allKeysPlaced = false;
    PhotonView PV;
    public GameObject vault;
    Animator gateAnimator; 

    public Material indicatorMaterial;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        gateAnimator = vault.GetComponent<Animator>(); 

    }
    void Update()
    {
        if(slot > 4)
        {
            allKeysPlaced = true;

            PV.RPC("OpenVault", RpcTarget.AllBuffered);

        }
        
       
    }
    public int getKeysPlaced()
    {
        return slot - 1; 
    }
    public string GetOpenSlot()
    {
        return "Slot" + slot;
    }
    [PunRPC]
    void IncrementSlotNum()
    {
        slot++;
        Debug.Log(slot);
    }
    [PunRPC]
    void OpenVault()
    {
        gateAnimator.SetTrigger("OpenGate"); 
    
       
    }
}
