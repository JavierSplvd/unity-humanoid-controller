using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject hand;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Pickable"))
        {
            other.gameObject.transform.position = hand.transform.position;
            other.gameObject.transform.parent = hand.transform;
            audioSource.Play();
        }    
    }
}
