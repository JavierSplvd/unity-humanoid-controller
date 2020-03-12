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
    public GameObject hand;

    public string primaryAttackAnimTrigger = "attack";
    public string primaryAttackButton = "Fire1";
    public string primaryAttackStateName = "Attack";

    public string secondaryAttackAnimTrigger = "thrust attack";
    public string secondaryAttackButton = "Fire2";
    public string secondaryAttackStateName = "Thrust Attack";

    public string terciaryAttackAnimTrigger = "kick attack";
    public string terciaryAttackButton = "Fire3";
    public string terciaryAttackStateName = "Kick Attack";
    
    private Command currentCommand;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        baseHumanController = GetComponent<BaseHumanController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ResetTriggers();
        if(Input.GetButtonDown(primaryAttackButton) && sword.transform.parent.Equals(hand.transform))
        {
            currentCommand = new HumanAttackCommand(primaryAttackAnimTrigger, primaryAttackButton, joystickDirection.forward);
        } else if(Input.GetButtonDown(primaryAttackButton))
        {
            // anim???
            sword.GetComponent<SwordController>().ReturnToPlayer(hand.transform);        
        } else if(Input.GetButtonDown(secondaryAttackButton))
        {
            currentCommand = new TeleportCommand(secondaryAttackAnimTrigger, sword.transform.position);
        } else if(Input.GetButtonDown(terciaryAttackButton))
        {
            currentCommand = new HumanAttackCommand(terciaryAttackAnimTrigger, terciaryAttackButton, joystickDirection.forward);
        }

        if(currentCommand != null) {
            currentCommand.Execute(gameObject);
        }
        
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(primaryAttackStateName))
        {
            sword.GetComponent<SwordController>().SetCommand(new GoToHandCommand(hand.transform));
            baseHumanController.shouldSteer = false;
        }
        else
        {
            
        }
        
        if(state.IsName(primaryAttackStateName) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f)
        {
            sword.GetComponent<SwordController>().Throw(joystickDirection.forward);
        }

        if (!state.IsName(primaryAttackStateName) || !state.IsName(secondaryAttackStateName) || !state.IsName(terciaryAttackStateName))
        {
            currentCommand = null;
        }
    }

    private void ResetTriggers()
    {
        anim.ResetTrigger(primaryAttackAnimTrigger);
        anim.ResetTrigger(secondaryAttackAnimTrigger);
        anim.ResetTrigger(terciaryAttackAnimTrigger);
    }

}
