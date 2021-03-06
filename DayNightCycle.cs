﻿using UnityEngine;

[ExecuteInEditMode]
public class DayNightCycle : MonoBehaviour
{
    [Range(0,24)]
    public float time;

    public float peakIntensity = 3.14f;
    public float minIntensity = 0f;

    public float maxAngle = 50;
    public float minAngle = 0;

    public float minTemp = 6600;
    public float maxTemp = 10000;

    public bool auto;

    public GameObject sunObject;

    private Light _light;
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _light.useColorTemperature = true;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = 0f;
        if(time > 12)
        {
            float normalizedTime = (time - 12) / 12;
            angle = Mathf.Lerp(maxAngle, minAngle, normalizedTime);
            _light.intensity = Mathf.Lerp(peakIntensity, minIntensity, normalizedTime);
            transform.rotation = Quaternion.Euler(angle,0,0);
            _light.colorTemperature = Mathf.Lerp(minTemp, maxTemp, normalizedTime);
            
        }
        else
        {
            float normalizedTime = time / 12;
            angle = Mathf.Lerp(minAngle, maxAngle, normalizedTime);
            _light.intensity = Mathf.Lerp(minIntensity, peakIntensity, normalizedTime);
            transform.rotation = Quaternion.Euler(angle,0,0);
            _light.colorTemperature = Mathf.Lerp(maxTemp, minTemp, normalizedTime);

        }
        sunObject.transform.rotation = Quaternion.Euler(angle,0,0);
        PassTime();
    }
    void PassTime()
    {
        if(auto)
        {
            time = time + Time.deltaTime;
            if(time > 24)
            {
                time = time - 24;
            }
        }
    }
}
