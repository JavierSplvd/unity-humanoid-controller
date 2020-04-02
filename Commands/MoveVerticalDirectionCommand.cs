using UnityEngine;

public class MoveVerticalDirectionCommand : Command
{
    private float speed;
    private Transform target;
    private Vector3 speedVector;
    public MoveVerticalDirectionCommand(float speed, Transform target)
    {
        speedVector = Vector3.zero;
        this.speed = speed;
        this.target = target;
    }

    public override void Execute(GameObject gameObject, float scale = 1f)
    {
        speedVector.y = speed;
        if(gameObject.transform.position.y > target.position.y)
        {
            speedVector = speedVector * -1;
        }
        gameObject.transform.Translate(speedVector * scale * Time.deltaTime, Space.Self);
    }
}