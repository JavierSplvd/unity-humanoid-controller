using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimator : MonoBehaviour
{

    public GameObject target;
    public float targetOffsetToTheGround;
    public float interpolationStep;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdatePosition();
        Steer();
    }

    void UpdatePosition(){
        transform.position = target.transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - targetOffsetToTheGround, transform.position.z);
    }

    void Steer(){
        Vector3 velocityProjected = Vector3.ProjectOnPlane(target.GetComponent<Rigidbody>().velocity, Vector3.up);
        Quaternion targetVelocity = Quaternion.LookRotation(velocityProjected, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetVelocity, interpolationStep);
    }
}
