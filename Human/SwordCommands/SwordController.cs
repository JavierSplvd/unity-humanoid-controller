using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Command command;
    private Transform swordOriginalParent;
    public float speed;
    private Vector3 positionWhereWasThrown;
    public float maxDistance;

    private void Start() {
        swordOriginalParent = gameObject.transform.parent;
        positionWhereWasThrown = transform.position;
    }
    private void Update() {
        if(command != null)
        {
            command.Execute(gameObject);
        }
    }

    public void SetCommand(Command command)
    {
        this.command = command;
    }

    public void Throw(Vector3 direction)
    {
        positionWhereWasThrown = transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        transform.parent = swordOriginalParent;
        command = new ThrowSwordCommand(new Vector3(direction.x, direction.y, direction.z), speed, positionWhereWasThrown, maxDistance);
    }

    public void ReturnToPlayer(Transform objectToReturn)
    {
        command = new ReturnToThePlayer(objectToReturn, speed);
    }
}