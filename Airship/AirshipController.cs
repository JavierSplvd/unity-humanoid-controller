using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirshipInvoker))]
public class AirshipController : MonoBehaviour
{
    private AirshipInvoker airshipInvoker;
    public GameObject navMeshAgent;

    public float rotationSpeed;
    public float forwardSpeed;
    // Commands
    private Command steerCommand;
    private Command moveForwardCommand;
    // Start is called before the first frame update
    void Start()
    {
        if(navMeshAgent != null)
        {
            airshipInvoker = GetComponent<AirshipInvoker>();
            // Commands
            steerCommand = new SteerToTargetCommand(navMeshAgent, rotationSpeed);
            moveForwardCommand = new MoveForwardSimpleCommand(forwardSpeed);
            airshipInvoker.Add(steerCommand);
            airshipInvoker.Add(moveForwardCommand);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is null");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
