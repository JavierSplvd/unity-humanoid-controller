using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class HumanController : MonoBehaviour
{
    private Animator anim;
    private CapsuleCollider capsuleCollider;
    private Vector3 inputWorldCoordinates;
    private Vector3 inputCameraReference;
    public Transform movementAxis;
    public float distToGround = 0.1f;
    public float singleStep = 1f;
    public bool shouldSteer = true;
    public string pivotStateName = "Pivot";
    public string locomotionToPivotStateName = "Locomotion -> Pivot";
    private float angle = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        inputWorldCoordinates = new Vector3();
        inputCameraReference = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        inputWorldCoordinates = GetInputInWorldCoordinates(h, v);
        inputCameraReference = GetInputWithCameraAsReferenceCoordinates();
        anim.SetFloat("forward", inputCameraReference.magnitude);
        angle = Vector3.Angle(inputCameraReference, transform.forward);
        anim.SetFloat("direction", angle);
        Debug.DrawRay(transform.position, inputWorldCoordinates * 10, Color.green);
        Debug.DrawRay(transform.position, inputCameraReference * 10, Color.blue);
        Debug.DrawRay(transform.position, movementAxis.forward * 10, Color.yellow);
    }

    void LateUpdate() {        
        Steer();
    }


    void Steer()
    {
        if (!shouldSteer)
        {
            return;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(pivotStateName) || anim.GetAnimatorTransitionInfo(0).IsName(locomotionToPivotStateName))
        {
            return;
        }
        if (inputCameraReference.sqrMagnitude < 0.01f)
        {
            return;
        }
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, inputCameraReference, singleStep * angle / 180, 0.0f);

        Quaternion rotation = Quaternion.LookRotation(newDirection, Vector3.up);
        transform.rotation = rotation;
    }


    public Vector3 GetInputWithCameraAsReferenceCoordinates()
    {
        Vector3 forwardProjected = Vector3.ProjectOnPlane(movementAxis.forward * Input.GetAxis("Vertical"), Vector3.up);
        Vector3 rightProjected = Vector3.ProjectOnPlane(movementAxis.right * Input.GetAxis("Horizontal"), Vector3.up);

        Vector3 inputMovementDirection = forwardProjected + rightProjected;
        return inputMovementDirection;
    }

    public Vector3 GetInputInWorldCoordinates(float horizontal, float vertical)
    {
        Vector3 forwardProjected = Vector3.ProjectOnPlane(Vector3.forward * vertical, Vector3.up);
        Vector3 rightProjected = Vector3.ProjectOnPlane(Vector3.right * horizontal, Vector3.up);

        Vector3 inputMovementDirection = forwardProjected + rightProjected;
        return inputMovementDirection;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround);
    }

    void Jump()
    {
        if (Input.GetKeyUp("space") && IsGrounded())
        {

        }
    }

    float GetDotProductVelocityInputDirection()
    {
        return Vector3.Dot(GetInputWithCameraAsReferenceCoordinates(), transform.forward);
    }

}