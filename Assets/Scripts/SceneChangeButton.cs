using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    [Header("Scene Change Settings")]
    [Tooltip("Name of the scene to load. Must match exactly the name in Build Settings.")]
    public string sceneName;

    [Tooltip("The button that will trigger the scene change.")]
    public Button changeSceneButton;

    void Start()
    {
        if (changeSceneButton != null)
            changeSceneButton.onClick.AddListener(ChangeScene);
        else
            Debug.LogWarning("[SceneChangeButton] No button assigned.");
    }

    public void ChangeScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[SceneChangeButton] No scene name specified.");
            return;
        }

        // Resume time in case game was paused
        Time.timeScale = 1f;

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}
