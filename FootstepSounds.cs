using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    public Transform feet;
    public SoundManager soundManager;
    public LayerMask rayMask;
    public float raycastDistance;
    private bool isNewStep = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate(){
        if(CheckCollision(feet.position) && isNewStep)
        {
            soundManager.Play();
            isNewStep = false;
        }
    }
       
    bool CheckCollision(Vector3 position)
    {
        bool hit = Physics.Raycast(position - Vector3.up * raycastDistance * 0.8f, - Vector3.up, raycastDistance, rayMask);
        if(!hit)
        {
            isNewStep = true;
        }
        return hit;
    }


}
