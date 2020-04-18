using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private bool opened;
    [SerializeField]
    private float closedAngle, openedAngle, rotationSpeed;
    [SerializeField]
    private GameObject[] keys;
    [SerializeField]
    private GameObject cameraForThePortal;

    private GameObject pivot;
    private Quaternion originalRot;
    private GameObject endgame;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).name.Equals("pivot"))
            {
                pivot = transform.GetChild(i).gameObject;
            }
            if(transform.GetChild(i).name.Equals("endgame"))
            {
                endgame = transform.GetChild(i).gameObject;
            }
        }
        if(pivot == null)
        {
            throw new MissingReferenceException("Missing pivot children for a door.");
        }
        originalRot = pivot.transform.rotation;

        cameraForThePortal.SetActive(false);
        endgame.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(opened)
        {
            pivot.transform.rotation = calcRotation(openedAngle);
        }
        else
        {
            pivot.transform.rotation = calcRotation(closedAngle);
        }
    }

    private Quaternion calcRotation(float angle)
    {
        return Quaternion.RotateTowards(pivot.transform.rotation, originalRot * Quaternion.Euler(0, angle, 0), rotationSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        foreach(GameObject go in keys)
        {
            if(other.gameObject.Equals(go))
            {
                opened = true;
                cameraForThePortal.SetActive(true);
                endgame.SetActive(true);
                audioSource.Play();
            }
        }
    }
}
