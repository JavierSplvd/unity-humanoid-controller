using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    private Image image;
    private Color color;
    private float currentAlpha = 1f;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        color = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        currentAlpha = Mathf.Clamp(currentAlpha - Time.deltaTime, 0, 1f);
        color.a = currentAlpha;
        image.color = color;
    }
}
