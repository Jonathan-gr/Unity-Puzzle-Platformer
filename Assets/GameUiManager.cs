using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject loseUI;

    private bool TryShowOnly(GameObject target)
    {

        if (target != null && (winUI.activeSelf || loseUI.activeSelf))
        {
            return false;
        }

        pauseUI.SetActive(false);
        winUI.SetActive(false);
        loseUI.SetActive(false);

        if (target != null)
            target.SetActive(true);
        return true;
    }

    public bool TryShowPause() => TryShowOnly(pauseUI);
    public bool TryShowWin() => TryShowOnly(winUI);
    public bool TryShowLose() => TryShowOnly(loseUI);
    public bool HideAll() => TryShowOnly(null);
}