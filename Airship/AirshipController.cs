using System;
using UnityEngine;

[RequireComponent(typeof(AirshipInvoker))]
public class AirshipController : MonoBehaviour
{
    private AirshipInvoker airshipInvoker;
    private readonly RouteStateMachine routeStateMachine = new RouteStateMachine();

    // Route objects
    public Transform dockTransform;
    public Transform pivotTransform;
    public Transform destinationTransform;

    public String state;

    public float timeToGetMaxSpeed = 2f;

    // Commands params
    public float rotationSpeed;
    public float forwardSpeed;
    public float swingAmplitude;
    public float swingAngularVel;
    // Commands
    private Command steerCommand;
    private Command moveForwardCommand;
    private Command verticalSwingCommand;
    private Command goVerticalToPivot;
    private Command goToDockCommand;
    private Command alignWithDock;
    private Command steerToPivotCommand;
    private Command steerToDestinationCommand;
    void Start()
    {
        airshipInvoker = GetComponent<AirshipInvoker>();
        // Dock commands
        goToDockCommand = new MoveTowardsSimpleCommand(dockTransform.gameObject,forwardSpeed);
        alignWithDock = new AlignCommand(rotationSpeed, dockTransform);
        goVerticalToPivot = new MoveVerticalDirectionCommand(forwardSpeed, pivotTransform);
        steerToPivotCommand = new SteerToTargetCommand(pivotTransform.gameObject, rotationSpeed);
        steerToDestinationCommand = new SteerToTargetCommand(destinationTransform.gameObject, rotationSpeed);
        moveForwardCommand = new MoveForwardSimpleCommand(forwardSpeed);
        verticalSwingCommand = new VerticalSwingCommand(swingAmplitude, swingAngularVel);
        // InvokeRepeating("UpdateController", 1f, 1f);
    }

    void Update()
    {
        state = routeStateMachine.GetState().ToString();

        airshipInvoker.SetScaleMultiplier(Mathf.Clamp(routeStateMachine.GetDeltaTime() / timeToGetMaxSpeed, 0, 1));
        airshipInvoker.ClearAllCommands();
        switch(routeStateMachine.GetState())
        {
            case RouteStateMachine.State.GoToDock:
                airshipInvoker.Add(goToDockCommand);
                airshipInvoker.Add(alignWithDock);
                break;
            case RouteStateMachine.State.WaitInDock:
                airshipInvoker.Add(verticalSwingCommand);
                break;
            case RouteStateMachine.State.PivotGoing:
                airshipInvoker.Add(goVerticalToPivot);
                break;
            case RouteStateMachine.State.GoToDestination:
                airshipInvoker.Add(moveForwardCommand);
                airshipInvoker.Add(steerToDestinationCommand);
                break;
            case RouteStateMachine.State.WaitInDestination:
                break;
            case RouteStateMachine.State.PivotReturning:
                airshipInvoker.Add(moveForwardCommand);
                airshipInvoker.Add(steerToPivotCommand);
                break;
        }
        routeStateMachine.UpdateState(transform.position, dockTransform.position, pivotTransform.position, destinationTransform.position);
    }

    class RouteStateMachine
    {
        public enum State
        {
            GoToDock,
            WaitInDock,
            PivotGoing,
            GoToDestination,
            WaitInDestination,
            PivotReturning
        }
        
        private State currentState;
        private State[] states;
        private float deltaTime = 0;

        public RouteStateMachine()
        {
            currentState = State.GoToDock;
            states = new State[6];
            states[0] = State.GoToDock;
            states[1] = State.WaitInDock;
            states[2] = State.PivotGoing;
            states[3] = State.GoToDestination;
            states[4] = State.WaitInDestination;
            states[5] = State.PivotReturning;
        }

        private void AdvanceState()
        {
            ResetDeltaTime();
            int i = Array.IndexOf(states, currentState);
            if(i + 1 == states.Length)
            {
                currentState = states[0];
            }
            else
            {
                currentState = states[i + 1];
            }
        }

        public State GetState()
        {
            return currentState;
        }

        private void ResetDeltaTime()
        {
            deltaTime = 0;
        }

        private void UpdateDeltaTime()
        {
            deltaTime += Time.deltaTime;
        }

        public float GetDeltaTime()
        {
            return deltaTime;
        }
        public void UpdateState(Vector3 position, Vector3 dockPosition, Vector3 pivotPosition, Vector3 destinationPosition)
        {
            UpdateDeltaTime();
            switch(currentState)
            {
                case State.GoToDock:
                    if(Vector3.Distance(position, dockPosition)< 1f)
                    {
                        AdvanceState();
                    }
                    break;
                case State.WaitInDock:
                    if(deltaTime > 30f)
                    {
                        AdvanceState();
                    }
                    break;
                case State.PivotGoing:
                    if(Vector3.Distance(position, pivotPosition)< 1f)
                    {
                        AdvanceState();
                    }
                    break;
                case State.GoToDestination:
                    if(Vector3.Distance(position, destinationPosition)< 1f)
                    {
                        AdvanceState();
                    }
                    break;
                case State.WaitInDestination:
                    if(deltaTime > 30f)
                    {
                        AdvanceState();
                    }
                    break;
                case State.PivotReturning:
                    if(Vector3.Distance(position, pivotPosition)< 1f)
                    {
                        AdvanceState();
                    }
                    break;
            }
        }

    }
}
