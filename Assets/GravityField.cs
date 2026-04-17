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
    public float rotationSpeed = 5f; // How fast they flip

    private Dictionary<Rigidbody2D, float> oldSpeed = new Dictionary<Rigidbody2D, float>();
    private Dictionary<Rigidbody2D, float> oldGravityScale = new Dictionary<Rigidbody2D, float>();

    // Track transforms of objects currently inside the field
    private HashSet<Transform> activeTransforms = new HashSet<Transform>();

    public List<string> allowedTags = new List<string> { "Player", "MovableBox", "Lizard" };
    private HashSet<string> tagSet;
    private SpriteRenderer sr;

    void Awake()
    {
        tagSet = new HashSet<string>(allowedTags);
        if (gravityGoesUp)
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.flipY = true;
            gravityForce = gravityForceUp;
            speedMultiplier = speedMultiplierUp;
        }
        else
        {
            gravityForce = gravityForceDown;
            speedMultiplier = speedMultiplierDown;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!tagSet.Contains(other.tag)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        // Add to the rotation tracking set
        activeTransforms.Add(other.transform);

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

        // Apply Smooth Rotation
        float targetZ = gravityGoesUp ? 180f : 0f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZ);

        other.transform.rotation = Quaternion.Lerp(
            other.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        // Remove from tracking
        activeTransforms.Remove(other.transform);

        // Reset rotation immediately or start a separate coroutine if you want it smooth outside
        other.transform.rotation = Quaternion.Euler(0, 0, 0);

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
