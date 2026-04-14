using UnityEngine;

public class SpaceShipPausesGame : MonoBehaviour
{
    public GameUIManager uiManager;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                uiManager.TryShowWin();
            }
        }
    }
}