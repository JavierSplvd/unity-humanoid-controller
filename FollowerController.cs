using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FollowerController : MonoBehaviour
{
    private CharacterController characterController;
    private List<Command> commands;
    [Range(0, 30)]
    public float rotationSpeed;
    [Range(0, 10)]
    public float forwardSpeed;
    public GameObject target;
    public LayerMask rayMask;
    public float rayDist = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        commands = new List<Command>();
        Command steer = new SteerToTargetCommand(target, rotationSpeed);
        Command advance = new MoveForwardCommand(forwardSpeed);
        commands.Add(steer);
        commands.Add(advance);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Command c in commands)
        {
            c.Execute(gameObject);
        }
        CheckCollision();
    }

    void CheckCollision()
    {
        Vector3 directionToCastRay = transform.position + transform.forward * characterController.radius;
        bool collision = Physics.Raycast(directionToCastRay, transform.forward, rayDist, rayMask);
        if(collision)
        {
            Debug.Log("!!!");
        }
    }
}
