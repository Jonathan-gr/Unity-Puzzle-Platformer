using UnityEngine;

public class LizardMovement : MonoBehaviour, IMoveable
{
    public float moveSpeed = 2f;
    public LayerMask groundLayer;
    public Transform wallCheck;

    private Rigidbody2D rb;
    private int direction = 1;
    private SpriteRenderer spriteRenderer;

    private bool canMove = true;
    private Animator animator;
    public bool isShocked = false;

    private float lastX;
    private float stuckTimer = 0f;
    private const float stuckThreshold = 0.05f;
    private const float minMoveDelta = 0.001f;

    public float pushStrength = 5;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        lastX = rb.position.x;
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            lastX = rb.position.x;
            stuckTimer = 0f;
            return;
        }

        // Apply movement
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        float deltaX = rb.position.x - lastX;
        bool isTryingToMove = Mathf.Abs(rb.linearVelocity.x) > 0.01f;
        bool isBlocked = Mathf.Abs(deltaX) < minMoveDelta;

        if (isTryingToMove && isBlocked)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= stuckThreshold)
            {
                Flip();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastX = rb.position.x;

        // --- NEW: UPSIDE DOWN VISUAL CORRECTION ---
        UpdateVisualOrientation();
    }

    void UpdateVisualOrientation()
    {
        // Check if upside down (Z rotation near 180)
        bool isUpsideDown = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 180f)) < 10f;

        Vector3 scale = transform.localScale;

        // We use scale.y to keep the scale consistent if you've resized the lizard
        float baseScale = Mathf.Abs(scale.y);

        if (isUpsideDown)
        {
            // SWAPPED: If it was wrong before, we flip these two
            scale.x = (direction == 1) ? baseScale : -baseScale;
        }
        else
        {
            // SWAPPED: If it was wrong before, we flip these two
            scale.x = (direction == 1) ? -baseScale : baseScale;
        }

        transform.localScale = scale;
    }


    void Flip()
    {
        direction *= -1;
        // We no longer flip scale here because UpdateVisualOrientation handles it every frame
    }

    // ... (Keep your LizardHit and OnCollisionStay2D logic exactly the same) ...

    public void LizardHit()
    {
        canMove = !canMove;
        isShocked = !isShocked;
        animator.SetBool("isShocked", isShocked);

        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
