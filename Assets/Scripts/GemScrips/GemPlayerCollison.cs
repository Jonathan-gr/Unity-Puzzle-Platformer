using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class GemPlayerCollision : MonoBehaviour
{

    private GemContainer gemContainer;

    void Start()
    {
        // Find the GemContainer once at start
        gemContainer = FindAnyObjectByType<GemContainer>();

        if (gemContainer == null)
        {
            Debug.LogError("GemContainer not found in scene! Make sure it's active.", this);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gemContainer != null)
            {
                gemContainer.AddGem();
                Destroy(gameObject);
            }
        }
    }
}