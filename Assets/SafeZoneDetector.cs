using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneDetector : MonoBehaviour
{
    public bool inHitBox = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "SurvivorController(Clone)")
        {
            inHitBox = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SurvivorController(Clone)")
        {
            inHitBox = false;
        }
    }
}
