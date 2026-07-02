using UnityEngine;

public class SpikePlayerCollision : MonoBehaviour
{

    public GameUIManager uiManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Sd");
        if (!collision.CompareTag("Player")) return;


        uiManager.TryShowLose();
        Time.timeScale = 0f;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Sd");
        if (!collision.gameObject.CompareTag("Player")) return;


        uiManager.TryShowLose();
        Time.timeScale = 0f;
    }
}
