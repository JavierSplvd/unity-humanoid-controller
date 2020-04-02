using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCommand : Command
{
    public string attackAnimTrigger = "attack";
    public string attackButton = "Attack";
    private Vector3 targetPosition;
    private bool attackButtonIsDown = false;
    private Animator anim;
    private CharacterController characterController;

    public TeleportCommand(string attackAnimTrigger, Vector3 targetPosition)
    {
        this.attackAnimTrigger = attackAnimTrigger;
        this.targetPosition = targetPosition;
    }

    // Update is called once per frame
    public override void Execute(GameObject gameObject, float scale = 1f)
    {
        if(anim == null) {
            anim = gameObject.GetComponent<Animator>();
        }
        if(characterController == null) {
            characterController = gameObject.GetComponent<CharacterController>();
        }

        characterController.enabled = false;
        gameObject.transform.position = new Vector3(targetPosition.x, gameObject.transform.position.y, targetPosition.z);
        characterController.enabled = true;

        Debug.Log(gameObject.transform.position);
        anim.SetTrigger(attackAnimTrigger);
    }

}
