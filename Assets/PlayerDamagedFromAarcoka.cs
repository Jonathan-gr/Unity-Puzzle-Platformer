using UnityEngine;

public class PlayerDamagedFromAarcoka : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player Hit1");
            Debug.Log("Player Hit2");
        }
    }

    // Update is called once per frame

}
