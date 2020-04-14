using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedWhiteRabbitInvoker : Invoker
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

    public GameObject navAgent;
    public float forwardSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        invoker = new CorruptedWhiteRabbitInvoker(gameObject);
        swingCommand = new VerticalSwingCommand(0.4f, 1f);
        moveForwardCommand = new MoveForwardCommand(forwardSpeed);
        steerCommand = new SteerToTargetCommand(navAgent, forwardSpeed);

        invoker.AddCommand(swingCommand);
        invoker.AddCommand(moveForwardCommand);
        invoker.AddCommand(steerCommand);
    }

    // Update is called once per frame
    void Update()
    {
        invoker.Update();
    }
}
