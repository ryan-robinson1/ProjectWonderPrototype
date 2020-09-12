using UnityEngine;
using Photon.Pun;

public class RigidBodySync : MonoBehaviourPun, IPunObservable
{

    Rigidbody r;
     
    Vector3 latestPos;
    Quaternion latestRot;
    Vector3 velocity;
    Vector3 angularVelocity;
    public Vector3 rescaleVector; 

    bool valuesReceived = false;
    public bool inHand = false;
    public bool inHandGlobal = false;
    public bool pickedUpFlag = false;
    public bool rescale; 


    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
  
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(r.velocity);
            stream.SendNext(r.angularVelocity);
        }
        else
        { 
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            velocity = (Vector3)stream.ReceiveNext();
            angularVelocity = (Vector3)stream.ReceiveNext();

            valuesReceived = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inHand && pickedUpFlag)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            pickedUpFlag = false;
        }
        if (!photonView.IsMine && valuesReceived)
        {
            //Update Object position and Rigidbody parameters
            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 5);
            r.velocity = velocity;
            r.angularVelocity = angularVelocity;
        }
    }

    
    [PunRPC]
    void setKinematic()
    {
        r.isKinematic = true;
        

    }
    [PunRPC]
    void setGravity()
    {
        r.GetComponent<Rigidbody>().isKinematic = false;
        r.GetComponent<Rigidbody>().useGravity = true;
    }
    [PunRPC]
    void setInHandGlobalTrue()
    {
        inHandGlobal = true; 
    }
    [PunRPC]
    void setInHandGlobalFalse()
    {
        inHandGlobal = false;
    }
    [PunRPC]
    void Rescale()
    {
       transform.parent = null;
       transform.localScale = rescaleVector;
    }
}