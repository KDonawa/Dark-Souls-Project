using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    float vertical;
    float horizontal;
    bool runInput;
    //float delta;

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
        //delta = Time.deltaTime
        states.Tick(Time.deltaTime);
    }
    void FixedUpdate ()
    {
        //delta = Time.fixedDeltaTime;
        GetInput();
        UpdateStates();
        states.FixedTick(Time.fixedDeltaTime);
        camManager.Tick(Time.deltaTime);
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

        // set run state
        states.runToggle = runInput;
      
    }

    void GetInput()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        runInput = Input.GetButton("Fire3"); // left shift
    }
}
