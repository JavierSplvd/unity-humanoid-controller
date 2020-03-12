using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToThePlayer : Command
{
    private Transform hand;
    private Animator anim;
    private float speed;
    private float currentSpeed;

    public ReturnToThePlayer(Transform hand, float speed)
    {
        this.speed = speed;
        this.hand = hand;
    }

    // Update is called once per frame
    public override void Execute(GameObject gameObject)
    {
        if(Vector3.Distance(hand.transform.position, gameObject.transform.position) < 0.1f)
        {
            gameObject.transform.parent = hand.transform;
            return;
        }
        Vector3 direction = (hand.position - gameObject.transform.position).normalized;
        currentSpeed = speed * 1.1f;
        gameObject.transform.position = gameObject.transform.position + direction * speed * Time.deltaTime;

    }

}
