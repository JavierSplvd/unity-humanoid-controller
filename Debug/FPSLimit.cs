using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimit : MonoBehaviour
{
    public enum Option { Off, FPS30, FPS60 }
    public Option currentMode = Option.FPS30;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Application.targetFrameRate);
    }

    void Awake()
    {
        if (currentMode.Equals(Option.Off))
        {
            //
        }
        else if (currentMode.Equals(Option.FPS30))
        {
            QualitySettings.vSyncCount = 2;
            #if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 30;
            #endif
        }
        else if (currentMode.Equals(Option.FPS60))
        {
            QualitySettings.vSyncCount = 2;
            #if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 60;
            #endif
        }
    }
}
