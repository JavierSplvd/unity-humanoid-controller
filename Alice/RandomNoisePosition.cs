using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoisePosition : MonoBehaviour
{
    public float mult, amplitudeX, amplitudeY, amplitudeZ;

    private float x, y, z;
    private Vector3 initialPos, modPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        x = Mathf.Sin(Time.time * mult) * amplitudeX + Random.Range(-1, 1) * amplitudeX;
        y = Mathf.Cos(Time.time * mult) * amplitudeY + Random.Range(-1, 1) * amplitudeY;
        z = Mathf.Sin(Time.time * mult) * amplitudeZ + Random.Range(-1, 1) * amplitudeZ;
        modPos.x = initialPos.x + x;
        modPos.y = initialPos.y + y;
        modPos.z = initialPos.z + z;
        transform.position = Vector3.Lerp(transform.position, modPos, 0.01f);

    }
}
