using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    [Header("UI Panel Pause")]
    public GameObject pauseMenuUI;

    [Header("Keyboard Key")]
    public KeyCode pauseKey = KeyCode.Escape;

    public static bool isPaused = false;

    void Update()
    {
        // Pause pakai tombol keyboard
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    // Dipanggil oleh ikon UI (button pause)
    public void PauseButton()
    {
        Pause();
    }

    // Dipanggil oleh ikon UI (button resume)
    public void ResumeButton()
    {
        Resume();
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = true; // atau false kalau gamenya fps
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
