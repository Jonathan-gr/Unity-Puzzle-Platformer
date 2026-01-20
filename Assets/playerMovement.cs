using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMoveable
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 8;

    [Header("Ground Detection")]
    public Transform groundCheck;
    [SerializeField] private LayerMask combinedGroundMask;

    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private float horizontalInput;
    public bool isGrounded;
    private bool shouldJump;

    private Animator animator;

    public Vector2 boxSize = new Vector2(0.5f, 0.1f); // Width should be slightly narrower than your player
    public float castDistance = 0.1f;                // How far down to look

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

    }

    private void Update()
    {

        // 1. Input Management
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            shouldJump = true;
        }

        // 2. Flip character based on direction

        Vector3 scale = transform.localScale;
        if (horizontalInput > 0)
            scale.x = Mathf.Abs(scale.x);
        else if (horizontalInput < 0)
            scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;

        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {

        // 3. Ground Check
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = CheckIsGrounded();
        // 4. Horizontal Movement

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);


        // 5. Jump Physics
        if (shouldJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            shouldJump = false;


            // SoundManager.Instance.PlaySFX(jumpSound, jumpVolume);
        }



    }
    public bool CheckIsGrounded()
    {
        // One cast detects both layers at once
        RaycastHit2D hit = Physics2D.BoxCast(groundCheck.position, boxSize, 0f, Vector2.down, castDistance, combinedGroundMask);
        return hit.collider != null;
    }

    private void UpdateAnimatorParameters()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", horizontalInput != 0);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }


}




