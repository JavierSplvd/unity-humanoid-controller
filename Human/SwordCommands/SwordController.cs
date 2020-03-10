using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Command command;
    private Transform swordOriginalParent;
    public float speed;

    private void Start() {
        swordOriginalParent = gameObject.transform.parent;

    }
    private void Update() {
        Debug.Log(command);
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
        transform.rotation = Quaternion.LookRotation(direction);
        transform.parent = swordOriginalParent;
        command = new ThrowSwordCommand(direction, speed);
    }
}