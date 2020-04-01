using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DustTrail : MonoBehaviour
{
    private Transform target;
    private float previousVelocity;
    private float currentVelocity;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private ParticleSystem _particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        target = transform;
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = target.position;
        currentVelocity = (currentPosition - previousPosition).magnitude;
        if(previousVelocity < 0.1 && currentVelocity > 0.1)
        {
            Debug.Log("!");
            _particleSystem.Stop();
            _particleSystem.Play();
        }
        else if (currentVelocity > 0.1f)
        {
            // nothing
        }
        else
        {
            _particleSystem.Stop();
        }

        previousPosition = currentPosition;
        previousVelocity = currentVelocity;
    }
}
