using UnityEngine;

public class MoveForwardSimpleCommand : Command
{
    private float forwardSpeed;
    private Vector3 delta = Vector3.zero;

    public MoveForwardSimpleCommand(float forwardSpeed)
    {
        this.forwardSpeed = forwardSpeed;
    }

    public override void Execute(GameObject gameObject)
    {
        delta = gameObject.transform.forward * forwardSpeed * Time.deltaTime;
        gameObject.transform.position += delta;
    }
}