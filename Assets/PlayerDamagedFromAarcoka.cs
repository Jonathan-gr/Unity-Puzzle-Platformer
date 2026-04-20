using UnityEngine;

public class PlayerDamagedFromAarcoka : MonoBehaviour
{
    public float knockbackForce = 8f;
    public float upwardForce = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Debug.Log("Player Hit");

        Rigidbody2D playerRb = collision.attachedRigidbody;
        if (playerRb == null) return;

        //  Direction from attack → player
        Vector2 direction = (collision.transform.position - transform.position).normalized;

        // 👉Optional: flatten vertical so it doesn't get weird
        direction.y = 0;

        // 👉 Apply knockback
        Vector2 force = direction * knockbackForce + Vector2.up * upwardForce;

        playerRb.linearVelocity = Vector2.zero; // optional: reset before applying
        playerRb.AddForce(force, ForceMode2D.Impulse);
    }

    void Start()
    {
        Destroy(gameObject, 0.2f);
    }
}