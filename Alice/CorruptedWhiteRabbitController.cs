using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedWhiteRabbitInvoker : Alice.Invoker
{
    private List<Command> commands;
    private GameObject gameObject;

    public CorruptedWhiteRabbitInvoker(GameObject gameObject)
    {
        this.gameObject = gameObject;
        commands = new List<Command>();
    }

    public override void AddCommand(Command c)
    {
        commands.Add(c);
    }

    public override void ClearAllCommands()
    {
        commands = System.Array.Empty<Command>().ToList();
    }

    public override void Update()
    {
        foreach(Command c in commands)
        {
            c.Execute(gameObject,1);
        }
    }


}
[RequireComponent(typeof(CharacterController))]
public class CorruptedWhiteRabbitController : MonoBehaviour
{
    private CorruptedWhiteRabbitInvoker invoker;
    private Command swingCommand;
    private Command moveForwardCommand;
    private Command steerCommand;
    private Vector3 bodyTargetPosition = Vector3.zero;

    public GameObject navAgent;
    public float forwardSpeed = 1f;
    public GameObject frontLeftLeg, frontRightLeg, backLeftLeg, backRightLeg, body;
    public float bodyHeight;
    // Start is called before the first frame update
    void Start()
    {
        invoker = new CorruptedWhiteRabbitInvoker(gameObject);
        swingCommand = new VerticalSwingCommand(0.4f, 1f);
        moveForwardCommand = new MoveForwardCommand(forwardSpeed);
        steerCommand = new SteerToTargetCommand(navAgent, forwardSpeed);

        AddMovementBehaviour();
    }

    private void AddMovementBehaviour()
    {
        invoker.ClearAllCommands();
        invoker.AddCommand(swingCommand);
        invoker.AddCommand(moveForwardCommand);
        invoker.AddCommand(steerCommand);
    }

    // Update is called once per frame
    void Update()
    {
        invoker.Update();
        LevelBodyWithLegs();
        if(Vector3.Distance(transform.position, navAgent.transform.position) < 1f)
        {
            invoker.ClearAllCommands();
        }
        else
        {
            AddMovementBehaviour();
        }
    }

    private void LevelBodyWithLegs()
    {
        if(frontLeftLeg != null && frontRightLeg != null && backLeftLeg != null && backRightLeg != null)
        {
            float y = (frontLeftLeg.transform.position.y + frontRightLeg.transform.position.y + backLeftLeg.transform.position.y + backRightLeg.transform.position.y) * 0.25f + bodyHeight;
            bodyTargetPosition.x = body.transform.position.x;
            bodyTargetPosition.z = body.transform.position.z;
            bodyTargetPosition.y = y;
            body.transform.position = Vector3.Lerp(body.transform.position, bodyTargetPosition, 0.5f);

            Vector3 n = Vector3.Cross(frontRightLeg.transform.position - frontLeftLeg.transform.position, backLeftLeg.transform.position - frontRightLeg.transform.position);
            body.transform.rotation = Quaternion.FromToRotation(body.transform.forward, n) * body.transform.rotation;
        }

        
    }
}
