using System.Collections;
using System.Linq;
using UnityEngine;

public class AliceEmitSound : MonoBehaviour
{
    public float runningAverage = 0f;
    public float speedToMakeNoise;
    public GameObject soundTracker;
    private Queue velocities = new Queue();
    private Vector3 lastPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i<60; i++)
        {
            velocities.Enqueue(0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(velocities.Count > 60)
        {
            velocities.Dequeue();
        }
        velocities.Enqueue((transform.position - lastPosition).sqrMagnitude / Time.deltaTime);

        runningAverage = velocities.ToArray().DefaultIfEmpty(0).Average(x => (float) x);

        lastPosition = transform.position;

        if(runningAverage > speedToMakeNoise)
        {
            soundTracker.transform.position = transform.position;
        }
    }
}
