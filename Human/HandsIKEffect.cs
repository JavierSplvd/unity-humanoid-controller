using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HandsIKEffect : MonoBehaviour
{
    [Tooltip("Capa de los objetos donde se puede ajustar el pie")]
    public LayerMask RayMask;
    
    Animator anim;
    RaycastHit hit;
    float weight;

    public bool IkActive = true;
    [Range(1,2)]
    public float handHeight;
    [Range(0,1)]
    public float raycastDistance;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorIK()
    {
        if(CheckLeftHand() && IkActive)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, hit.point + hit.normal * 0.1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(2f * transform.up + transform.right + transform.forward));
        }

        if(CheckRightHand() && IkActive)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, hit.point + hit.normal * 0.1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(2f * transform.up - transform.right + transform.forward));
        }
    }

    bool CheckLeftHand()
    {
        Vector3 handPosition = transform.position + handHeight * Vector3.up - transform.right * 0.2f;
        return CheckRay(handPosition);
        
    }

    bool CheckRightHand()
    {
        Vector3 handPosition = transform.position + handHeight * Vector3.up + transform.right * 0.2f;
        return CheckRay(handPosition);
        
    }

    bool CheckRay(Vector3 position)
    {
        if (Physics.Raycast(position, transform.forward, out hit, raycastDistance, RayMask))
        {
            weight = Time.deltaTime + weight;
            return true;
        }
        else
        {
            weight = 0;
        }
        return false;
    }
}
