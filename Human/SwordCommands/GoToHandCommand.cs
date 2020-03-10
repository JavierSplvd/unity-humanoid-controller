using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToHandCommand : Command
{
    private Transform hand;
    private Animator anim;

    public GoToHandCommand(Transform hand)
    {
        this.hand = hand;
    }

    // Update is called once per frame
    public override void Execute(GameObject gameObject)
    {
        gameObject.transform.parent = hand;
        gameObject.transform.position = hand.transform.position;
        // gameObject.transform.rotation = hand.transform.rotation;
    }

}
