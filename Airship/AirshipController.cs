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
    public float swingAmplitude;
    public float swingAngularVel;
    // Commands
    private Command steerCommand;
    private Command moveForwardCommand;
    private Command verticalSwingCommand;
    // Start is called before the first frame update
    void Start()
    {
        if(navMeshAgent != null)
        {
            airshipInvoker = GetComponent<AirshipInvoker>();
            // Commands
            steerCommand = new SteerToTargetCommand(navMeshAgent, rotationSpeed);
            moveForwardCommand = new MoveForwardSimpleCommand(forwardSpeed);
            verticalSwingCommand = new VerticalSwingCommand(swingAmplitude, swingAngularVel);
            // airshipInvoker.Add(steerCommand);
            // airshipInvoker.Add(moveForwardCommand);
            airshipInvoker.Add(verticalSwingCommand);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is null");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, navMeshAgent.transform.position) < 2)
        {
            airshipInvoker.Remove(steerCommand);
            airshipInvoker.Remove(moveForwardCommand);
        }
    }
}
