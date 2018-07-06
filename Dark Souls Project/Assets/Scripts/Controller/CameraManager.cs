using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager singleton;

    public bool lockon;
    public float followSpeed = 9;
    public float mouseSpeed = 2;
    public float turnSmoothing = 0.1f; // the time the smoothing takes
    public float minAngle = -35;
    public float maxAngle = 35;
    public float lookAngle;
    public float tiltAngle;


    public Transform target;
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
        target = t;
        transform.position = target.position;
        camTrans = Camera.main.transform;
        pivot = camTrans.parent;

    }

    public void Tick(float d)
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        FollowTarget(d);
        HandleRotations(d, h, v, mouseSpeed);
    }

    void FollowTarget(float d)
    {
        Vector3 targetPos = Vector3.Lerp(transform.position, target.position, followSpeed * d);
        transform.position = targetPos;
    }
    void HandleRotations(float d, float h, float v, float mouseSpeed)
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
        if(lockon)
        {

        }

        // rotate camera holder about y-axis
        lookAngle += smoothX * mouseSpeed;
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);

        // rotate pivot about x-axis
        tiltAngle -= smoothY * mouseSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

    }
}
