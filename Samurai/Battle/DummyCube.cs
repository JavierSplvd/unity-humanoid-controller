using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCube : MonoBehaviour, DamageableInterface
{
    public void ReceiveDamage(int attackValue)
    {
        Debug.Log("DummyDamage! " + attackValue);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
