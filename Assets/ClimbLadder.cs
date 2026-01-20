using UnityEngine;
public class PlayerClimb : MonoBehaviour
{
    public LayerMask ladderLayer;
    private BoxCollider2D myCollider;
    private Rigidbody2D rb;
    private Animator animator;

    public bool isClimbingLadder = false;
    public bool isHoldingOntoLadder = false;

    public float ClimbSpeed = 4.5f;

    private float defaultGravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        defaultGravity = rb.gravityScale;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isTouchingLadder = myCollider.IsTouchingLayers(ladderLayer);
        bool ladderInput = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow);
        bool horizontalInput = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        // If we LEFT the ladder this frame
        if (!isTouchingLadder && IsOnLadder)
        {
            PlayerLeavingLadder();
            return;
        }

        if (!isTouchingLadder)
            return;

        // Now we KNOW we're touching the ladder
        if (horizontalInput && IsOnLadder)
        {
            PlayerLeavingLadder();
            return;
        }

        if (ladderInput)
        {
            rb.gravityScale = 0;
            int direction = Input.GetKey(KeyCode.UpArrow) ? 1 : -1;
            PlayerClimbingLadder(direction);
        }
        else if (isClimbingLadder)
        {
            isHoldingOntoLadder = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            animator.SetBool("isHoldingOntoLadder", true);
            animator.SetBool("isClimbingLadder", false);
            isClimbingLadder = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerLeavingLadder();
        }
    }

    public bool IsOnLadder => isClimbingLadder || isHoldingOntoLadder;



    void snapToLadder()
    {


        Collider2D ladderCol = Physics2D.OverlapBox(
            myCollider.bounds.center, myCollider.bounds.size, 0f, ladderLayer);

        if (ladderCol != null)
        {
            float targetX = ladderCol.bounds.center.x;
            float snapSpeed = 10f;
            float newX = Mathf.MoveTowards(transform.position.x, targetX, snapSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    void PlayerClimbingLadder(int direction)
    {
        isClimbingLadder = true;
        isHoldingOntoLadder = false;

        animator.SetBool("isClimbingLadder", isClimbingLadder);
        animator.SetBool("isHoldingOntoLadder", false);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, direction * ClimbSpeed);
        snapToLadder();

    }
    public void PlayerLeavingLadder()
    {
        rb.gravityScale = defaultGravity;
        isClimbingLadder = false;
        isHoldingOntoLadder = false;
        animator.SetBool("isClimbingLadder", isClimbingLadder);
        animator.SetBool("isHoldingOntoLadder", false);


    }


}
