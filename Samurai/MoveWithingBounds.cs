using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveWithingBounds : MonoBehaviour
{
    [SerializeField]
    private float verticalBound, horizontalBound;
    [SerializeField]
    private GameObject target, actual;
    private Vector2 mouseDelta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
        LerpActual();
    }

    private void UpdateTarget()
    {
        float y = Mathf.Clamp(target.transform.position.y + mouseDelta.y * Time.deltaTime, 
            transform.position.y - verticalBound,
            transform.position.y + verticalBound
        );
        target.transform.position = new Vector3(
            target.transform.position.x,
            y,
            target.transform.position.z
        );
    }

    private void LerpActual()
    {
        actual.transform.position = Vector3.Lerp(actual.transform.position, target.transform.position, 3f * Time.deltaTime);

    }

    public void Move(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
        Debug.Log("Mouse move: " + mouseDelta);
    }
}
