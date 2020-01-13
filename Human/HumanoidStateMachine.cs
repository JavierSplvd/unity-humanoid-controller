using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HumanController))]
public class HumanoidStateMachine : MonoBehaviour
{
    public enum State {Idle, WalkingForward, RunningForward, Air}

    public State currentState;
    public float minVelocityThreshold;
    private Rigidbody rigidBody;
    private HumanController humanController;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        humanController = GetComponent<HumanController>();
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(rigidBody.velocity.sqrMagnitude < minVelocityThreshold)
        {
            currentState = State.Idle;
        }
        else if (!humanController.IsGrounded())
        {
            currentState = State.Air;
        } else {
            currentState = State.WalkingForward;
        }
    }
}
