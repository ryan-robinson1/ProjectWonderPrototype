using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
    public bool checkPlayer = false;
    public GameObject player; 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.transform.tag);
        if ((other.gameObject.transform.tag == "Survivor" && other.gameObject.transform.GetComponent<HitByWeb>().frozen))
        {
            Debug.Log("Made it in here");
            checkPlayer = true;
            player = other.gameObject;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.transform.tag);
        if (other.gameObject == player && !other.gameObject.transform.GetComponent<HitByWeb>().frozen)
        {
            checkPlayer = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.transform.tag);
        if (other.gameObject == player)
        {
            checkPlayer = false;
        }
    }
}
