using UnityEngine;

public class PlayerBounceOnLizard : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float baseBounce = 1f;
    public float bounceMultiplier = 1.1f;
    public float maxBounceVelocity = 25f;
    public float minFallSpeed = 2f;

    [Header("Lizard Reaction")]
    public float lizardPushDownForce = 18f;
    public float minLizardPush = 8f;

    [Header("Crush Settings")]
    public GameObject LizardDiedPrefab;
    public float crushHeight = 23f;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("1");
        if (!collision.CompareTag("Player")) return;

        Rigidbody2D playerRb = collision.attachedRigidbody;
        if (playerRb == null) return;
        Debug.Log("2");
        Rigidbody2D lizardRb = GetComponentInParent<Rigidbody2D>();

        float fallSpeed = -playerRb.linearVelocity.y;

        if (fallSpeed < minFallSpeed) return;
        Debug.Log("3");
        // === CRUSH ===
        if (fallSpeed > crushHeight)
        {
            if (LizardDiedPrefab != null)
                Instantiate(LizardDiedPrefab, transform.position, Quaternion.identity);

            Destroy(transform.parent.gameObject);
            return;
        }

        // === BOUNCE PLAYER UP ===
        float bounceVelocity = fallSpeed * bounceMultiplier + baseBounce;
        bounceVelocity = Mathf.Min(bounceVelocity, maxBounceVelocity);

        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceVelocity);

        // === PUSH LIZARD DOWN ===
        if (lizardRb != null)
        {
            float push = Mathf.Max(minLizardPush, fallSpeed * 0.9f);
            lizardRb.linearVelocity = new Vector2(lizardRb.linearVelocity.x, -push);
            lizardRb.AddForce(Vector2.down * lizardPushDownForce, ForceMode2D.Impulse);
        }
    }
}