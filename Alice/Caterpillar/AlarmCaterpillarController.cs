using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlarmCaterpillarInvoker : Invoker
{
    private List<Command> commands;
    private GameObject gameObject;

    public AlarmCaterpillarInvoker(GameObject gameObject)
    {
        this.gameObject = gameObject;
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

public class HitListener : Listener
{
    private AlarmCaterpillarController controller;

    public HitListener(AlarmCaterpillarController controller)
    {
        this.controller = controller;
    }
    public override void Notify()
    {
        controller.NotifyHit();
    }
}

public class AlarmCaterpillarController : MonoBehaviour
{
    public LayerMask rayMask;
    public float scanDistance;
    public float currentAlarmLevel;
    public float maxAlarmLevel;
    public float speedAlarmLevel;
    public GameObject navAgent;
    private AlarmCaterpillarInvoker invoker;
    private Command scanCommand;
    private Command moveForwardCommand;
    private Command steerCommand;
    private Listener hitListener;

    void Start()
    {
        hitListener = new HitListener(this);
        scanCommand = new ScanCommand(rayMask, scanDistance, hitListener);

        moveForwardCommand = new MoveForwardCommand(1f);
        steerCommand = new SteerToTargetCommand(navAgent, 1f);

        invoker = new AlarmCaterpillarInvoker(gameObject);
    }

    void Update()
    {
        invoker.ClearAllCommands();
        if(currentAlarmLevel < maxAlarmLevel)
        {
            ScanBehaviour();
            ReduceAlarmLevel();

        }
        else
        {
            MoveBehaviour();
        }
        invoker.Update();
    }

    private void ScanBehaviour()
    {
        invoker.AddCommand(scanCommand);
    }

    private void MoveBehaviour()
    {
        invoker.AddCommand(moveForwardCommand);
        invoker.AddCommand(steerCommand);
        float chance = Random.Range(0,1000);
        if(chance < 1)
        {
            currentAlarmLevel = 0;
        }
    }

    public void NotifyHit()
    {
        RaiseAlarmLevel();
    }

    private void RaiseAlarmLevel()
    {
        currentAlarmLevel += Time.deltaTime * speedAlarmLevel * 1.3f;
        if(currentAlarmLevel> maxAlarmLevel)
        {
            currentAlarmLevel = maxAlarmLevel;
        }
    }

    private void ReduceAlarmLevel()
    {
        currentAlarmLevel -= Time.deltaTime * speedAlarmLevel * 0.3f;
        if(currentAlarmLevel<0)
        {
            currentAlarmLevel = 0;
        }
    }
}
