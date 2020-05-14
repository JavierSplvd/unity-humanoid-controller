using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVelocity : MonoBehaviour
{
    [SerializeField]
    private float velocity, threshold;
    private Vector3 pastPos = Vector3.zero;

    public delegate void SwordIsQuick();
    public event SwordIsQuick OnSwordIsQuick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector3.Distance(transform.position, pastPos) / Time.deltaTime;
        pastPos = transform.position;
        if(velocity > threshold)
            OnSwordIsQuick();
    }
}
