using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerPushRigidBody : MonoBehaviour
{
    // this script pushes all rigidbodies that the character touches
    public float pushPower = 2.0f;

    private Vector3 applicationForce;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        Vector3 force;

        // no rigidbody
        if (body == null || body.isKinematic || body.position.y < hit.controller.gameObject.transform.position.y) { return; }

        // We use gravity and weight to push things down, we use
        // our velocity and push power to push things other directions
        force = hit.controller.gameObject.transform.forward * pushPower * Time.deltaTime;
        force += body.mass * -Physics.gravity * Time.deltaTime;
        // Apply the push
        applicationForce.x = hit.point.x;
        applicationForce.z = hit.point.z;
        applicationForce.y = body.gameObject.transform.position.y;
        body.AddForceAtPosition(force, applicationForce);

    }
}
