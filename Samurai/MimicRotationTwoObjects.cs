using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicRotationTwoObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject target1, target2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(target2.transform.position - target1.transform.position, Vector3.up);
        
    }
}
