using UnityEngine;

public class MoveForwardCommand : Command
{
    private float forwardSpeed;

    public MoveForwardCommand(float forwardSpeed)
    {
        this.forwardSpeed = forwardSpeed;
    }

    public override void Execute(GameObject gameObject)
    {
        gameObject.GetComponent<CharacterController>().Move(gameObject.transform.forward * forwardSpeed * Time.deltaTime);

    }
}