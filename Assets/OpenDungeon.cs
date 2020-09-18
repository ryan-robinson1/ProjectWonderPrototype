using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OpenDungeon : MonoBehaviour
{
    Animator dungeonAnimator;
    PhotonView PV;
    int idleId = Animator.StringToHash("DungeonIdle");

    float doorOpenTimer = 6f;
    float doorOpenTimerTracker = float.PositiveInfinity;
    float idleStateTimer = 2f;
    float idleStateTimerTracker = float.PositiveInfinity;
    public bool dungeonClosed = true;
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
        if (Time.time - idleStateTimerTracker > idleStateTimer)
        {
            dungeonClosed = true;
            idleStateTimerTracker = float.PositiveInfinity;
        }
    }

    [PunRPC]
    void OpenDungeonDoor()
    {
        dungeonAnimator.SetTrigger("DoorOpen");
        dungeonClosed = false;
        doorOpenTimerTracker = Time.time; 
     
    }
    [PunRPC]
    void CloseDungeonDoor()
    {
        dungeonAnimator.SetTrigger("DoorClose");
        dungeonClosed = true;
        //idleStateTimerTracker = Time.time; 
    }
}
