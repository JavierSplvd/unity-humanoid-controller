using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina
{
    private float current;
    private float min = 0f;
    private float max = 100f;
    private float recSpeed;
    private float spendSpeed;

    public Stamina(float recSpeed, float spendSpeed)
    {
        current = max;
        this.recSpeed = recSpeed;
        this.spendSpeed = spendSpeed;
    }

    public void Recover(float deltaTime)
    {
        current += deltaTime * recSpeed;
        if(current > max)
        {
            current = max;
        }
    }

    public void Spend(float deltaTime)
    {
        current -= deltaTime * spendSpeed;
        if(current < min)
        {
            current = min;
        }
    }

    public float GetCurrent()
    {
        return current;
    }
    public float GetCurrentPercentage()
    {
        return current/max;
    }
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class BaseHumanController : MonoBehaviour
{
    private Animator anim;
    private CharacterController characterController;
    private Vector3 inputWorldCoordinates;
    private Vector3 inputCameraReferenceSystem;
    private Vector3 lastInputCameraReferenceSystem;
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
    public string jumpStateName = "Jump";
    public string climbingStateName = "Climbing";
    public string climbingFinsihStateName = "Climbing Finish";
    private float angle = 0f;
    public float initialJumpSpeed = 10f;
    public float gravity = 10f;
    public float currentVerticalSpeed = 0f;
    private Vector3 airborneMovement;
    [Range(0, 10)]
    public float airborneInitialHorizontalSpeed;
    [Range(0, 10)]
    public float airborneHorizontalDrag;
    [Tooltip("Capa de los objetos donde se puede ajustar el pie")]
    public LayerMask rayMask;
    public LayerMask climbingRayMask;

    private bool isClimbing = false;
    private float maxCapsuleHeight;
    private float currentCapsuleHeight;
    private Vector3 steerNewDirection = Vector3.zero;
    private Spring crouchMultiplier = new Spring(100, 1, 0);
    private Spring runMultiplier = new Spring(100, 1, 1);
    private RaycastHit hit;
    private float h,v;
    private HandsIKEffect handsIKEffect;
    private Stamina stamina;

    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        inputWorldCoordinates = new Vector3();
        inputCameraReferenceSystem = new Vector3();
        lastInputCameraReferenceSystem = new Vector3();
        airborneMovement = new Vector3(0, 0, 0);
        maxCapsuleHeight = characterController.height;
        currentCapsuleHeight = maxCapsuleHeight;
        handsIKEffect = GetComponent<HandsIKEffect>();
        stamina = new Stamina(20,30);
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        inputWorldCoordinates = GetInputInWorldCoordinates(h, v);
        inputCameraReferenceSystem = CalculateInputWithCameraAsReferenceSystem();
        inputCameraReferenceSystem = 0.4f * inputCameraReferenceSystem + 0.6f * lastInputCameraReferenceSystem;
        Run();
        anim.SetFloat("forward", inputCameraReferenceSystem.magnitude * runMultiplier.GetX());
        anim.SetFloat("forward absolute", v);
        anim.SetFloat("crouching", crouchMultiplier.GetX());
        angle = Vector3.Angle(inputCameraReferenceSystem, transform.forward);
        anim.SetFloat("direction", angle);
        Debug.DrawRay(transform.position, inputWorldCoordinates * 10, Color.green);
        Debug.DrawRay(transform.position, inputCameraReferenceSystem * 10, Color.blue);
        Debug.DrawRay(transform.position, movementAxis.forward * 10, Color.yellow);

        TriggerJump();
        ReduceCapsuleHeightWhileJumping();
        ChangeForwardMultiplier();
        OnClimbingEnter();
        OnClimbingStay();
        OnClimbingExit();
        lastInputCameraReferenceSystem = new Vector3(inputCameraReferenceSystem.x, inputCameraReferenceSystem.y, inputCameraReferenceSystem.z);
    }

    void LateUpdate()
    {
        CalculateVerticalMovement();

        Steer();
    }

    void Run()
    {
        if (Input.GetButton("Button B") && stamina.GetCurrent() > 0)
        {
            Debug.Log("Run");
            runMultiplier.SetX0(2f);
            stamina.Spend(Time.deltaTime);
        }
        else
        {
            runMultiplier.SetX0(1f);
            stamina.Recover(Time.deltaTime);
        }
        runMultiplier.FixedUpdate(Time.deltaTime);
    }

    void ChangeForwardMultiplier()
    {
        if (Input.GetButtonUp("Button A"))
        {
            switch (crouchMultiplier.GetX0())
            {
                case 0f:
                    crouchMultiplier.SetX0(0.9f);
                    break;
                case 0.9f:
                    crouchMultiplier.SetX0(0f);
                    break;
            }
        }
        crouchMultiplier.FixedUpdate(Time.deltaTime);

    }

    void Steer()
    {

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Idle") || state.IsName("Locomotion"))
        {
            shouldSteer = true;
        }
        if(state.IsName(climbingStateName))
        {
            shouldSteer = false;
        }
        if (!shouldSteer)
        {
            return;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(pivotStateName) || anim.GetAnimatorTransitionInfo(0).IsName(locomotionToPivotStateName))
        {
            return;
        }
        if (inputCameraReferenceSystem.sqrMagnitude < 0.01f)
        {
            return;
        }
        steerNewDirection = Vector3.RotateTowards(transform.forward, inputCameraReferenceSystem, singleStep * angle / 180, 0.0f);
        transform.rotation = Quaternion.LookRotation(steerNewDirection, Vector3.up);
    }

    void Falling()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(fallingStateName))
        {
            transform.Translate(inputCameraReferenceSystem.x * 0.1f, 0, inputCameraReferenceSystem.z * 0.1f);
            anim.applyRootMotion = false;
        }
        else
        {
            anim.applyRootMotion = true;
        }
    }

    void TriggerJump()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            Debug.Log("jump!");
            anim.SetTrigger("jump");
        }
        else
        {
            anim.ResetTrigger("jump");
        }
    }

    void ReduceCapsuleHeightWhileJumping()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(jumpStateName) && state.normalizedTime < 0.1f)
        {
            currentVerticalSpeed = initialJumpSpeed;
            characterController.height = maxCapsuleHeight * 0.8f;
        }
        else
        {
            characterController.height = Mathf.Clamp(characterController.height + 0.2f, 0, maxCapsuleHeight);
        }
    }


    public Vector3 CalculateInputWithCameraAsReferenceSystem()
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
        Vector3 bottomPositionWithOffset = new Vector3(transform.position.x, transform.position.y + characterController.center.y - characterController.height / 2 + distToGround, transform.position.z);

        bool centerRayHit = Physics.Raycast(bottomPositionWithOffset, -Vector3.up, distToGround, rayMask);
        bool frontRayHit = Physics.Raycast(bottomPositionWithOffset + transform.forward * characterController.radius, -Vector3.up, distToGround * 1.01f, rayMask);
        bool backRayHit = Physics.Raycast(bottomPositionWithOffset + transform.forward * -characterController.radius, -Vector3.up, distToGround * 1.01f, rayMask);
        bool rightRayHit = Physics.Raycast(bottomPositionWithOffset + transform.right * characterController.radius, -Vector3.up, distToGround * 1.01f, rayMask);
        bool leftRayHit = Physics.Raycast(bottomPositionWithOffset + transform.right * -characterController.radius, -Vector3.up, distToGround * 1.01f, rayMask);


        bool isGrounded = centerRayHit || frontRayHit || backRayHit || rightRayHit || leftRayHit;
        anim.SetBool("grounded", isGrounded);
        Debug.DrawRay(bottomPositionWithOffset, -Vector3.up * distToGround, Color.green);
        Debug.DrawRay(bottomPositionWithOffset + transform.forward * characterController.radius, -Vector3.up * 1.01f * distToGround, Color.green);
        Debug.DrawRay(bottomPositionWithOffset + transform.forward * -characterController.radius, -Vector3.up * 1.01f * distToGround, Color.green);
        Debug.DrawRay(bottomPositionWithOffset + transform.right * characterController.radius, -Vector3.up * 1.01f * distToGround, Color.green);
        Debug.DrawRay(bottomPositionWithOffset + transform.right * -characterController.radius, -Vector3.up * 1.01f * distToGround, Color.green);
        Debug.DrawRay(bottomPositionWithOffset, -Vector3.up * 1.01f * distToGround, Color.green);
        return isGrounded;
    }

    void CalculateVerticalMovement()
    {
        if (IsGrounded())
        {
            currentVerticalSpeed = -gravity * Time.deltaTime;

            // if (Input.GetButtonUp("Jump"))
            // {
            //     currentVerticalSpeed = initialJumpSpeed;
            //     airborneCurrentHorizontalSpeed = airborneInitialHorizontalSpeed * inputCameraReferenceSystem.sqrMagnitude;
            // }
        }
        else if (isClimbing || anim.GetCurrentAnimatorStateInfo(0).IsName(climbingFinsihStateName))
        {
            currentVerticalSpeed = 0;
        }
        else
        {
            currentVerticalSpeed -= gravity * Time.deltaTime;
            // If a jump is approximately peaking, make it absolute.
            if (Mathf.Approximately(currentVerticalSpeed, 0f))
            {
                currentVerticalSpeed = 0f;
            }
            // airborneCurrentHorizontalSpeed -= airborneHorizontalDrag * Time.deltaTime;
            // if (airborneCurrentHorizontalSpeed < 0f)
            // {
            //     airborneCurrentHorizontalSpeed = 0;
            // }
        }
        anim.SetFloat("jump speed", currentVerticalSpeed);
        // airborneMovement = currentVerticalSpeed * Vector3.up * Time.deltaTime + inputCameraReferenceSystem * Time.deltaTime * airborneCurrentHorizontalSpeed;
        airborneMovement = currentVerticalSpeed * Vector3.up * Time.deltaTime;
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
        return Vector3.Dot(CalculateInputWithCameraAsReferenceSystem(), transform.forward);
    }

    public Vector3 GetInputInCameraCoordinates()
    {
        return inputCameraReferenceSystem;
    }

    private void OnClimbingEnter() 
    {
        bool topRay = Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, maxDistance:0.4f, layerMask:climbingRayMask);
        bool bottomRay = Physics.Raycast(transform.position, transform.forward, out hit, maxDistance:0.4f, layerMask:climbingRayMask);

        if((topRay || bottomRay) && hit.collider.tag.Equals("Climbing"))
        {
            Debug.Log("enter climbing");
            isClimbing = true;
            anim.SetBool("climbing", true);
        }    
    }

    private void OnClimbingStay()
    {
        if(isClimbing)
        {
            bool ray = Physics.Raycast(transform.position, transform.forward, out hit, maxDistance:0.4f, layerMask:climbingRayMask);
            transform.forward = Vector3.Lerp(transform.forward, - hit.normal, 0.2f);
            handsIKEffect.IkActive = false;
        }
        else
        {
            handsIKEffect.IkActive = true;
        }
    }

    private void OnClimbingExit() 
    {
        bool topRay = Physics.Raycast(transform.position + 1.5f * Vector3.up, transform.forward, out hit, maxDistance:0.6f, layerMask:climbingRayMask);
        bool bottomRay = Physics.Raycast(transform.position + 0.1f * Vector3.up, transform.forward, out hit, maxDistance:0.6f, layerMask:climbingRayMask);

        if(!topRay && bottomRay)
        {
            Debug.Log("exit!?");
            anim.SetTrigger("finish climbing");
        }
        else if(!topRay && !bottomRay)
        {
            isClimbing = false;
            anim.SetBool("climbing", false);
        }
        else if(topRay && !bottomRay && v < 0)
        {
            isClimbing = false;
            anim.SetBool("climbing", false);
        }
    }

    public float GetPercentageCurrentStamina()
    {
        return stamina.GetCurrentPercentage();
    }


}