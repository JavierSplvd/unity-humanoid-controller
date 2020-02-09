using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimator : MonoBehaviour
{

    public GameObject target;
    private HumanController hc;

    public float targetOffsetToTheGround = 0.5f;
    public float interpolationStep = 1f;
    public float minVelocityToSteer = 0.05f;
    public float minVelocityThresholdToConsiderWalking = 0.3f;
    public float minSqrVelocityToConsiderRunning = 15f;
    private Animator model;

    public float angleDiffCoefficient = 0f;

    public string walkingParam = "isWalking";
    public string idleParam = "isIdle";
    public string runningParam = "isRunning";
    public string jumpingParam = "isJumping";
    public string fallingParam = "isFalling";
    public string directionParam = "direction";
    public string sqrVelocityparam = "sqrVelocity";
    public string idlePivotingLeftParam = "idlePivotingLeft";

    // Start is called before the first frame update
    void Start()
    {
        model = GetComponent<Animator>();
        hc = target.GetComponent<HumanController>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        ClearAnimatorParams();

        UpdatePosition();
        Steer();
        Animate();
        // GetAngleDiff();
    }

    void UpdatePosition(){
        transform.position = target.transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - targetOffsetToTheGround, transform.position.z);
    }

    void Steer(){
        Vector3 velocityProjected = Vector3.ProjectOnPlane(target.GetComponent<Rigidbody>().velocity, Vector3.up);
        if(velocityProjected.sqrMagnitude > minVelocityToSteer)
        {
            Quaternion targetVelocity = Quaternion.LookRotation(velocityProjected, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetVelocity, interpolationStep);

        }
        
    }

    void Animate(){
        float vel = this.hc.GetHorizontalSqrVelocity();

        bool isJumping = this.hc.IsJumpingAndGoingUpwards();
        if(isJumping){
            model.SetBool(jumpingParam, true);
            return;
        }
        bool isFalling = this.hc.IsFalling();
        if(isFalling) {
            model.SetBool(fallingParam, true);
            return;
        }


        if(vel < minVelocityThresholdToConsiderWalking)
        {
            model.SetBool(idleParam, true);
        } 
        else if (vel > minSqrVelocityToConsiderRunning)
        {
            model.SetBool(runningParam, true);
        } 
        else {
            model.SetBool(walkingParam, true);
        }
    }

    void AnimatePivoting(){
        model.SetBool(idlePivotingLeftParam, true);
    }

    void ClearAnimatorParams(){
        model.SetBool(idleParam, false);
        model.SetBool(walkingParam, false);
        model.SetBool(runningParam, false);
        model.SetBool(idlePivotingLeftParam, false);
        model.SetBool(jumpingParam, false);
        model.SetBool(fallingParam, false);
    }

    void GetAngleDiff() {
        bool inputDirectionIsNotZero = this.hc.GetInputDirection().sqrMagnitude > 0.09f;
        if(inputDirectionIsNotZero) { 
            // A dot product of 1 means they are parallel.
            Debug.Log(Vector3.Dot(this.hc.GetInputDirection(), transform.forward));
            angleDiffCoefficient = 1 - Vector3.Dot(this.hc.GetInputDirection(), transform.forward);
            model.SetFloat(directionParam, angleDiffCoefficient);
        }
        float targetVelocity = this.hc.GetHorizontalSqrVelocity();
        if(targetVelocity > minVelocityThresholdToConsiderWalking) {
            model.SetFloat(directionParam, 0);
        }
        model.SetFloat(sqrVelocityparam, targetVelocity);

        Debug.DrawRay(transform.position, this.hc.GetInputDirection(), Color.blue);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }
}
