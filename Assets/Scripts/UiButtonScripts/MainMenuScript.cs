using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class MainMenu : MonoBehaviour
{

    public int nextLevel = 1;
    public void PlayGame()
    {
        // Loads the next scene in the build queue
        SceneManager.LoadScene("Level" + nextLevel);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
