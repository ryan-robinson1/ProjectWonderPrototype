using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrapplingScript : MonoBehaviourPunCallbacks
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask Grappleable;
    public Transform gunTip, camera,player;
    private float maxDistance = 35f;
    private SpringJoint joint;
    float distanceFromPoint;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
       
    }
    private void Update()
    {
        distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
        if (Input.GetMouseButtonDown(1))
        {
            lr.enabled = true;
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }


    }
    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.position,camera.forward,out hit, maxDistance,Grappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float initialDistanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = initialDistanceFromPoint * 0.45f;
            joint.minDistance = initialDistanceFromPoint * 0.25f;

            joint.spring = 20f;
            joint.damper = 4f;
            joint.massScale = 4.5f;

            lr.positionCount = 2; 
        }
        
    }
  
    void DrawRope()
    {
        if (!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }
    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
}
