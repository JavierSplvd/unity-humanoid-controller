using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HedronController : MonoBehaviour
{
    [Range(0, 5)]
    public float speed = 1f;
    [Range(0, 100)]
    public float angularSpeed = 1f;
    [Range(0, 20)]
    public float distance = 1f;
    public GameObject target;
    private CharacterController characterController;
    private Vector3 directionToGo;
    private List<Command> commandsWhenPursuing;
    private List<Command> commandsWhenIdle;

    private MoveTowardsCommand moveTowardsCommand;
    private RotateAroundSelfCommand rotate;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        directionToGo = new Vector3(0, 0, 0);
        commandsWhenPursuing = new List<Command>();
        moveTowardsCommand = new MoveTowardsCommand(target, speed);
        rotate = new RotateAroundSelfCommand(angularSpeed);
        commandsWhenPursuing.Add(moveTowardsCommand);
        commandsWhenPursuing.Add(rotate);

        commandsWhenIdle = new List<Command>();
        commandsWhenIdle.Add(rotate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < distance)
        {
            foreach (Command c in commandsWhenIdle)
            {
                c.Execute(gameObject);
            }
        }
        else
        {
            foreach (Command c in commandsWhenPursuing)
            {
                c.Execute(gameObject);
            }

        }

    }

}
