using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public class MoveWithingBounds : MonoBehaviour
    {
        [SerializeField]
        private float verticalBound, horizontalBound;
        [SerializeField]
        private GameObject target, actual;
        [SerializeField]
        private bool vertical, horizontal = true;
        private Camera cameraMain;
        private Vector2 mouseDelta;
        void Start()
        {

        }

        void Update()
        {
            UpdateTarget();
            LerpActual();
        }

        private void UpdateTarget()
        {

            HorizontalMovement();
            VerticalMovement();
        }

        private void VerticalMovement()
        {
            if(vertical)
                return;
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

        private void HorizontalMovement()
        {
            if(horizontal)
                return;
            float fDelta = mouseDelta.x * Time.deltaTime * CameraSign();
            Vector3 forward = target.transform.forward * fDelta;
            Vector3 basePos = transform.position;
            basePos.y = target.transform.position.y; // with this we avoid missing the vertical movement.
            target.transform.position = target.transform.position + forward;
            target.transform.position = Vector3.ClampMagnitude(target.transform.position - basePos, horizontalBound) + basePos;
        }

        private void LerpActual()
        {
            actual.transform.position = Vector3.Lerp(actual.transform.position, target.transform.position, 10f * Time.deltaTime);

        }

        private int CameraSign()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                }
            }
            float angle = Vector3.SignedAngle(cameraMain.gameObject.transform.forward, transform.forward, Vector3.up);
            Debug.Log(angle);
            if (60 > angle && angle < 120)
                return -1;
            else
                return 1;
        }

        // public void Move(InputAction.CallbackContext context)
        // {
        //     mouseDelta = context.ReadValue<Vector2>();
        // }
    }
}
