using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OpenDungeon : MonoBehaviour
{
    Animator dungeonAnimator;
    PhotonView PV;

    float doorOpenTimer = 6f;
    float doorOpenTimerTracker = float.PositiveInfinity;
    void Start()
    {
        dungeonAnimator = this.GetComponentInParent<Animator>();
        PV = this.GetComponent<PhotonView>(); 
    }

    void Update()
    {
        if (Time.time - doorOpenTimerTracker > doorOpenTimer)
        {
            PV.RPC("CloseDungeonDoor", RpcTarget.AllBuffered);
            doorOpenTimerTracker = float.PositiveInfinity;
        }
    }

    [PunRPC]
    void OpenDungeonDoor()
    {
        dungeonAnimator.SetTrigger("DoorOpen");
        doorOpenTimerTracker = Time.time; 
     
    }
    [PunRPC]
    void CloseDungeonDoor()
    {
        dungeonAnimator.SetTrigger("DoorClose");
    }
}
