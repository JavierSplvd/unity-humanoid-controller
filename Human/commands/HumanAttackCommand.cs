using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttackCommand : Command
{
    public string attackAnimTrigger = "attack";
    public string attackButton = "Attack";
    private Vector3 initialDirection;
    private bool attackButtonIsDown = false;
    private Animator anim;

    public HumanAttackCommand(string attackAnimTrigger, string attackButton, Vector3 initialDirection)
    {
        this.attackAnimTrigger = attackAnimTrigger;
        this.attackButton = attackButton;
        this.initialDirection = initialDirection;
    }

    // Update is called once per frame
    public override void Execute(GameObject gameObject)
    {
        if(anim == null) {
            anim = gameObject.GetComponent<Animator>();
        }
        gameObject.transform.rotation = Quaternion.LookRotation(initialDirection, Vector3.up);
        attackButtonIsDown = Input.GetButtonDown(attackButton);
        TriggerAttackAnimState();
    }

    private void TriggerAttackAnimState()
    {
        if (attackButtonIsDown)
        {
            anim.SetTrigger(attackAnimTrigger);
        }
        else
        {
            anim.ResetTrigger(attackAnimTrigger);
        }
    }
}
