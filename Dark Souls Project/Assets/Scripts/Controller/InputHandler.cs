using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public GamePhase curPhase;
    public StatesManager states;
    public CameraManager camManager;

    Transform camTrans;

    float b_timer;
    float delta;

    float vertical;
    float horizontal;

    bool b_input; //space_input
    bool a_input; // f_input
    bool x_input; // x_input  
    bool y_input; // leftAlt_input
    bool rb_input; // LMB_input
    bool rt_input; // RMB_input
    bool lb_input; // leftShift_input
    bool lt_input; //tab_input

    private void Start()
    {
        InGame_Initialize();
    }
    public void InGame_Initialize()
    {
        states.Initialize();
        camManager.Initialize(states);
        camTrans = camManager._transform;
    }
    void FixedUpdate()
    {
        delta += Time.deltaTime;
        GetInput_FixedUpdate();

        switch(curPhase)
        {
            case GamePhase.inGame:
                InGame_UpdateStates_FixedUpdate();
                states.FixedTick(delta);
                camManager.FixedTick(delta);
                break;
            case GamePhase.inMenu:
                break;
            case GamePhase.inInventory:
                break;
            default:
                break;
        }
    }
    void Update()
    {
        delta += Time.deltaTime;
        GetInput_Update();

        switch (curPhase)
        {
            case GamePhase.inGame:
                InGame_UpdateStates_Update();
                states.Tick(delta);                
                break;
            case GamePhase.inMenu:
                break;
            case GamePhase.inInventory:
                break;
            default:
                break;
        }
    }

    void GetInput_FixedUpdate()
    {
        vertical = Input.GetAxis(StaticStrings.Vertical);
        horizontal = Input.GetAxis(StaticStrings.Horizontal);
    }
    void GetInput_Update()
    {
        b_input = Input.GetButton(StaticStrings.B);
        a_input = Input.GetButtonUp(StaticStrings.A);
        x_input = Input.GetButtonUp(StaticStrings.X);
        y_input = Input.GetButton(StaticStrings.Y);

        rt_input = Input.GetButton(StaticStrings.RT);
        lt_input = Input.GetButton(StaticStrings.LT);
        rb_input = Input.GetButton(StaticStrings.RB);
        lb_input = Input.GetButton(StaticStrings.LB);

        if (b_input)
            b_timer += delta;
    }

    void InGame_UpdateStates_FixedUpdate()
    {
        states.input.vertical = vertical;
        states.input.horizontal = horizontal;
        states.input.moveAmount = Mathf.Clamp01(Mathf.Abs(vertical) + Mathf.Abs(horizontal));

        Vector3 moveDir = camTrans.forward * vertical;
        moveDir += camTrans.right * horizontal;
        moveDir.Normalize();
        states.input.moveDir = moveDir;
    }
    void InGame_UpdateStates_Update()
    {

    }
}

public enum GamePhase
{
    inGame, inMenu, inInventory
}
