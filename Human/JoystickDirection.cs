using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickDirection : MonoBehaviour
{
    public BaseHumanController baseHumanController;
    private Vector3 joystickDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = baseHumanController.GetInputInCameraCoordinates();
        if(direction.sqrMagnitude > 0.04) {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
        }
        
    }
}
