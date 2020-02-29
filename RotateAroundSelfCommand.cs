using UnityEngine;

public class RotateAroundSelfCommand : Command
{
    private float speed;
    private CharacterController characterController;
    public RotateAroundSelfCommand(float speed)
    {
        this.speed = speed;
    }

    public override void Execute(GameObject gameObject)
    {
        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, speed * Time.deltaTime);

    }

}