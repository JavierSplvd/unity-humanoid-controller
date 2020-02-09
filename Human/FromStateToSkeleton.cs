using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromStateToSkeleton : MonoBehaviour
{
    public HumanoidStateMachine stateMachine;

    public float smooth;

    public Transform head;
    public Transform armRight;
    public Transform forearmRight;
    public Transform handRight;
    public Transform armLeft;
    public Transform forearmLeft;
    public Transform handLeft;
    public Transform thighLeft;
    public Transform legLeft;
    public Transform thighRight;
    public Transform legRight;
    public Transform torso;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(stateMachine.currentState == HumanoidStateMachine.State.WalkingForward) 
        {
            Quaternion target = torso.transform.rotation * Quaternion.Euler(-160, -50, -90);
            armRight.transform.rotation = Quaternion.Slerp(armRight.transform.rotation, target,  Time.deltaTime * smooth);

            target = torso.transform.rotation * Quaternion.Euler(160, 45, -90);
            armLeft.transform.rotation = Quaternion.Slerp(armLeft.transform.rotation, target,  Time.deltaTime * smooth);
        } else if (stateMachine.currentState == HumanoidStateMachine.State.Idle)
        {
            Quaternion target = torso.transform.rotation * Quaternion.Euler(-160, 0, -90);
            armRight.transform.rotation = Quaternion.Slerp(armRight.transform.rotation, target,  Time.deltaTime * smooth);

            target = torso.transform.rotation * Quaternion.Euler(170, 0, -90);
            armLeft.transform.rotation = Quaternion.Slerp(armLeft.transform.rotation, target,  Time.deltaTime * smooth);
        }  
    }
}
