using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MiddlePointBetweenTwoObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject object1, object2;
    // Start is called before the first frame update
    void Start()
    {
        if(object1 == null || object2 == null)
            throw new MissingReferenceException("Some of the objects is missing (null).");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (object1.transform.position + object2.transform.position) * 0.5f;
    }
}
