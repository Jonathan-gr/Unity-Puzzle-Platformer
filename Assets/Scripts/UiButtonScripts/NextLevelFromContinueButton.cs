using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevelFromContinueButton : MonoBehaviour
{


    public string nextLevel = "Sample Scene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevel);
    }
}
