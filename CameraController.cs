using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraOrbit;
    public Transform target;
    public float smoothTime = 1f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cameraOrbit.position = target.position;
    }

    void Update()
    {
        //cameraOrbit.position = Vector3.SmoothDamp(target.position, cameraOrbit.position, ref velocity, smoothTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);

        transform.LookAt(target.position);
    }
}