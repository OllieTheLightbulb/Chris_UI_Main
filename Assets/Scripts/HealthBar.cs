using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public bool autoStart = true;

    [Tooltip("How many pixels to reduce per second.")]
    public float reduceAmountPerSecond = 1f;

    private RectTransform bar;
    private float currentWidth;

    void Start()
    {
        bar = healthBarImage.rectTransform;
        currentWidth = bar.sizeDelta.x; // Start width
    }

    void Update()
    {
        if (!autoStart || currentWidth <= 0) return;

        // Reduce smoothly based on time and rate
        currentWidth -= reduceAmountPerSecond * Time.deltaTime;

        // Clamp to 0 so it doesn't go negative
        currentWidth = Mathf.Max(0, currentWidth);

        // Apply new width
        bar.sizeDelta = new Vector2(currentWidth, bar.sizeDelta.y);
    }
}