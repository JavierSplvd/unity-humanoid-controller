using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnim : MonoBehaviour
{

     Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") > 0) 
        {
            GoingForward();
        } 
        else if (Input.GetAxis("Vertical") < 0)
        {
            GoingBackward();
        }
        else
        {
            Idle();
        }
    }

    public void GoingForward()
    {
        animator.SetBool("isGoingForward", true);
        animator.SetBool("isGoingBackward", false);
    }

    public void GoingBackward()
    {
        animator.SetBool("isGoingForward", false);
        animator.SetBool("isGoingBackward", true);
    }

    public void Idle()
    {
        animator.SetBool("isGoingForward", false);
        animator.SetBool("isGoingBackward", false);
    }
}
