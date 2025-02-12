using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSliderPositions : MonoBehaviour
{
    Vector2 sliderStart;
    Vector2 sliderEnd;
    RectTransform rTransform;
    Slider slider;

    void Start()
    {
        rTransform = GetComponent<RectTransform>();
        slider = GetComponent<Slider>();
    }

    public Vector2 GetProgressPositonWorld()
    {
        sliderStart = (Vector2)transform.position + (rTransform.rect.width / 2 * Vector2.left);
        sliderEnd = (Vector2)transform.position + (rTransform.rect.width / 2 * Vector2.right);
        
        Vector2 progressPosition = sliderStart + (sliderEnd - sliderStart) * (slider.value / slider.maxValue);

        return Camera.main.ScreenToWorldPoint(progressPosition);
    }
}
