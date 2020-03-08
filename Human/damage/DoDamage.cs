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

    void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.GetComponent<Damageable>() != null)
        {
            other.gameObject.GetComponent<Damageable>().Damage();
        }


    }

}
