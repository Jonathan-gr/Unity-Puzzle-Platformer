using NUnit.Framework;
using UnityEngine;

public class LizardMovement : MonoBehaviour, IMoveable
{
    public float moveSpeed = 2f;
    public LayerMask groundLayer;

    public Transform wallCheck;

    private Rigidbody2D rb;
    private int direction = -1;

    private SpriteRenderer spriteRenderer;

    public int i = 0;
    private bool canMove = true;
    private float defaultGravity;
    private Animator animator;
    public bool isShocked = false;

    private float lastX;
    private float stuckTimer = 0f;
    private const float stuckThreshold = 0.05f;
    private const float minMoveDelta = 0.001f;

    private float realMoveSpeed;

    [Header("Sound")]
    public AudioClip staticShockSound;
    public float lazerShockerVolume = 0.1f;

    private AudioSource activeLoop;

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
        defaultGravity = rb.gravityScale;
        animator = GetComponent<Animator>();
        lastX = rb.position.x;
        realMoveSpeed = moveSpeed;


    }
    void Start()
    {
        if (isShocked)
        {
            LizardHit();
        }
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

        // Measure movement from LAST frame
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
    }


    void Flip()
    {
        direction *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1; // Simply multiply by -1 to invert
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (wallCheck != null)
            Gizmos.DrawLine(wallCheck.position,
                wallCheck.position + Vector3.right * direction * 0.1f);

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MoveableBox"))
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody2D boxRb))
            {
                ContactPoint2D contact = collision.GetContact(0);

                // 1. Ensure we are pushing from the side (horizontal)
                if (contact.normal.y > -0.3f && contact.normal.y < 0.3f)
                {
                    // 2. Calculate direction: from Lizard to Box
                    Vector2 pushDir = new Vector2(contact.normal.x * -1, 0);

                    // 3. Apply force to the box
                    // ForceMode2D.Force is better for constant pushing
                    boxRb.AddForce(pushDir * pushStrength, ForceMode2D.Force);


                }
            }
        }
    }



    public void LizardHit()
    {
        canMove = !canMove;
        isShocked = !isShocked;
        //rb.gravityScale = (rb.gravityScale == 0) ? defaultGravity : 0f;
        animator.SetBool("isShocked", isShocked);


        if (isShocked)
        {
            //activeLoop = SoundManager.Instance.PlayLoopingSFX(staticShockSound);
        }
        else if (activeLoop != null)
        {
            //activeLoop.Stop();
            //Destroy(activeLoop.gameObject);
        }


        // Optional: Trigger a "Stun" or "Idle" animation
        // GetComponent<Animator>().SetBool("isMoving", false);
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }


}
