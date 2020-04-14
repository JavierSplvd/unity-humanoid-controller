using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderIKKeepOffset : MonoBehaviour
{
    public GameObject objectToKeepOffset;
    public float percentageDistance = 0.5f;

    private float maxDistanceToTriggerMovement;
    private Vector3 possiblePosition = Vector3.zero;
    private Vector3 initialOffset;
    // Start is called before the first frame update
    void Start()
    {
        initialOffset = - objectToKeepOffset.transform.position + gameObject.transform.position;
        maxDistanceToTriggerMovement = initialOffset.magnitude * percentageDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(objectToKeepOffset.transform.position + objectToKeepOffset.transform.rotation * initialOffset, transform.position) > maxDistanceToTriggerMovement)
        {
            transform.position = objectToKeepOffset.transform.position + objectToKeepOffset.transform.rotation * initialOffset;
        }
        Debug.DrawRay(objectToKeepOffset.transform.position, objectToKeepOffset.transform.rotation * initialOffset, Color.green, 0.1f);
    }
}
