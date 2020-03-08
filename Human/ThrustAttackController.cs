using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(BaseHumanController))]
public class ThrustAttackController : MonoBehaviour
{
    private Animator anim;
    private CharacterController characterController;
    private BaseHumanController baseHumanController;

    public GameObject sword;
    private Transform swordOriginalParent;
    public GameObject hand;

    public String thrustAttackAnimTrigger = "thrust attack";

    public string thrustAttackButton = "Fire2";
    private bool attackButtonIsDown = false;
    public string thrustAttackStateName = "Thrust Attack";
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
        attackButtonIsDown = Input.GetButtonDown(thrustAttackButton);
        TriggerAttackAnimState();

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(thrustAttackStateName))
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
    }

    private void LateUpdate()
    {
    }
    private void TriggerAttackAnimState()
    {
        if (attackButtonIsDown)
        {
            anim.SetTrigger(thrustAttackAnimTrigger);
        }
        else
        {
            anim.ResetTrigger(thrustAttackAnimTrigger);
        }
    }
}
