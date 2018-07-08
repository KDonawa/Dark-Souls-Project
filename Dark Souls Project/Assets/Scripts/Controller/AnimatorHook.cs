using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHook : MonoBehaviour {

    public float rm_multi;


    float roll_t;
    bool isRolling;

    Animator anim;
    StateManager states;

    
    public void Init(StateManager st)
    {
        states = st;
        anim = st.anim;

    }

    void OnAnimatorMove()
    {
        if (states.canMove) return;

        states.rigid.drag = 0;

        if (rm_multi == 0)
            rm_multi = 1;

        Vector3 delta = anim.deltaPosition;
        delta.y = 0;
        Vector3 v = (delta * rm_multi) / Time.deltaTime;
        states.rigid.velocity = v;

    }


}
