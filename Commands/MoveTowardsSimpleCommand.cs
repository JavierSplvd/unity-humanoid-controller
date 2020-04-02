using UnityEngine;

public class MoveTowardsSimpleCommand : Command
{
    private GameObject target;
    private float speed;
    private Vector3 directionToGo;
    public MoveTowardsSimpleCommand(GameObject target, float speed)
    {
        this.target = target;
        this.speed = speed;
        directionToGo = Vector3.zero;
    }

    public override void Execute(GameObject gameObject, float scale = 1f)
    {
        directionToGo = target.transform.position - gameObject.transform.position;
        gameObject.transform.position += directionToGo.normalized * speed * scale * Time.deltaTime;
    }

}