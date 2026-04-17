using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMoveable
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 8;

    public float gravityJumpDown = 4f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    [SerializeField] private LayerMask combinedGroundMask;

    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private float horizontalInput;
    public bool isGrounded;
    private bool shouldJump;

    private Animator animator;

    public Vector2 boxSize = new Vector2(0.5f, 0.1f);
    public float castDistance = 0.1f;

    [Header("Sound SFX")]
    public AudioClip jumpSound;
    public float jumpVolume = 0.1f;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (gravityJumpDown == 0)
            gravityJumpDown = 1f;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            shouldJump = true;
        }

        // --- FIX 1: FACING DIRECTION ---
        // We check if the Z rotation is approximately 180
        bool isUpsideDown = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 180f)) < 10f;

        Vector3 scale = transform.localScale;
        if (horizontalInput > 0)
        {
            // If upside down, a positive scale.x actually points Left. 
            // So we flip the logic here.
            scale.x = isUpsideDown ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        }
        else if (horizontalInput < 0)
        {
            scale.x = isUpsideDown ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        }
        transform.localScale = scale;

        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {
        isGrounded = CheckIsGrounded();

        // 4. Horizontal Movement
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // --- FIX 2: JUMP DIRECTION ---
        if (shouldJump)
        {
            // 1. Determine direction (Up if normal, Down if upside down)
            bool isUpsideDown = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 180f)) < 10f;
            Vector2 jumpDir = isUpsideDown ? Vector2.down / gravityJumpDown : Vector2.up;

            // 2. Reset Y velocity ONLY so the jump is consistent
            // But we keep the X velocity so we don't stop moving sideways
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

            // 3. Apply the jump as an Impulse
            // Impulse is perfect for jumps because it's an instant "kick"
            rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);

            shouldJump = false;
        }
    }

    public bool CheckIsGrounded()
    {
        // --- FIX 3: GROUND CHECK DIRECTION ---
        // If upside down, we need to cast the box UP toward the ceiling
        bool isUpsideDown = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 180f)) < 10f;
        Vector2 castDir = isUpsideDown ? Vector2.up : Vector2.down;

        RaycastHit2D hit = Physics2D.BoxCast(groundCheck.position, boxSize, 0f, castDir, castDistance, combinedGroundMask);
        return hit.collider != null;
    }


    private void UpdateAnimatorParameters()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", horizontalInput != 0);

        // --- FIX 4: ANIMATOR VELOCITY ---
        // If upside down, a negative Y velocity is actually "rising" toward the ceiling
        bool isUpsideDown = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 180f)) < 10f;
        float visualYVelocity = isUpsideDown ? -rb.linearVelocity.y : rb.linearVelocity.y;

        animator.SetFloat("yVelocity", visualYVelocity);
    }
}
