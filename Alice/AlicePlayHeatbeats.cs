using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlicePlayHeatbeats : MonoBehaviour
{
    [SerializeField]
    private BaseHumanController controller;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying && controller.GetPercentageCurrentStamina() < 0.1f)
        {
            audioSource.Play();
        }
    }
}
