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
    public GameObject target;
    private CharacterController characterController;
    private Vector3 directionToGo;
    private List<Command> commands;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        directionToGo = new Vector3(0, 0, 0);
        commands = new List<Command>();
        MoveTowardsCommand moveTowardsCommand = new MoveTowardsCommand(target, speed);
        RotateAroundSelfCommand rotate = new RotateAroundSelfCommand(angularSpeed);
        commands.Add(moveTowardsCommand);
        commands.Add(rotate);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Command c in commands)
        {
            c.Execute(gameObject);
        }

    }

}
