using UnityEngine;

public class LizardBounceOnPlayer : MonoBehaviour
{
    [Header("Lizard Bounce Settings")]
    public float lizardBounceUpForce = 10f;
    public float minLizardBounceSpeed = 1f;

    [Header("Player Reaction")]
    public float playerPushDownForce = 8f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Rigidbody2D playerRb = collision.attachedRigidbody;
        Rigidbody2D lizardRb = GetComponentInParent<Rigidbody2D>();

        if (lizardRb == null) return;

        // Only trigger if lizard is falling fast enough
        if (lizardRb.linearVelocity.y > -minLizardBounceSpeed)
            return;

        // Make lizard bounce upward
        lizardRb.linearVelocity = new Vector2(
            lizardRb.linearVelocity.x,
            lizardBounceUpForce
        );

        // Optional: Push player down a bit
        if (playerRb != null)
        {
            playerRb.AddForce(Vector2.down * playerPushDownForce, ForceMode2D.Impulse);
        }

        Debug.Log("Lizard bounced off Player!");
    }
}