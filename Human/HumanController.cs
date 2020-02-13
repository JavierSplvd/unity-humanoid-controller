using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class HumanController : MonoBehaviour
{
    private Animator anim;
    private CharacterController characterController;
    private Vector3 inputWorldCoordinates;
    private Vector3 inputCameraReference;
    public Transform movementAxis;
    public float distToGround = 0.1f;
    public float singleStep = 1f;
    public bool shouldSteer = true;
    public string pivotStateName = "Pivot";
    public string locomotionToPivotStateName = "Locomotion -> Pivot";
    public string locomotionJumpToLocomotionName = "Locomotion jump -> Locomotion";
    public string fallingToIdleName = "Falling -> Idle";
    public string idleJumpStateName = "Idle jump";
    public string locomotionJumpStateName = "Locomotion jump";
    public string fallingStateName = "Falling";
    private float angle = 0f;
    public bool debugGrounded = false;
    public AnimationCurve jumpCurve;
    public float initialJumpSpeed = 10f;
    public float gravity = 10f;
    public float m_VerticalSpeed = 0f;
    private Vector3 airborneMovement;
    private float airborneCurrentHorizontalSpeed = 0f;
    [Range(0, 10)]
    public float airborneInitialHorizontalSpeed;
    [Range(0, 10)]
    public float airborneHorizontalDrag;



    [Tooltip("Capa de los objetos donde se puede ajustar el pie")]
    public LayerMask rayMask;

    private int randomIdleState = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        inputWorldCoordinates = new Vector3();
        inputCameraReference = new Vector3();
        airborneMovement = new Vector3(0, 0, 0);
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

        randomIdleState = Random.Range(0, 10);
        anim.SetInteger("random idle", randomIdleState);

        // Jump();
        // Falling();
    }

    void LateUpdate()
    {
        CalculateVerticalMovement();

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

    void Falling()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(fallingStateName))
        {
            transform.Translate(inputCameraReference.x * 0.1f, 0, inputCameraReference.z * 0.1f);
            anim.applyRootMotion = false;
        }
        else
        {
            anim.applyRootMotion = true;

        }
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
        Vector3 bottomPositionWithOffset = new Vector3(transform.position.x, transform.position.y + distToGround, transform.position.z);

        bool isGrounded = Physics.Raycast(bottomPositionWithOffset, -Vector3.up, distToGround * 2f, rayMask);
        anim.SetBool("grounded", isGrounded);
        Debug.DrawRay(bottomPositionWithOffset, -Vector3.up * 2f * distToGround, Color.green);
        return isGrounded;
    }

    void CalculateVerticalMovement()
    {
        if (IsGrounded())
        {
            m_VerticalSpeed = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                m_VerticalSpeed = initialJumpSpeed;
                airborneCurrentHorizontalSpeed = airborneInitialHorizontalSpeed * inputCameraReference.sqrMagnitude;
            }
        }
        else
        {
            if (!Input.GetButtonDown("Jump") && m_VerticalSpeed > 0.0f)
            {
                // This is what causes holding jump to jump higher that tapping jump.
                m_VerticalSpeed -= gravity * Time.deltaTime;
            }

            // If a jump is approximately peaking, make it absolute.
            if (Mathf.Approximately(m_VerticalSpeed, 0f))
            {
                m_VerticalSpeed = 0f;
            }
            m_VerticalSpeed -= gravity * Time.deltaTime;
            airborneCurrentHorizontalSpeed -= airborneHorizontalDrag * Time.deltaTime;
            if(airborneCurrentHorizontalSpeed < 0f)
            {
                airborneCurrentHorizontalSpeed = 0;
            }
        }
        anim.SetFloat("jump speed", m_VerticalSpeed);
        airborneMovement = m_VerticalSpeed * Vector3.up * Time.deltaTime + inputCameraReference * Time.deltaTime * airborneCurrentHorizontalSpeed;
        characterController.Move(airborneMovement);
    }

    float getProgressOfTheJumpAnimation()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(idleJumpStateName) || state.IsName(locomotionJumpStateName))
        {
            return state.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    void ResetJumpParam()
    {
        if (anim.GetAnimatorTransitionInfo(0).IsName(locomotionJumpToLocomotionName) || anim.GetAnimatorTransitionInfo(0).IsName(fallingToIdleName))
        {
            anim.SetBool("jump", false);
        }
    }

    float GetDotProductVelocityInputDirection()
    {
        return Vector3.Dot(GetInputWithCameraAsReferenceCoordinates(), transform.forward);
    }

}