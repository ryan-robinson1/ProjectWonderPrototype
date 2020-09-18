using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Teleport : MonoBehaviour
{
     Vector3 TPPoint2 = new Vector3(480, -35, -800);
     Vector3 TPPoint3 = new Vector3(28, -23, -179);
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    [PunRPC]
    public void TeleportPlayer(float random)
    {
        if(random<.5)
            gameObject.transform.position = TPPoint3;
        else
            gameObject.transform.position = TPPoint2;
    }
}
