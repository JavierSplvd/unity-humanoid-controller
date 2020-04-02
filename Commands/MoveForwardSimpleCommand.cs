using UnityEngine;

public class MoveForwardSimpleCommand : Command
{
    private float forwardSpeed;
    private Vector3 delta = Vector3.zero;

    public MoveForwardSimpleCommand(float forwardSpeed)
    {
        this.forwardSpeed = forwardSpeed;
    }

    public override void Execute(GameObject gameObject, float scale = 1f)
    {
        delta = gameObject.transform.forward * forwardSpeed * scale * Time.deltaTime;
        gameObject.transform.position += delta;
    }
}