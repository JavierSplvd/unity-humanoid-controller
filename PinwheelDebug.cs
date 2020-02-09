using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinwheelDebug : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float radius = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = rigidbody.position;
        float velocity = rigidbody.velocity.magnitude;
        transform.Rotate(velocity * Time.deltaTime, 0, 0, Space.Self);
    }
}
