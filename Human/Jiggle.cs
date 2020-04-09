using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jiggle : MonoBehaviour
{

    public bool debugMode = true;
 
	// Target and dynamic positions
	public Vector3 targetPos = new Vector3();
	public Vector3 dynamicPos = new Vector3();
 
	// Bone settings
	public Vector3 boneAxis = new Vector3(0,0,1);
	public float targetDistance = 2.0f;
	public float maxDistancePercentage = 0.1f;
 
	// Dynamics settings
	public float bStiffness = 0.1f;
	public float bMass = 0.9f;
	public float bDamping = 0.75f;
	public float bGravity = 0.75f;
 
	// Dynamics variables
	Vector3 force = new Vector3();
	Vector3 acc = new Vector3();
	Vector3 vel = new Vector3();
 
	// Squash and stretch variables
	public bool SquashAndStretch = true;
	public float sideStretch = 0.15f;
	public float frontStretch = 0.2f;

	private Vector3 forwardVector = Vector3.zero;
	private Vector3 upVector = Vector3.zero;
	private Vector3 dynamicPosSanitized = Vector3.zero;
 
	void Awake(){
		// Set targetPos and dynamicPos at startup
		Vector3 targetPos = transform.position + transform.TransformDirection(new Vector3((boneAxis.x * targetDistance),(boneAxis.y * targetDistance),(boneAxis.z * targetDistance)));
		dynamicPos = targetPos;
	}
 
	void LateUpdate(){
		// Reset the bone rotation so we can recalculate the upVector and forwardVector
		transform.rotation = new Quaternion();
 
		// Update forwardVector and upVector
		forwardVector = transform.TransformDirection(new Vector3((boneAxis.x * targetDistance),(boneAxis.y * targetDistance),(boneAxis.z * targetDistance)));
		upVector = transform.TransformDirection(new Vector3(0,1,0));
 
		// Calculate target position
		targetPos = transform.position + transform.TransformDirection(new Vector3((boneAxis.x * targetDistance),(boneAxis.y * targetDistance),(boneAxis.z * targetDistance)));
 
		// Calculate force, acceleration, and velocity per X, Y and Z
		force.x = (targetPos.x - dynamicPos.x) * bStiffness;
		acc.x = force.x / bMass;
		vel.x += acc.x * (1 - bDamping);
 
		force.y = (targetPos.y - dynamicPos.y) * bStiffness;
		force.y -= bGravity / 10; // Add some gravity
		acc.y = force.y / bMass;
		vel.y += acc.y * (1 - bDamping);
 
		force.z = (targetPos.z - dynamicPos.z) * bStiffness;
		acc.z = force.z / bMass;
		vel.z += acc.z * (1 - bDamping);
 
		// Update dynamic postion
		dynamicPos += vel + force;

		dynamicPosSanitized = dynamicPos;
		if(Vector3.Distance(dynamicPos, forwardVector + transform.position) > targetDistance * maxDistancePercentage)
		{
			dynamicPosSanitized = forwardVector + transform.position + (- forwardVector - transform.position + dynamicPos).normalized * targetDistance * maxDistancePercentage;
		}
		// dynamicPosSanitized = new Vector3(forwardVector.x + transform.position.x, 
		// 	dynamicPosSanitized.y, 
		// 	forwardVector.z + transform.position.z
		// );


 
		// Set bone rotation to look at dynamicPos
		// transform.LookAt(dynamicPos, upVector);
        transform.LookAt(dynamicPosSanitized, -Vector3.right);
		if(transform.right.y < 0)
        {
            transform.Rotate(new Vector3(0,0,180));
        }
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90);

        // transform.localRotation = new Quaternion (boneAxis.x, boneAxis.y, boneAxis.z, 0.0f);
		// ==================================================
		// Squash and Stretch section
		// ==================================================
		if(SquashAndStretch){
			// Create a vector from target position to dynamic position
			// We will measure the magnitude of the vector to determine
			// how much squash and stretch we will apply
			Vector3 dynamicVec = dynamicPos - targetPos;
 
			// Get the magnitude of the vector
			float stretchMag = dynamicVec.magnitude;
 
			// Here we determine the amount of squash and stretch based on stretchMag
			// and the direction the Bone Axis is pointed in. Ideally there should be
			// a vector with two values at 0 and one at 1. Like Vector3(0,0,1)
			// for the 0 values, we assume those are the sides, and 1 is the direction
			// the bone is facing
			float xStretch;
			if(boneAxis.x == 0) xStretch = 1 + (-stretchMag * sideStretch);
			else xStretch = 1 + (stretchMag * frontStretch);
 
			float yStretch;
			if(boneAxis.y == 0) yStretch = 1 + (-stretchMag * sideStretch);
			else yStretch = 1 + (stretchMag * frontStretch);
 
			float zStretch;
			if(boneAxis.z == 0) zStretch = 1 + (-stretchMag * sideStretch);
			else zStretch = 1 + (stretchMag * frontStretch);
 
			// Set the bone scale
			transform.localScale = new Vector3(xStretch, yStretch, zStretch);
		}
 
		// ==================================================
		// DEBUG VISUALIZATION
		// ==================================================
		// Green line is the bone's local up vector
		// Blue line is the bone's local foward vector
		// Yellow line is the target postion
		// Red line is the dynamic postion
		if(debugMode){
			Debug.DrawRay(transform.position, forwardVector, Color.blue);
			Debug.DrawRay(transform.position, upVector, Color.green);
			Debug.DrawRay(targetPos, Vector3.up * 0.2f, Color.yellow);
			Debug.DrawRay(dynamicPos, Vector3.up * 0.2f, Color.red);
			Debug.DrawRay(dynamicPosSanitized, Vector3.up * 0.2f, Color.magenta);
		}
		// ==================================================
	}
}
