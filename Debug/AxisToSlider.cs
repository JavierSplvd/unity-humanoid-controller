﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AxisToSlider : MonoBehaviour
{
    private Slider slider;
    public string axisToWatch;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Input.GetAxis(axisToWatch);
    }
}
