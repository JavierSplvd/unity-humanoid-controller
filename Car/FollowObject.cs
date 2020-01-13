using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{

    public GameObject targetObject;
    private Vector3 targetPosition;
    public float distance = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPosition = targetObject.transform.position - targetObject.transform.forward * 8 + Vector3.up * 4;
        distance = (targetObject.transform.position - transform.position).sqrMagnitude;
        transform.position = Vector3.Lerp(transform.position, targetPosition, distance / (distance + 5));
        Quaternion targetRotation = Quaternion.LookRotation(targetObject.transform.forward, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 200);
    }
}
