using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSwordCommand : Command
{
    private Vector3 direction;
    private Animator anim;
    private float speed;

    public ThrowSwordCommand(Vector3 direction)
    {
        this.direction = direction;
        this.speed = speed;
    }

    // Update is called once per frame
    public override void Execute(GameObject gameObject)
    {
        gameObject.transform.position = gameObject.transform.position + direction * speed * Time.deltaTime;
    }

}
