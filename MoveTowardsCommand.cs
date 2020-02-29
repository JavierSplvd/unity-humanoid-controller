using UnityEngine;

public class MoveTowardsCommand : Command
{
    private GameObject target;
    private float speed;
    private Vector3 directionToGo;
    private CharacterController characterController;
    public MoveTowardsCommand(GameObject target, float speed)
    {
        this.target = target;
        this.speed = speed;
        directionToGo = Vector3.zero;
    }

    public override void Execute(GameObject gameObject)
    {
        directionToGo = target.transform.position - gameObject.transform.position;
        gameObject.GetComponent<CharacterController>().Move(directionToGo.normalized * speed * Time.deltaTime);
    }

}