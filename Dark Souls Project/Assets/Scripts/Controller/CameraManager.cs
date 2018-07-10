using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public bool lockOn;
    public float followSpeed = 7;
    public float mouseSpeed = 2;
    public float turnSmoothing = 0.1f; // the time the smoothing takes
    public float minAngle = -35;
    public float maxAngle = 35;
    public float lookAngle;
    public float tiltAngle;


    [HideInInspector] public Transform player;
    [HideInInspector] public Transform lockOnTransform;
    public Transform pivot; // used to rotate camera up and down (around x-axis)
    public Transform camTrans;
    [HideInInspector] public Transform _transform;

    float smoothX;
    float smoothY;
    float smoothXvelocity;
    float smoothYvelocity;

    public float defZ;
    float curZ;
    public float zSpeed;

    bool usedRightAxis;
    bool changeTargetLeft;
    bool changeTargetRight;

    StatesManager states;

    public void Initialize(StatesManager st)
    {
        states = st;
        _transform = this.transform;
        player = st._transform;
        _transform.position = player.position; // I added this in I think
        curZ = defZ;
        

    }

    public void FixedTick(float d)
    {
        float h = Input.GetAxis(StaticStrings.mouseX);
        float v = Input.GetAxis(StaticStrings.mouseY);

        float targetSpeed = mouseSpeed;

        

        FollowTarget(); //d
        HandleRotations(h, v); // h,v,d,targetSpeed
        //HandlePivotPosition();
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


        //if (lockOn /*&& lockOnTransform*/)
        //{
        //    Vector3 targetDir = lockOnTransform.position - transform.position;
        //    targetDir.y = 0;

        //    if (targetDir.sqrMagnitude < 3f)
        //        targetDir = transform.forward;

        //    Quaternion targetRot = Quaternion.LookRotation(targetDir);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 9);
        //    return;
        //}
        //else
        {
            // rotate camera holder about y-axis
            lookAngle += smoothX * mouseSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);

        }

    }

    void HandlePivotPosition()
    {
        float targetZ = defZ;
        CameraCollision(defZ, ref targetZ);

        curZ = Mathf.Lerp(curZ, targetZ, states.delta * zSpeed);
        Vector3 tp = Vector3.zero;
        tp.z = curZ;
        camTrans.localPosition = tp;
    }

    void CameraCollision(float targetZ, ref float actualZ)
    {
        float step = Mathf.Abs(targetZ);
        int stepCount = 2;
        float stepIncrement = step / stepCount;

        RaycastHit hit;
        Vector3 origin = pivot.position;
        Vector3 direction = -pivot.forward;

        if(Physics.Raycast(origin, direction, out hit, states.ignoreForGroundCheck))
        {
            float dist = Vector3.Distance(hit.point, origin);
            actualZ = -(dist * 0.5f);
        }
        else
        {
            for (int k = 0; k <stepCount+1;k++)
            {
                for (int j = 0; j < 4;j++)
                {
                    Vector3 dir = Vector3.zero;
                    Vector3 secondOrigin = origin + (direction * k) * stepCount;

                    switch(j)
                    {
                        case 0:
                            dir = camTrans.right;
                            break;
                        case 1:
                            dir = -camTrans.right;
                            break;
                        case 2:
                            dir = camTrans.up;
                            break;
                        case 3:
                            dir = -camTrans.up;
                            break;                        
                    }

                    Debug.DrawRay(secondOrigin, dir * 0.2f, Color.red);
                    if(Physics.Raycast(secondOrigin, dir, out hit, 0.2f, states.ignoreForGroundCheck))
                    {
                        Debug.Log(hit.transform.root.name);
                        float distance = Vector3.Distance(secondOrigin, origin);
                        actualZ = -distance * 0.5f;
                        if (actualZ < 0.2f)
                            actualZ = 0;
                        return;
                    }
                }
            }
        }
    }

    public static CameraManager singleton;
    private void Awake()
    {
        singleton = this;
    }

    // not used but useful
    public void AlignWithPlayerDirection()
    {
        Vector3 targetDir = player.transform.forward;
        Quaternion targetRot = Quaternion.LookRotation(targetDir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime);
    }
}
