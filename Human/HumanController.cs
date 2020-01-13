using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class HumanController : MonoBehaviour {
    private Rigidbody _rigidBody;

    public float rigidBodyGroundDrag = 0f;
    public float rigidBodyAirDrag = 0f;
    public float rigidBodyStillDrag = 4f;
    public float walkingForce = 20f;
    public float runningForce = 50f;
    public float jumpForce = 200f;
    public float distToGround = 0.5f;
    public Transform movementAxis;
    public float maxSqrVelocity = 10f;
    public float currentSqrVelocity;
    // Start is called before the first frame update
    void Start () {
        _rigidBody = GetComponent<Rigidbody> ();
        _rigidBody.drag = rigidBodyGroundDrag;
    }

    // Update is called once per frame
    void FixedUpdate () {
        Translation ();
        Jump ();
        Stop ();
        currentSqrVelocity = _rigidBody.velocity.sqrMagnitude;
    }

    void Translation () {
        Vector3 forwardProjected = Vector3.ProjectOnPlane (movementAxis.forward * Input.GetAxis ("Vertical"), Vector3.up);
        Vector3 rightProjected = Vector3.ProjectOnPlane (movementAxis.right * Input.GetAxis ("Horizontal"), Vector3.up);

        Vector3 inputMovementDirection = forwardProjected + rightProjected;
        if(inputMovementDirection.sqrMagnitude > 1)
        {
            inputMovementDirection = inputMovementDirection.normalized;
        }
        float translationForce = walkingForce;
        if (Input.GetKey ("left shift")) {
            translationForce = runningForce;
        }
        if (_rigidBody.velocity.sqrMagnitude > maxSqrVelocity) {
            _rigidBody.AddForceAtPosition (-_rigidBody.velocity.normalized * translationForce, transform.position);
            Debug.DrawRay(transform.position, -_rigidBody.velocity.normalized, Color.green);
        }
        _rigidBody.AddForceAtPosition (inputMovementDirection * translationForce, transform.position);
        Debug.DrawRay(transform.position, inputMovementDirection, Color.green);


    }

    void Stop () {
        if (Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0 && IsGrounded ()) {
            _rigidBody.drag = rigidBodyStillDrag;
        } else if (!IsGrounded ()) {
            _rigidBody.drag = rigidBodyAirDrag;
        } else {
            _rigidBody.drag = rigidBodyGroundDrag;
        }
    }

    public bool IsGrounded () {
        return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Jump () {
        if (Input.GetKeyUp ("space") && IsGrounded ()) {
            _rigidBody.AddForceAtPosition (Vector3.up * jumpForce, transform.position);
        }
    }
}