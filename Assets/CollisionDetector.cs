using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public GameScript gameManager;
    public OpenDungeon dungeon;
    public SafeZoneDetector safeZone;
    public GameObject safeZoneObject; 
    public bool inHitBox = false;
    bool playerCounted = false;

    private void Update()
    {
    
        if (!dungeon.dungeonClosed)
        {
            safeZoneObject.SetActive(false);
        }
        else
        {
            safeZoneObject.SetActive(true); 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
      /*  if (other.gameObject.name == "SurvivorController(Clone)" && dungeon.dungeonClosed && !safeZone.inHitBox)
        {
            gameManager.numPlayersInJail++; 
            Debug.Log("PutInJailByEnter");
            playerCounted = true; 
        }*/
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SurvivorController(Clone)" && !dungeon.dungeonClosed)
        {
            gameManager.numPlayersInJail--;
            inHitBox = false;
            playerCounted = false;
            Debug.Log("LeftJail");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        inHitBox = true;
        if (other.gameObject.name == "SurvivorController(Clone)" && dungeon.dungeonClosed && !safeZone.inHitBox && !playerCounted)
        {
            gameManager.numPlayersInJail++;
            playerCounted = true; 
            Debug.Log("PutInJailByStay");
        }
    }
}
