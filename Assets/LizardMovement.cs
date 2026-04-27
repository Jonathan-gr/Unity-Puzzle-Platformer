using UnityEngine;

public class LizardMovement : MonoBehaviour, IMoveable
{
    public float moveSpeed = 2f;


    public Transform visual;

    private Rigidbody2D rb;
    private int direction = 1;

    private bool canMove = true;
    public Animator animator;
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

        if (visual == null)
        {
            Debug.Log(visual);
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
    }



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
