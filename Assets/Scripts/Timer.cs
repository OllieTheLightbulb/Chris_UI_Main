using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Tooltip("Assign the TextMeshProUGUI object here.")]
    public TMP_Text timerText;

    private float elapsedTime = 0f;

    void Start()
    {
        if (timerText == null)
            timerText = GetComponent<TMP_Text>(); // Auto-assign if attached to TMP object
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Minutes
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        // Seconds
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        // Milliseconds (hundredths of a second)
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100f) % 100f);

        // Format: 00:00:00
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
