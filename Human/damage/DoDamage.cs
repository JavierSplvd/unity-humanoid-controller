using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            if(contact.otherCollider.gameObject.GetComponent<Damageable>() != null)
            {
                Debug.Log("DAMAGE");
            }
        }

    }

}
