using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Command command;

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
}