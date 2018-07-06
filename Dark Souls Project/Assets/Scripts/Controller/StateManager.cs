using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    public GameObject activeModel;

    [Header("Inputs")]
    public float vertical;
    public float horizontal;
    public float moveAmount;
    public Vector3 moveDir;

    [Header("Stats")]
    public float moveSpeed = 7;
    public float runSpeed = 9.5f;
    public float rotateSpeed = 5;
    public float distToGround = 0.5f;

    [Header("States")]
    public bool runToggle;
    public bool onGround;
    public bool lockOn;
  
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] public LayerMask ignoreLayers;

    float delta;


    public void Init()
    {
        SetupAnimator();
        rigid = GetComponent<Rigidbody>();
        ignoreLayers = ~(1 << 10);

        //anim.SetBool("onGround", true);
    }
    public void Tick(float d)
    {
        delta = d;
        onGround = OnGround();
        anim.SetBool("OnGround", onGround);

    }
    public void FixedTick(float d)
    {
        delta = d;
        rigid.drag = (moveAmount > 0 || !onGround) ? 0f : 4f;
        if (runToggle) { lockOn = false; }

        MoveTarget();
        RotateTarget();
        AnimateMovement();

    }

    void MoveTarget()
    {      
        float targetSpeed = runToggle ? runSpeed : moveSpeed;
        if (moveAmount > 0 && onGround)
            rigid.velocity = moveDir * (targetSpeed * moveAmount);
    }
    void RotateTarget()
    {
        if (moveAmount > 0 && !lockOn)
        {
            Vector3 targetDir = moveDir;
            //targetDir.y = 0;
            //if (targetDir == Vector3.zero)
                //targetDir = transform.forward;

            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            Quaternion currentRot = Quaternion.Slerp(transform.rotation, targetRot, delta * moveAmount * rotateSpeed);
            transform.rotation = currentRot;
        }
        
    }
    void AnimateMovement()
    {
        anim.SetBool("IsRunning", runToggle && moveAmount > 0);
        anim.SetFloat("vertical", moveAmount, 0.4f, delta);
    }

    void SetupAnimator()
    {
        if (activeModel)
        {
            anim = activeModel.GetComponent<Animator>();
            anim.applyRootMotion = false;
        }
    }
    public bool OnGround()
    {
        Vector3 origin = transform.position + Vector3.up * distToGround;
        Vector3 dir = -Vector3.up;
        float dist = distToGround + 0.3f;

        RaycastHit hit;
        Debug.DrawRay(origin, dir * dist, Color.red);
        if(Physics.Raycast(origin, dir, out hit, dist, ignoreLayers))
        {
            Vector3 targetPos = hit.point;
            transform.position = targetPos;
            return true;
        }

        return false;
    }
}
