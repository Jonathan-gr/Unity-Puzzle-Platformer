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

        animator = transform.Find("Visual").GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animator not found on Visual child!", this);

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
        // Find the rotating visual object
        Transform visual = transform.Find("Visual");     // Change to "Body" if you used that name

        if (visual == null)
        {
            Debug.LogWarning("Visual child not found on Lizard!", this);
            return;
        }

        // Check the child's rotation instead of root
        bool isUpsideDown = Mathf.Abs(Mathf.DeltaAngle(visual.eulerAngles.z, 180f)) < 15f;

        Vector3 scale = transform.localScale;
        float baseScale = Mathf.Abs(scale.y);

        if (isUpsideDown)
        {
            scale.x = (direction == 1) ? baseScale : -baseScale;
        }
        else
        {
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
