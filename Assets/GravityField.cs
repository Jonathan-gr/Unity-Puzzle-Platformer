using UnityEngine;
using System.Collections.Generic;

public class GravityField : MonoBehaviour
{
    public bool gravityGoesUp = false;
    public float gravityForceDown = 9.81f;
    public float gravityForceUp = 10f;
    public float gravityForce;

    [Header("Movement Settings")]
    public float speedMultiplier = 0.5f;
    public float speedMultiplierDown = 0.3f;
    public float speedMultiplierUp = 0.6f;
    public float newGravityScale = 1f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;

    private Dictionary<Rigidbody2D, float> oldSpeed = new Dictionary<Rigidbody2D, float>();
    private Dictionary<Rigidbody2D, float> oldGravityScale = new Dictionary<Rigidbody2D, float>();

    public List<string> allowedTags = new List<string> { "Player", "MovableBox", "Lizard" };
    private HashSet<string> tagSet;

    void Awake()
    {
        tagSet = new HashSet<string>(allowedTags);

        if (gravityGoesUp)
        {
            gravityForce = gravityForceUp;
            speedMultiplier = speedMultiplierUp;
            transform.rotation = Quaternion.Euler(0, 0, 180);

        }
        else
        {
            gravityForce = gravityForceDown;
            speedMultiplier = speedMultiplierDown;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!tagSet.Contains(other.tag)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        if (other.TryGetComponent(out IMoveable moveable))
        {
            if (!oldSpeed.ContainsKey(rb))
            {
                oldSpeed[rb] = moveable.MoveSpeed;
                oldGravityScale[rb] = rb.gravityScale;
                rb.gravityScale = newGravityScale;
                moveable.MoveSpeed *= speedMultiplier;
            }
        }
        rb.linearVelocity *= speedMultiplier;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null || !tagSet.Contains(other.tag)) return;

        // Apply Gravity Force
        Vector2 direction = gravityGoesUp ? Vector2.up : Vector2.down;
        rb.AddForce(direction * gravityForce * rb.mass);

        // === DIFFERENT ROTATION BEHAVIOR ===
        if (other.CompareTag("Player"))
        {
            // PLAYER: Rotate root (old behavior)
            float targetZ = gravityGoesUp ? 180f : 0f;
            other.transform.rotation = Quaternion.Lerp(
                other.transform.rotation,
                Quaternion.Euler(0, 0, targetZ),
                Time.deltaTime * rotationSpeed
            );
        }
        else if (other.CompareTag("Lizard"))
        {
            // LIZARD: Rotate only Visual child
            Transform visual = other.transform.Find("Visual");
            if (visual != null)
            {
                float targetZ = gravityGoesUp ? 180f : 0f;
                visual.rotation = Quaternion.Lerp(
                    visual.rotation,
                    Quaternion.Euler(0, 0, targetZ),
                    Time.deltaTime * rotationSpeed
                );
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        // Reset Rotation
        if (other.CompareTag("Player"))
        {
            other.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (other.CompareTag("Lizard"))
        {
            Transform visual = other.transform.Find("Visual");
            if (visual != null)
                visual.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Reset movement values
        if (other.TryGetComponent(out IMoveable moveable) &&
            oldSpeed.TryGetValue(rb, out float originalSpeed) &&
            oldGravityScale.TryGetValue(rb, out float originalGravityScale))
        {
            moveable.MoveSpeed = originalSpeed;
            rb.gravityScale = originalGravityScale;
            oldSpeed.Remove(rb);
            oldGravityScale.Remove(rb);
        }
    }
}