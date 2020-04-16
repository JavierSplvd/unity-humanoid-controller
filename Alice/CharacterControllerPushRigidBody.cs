using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerPushRigidBody : MonoBehaviour
{
    // this script pushes all rigidbodies that the character touches
    public float pushPower = 2.0f;
    public float weight = 6.0f;

    private Vector3 applicationForce;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        Vector3 force;

        // no rigidbody
        if (body == null || body.isKinematic) { return; }

        // We use gravity and weight to push things down, we use
        // our velocity and push power to push things other directions
        if (hit.moveDirection.y < -0.3)
        {
            force = new Vector3(0, -0.5f, 0) * Physics.gravity.sqrMagnitude * weight;
        }
        else
        {
            force = hit.controller.velocity * pushPower;
        }

        // Apply the push
        applicationForce.x = hit.point.x;
        applicationForce.z = hit.point.z;
        applicationForce.y = hit.point.y * 0.5f + (transform.position.y + 1.5f) * 0.5f;
        body.AddForceAtPosition(force, applicationForce);
        Debug.Log("hit: " + applicationForce.ToString());

    }
}
