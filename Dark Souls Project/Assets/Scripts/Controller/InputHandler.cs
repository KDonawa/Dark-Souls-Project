using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    float vertical;
    float horizontal;

    // attack buttons
    public bool key1, key2, LMB, RMB;

    // other buttons
    public bool space_key, G_key, leftShift, leftAlt;

    StateManager states;
    CameraManager camManager;


    void Start ()
    {
        states = GetComponent<StateManager>();
        states.Init();
        camManager = CameraManager.singleton;
        camManager.Init(transform);
	}

    void Update()
    {
        states.Tick();
        GetInput();
        UpdateStates();
    }
    void FixedUpdate ()
    {
        states.FixedTick();
        camManager.FixedTick();
    }

    void UpdateStates()
    {
        states.horizontal = horizontal;
        states.vertical = vertical;

        // set move direction to the direction the camera is facing
        Vector3 v = vertical * camManager.transform.forward;
        Vector3 h = horizontal * camManager.transform.right;
        states.moveDir = (v + h).normalized;

        // set move amount
        float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        states.moveAmount = Mathf.Clamp01(m);

        // set roll state
        states.rollInput = space_key;
        
        // set run state
        states.runToggle = leftShift;

        // states.attack1 = LMB;
        // etc.

        //toggle 2h
        if (leftAlt)
        {
            states.isTwoHanded = !states.isTwoHanded;
            states.HandleTwoHanded();
        }

        // toggle for lockon
        if(G_key)
        {
            states.lockOn = !states.lockOn;

            if (!states.lockonTarget)
                states.lockOn = false;

            camManager.lockonTarget = states.lockonTarget.transform;
            camManager.lockOn = states.lockOn;
        }


    }

    void GetInput()
    {
        // movement input
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        // attack input   
        LMB = Input.GetKeyDown(KeyCode.Mouse0);
        RMB = Input.GetKeyDown(KeyCode.Mouse1);
        key1 = Input.GetKeyDown(KeyCode.Alpha1);
        key2 = Input.GetKeyDown(KeyCode.Alpha2);

        // other input
        leftShift = Input.GetKey(KeyCode.LeftShift);

        leftAlt = Input.GetKeyDown(KeyCode.LeftAlt);      
        G_key = Input.GetKeyDown(KeyCode.G);
        space_key = Input.GetKeyDown(KeyCode.Space);

    }


  
}
