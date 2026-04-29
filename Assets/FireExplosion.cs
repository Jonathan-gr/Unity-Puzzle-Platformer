using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float upwardModifier = 0.5f; // adds slight upward bias
    public float stunDuration = 0.3f;
    public List<string> allowedTags = new List<string> { "Player", "MovableBox", "Lizard" };


    void Start()
    {

        Explode();
    }


    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (!allowedTags.Contains(hit.tag)) continue; // use your existing list

            Vector2 direction = hit.transform.position - transform.position;
            if (direction.magnitude < 0.1f) direction = Vector2.up;

            float distance = direction.magnitude;
            float falloff = 1f - Mathf.Clamp01(distance / explosionRadius);

            direction.Normalize();
            direction.y += upwardModifier;
            direction.Normalize();

            Vector2 force = direction * explosionForce * falloff;

            if (hit.CompareTag("Player"))
            {
                PlayerMovement playerMove = hit.GetComponent<PlayerMovement>();
                playerMove?.GetKnockedBack(force, stunDuration);
            }
            else if (hit.CompareTag("Lizard"))
            {
                LizardMovement lizard = hit.GetComponent<LizardMovement>();
                lizard?.GetKnockedBack(force, stunDuration);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}