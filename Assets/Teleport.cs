using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Teleport : MonoBehaviour
{
     Vector3 TPPoint2 = new Vector3(480, -30, -800);
     Vector3 TPPoint3 = new Vector3(28, -35, -186);
        System.Random random2 = new System.Random();

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
      /*  TPPoint2.x = random2.Next(4);
        TPPoint3.x = random2.Next(4);
        TPPoint2.z = random2.Next(4);
        TPPoint3.z = random2.Next(4);*/

        if (random<.5)
            gameObject.transform.position = TPPoint3;
        else
            gameObject.transform.position = TPPoint2;
    }
}
