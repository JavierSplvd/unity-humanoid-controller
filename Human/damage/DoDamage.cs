using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    private bool damageActive;
    private int damageValue;
    private Cooldown cooldown;

    void Start()
    {
        cooldown = new SimpleCooldown(0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        cooldown.Update();
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.tag.Equals("Damageable") && cooldown.IsAvailable() && damageActive)
        {
            other.gameObject.SendMessage("ReceiveDamage", damageValue);
            cooldown.Heat();
            damageActive = false;
        }
    }

    public void SetDamageValue(int v)
    {
        damageValue = v;
    }
    public void SetDamageActive()
    {
        damageActive = true;
    }

}
