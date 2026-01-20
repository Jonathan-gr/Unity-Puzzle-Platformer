using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartLevelFromButton : MonoBehaviour
{


    public void LoadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
