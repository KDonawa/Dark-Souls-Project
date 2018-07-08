using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    public GameObject activeModel;
    //public float delta;

    [Header("Inputs")]
    public float vertical;
    public float horizontal;
    public float moveAmount;
    public Vector3 moveDir;
    //public bool attack1, attack2, attack3;
    public bool rollInput;
    InputHandler input;

    [Header("Stats")]
    public float moveSpeed = 7;
    public float runSpeed = 9.5f;
    public float rotateSpeed = 5;
    public float distToGround = 0.5f;
    public float rollSpeed = 1;

    [Header("States")]
    public bool runToggle;
    public bool onGround;
    public bool lockOn;
    public bool inAction;
    public bool canMove;
    public bool isTwoHanded;
    

    [Header("Other")]
    public TargetEnemy lockonTarget;
    //public AnimationCurve rollCurve;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] public AnimatorHook a_hook;
    [HideInInspector] public LayerMask ignoreLayers;

    
    float _actionDelay;

    CameraManager camManager;

    public void Init()
    {
        SetupAnimator();
        rigid = GetComponent<Rigidbody>();
        camManager = CameraManager.singleton;
        a_hook = activeModel.AddComponent<AnimatorHook>();
        input = GetComponent<InputHandler>();
        a_hook.Init(this);

        ignoreLayers = ~(1 << 10);

        //anim.SetBool("onGround", true);
    }
    public void Tick()
    {

        onGround = OnGround();
        anim.SetBool("OnGround", onGround);

    }
    public void FixedTick()
    {
        canMove = anim.GetBool("canMove");
        if (!canMove) return;

        HandleRolls();
        HandleAttacks();

        if (inAction)
        {
            anim.applyRootMotion = true;
            _actionDelay += Time.fixedDeltaTime;
            if (_actionDelay > 0.3f)
            {
                inAction = false;
                _actionDelay = 0;
                anim.applyRootMotion = false;
            }
            else
                return;
        }
        
        Move();
        Rotate();
        AnimateMovement();

    }

    public bool OnGround()
    {
        Vector3 origin = transform.position + Vector3.up * distToGround;

        Vector3 dir = -Vector3.up;
        float dist = distToGround + 0.3f;

        RaycastHit hit;
        Debug.DrawRay(origin, dir * dist, Color.red);
        if (Physics.Raycast(origin, dir, out hit, dist, ignoreLayers))
        {
            Vector3 targetPos = hit.point;
            transform.position = targetPos;
            return true;
        }

        return false;
    }  
    public void HandleTwoHanded()
    {
        anim.SetBool("IsTwoHanded", isTwoHanded);
    }

    void HandleAttacks()
    {
        string targetAnim;
        if (input.key1) { targetAnim = "oh_attack_1"; }
        else if (input.LMB) { targetAnim = "oh_attack_2"; }
        else if (input.key2) { targetAnim = "oh_attack_3"; }
        else if (input.RMB) { targetAnim = "th_attack_1"; }
        else return;

        canMove = false;
        inAction = true;

        anim.CrossFade(targetAnim, 0.2f);
    }
    void HandleRolls()
    {
        a_hook.rm_multi = 1;

        if (!rollInput) return;

        float v = moveAmount > 0.3f ? 1 : 0;
        float h = 0;

        if (v != 0)
        {
            if (moveDir == Vector3.zero)
                moveDir = transform.forward;
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = targetRot;
            a_hook.rm_multi = rollSpeed;
        }
        else
        {
            a_hook.rm_multi = 1.3f;
        }

        anim.SetFloat("vertical", v);
        anim.SetFloat("horizontal", h);

        canMove = false;
        inAction = true;
        //anim.CrossFade("Rolls", 0.2f);
        anim.Play("Rolls");

    }
    void Move()
    {
        if (runToggle)
        {
            lockOn = false;
            camManager.lockOn = false;
        }

        rigid.drag = (moveAmount > 0 || !onGround) ? 0f : 4f;
        float targetSpeed = runToggle ? runSpeed : moveSpeed;
        if (moveAmount > 0 && onGround)
            rigid.velocity = moveDir * (targetSpeed * moveAmount);
    }
    void Rotate()
    {
        if (moveAmount == 0) return;
       
        Vector3 targetDir = !lockOn ? moveDir : (lockonTarget.transform.position - transform.position).normalized;
        targetDir.y = 0;
        //if (targetDir == Vector3.zero)
        //    targetDir = transform.forward;

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion currentRot = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * moveAmount * rotateSpeed);
        transform.rotation = currentRot;
        
      
        
    }
    void AnimateMovement()
    {
        anim.SetBool("lockon", lockOn);
        if (lockOn)
            AnimateLockonMovement(moveDir);
        else
        {
            anim.SetBool("IsRunning", runToggle && moveAmount > 0);
            anim.SetFloat("vertical", moveAmount, 0.4f, Time.deltaTime);
        }
    }
    void AnimateLockonMovement(Vector3 moveDir)
    {
        Vector3 relativeDir = transform.InverseTransformDirection(moveDir);
        float h = relativeDir.x;
        float v = relativeDir.z;

        anim.SetFloat("vertical", v, 0.2f, Time.deltaTime);
        anim.SetFloat("horizontal", h, 0.2f, Time.deltaTime);
    }

    void SetupAnimator()
    {
        if (activeModel == null)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim == null)
                Debug.Log("No model found");
            else
                activeModel = anim.gameObject;          
        }
        if (anim == null)
            anim = activeModel.GetComponent<Animator>();

        anim.applyRootMotion = false;
    }

    
}
