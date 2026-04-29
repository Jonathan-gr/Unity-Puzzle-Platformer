using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float stunDuration = 0.3f;
    public List<string> allowedTags = new List<string> { "Player", "MovableBox", "Lizard" };

    [Header("Refined Tuning")]
    public float horizontalPowerScale = 1.0f;
    public float verticalPowerScale = 1.0f;
    [Tooltip("The minimum upward force applied, regardless of blast position.")]
    public float baseUpwardForce = 5f;

    void Start()
    {
        Explode();
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (!allowedTags.Contains(hit.tag)) continue;

            // 1. Direction and Distance
            Vector2 rawDirection = (Vector2)hit.transform.position - (Vector2)transform.position;
            float distance = rawDirection.magnitude;
            float falloff = 1f - Mathf.Clamp01(distance / explosionRadius);

            // 2. Separate X and Y
            // We normalize the raw direction to get the proper 'push'
            Vector2 dir = rawDirection.normalized;

            // 3. Construct the force
            // We add baseUpwardForce to ensure there is ALWAYS an upward kick
            float forceX = dir.x * explosionForce * horizontalPowerScale;
            float forceY = (dir.y + baseUpwardForce) * explosionForce * verticalPowerScale;

            Vector2 finalForce = new Vector2(forceX, forceY) * falloff;

            ApplyKnockback(hit, finalForce);
        }
    }

    void ApplyKnockback(Collider2D hit, Vector2 force)
    {
        if (hit.CompareTag("Player"))
        {
            PlayerMovement playerMove = hit.GetComponent<PlayerMovement>();

            // CRITICAL: We must ensure the player's script doesn't 
            // immediately overwrite this velocity in its next Update()
            playerMove?.GetKnockedBack(force, stunDuration);
        }
        else if (hit.CompareTag("Lizard"))
        {
            LizardMovement lizard = hit.GetComponent<LizardMovement>();
            lizard?.GetKnockedBack(force, stunDuration);
        }
        else
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null) rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
