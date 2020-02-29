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
    public string dashButton = "Dash";
    private Vector3 dashDirection = new Vector3();

    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        baseHumanController = GetComponent<BaseHumanController>();
    }

    void Update()
    {
        dashInput = Input.GetButtonDown(dashButton);
        Debug.Log(dashInput);
        TriggerDashAnimState();
        DashClock();
    }

    void LateUpdate()
    {
        HorizontalMovement();
    }

    void HorizontalMovement()
    {
        if (isInDash)
        {
            Vector3 m = dashDirection * Time.deltaTime * dashSpeed;
            characterController.Move(m);
        }

    }

    void SteerToInputDirection(Vector3 inputDirection)
    {
        Quaternion rotation = Quaternion.LookRotation(inputDirection, Vector3.up);
        transform.rotation = rotation;
    }

    void TriggerDashAnimState()
    {
        if (dashInput && !isInDash)
        {
            isInDash = true;
            anim.SetBool("dash", isInDash);
            dashCurrentTime = dashMaxTime;
            baseHumanController.shouldSteer = false;
            dashDirection = baseHumanController.GetInputInCameraCoordinates();
            SteerToInputDirection(dashDirection);
        }
    }

    void DashClock()
    {
        if (isInDash)
        {
            dashCurrentTime -= Time.deltaTime;
            if (dashCurrentTime < 0f)
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
        baseHumanController.shouldSteer = true;
    }
}
