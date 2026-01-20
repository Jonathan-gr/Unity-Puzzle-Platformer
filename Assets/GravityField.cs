using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class GravityField : MonoBehaviour
{
    public bool gravityGoesUp = false;
    public float gravityForceDown = 9.81f;
    public float gravityForceUp = 10f;

    public float gravityForce;

    [Header("Movement Settings")]
    public float speedMultiplier = 0.5f; // Used to dampen existing velocity

    public float speedMultiplierDown = 0.3f;
    public float speedMultiplierUp = 0.6f;

    public float newGravityScale = 1f;

    private Dictionary<Rigidbody2D, float> oldSpeed = new Dictionary<Rigidbody2D, float>();
    private Dictionary<Rigidbody2D, float> oldGravitySacle = new Dictionary<Rigidbody2D, float>();

    public List<string> allowedTags = new List<string> { "Player", "MovableBox", "Lizard" };


    private SpriteRenderer sr;
    void Awake()
    {
        if (gravityGoesUp)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.flipY = true;
            gravityForce = gravityForceUp;
            speedMultiplier = speedMultiplierUp;
            return;
        }
        gravityForce = gravityForceDown;
        speedMultiplier = speedMultiplierDown;


    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        if (other.TryGetComponent(out IMoveable moveable))
        {
            // Store original speed if not already stored
            if (!oldSpeed.ContainsKey(rb))
            {
                oldSpeed[rb] = moveable.MoveSpeed;
                oldGravitySacle[rb] = rb.gravityScale;
                rb.gravityScale = newGravityScale;
                moveable.MoveSpeed *= speedMultiplier;
            }
        }
        // Optional: Immediate speed burst or dampening upon entry
        rb.linearVelocity *= speedMultiplier;

    }

    void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;
        if (!allowedTags.Contains(other.tag)) return;

        Vector2 direction = gravityGoesUp ? Vector2.up : Vector2.down;
        rb.AddForce(direction * gravityForce * rb.mass);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        // Restore speed using the stored value in dictionary
        if (other.TryGetComponent(out IMoveable moveable) && oldSpeed.TryGetValue(rb, out float originalSpeed)
        && oldGravitySacle.TryGetValue(rb, out float originalGravitySacle))
        {
            moveable.MoveSpeed = originalSpeed;
            rb.gravityScale = originalGravitySacle;
            oldSpeed.Remove(rb);
            oldGravitySacle.Remove(rb);
        }
    }
}
