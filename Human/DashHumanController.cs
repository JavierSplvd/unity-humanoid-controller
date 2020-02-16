using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(BaseHumanController))]
public class DashHumanController : MonoBehaviour
{
    private Animator anim;
    private CharacterController characterController;
    private BaseHumanController baseHumanController;
    private bool dashInput = false;
    private bool isInDash = false;
    [Range(0, 3)]
    public float dashMaxTime;
    private float dashCurrentTime;
    [Range(0, 10)]
    public float dashSpeed;

    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        baseHumanController = GetComponent<BaseHumanController>();
    }

    void Update()
    {
        dashInput = Input.GetButtonDown("");

        TriggerDashAnimState();
        DashClock();
    }

    void LateUpdate() {
        HorizontalMovement();
    }

    void HorizontalMovement()
    {
        Vector3 m = baseHumanController.GetInputInCameraCoordinates() * Time.deltaTime * dashSpeed;
        characterController.Move(m);
    }

    void TriggerDashAnimState()
    {
        if(dashInput)
        {
            isInDash = true;
            anim.SetBool("dash", isInDash);
            dashCurrentTime = dashMaxTime;
        }
    }

    void DashClock()
    {
        if(isInDash)
        {
            dashCurrentTime -= Time.deltaTime;
            if(dashCurrentTime < 0f)
            {
                dashCurrentTime = 0f;
                StopDashing();
            }
        }
    }

    void StopDashing()
    {
        isInDash = false;
        anim.SetBool("dash", isInDash);
    }
}
