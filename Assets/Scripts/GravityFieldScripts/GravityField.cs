using UnityEngine;
using System.Collections.Generic;

public class GravityField : MonoBehaviour, IButtonListener
{
    public bool gravityGoesUp = false;
    public float gravityForceDown = 9.81f;
    public float gravityForceUp = 10f;
    private float gravityForce;

    [Header("Movement Settings")]
    public float speedMultiplierDown = 0.3f;
    public float speedMultiplierUp = 0.6f;
    public float newGravityScale = 0f; // 0 = no Unity gravity, field handles it

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;

    [Header("Default Values")]
    public float defaultGravityScale = 2f;
    public float defaultMoveSpeed = 6f;
    public float defaultLizardMoveSpeed = 2f;

    public List<string> allowedTags = new List<string> { "Player", "MovableBox", "Lizard" };
    private HashSet<string> tagSet;

    // Track original values per rigidbody
    private static Dictionary<Rigidbody2D, float> originalSpeed = new Dictionary<Rigidbody2D, float>();
    private static Dictionary<Rigidbody2D, float> originalGravityScale = new Dictionary<Rigidbody2D, float>();

    // Static: shared across ALL GravityField instances
    // Tracks how many fields each rigidbody is currently inside
    private static Dictionary<Rigidbody2D, int> fieldCount = new Dictionary<Rigidbody2D, int>();
    // Tracks which field is currently "owning" each rigidbody
    private static Dictionary<Rigidbody2D, GravityField> activeField = new Dictionary<Rigidbody2D, GravityField>();

    void Awake()
    {
        tagSet = new HashSet<string>(allowedTags);
        UpdateFieldSettings();
    }

    void UpdateFieldSettings()
    {
        if (gravityGoesUp)
        {
            gravityForce = gravityForceUp;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            gravityForce = gravityForceDown;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!tagSet.Contains(other.tag)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        bool alreadyInAField = fieldCount.ContainsKey(rb) && fieldCount[rb] > 0;

        if (!fieldCount.ContainsKey(rb))
            fieldCount[rb] = 0;
        fieldCount[rb]++;

        if (!alreadyInAField)
        {
            // First field entry ever — save originals and apply modifications
            IMoveable moveable = GetMoveable(rb);
            if (moveable != null)
            {
                originalSpeed[rb] = moveable.MoveSpeed;
                originalGravityScale[rb] = rb.gravityScale;
                rb.gravityScale = newGravityScale;
                moveable.MoveSpeed *= GetSpeedMultiplier();
            }
            else
            {
                originalGravityScale[rb] = rb.gravityScale;
                rb.gravityScale = newGravityScale;
            }

            rb.linearVelocity *= GetSpeedMultiplier();
        }
        else
        {
            // Already in another field — just update gravity scale to this field's
            // but DON'T touch MoveSpeed or re-apply multiplier
            rb.gravityScale = newGravityScale;
        }

        // This field takes ownership
        activeField[rb] = this;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!tagSet.Contains(other.tag)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        // Only the active field applies force — prevents double gravity
        if (!activeField.TryGetValue(rb, out GravityField owner) || owner != this)
            return;

        Vector2 direction = gravityGoesUp ? Vector2.up : Vector2.down;
        rb.AddForce(direction * gravityForce * rb.mass);

        HandleRotation(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!tagSet.Contains(other.tag)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        if (!fieldCount.ContainsKey(rb)) return;
        fieldCount[rb]--;

        if (fieldCount[rb] <= 0)
        {
            // Left ALL fields — restore everything
            fieldCount.Remove(rb);
            activeField.Remove(rb);

            IMoveable moveable = GetMoveable(rb);
            if (moveable != null && originalSpeed.ContainsKey(rb))
                moveable.MoveSpeed = originalSpeed[rb];

            if (originalGravityScale.ContainsKey(rb))
                rb.gravityScale = originalGravityScale[rb];

            originalSpeed.Remove(rb);
            originalGravityScale.Remove(rb);

            ResetRotation(other);
        }
        // If still in another field, do nothing — active field is already
        // set to the new field by its OnTriggerEnter2D
    }

    // ─── Helpers ────────────────────────────────────────────────────────────────

    float GetSpeedMultiplier()
    {
        return gravityGoesUp ? speedMultiplierUp : speedMultiplierDown;
    }

    IMoveable GetMoveable(Rigidbody2D rb)
    {
        IMoveable m = rb.GetComponent<IMoveable>();
        if (m == null) m = rb.GetComponentInChildren<IMoveable>();
        return m;
    }

    void HandleRotation(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            float targetZ = gravityGoesUp ? 180f : 0f;
            other.transform.rotation = Quaternion.Lerp(
                other.transform.rotation,
                Quaternion.Euler(0, 0, targetZ),
                Time.deltaTime * rotationSpeed
            );
        }
        else if (other.CompareTag("Lizard"))
        {
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

    void ResetRotation(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.transform.root.rotation = Quaternion.Euler(0, 0, 0);
        else if (other.CompareTag("Lizard"))
        {
            Transform visual = other.transform.root.Find("Visual");
            if (visual != null)
                visual.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnButtonPressed(MonoBehaviour sender)
    {
        gravityGoesUp = !gravityGoesUp;
        UpdateFieldSettings();
    }

    public void OnButtonReleased(MonoBehaviour sender) { }
}