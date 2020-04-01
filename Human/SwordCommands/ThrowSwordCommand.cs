using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSwordCommand : Command
{
    private Vector3 direction;
    private Vector3 originalPosition;
    private Animator anim;
    private float speed;
    private float currentSpeed;
    private float maxDistance;

    public ThrowSwordCommand(Vector3 direction, float speed, Vector3 originalPosition, float maxDistance)
    {
        this.originalPosition = originalPosition;
        this.direction = direction;
        this.speed = speed;
        this.maxDistance = maxDistance;
    }

    // Update is called once per frame
    public override void Execute(GameObject gameObject)
    {
        if(Vector3.Distance(originalPosition, gameObject.transform.position) > maxDistance)
        {
            return;
        }
        currentSpeed = speed * 0.90f;
        gameObject.transform.position = gameObject.transform.position + direction * currentSpeed * Time.deltaTime;
    }

}
