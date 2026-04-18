using UnityEngine;

public class BounceOffLizard : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float baseBounce = 1f;
    public float bounceMultiplier = 1.1f;
    public float maxBounceVelocity = 25f;
    public float minFallSpeed = 2f;

    [Header("Lizard Reaction")]
    public float lizardPushDownForce = 18f;
    public float minLizardPush = 8f;

    [Header("Player Reaction - Lizard Landing on Player")]
    public float lizardBounceUpForce = 22f;     // How high lizard jumps when landing on player
    public float minLizardBounceSpeed = 10f;

    public GameObject LizardDiedPrefab;
    public float crushHeight = 23f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Rigidbody2D playerRb = collision.attachedRigidbody;
        Rigidbody2D lizardRb = GetComponentInParent<Rigidbody2D>();

        if (playerRb == null) return;

        float relativeYVelocity = playerRb.linearVelocity.y - (lizardRb != null ? lizardRb.linearVelocity.y : 0);

        // === CASE 1: Player falling onto Lizard ===
        if (relativeYVelocity < -minFallSpeed)   // Player is moving down relative to lizard
        {
            float fallSpeed = -playerRb.linearVelocity.y;

            if (fallSpeed > crushHeight)
            {
                if (LizardDiedPrefab != null)
                    Instantiate(LizardDiedPrefab, transform.position, Quaternion.identity);
                Destroy(transform.parent.gameObject);
                return;
            }

            // Bounce Player Up
            float bounceVelocity = fallSpeed * bounceMultiplier + baseBounce;
            bounceVelocity = Mathf.Min(bounceVelocity, maxBounceVelocity);

            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceVelocity);

            // Push Lizard Down
            if (lizardRb != null)
            {
                float push = Mathf.Max(minLizardPush, fallSpeed * 0.9f);
                lizardRb.linearVelocity = new Vector2(lizardRb.linearVelocity.x, -push);
                lizardRb.AddForce(Vector2.down * lizardPushDownForce, ForceMode2D.Impulse);
            }
        }
        // === CASE 2: Lizard falling onto Player ===
        else if (lizardRb != null && lizardRb.linearVelocity.y < -minLizardBounceSpeed)
        {
            float lizardFallSpeed = -lizardRb.linearVelocity.y;

            // Make lizard bounce upward
            lizardRb.linearVelocity = new Vector2(
                lizardRb.linearVelocity.x,
                lizardBounceUpForce
            );

            // Optional: Give player a small downward push
            playerRb.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);

            Debug.Log("Lizard bounced off Player!");
        }
        // if (bounceVelocity < 12f)
        // {
        //     //SoundManager.Instance.PlaySFX(smallBounceSound, smallBounceVolume);
        // }
        // else
        // {
        //     //SoundManager.Instance.PlaySFX(largeBounceSound, bigBounceVolume);
        // }
    }
}