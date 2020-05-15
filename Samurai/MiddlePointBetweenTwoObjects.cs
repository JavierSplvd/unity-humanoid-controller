using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MiddlePointBetweenTwoObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject object1, object2;
    [SerializeField]
    private float speed = 10f;

    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        if(object1 == null || object2 == null)
            throw new MissingReferenceException("Some of the objects is missing (null).");
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = (object1.transform.position + object2.transform.position) * 0.5f;
        transform.position = Vector3.LerpUnclamped(transform.position, targetPos,  speed * Time.deltaTime);
    }
}
