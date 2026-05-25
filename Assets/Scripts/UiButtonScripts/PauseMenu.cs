using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameUIManager uiManager;

    private bool isPaused = false;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {

        isPaused = uiManager.TryShowPause();

        if (!isPaused)
        {
            return;
        }
        Time.timeScale = 0f;


    }

    public void ResumeGame()
    {
        isPaused = false;

        uiManager.HideAll();

        Time.timeScale = 1f;

    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}