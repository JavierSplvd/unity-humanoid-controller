using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundItself : MonoBehaviour
{

    public enum Axis { WorldUp, ObjectUp, ObjectForward}
    public float speed;
    public Axis axis;
    public bool addRandomPhase;
    private Vector3 axisVector = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        AsignAxisVector();

        if(addRandomPhase)
        {
            transform.RotateAround(transform.position, axisVector, Random.Range(0,90));
        }
    }
    // Update is called once per frame
    void Update()
    {
        AsignAxisVector();
        transform.RotateAround(transform.position, axisVector, Time.deltaTime * speed);

        Debug.DrawRay(transform.position, transform.forward, Color.green, 0.1f);
    }

    private void AsignAxisVector()
    {
        if(axis.Equals(Axis.WorldUp))
        {
            axisVector = Vector3.up;
        }
        else if (axis.Equals(Axis.ObjectUp))
        {
            axisVector = transform.up;
        }
        else if (axis.Equals(Axis.ObjectForward))
        {
            axisVector = transform.forward;
        }
    }
}
