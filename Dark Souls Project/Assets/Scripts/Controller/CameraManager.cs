using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager singleton;

    public bool lockOn;
    public float followSpeed = 7;
    public float mouseSpeed = 2;
    public float turnSmoothing = 0.1f; // the time the smoothing takes
    public float minAngle = -35;
    public float maxAngle = 35;
    public float lookAngle;
    public float tiltAngle;


    public Transform player;
    public Transform lockonTarget;
    [HideInInspector] public Transform pivot; // used to rotate camera up and down (around x-axis)
    [HideInInspector] public Transform camTrans;

    float smoothX;
    float smoothY;
    float smoothXvelocity;
    float smoothYvelocity;

    void Awake()
    {
        singleton = this;
    }

    public void Init(Transform t)
    {
        player = t;
        transform.position = player.position;
        camTrans = Camera.main.transform;
        pivot = camTrans.parent;

    }

    public void FixedTick()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        FollowTarget();
        HandleRotations(h, v);
    }

    void FollowTarget()
    {
        Vector3 targetPos = Vector3.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
        transform.position = targetPos;
    }
    void HandleRotations(float h, float v)
    {
        if (turnSmoothing > 0f) 
        {
            smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
        }
        else
        {
            smoothX = h;
            smoothY = v;
        }

        // rotate pivot about x-axis
        tiltAngle -= smoothY * mouseSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);


        if (lockOn && lockonTarget)
        {
            Vector3 targetDir = lockonTarget.position - transform.position;
            targetDir.y = 0;
            
            if (targetDir.sqrMagnitude < 3f)
                targetDir = transform.forward;

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime);
            return;
        }
        else
        {
            // rotate camera holder about y-axis
            lookAngle += smoothX * mouseSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);

        }     
       
    }
    public void AlignWithPlayerDirection()
    {
        Vector3 targetDir = player.transform.forward;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime);
    }
}
