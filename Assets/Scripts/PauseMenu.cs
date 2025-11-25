using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Needed for reloading scenes

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign the pause menu UI GameObject here.")]
    public GameObject pauseMenuUI;

    [Tooltip("Assign your Resume button here.")]
    public Button resumeButton;

    [Tooltip("Assign your Restart button here.")]
    public Button restartButton;

    [Header("Settings")]
    [Tooltip("Key to toggle pause.")]
    public KeyCode pauseKey = KeyCode.P;

    private bool isPaused = false;
    private CursorLockMode previousLockMode;
    private bool previousCursorVisible;

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false); // hide at start

        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    void Pause()
    {
        // Save cursor state
        previousLockMode = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        // Show UI
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }

    public void Resume()
    {
        // Hide UI
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Resume game
        Time.timeScale = 1f;

        // Restore previous cursor state
        Cursor.lockState = previousLockMode;
        Cursor.visible = previousCursorVisible;

        isPaused = false;
    }

    public void RestartGame()
    {
        // Resume normal time
        Time.timeScale = 1f;

        // Reload current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
