using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(BaseHumanController))]
public class AttackController : MonoBehaviour
{
    private Animator anim;
    private CharacterController characterController;
    private BaseHumanController baseHumanController;
    public Transform joystickDirection;

    public GameObject sword;
    private Transform swordOriginalParent;
    public GameObject hand;

    public string attackAnimTrigger = "attack";
    public string attackButton = "Fire1";
    public string attackStateName = "Attack";

    public string thrustAttackAnimTrigger = "thrust attack";
    public string thrustAttackButton = "Fire2";
    public string thrustAttackStateName = "Thrust Attack";

    public string kickAttackAnimTrigger = "kick attack";
    public string kickAttackButton = "Fire3";
    public string kickAttackStateName = "Kick Attack";
    
    private Command currentCommand;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        baseHumanController = GetComponent<BaseHumanController>();
        swordOriginalParent = sword.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(attackButton))
        {
            currentCommand = new HumanAttackCommand(attackAnimTrigger, attackButton, joystickDirection.forward);
        } else if(Input.GetButtonDown(thrustAttackButton))
        {
            currentCommand = new HumanAttackCommand(thrustAttackAnimTrigger, thrustAttackButton, joystickDirection.forward);
        } else if(Input.GetButtonDown(kickAttackButton))
        {
            currentCommand = new HumanAttackCommand(kickAttackAnimTrigger, kickAttackButton, joystickDirection.forward);
        }

        if(currentCommand != null) {
            currentCommand.Execute(gameObject);
        }
        
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(attackStateName) || state.IsName(thrustAttackStateName))
        {
            sword.transform.parent = hand.transform;
            sword.transform.position = hand.transform.position;
            sword.transform.rotation = hand.transform.rotation;
            baseHumanController.shouldSteer = false;
        }
        else
        {
            sword.transform.parent = swordOriginalParent;
            sword.transform.position = swordOriginalParent.position;
            sword.transform.rotation = Quaternion.identity;
        }

        if (!state.IsName(attackStateName) || !state.IsName(thrustAttackStateName) || !state.IsName(kickAttackStateName))
        {
            currentCommand = null;
        }
    }

    private void LateUpdate()
    {
    }

}
