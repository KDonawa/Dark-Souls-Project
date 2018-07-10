using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesManager : MonoBehaviour {

    public ControllerStats stats;
    public States states;
    public InputVariables input;
    public GameObject activeModel;
        
    #region References
    [HideInInspector] public Animator anim;
    [HideInInspector] Rigidbody rigid;
    [HideInInspector] Collider controllerCollider;
    #endregion

    [HideInInspector] public LayerMask ignoreLayers;
    [HideInInspector] public LayerMask ignoreForGroundCheck;

    [HideInInspector] public Transform _transform;
    [HideInInspector] public float delta;

    public CharState charState;
    public enum CharState
    {
        moving, inAir, interacting, attacking
    }

    public void Initialize()
    {
        _transform = this.transform;
        SetupAnimator();

        rigid = GetComponent<Rigidbody>();
        rigid.angularDrag = Mathf.Infinity;
        rigid.drag = 4;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;

        // may need to change
        gameObject.layer = 9; 
        ignoreLayers = ~(1 << 10); 
        ignoreForGroundCheck = ~(1 << 10 | 1 << 11);

       
    }

    public void FixedTick(float d)
    {
        delta = d;

        //states.onGround = OnGround();

        switch (charState)
        {
            case CharState.moving:
                HandleRotation();
                HandleMovement();
                break;
            case CharState.inAir:
                break;
            case CharState.interacting:
                break;
            case CharState.attacking:
                break;
            default:
                break;
        }



    }

    public void Tick(float d)
    {
        delta = d;

        states.onGround = OnGround();

        switch (charState)
        {
            case CharState.moving:
                HandleMovementAnims();
                break;
            case CharState.inAir:
                break;
            case CharState.interacting:
                break;
            case CharState.attacking:
                break;
            default:
                break;
        }

    }

    void HandleMovementAnims()
    {
        if(states.isLockedOn)
        {

        }
        else
        {
            anim.SetBool(StaticStrings.IsRunning, states.isRunning);
            anim.SetFloat(StaticStrings.vertical, input.moveAmount, 0.15f, delta);
        }
    }
    void HandleRotation()
    {
        Vector3 targetDir = !states.isLockedOn ?
            input.moveDir : input.lockOnTransform == null ?
            input.lockOnTransform.position - _transform.position : input.moveDir;

        targetDir.y = 0;
        if (targetDir == Vector3.zero)
            targetDir = _transform.forward;

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion currentRot = Quaternion.Slerp(_transform.rotation, targetRot, delta * input.moveAmount * stats.rotateSpeed);
        _transform.rotation = currentRot;

    }
    void HandleMovement()
    {
        rigid.drag = input.moveAmount > 0 ? 0f : 4f;
        float speed = states.isRunning ? stats.sprintSpeed : stats.moveSpeed;
        Vector3 v = states.isLockedOn ? input.moveDir : _transform.forward;
        v *= (speed * input.moveAmount);
        //v.y = rigid.velocity.y;
        rigid.velocity = v;
     
    }
    void SetupAnimator()
    {
        if (activeModel == null)
        {
            anim = GetComponentInChildren<Animator>();
            activeModel = anim.gameObject;
        }
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;
        anim.GetBoneTransform(HumanBodyBones.LeftHand).localScale = Vector3.one;
        anim.GetBoneTransform(HumanBodyBones.RightHand).localScale = Vector3.one;

    }
    bool OnGround()
    {
        Vector3 origin = _transform.position;
        origin.y += 0.7f;
        Vector3 dir = -Vector3.up;
        float dist = 1.4f;
        RaycastHit hit;

        if(Physics.Raycast(origin, dir, out hit, dist, ignoreForGroundCheck))
        {
            Vector3 targetPos = hit.point;
            _transform.position = targetPos;
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class InputVariables
{
    public float moveAmount;
    public float horizontal;
    public float vertical;
    public Vector3 moveDir;
    public Transform lockOnTransform;
}

[System.Serializable]
public class States
{
    public bool onGround;
    public bool isRunning;
    public bool isLockedOn;
    public bool isInAction;
    public bool isMoveEnabled;
    public bool isDamageOn;
    public bool isRotateEnabled;
    public bool isAttackEnabled;
    public bool isSpellcasting;
    public bool isIKEnabled;
    public bool isUsingItem;
    public bool isAbleToBeParried;
    public bool isParryOn;
    public bool isLeftHand;
    public bool animIsOnEmpty;
    public bool closeWeapons;
    public bool isInvisible;

}

[System.Serializable]
public class NetworkStates
{
    public bool isLocal;
    public bool isInRoom;
}
