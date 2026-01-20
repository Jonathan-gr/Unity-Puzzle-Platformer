using UnityEngine;

public class BounceOffLizard : MonoBehaviour
{
    [Header("Bounce Settings")]
    [Tooltip("Base bounce force when landing gently")]
    public float baseBounce = 1f;

    [Tooltip("Extra multiplier applied to the downward speed to get bounce height")]
    public float bounceMultiplier = 1.1f;   // > 1.0 = bouncier than pure reflection

    [Tooltip("Maximum allowed upward velocity after bounce")]
    public float maxBounceVelocity = 25f;

    [Tooltip("Minimum downward speed required to trigger bounce")]
    public float minFallSpeed = 2f;

    [Header("Lizard Momentum Bonus")]
    [Tooltip("How much of the lizard's upward velocity gets added to the bounce")]
    public float lizardUpwardBonusFactor = 0.7f;

    [Header("Squash & First Jump")]
    public bool jumpedOnFirstLizard = false;
    public float lizardJumpBonus = 0f;
    public float crushHeight = 23f; // renamed for clarity

    public GameObject LizardDiedPrefab;

    [Header("Bounce Sound")]
    public AudioClip smallBounceSound;
    public AudioClip largeBounceSound;
    public float smallBounceVolume = 0.07f;
    public float bigBounceVolume = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Rigidbody2D playerRb = collision.attachedRigidbody;
        if (playerRb == null)
            return;

        // Only bounce if player is falling fast enough
        float fallSpeed = -playerRb.linearVelocity.y; // positive = downward
        if (fallSpeed < minFallSpeed)
            return;

        // --- Crush / kill lizard if falling too fast ---
        if (fallSpeed > crushHeight)
        {
            if (LizardDiedPrefab != null)
            {
                Instantiate(LizardDiedPrefab, transform.position, Quaternion.identity);
            }
            Destroy(transform.parent.gameObject);
            return;
        }

        // --- Get lizard's upward velocity (if moving up) ---
        Rigidbody2D lizardRb = GetComponentInParent<Rigidbody2D>();
        float lizardUpVelocity = 0f;
        if (lizardRb != null)
        {
            lizardUpVelocity = Mathf.Max(0f, lizardRb.linearVelocity.y); // only upward counts
        }

        // --- Calculate bounce velocity ---
        // Start with the impact speed, multiply by bounce factor, add lizard bonus

        float bounceVelocity = fallSpeed * bounceMultiplier;
        // Add bonus from lizard's upward movement
        bounceVelocity += lizardUpVelocity * lizardUpwardBonusFactor;


        bounceVelocity += baseBounce;


        // Cap the result
        bounceVelocity = Mathf.Min(bounceVelocity, maxBounceVelocity);

        // Preserve horizontal velocity, set new upward velocity
        playerRb.linearVelocity = new Vector2(
            playerRb.linearVelocity.x,
            bounceVelocity
        );

        // --- Sound based on strength ---
        if (bounceVelocity < 12f)
        {
            //SoundManager.Instance.PlaySFX(smallBounceSound, smallBounceVolume);
        }
        else
        {
            //SoundManager.Instance.PlaySFX(largeBounceSound, bigBounceVolume);
        }

    }
}