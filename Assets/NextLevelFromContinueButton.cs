using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevelFromContinueButton : MonoBehaviour
{


    public int nextLevel = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level" + nextLevel);
    }
}
