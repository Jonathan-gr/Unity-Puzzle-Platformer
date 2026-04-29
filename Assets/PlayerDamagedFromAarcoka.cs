using UnityEngine;

public class PlayerDamagedFromAarcoka : MonoBehaviour
{
    public float knockbackForce = 8f;
    public float upwardForce = 2f;
    public float stunDuration = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        // 1. Get the PlayerMovement script instead of just the Rigidbody
        PlayerMovement playerMove = collision.GetComponent<PlayerMovement>();

        if (playerMove != null)
        {
            Debug.Log("Player Hit - Calling Knockback Routine");

            // 2. Calculate the direction from the attack to the player
            Vector2 direction = (collision.transform.position - transform.root.position).normalized;

            // Force horizontal direction only
            direction.y = 0;
            direction.Normalize();

            Vector2 force = direction * knockbackForce + Vector2.up * upwardForce;
            Debug.DrawLine(transform.root.position, collision.transform.position, Color.red, 1f);

            // 4. Call the specific function that handles 'canMove = false'
            playerMove.GetKnockedBack(force, stunDuration);
        }
    }

    void Start()
    {
        Destroy(gameObject, 0.05f);
    }
}