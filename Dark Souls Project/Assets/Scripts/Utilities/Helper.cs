﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour {

    [Range(-1, 1)] public float vertical;
    [Range(-1, 1)] public float horizontal;

    public string[] oh_attacks;
    public string[] th_attacks;
    public bool playAnim;
    public bool twoHanded;
    public bool enableRM;
    public bool useItem;
    public bool interacting;
    public bool lockon;

    Animator anim;

	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	
	void Update ()
    {
        
        enableRM = !anim.GetBool("canMove");
        anim.applyRootMotion = enableRM;

        interacting = anim.GetBool("interacting");

        if(!lockon)
        {
            horizontal = 0;
            vertical = Mathf.Clamp01(vertical);
        }
        anim.SetBool("lockon", lockon);
        if (enableRM) return;
        if (useItem)
        {           
            anim.Play("use_item");
            useItem = false;
        }
        if(interacting)
        {
            playAnim = false;
            vertical = Mathf.Clamp(vertical, 0, 0.5f);
        }

        anim.SetBool("IsTwoHanded", twoHanded);
        if (playAnim)
        {
            string targetAnim;
            if(!twoHanded)
            {
                int r = Random.Range(0, oh_attacks.Length);
                targetAnim = vertical > 0.5f ? "oh_attack_3" : oh_attacks[r]; 
            }
            else
            {
                int r = Random.Range(0, th_attacks.Length);
                targetAnim = vertical > 0.5f ? "oh_attack_3" : th_attacks[r];

            }
            vertical = 0;
            anim.CrossFade(targetAnim, 0.2f);
            //anim.SetBool("canMove", false);
            //enableRM = true;
            playAnim = false;
        }
        anim.SetFloat("vertical", vertical);
        anim.SetFloat("horizontal", horizontal);

        

    }

    public void OpenDamageColliders() {}
    public void CloseDamageColliders() {}
}
