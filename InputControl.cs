using System.Collections;
using UnityEngine;

public class InputControl : MonoBehaviour {
   public GameObject cameraOrbit;

   public float rotateSpeed = 8f;
   public float restViewAngle = 20f;

   void Update()
   {
       if (Input.GetMouseButton(0))
       {
            float h = rotateSpeed * Input.GetAxis("Mouse X");
            float v = rotateSpeed * Input.GetAxis("Mouse Y");

            float polarAngle = Vector3.Angle(Vector3.up, cameraOrbit.transform.forward);
            Debug.Log("Angle: " + polarAngle);

            if (polarAngle < 10f){
                v = 0;
            }
            if (polarAngle > 170f){
                v = 0;
            }

            cameraOrbit.transform.eulerAngles = new Vector3(cameraOrbit.transform.eulerAngles.x + v, cameraOrbit.transform.eulerAngles.y + h, cameraOrbit.transform.eulerAngles.z);
        }
        else {
            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(restViewAngle, cameraOrbit.transform.eulerAngles.y, 0);

            // Dampen towards the target rotation
            cameraOrbit.transform.rotation = Quaternion.Slerp(cameraOrbit.transform.rotation, target,  Time.deltaTime * 10f);
        }

       float scrollFactor = Input.GetAxis("Mouse ScrollWheel");

       if (scrollFactor != 0)
       {
           cameraOrbit.transform.localScale = cameraOrbit.transform.localScale * (1f - scrollFactor);
       }

   }
}