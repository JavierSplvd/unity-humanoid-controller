using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToTarget : MonoBehaviour
{
    public Transform targetObject;
    [Range(0, 1)]
    public float bodyWeight;
    [Range(0, 1)]
    public float headWeight;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        // head IK
        animator.SetLookAtPosition(targetObject.position);
        animator.SetLookAtWeight(1f, bodyWeight, headWeight);

    }
}
