using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class CreatureController : MonoBehaviourPunCallbacks, IOnEventCallback {
    public float moveInputFactor = 5f;
    public Vector3 inputVelocity;
    public Vector3 worldVelocity;
    public Vector3 local_input;
    public float walkSpeed = 2f;
    public float sprintSpeed = 6f;
    public float rotateInputFactor = 10f;
    public float rotationSpeed = 10f;
    public float averageRotationRadius = 3f;
    private float mSpeed = 0;
    private float rSpeed = 0;
    public const byte SpiderMovementEventCode = 1;

    public ProceduralLegPlacement[] legs;
    public int index;
    public bool dynamicGait = false;
    public float timeBetweenSteps = 0.25f;
    public float stepDurationRatio = 2f;
    [Tooltip("Used if dynamicGait is true to calculate timeBetweenSteps")] public float maxTargetDistance = 1f;
    public float lastStep = 0;

    [Header("Alignment")]
    public bool useAlignment = true;
    public int[] nextLegTri;
    public AnimationCurve sensitivityCurve;
    public float desiredSufaceDist = -1f;
    public float dist;
    public bool grounded = false;
    bool pause = false;


    public float mouseSensitivity = 40;
    float verticalLookRotation;
    public GameObject head;
    PhotonView PV;
    public bool disableMovement = false;
    void Start() {
        PV = this.GetComponent<PhotonView>();
  
        this.tag = "Hunter";
        
        Cursor.lockState = CursorLockMode.None;
        for (int i = 0; i < legs.Length; i++) {
            averageRotationRadius += legs[i].restingPosition.z;
        }
        averageRotationRadius /= legs.Length;
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            disableMovement = true;

        }
        else
        {

        }
    }

    void Update() {
       
        if (useAlignment) CalculateOrientation();

        if (!disableMovement)
        {
     /*       //Pause system
            if (Input.GetKeyDown(KeyCode.Escape) && !pause)
            {
                Debug.Log("Pause");
                pause = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


            }
            else if (Input.GetKeyDown(KeyCode.Escape) && pause)
            {
                Debug.Log("Unpause");
                pause = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;


            }*/
            Move();
            Look();
            Debug.Log("hello");
        }

        if (dynamicGait) {
            if (grounded) {
                timeBetweenSteps = maxTargetDistance / Mathf.Max(worldVelocity.magnitude, Mathf.Abs(2 * Mathf.PI * rSpeed * Mathf.Deg2Rad * averageRotationRadius));
            } else {
                timeBetweenSteps = 0.25f;
            }
        }

        if (Time.time > lastStep + (timeBetweenSteps / legs.Length) && legs != null) {
            index = (index + 1) % legs.Length;
            if (legs[index] == null) return;

            for (int i = 0; i < legs.Length; i++) {
                legs[i].MoveVelocity(CalculateLegVelocity(i));
                
            }

            legs[index].stepDuration = Mathf.Min(1f, (timeBetweenSteps / legs.Length) * stepDurationRatio);
            PV.RPC("globalChangeWorldVelocity", RpcTarget.Others, index );
            PV.RPC("globalStep", RpcTarget.AllBuffered, index);
            lastStep = Time.time;
        }
    }

    public Vector3 CalculateLegVelocity(int legIndex) {
        Vector3 legPoint = (legs[legIndex].restingPosition);
        Vector3 legDirection = legPoint - transform.position;
        Vector3 rotationalPoint = ((Quaternion.AngleAxis((rSpeed * timeBetweenSteps) / 2f, transform.up) * legDirection) + transform.position) - legPoint;
        DrawArc(transform.position, legDirection, rSpeed / 2f, 10f, Color.black, 1f);
        return rotationalPoint + (worldVelocity * timeBetweenSteps) / 2f;
    }

    private void Move() {
        
            mSpeed = (Input.GetButton("Fire3") ? sprintSpeed : walkSpeed);
            Vector3 localInput = Vector3.ClampMagnitude(transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))), 1f);
            inputVelocity = Vector3.MoveTowards(inputVelocity, localInput, Time.deltaTime * moveInputFactor);
             


            rSpeed = Mathf.MoveTowards(rSpeed, Input.GetAxis("Turn") * rotationSpeed, rotateInputFactor * Time.deltaTime);




            worldVelocity = inputVelocity * mSpeed;

        object[] content = new object[] {worldVelocity};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(SpiderMovementEventCode,content , new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);


        transform.Rotate(0f, rSpeed * Time.deltaTime, 0f);

            transform.position += (worldVelocity * Time.deltaTime);
       
    }
    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        head.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code==SpiderMovementEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            worldVelocity = (Vector3)data[0];
            
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    [PunRPC]
    void globalStep(int index)
    {
        legs[index].Step();
    }
    [PunRPC]
    void globalChangeWorldVelocity(int index)
    {
        legs[index].worldVelocity = CalculateLegVelocity(index);
    }
  
    private void CalculateOrientation() {
        Vector3 up = Vector3.zero;
        float avgSurfaceDist = 0;

        grounded = false;

        Vector3 point, a, b, c;

        // Takes The Cross Product Of Each Leg And Calculates An Average Up
        for (int i = 0; i < legs.Length; i++) {
            point = legs[i].stepPoint;
            avgSurfaceDist += transform.InverseTransformPoint(point).y;
            a = (transform.position - point).normalized;
            b = ((legs[nextLegTri[i]].stepPoint) - point).normalized;
            c = Vector3.Cross(a, b);
            up += c * sensitivityCurve.Evaluate(c.magnitude) + (legs[i].stepNormal == Vector3.zero ? transform.forward : legs[i].stepNormal);
            grounded |= legs[i].legGrounded;

            Debug.DrawRay(point, a, Color.red, 0);

            Debug.DrawRay(point, b, Color.green, 0);

            Debug.DrawRay(point, c, Color.blue, 0);
        }
        up /= legs.Length;
        avgSurfaceDist /= legs.Length;
        dist = avgSurfaceDist;
        Debug.DrawRay(transform.position, up, Color.red, 0);

        // Asigns Up Using Vector3.ProjectOnPlane To Preserve Forward Orientation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, up), up), 22.5f * Time.deltaTime);
        if (grounded) {
            transform.Translate(0, -(-avgSurfaceDist + desiredSufaceDist) * 0.5f, 0, Space.Self);
        } else {
            // Simple Gravity
            transform.Translate(0, -20 * Time.deltaTime, 0, Space.World);
        }
    }

    public void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, averageRotationRadius);
    }

    public void DrawArc(Vector3 point, Vector3 dir, float angle, float stepSize, Color color, float duration) {
        if (angle < 0) {
            for (float i = 0; i > angle + 1; i -= stepSize) {
                Debug.DrawLine(point + Quaternion.AngleAxis(i, transform.up) * dir, point + Quaternion.AngleAxis(Mathf.Clamp(i - stepSize, angle, 0), transform.up) * dir, color, duration);
            }
        } else {
            for (float i = 0; i < angle - 1; i += stepSize) {
                Debug.DrawLine(point + Quaternion.AngleAxis(i, transform.up) * dir, point + Quaternion.AngleAxis(Mathf.Clamp(i + stepSize, 0, angle), transform.up) * dir, color, duration);
            }
        }
    }
}
