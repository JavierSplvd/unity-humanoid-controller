using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPosUI : MonoBehaviour
{
    [SerializeField]
    private float amplitudeX, amplitudeY, angularSpeed;
    [SerializeField]
    private Vector2 originalPos;
    private float phase;
    private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.localPosition;
        phase = Random.Range(0,3);
    }

    // Update is called once per frame
    void Update()
    {
        float x = originalPos.x + Mathf.Sin(Time.time * angularSpeed + phase) * amplitudeX;
        float y = originalPos.y + Mathf.Sin(Time.time * angularSpeed + phase) * amplitudeY;
        rect.localPosition = new Vector2(x, y);
    }
}
