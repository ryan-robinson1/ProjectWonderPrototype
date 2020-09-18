using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Claims;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviourPunCallbacks
{
    /**
     * PickUpScript: Allows player to pick up objects they look at
     * 
         * equipPosition: Transform, keeps track of location of "hand"
         * distance: Float, how far an object can be picked up from
         * inHand: Bool, tracks if object is equipped

     */


    public GameObject currentObject;
    public Transform equipPosition;
    public GameObject padlock;
    GameObject placeableSurface; 
    public float distance = 5f;
    public bool inHand;
    public GameObject EPrompt;
   
    
    PhotonView PV;
    private void Start()
    {
        EPrompt = GameObject.FindGameObjectWithTag("ETag");
    }

    void Update()
    {
    
        if(!CheckGrab() && !CheckPlace() && !CheckLock())
        {
            EPrompt.GetComponent<Image>().enabled = false;
        }
        if (CheckGrab() && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
            Debug.Log("Picked up");
        }
        else if (CheckLock() && Input.GetKeyDown(KeyCode.E))
        {
            padlock.GetComponent<PhotonView>().RPC("OpenDungeonDoor", RpcTarget.AllBuffered);
            Debug.Log("Picked up");
        }


        else if (inHand && Input.GetKeyDown(KeyCode.E) && !CheckPlace())
        {
            Drop();
            Debug.Log("Dropped");
        }

        if(CheckPlace() && Input.GetKeyDown(KeyCode.E))
        {
            Place();
        }
        
        
    }

    //Shoots ray from player. If rayed item is grabbable, return true. Return false otherwise
    private bool CheckGrab()
    {
        RaycastHit hit;

        

        if (Physics.Raycast(transform.position, transform.forward, out hit, 18f))
        {
            if ((hit.transform.tag == "CanGrab" || hit.transform.tag == "OnlyGrab") && !inHand)
            {
                EPrompt.GetComponent<Image>().enabled = true;
                currentObject = hit.transform.gameObject;
          

                if (currentObject.GetComponent<RigidBodySync>().inHandGlobal)
                    return false;
                

                return true;
            }

        }

        return false;
    }

    //Shoots ray from player. If rayed object is a placeable surface, return true. Return false otherwise
    private bool CheckPlace()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 18f))
        {
            if ((hit.transform.tag == "CanPlace" ) && inHand && currentObject.transform.Find("KeyTag") !=null)
            {
                EPrompt.GetComponent<Image>().enabled = true;
                placeableSurface = hit.transform.gameObject;
                return true;
            }
        }
        return false;
    }
    private bool CheckLock()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 18f))
        {
            
            if (hit.transform.tag == "Lock")
            {
                EPrompt.GetComponent<Image>().enabled = true;
                padlock = hit.transform.gameObject;
                return true;
            }

        }
        return false;
    }

    private void PickUp()
    {
        inHand = true;
        currentObject.transform.position = equipPosition.position;
        currentObject.transform.parent = equipPosition;
        currentObject.transform.localEulerAngles = new Vector3(0f, 100f, 100f);

        if(currentObject.GetComponent<RigidBodySync>().rescale)
        {
            PV.RPC("Rescale", RpcTarget.AllBuffered);
        }
       
        currentObject.GetComponent<RigidBodySync>().inHand = true;
        currentObject.GetComponent<RigidBodySync>().pickedUpFlag=true;

        PV = currentObject.GetComponent<PhotonView>();
        PV.RPC("setKinematic", RpcTarget.AllBuffered);
        PV.RPC("setInHandGlobalTrue", RpcTarget.AllBuffered);

    }


    private void Drop()
    {
       currentObject.GetComponent<RigidBodySync>().inHand = false;
       inHand = false;
       PV.RPC("setGravity", RpcTarget.AllBuffered);
       PV.RPC("setInHandGlobalFalse", RpcTarget.AllBuffered);
       currentObject.transform.parent = null;
       currentObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50);

    }
    private void Place()
    {
        currentObject.GetComponent<RigidBodySync>().inHand = false;
        inHand = false;
        PV.RPC("setInHandGlobalFalse", RpcTarget.AllBuffered);


        
        currentObject.transform.position = placeableSurface.transform.position;
        currentObject.transform.rotation = Quaternion.identity;
        currentObject.transform.parent = null;
        currentObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        currentObject.transform.parent = placeableSurface.transform;
        foreach (Transform t in currentObject.transform)
        {

            t.gameObject.tag = "Untagged";
            
        }

        currentObject.tag = "Untagged";
        placeableSurface.tag = "Untagged";
        
        if (placeableSurface.transform.parent.transform.parent.name == "Pedastools")
        {
            placeableSurface.GetComponentInParent<PhotonView>().RPC("IncrementSlotNum", RpcTarget.AllBuffered);
        }
        
       

        //currentObject.transform.rotation = Quaternion.identity; 


    }
    
}
