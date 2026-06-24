using UnityEngine;
using System.Collections.Generic;

public class GravityField : MonoBehaviour, IButtonListener
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

    [Header("Default Values (set these to match your player)")]
    public float defaultGravityScale = 2f;
    public float defaultMoveSpeed = 6f; // whatever your player's base speed is

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

        if (oldSpeed.ContainsKey(rb)) return; // block ALL duplicate processing

        IMoveable moveable = rb.GetComponent<IMoveable>();
        if (moveable == null)
            moveable = rb.GetComponentInChildren<IMoveable>();

        if (moveable != null)
        {
            oldSpeed[rb] = moveable.MoveSpeed;       // save original FIRST
            oldGravityScale[rb] = rb.gravityScale;   // save original FIRST

            rb.gravityScale = newGravityScale;
            moveable.MoveSpeed *= speedMultiplier;
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

        if (!tagSet.Contains(other.tag)) return; // add this check you're missing

        // Reset Rotation
        if (other.CompareTag("Player"))
        {
            other.transform.root.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (other.CompareTag("Lizard"))
        {
            Transform visual = other.transform.root.Find("Visual");
            if (visual != null)
                visual.rotation = Quaternion.Euler(0, 0, 0);
        }

        // Get IMoveable from root, not from the specific collider
        IMoveable moveable = rb.GetComponent<IMoveable>();
        if (moveable == null)
            moveable = rb.GetComponentInChildren<IMoveable>();

        if (moveable != null && oldSpeed.ContainsKey(rb))
        {
            moveable.MoveSpeed = defaultMoveSpeed;
            rb.gravityScale = defaultGravityScale;
            oldSpeed.Remove(rb);
            oldGravityScale.Remove(rb);
        }
    }

    public void OnButtonPressed(MonoBehaviour sender)
    {
        gravityGoesUp = !gravityGoesUp;

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

    public void OnButtonReleased(MonoBehaviour sender) { } // nothing on release
}