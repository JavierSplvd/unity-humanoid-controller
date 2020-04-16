using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderIKKeepOffset : MonoBehaviour
{
    public GameObject objectToKeepOffset;
    public float percentageDistance = 0.5f;
    public LayerMask rayMask;
    public float rayDist = 3f;

    private float maxDistanceToTriggerMovement;
    private Vector3 possiblePosition, oldPosition = Vector3.zero;
    private Vector3 initialOffset;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        initialOffset = - objectToKeepOffset.transform.position + gameObject.transform.position;
        maxDistanceToTriggerMovement = initialOffset.magnitude * percentageDistance;
        oldPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float m = Vector3.ProjectOnPlane(objectToKeepOffset.transform.position + objectToKeepOffset.transform.rotation * initialOffset - possiblePosition, Vector3.up).magnitude;
        if(m > maxDistanceToTriggerMovement)
        {

            possiblePosition = objectToKeepOffset.transform.position + objectToKeepOffset.transform.rotation * initialOffset;
            bool downwardCollision = Physics.Raycast(possiblePosition + Vector3.up, -Vector3.up, out hit, rayDist, rayMask);
            if(downwardCollision)
            {
                possiblePosition = hit.point;
            }

        }

        transform.position = Vector3.Lerp(transform.position, 
            possiblePosition, 
            0.5f
        );

        if(Vector3.Distance(transform.position, possiblePosition) < 0.001f)
        {
            oldPosition = transform.position;
        }
        Debug.DrawRay(objectToKeepOffset.transform.position, objectToKeepOffset.transform.rotation * initialOffset, Color.green, 0.1f);
        Debug.DrawRay(oldPosition, Vector3.up, Color.blue, 0.1f);
        Debug.DrawRay(possiblePosition, Vector3.up, Color.cyan, 0.1f);
        Debug.DrawRay(transform.position, Vector3.up, Color.yellow, 0.1f);
    }
}
