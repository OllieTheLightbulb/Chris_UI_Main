using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    [Tooltip("The button that will quit the game when clicked.")]
    public Button quitButton;

    void Start()
    {
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        else
            Debug.LogWarning("[QuitButton] No button assigned.");
    }

    public void QuitGame()
    {
        Debug.Log("[QuitButton] Quitting game...");

        // Quit the application
        Application.Quit();

#if UNITY_EDITOR
        // If running in the Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
