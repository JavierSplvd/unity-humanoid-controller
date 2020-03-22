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
    public BaseHumanController baseHumanController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        targetObject.position = transform.position + 10f * transform.forward + 1.5f * Vector3.up + baseHumanController.GetInputInCameraCoordinates();
        // head IK
        animator.SetLookAtPosition(targetObject.position);
        animator.SetLookAtWeight(1f, bodyWeight, headWeight);

    }
}
