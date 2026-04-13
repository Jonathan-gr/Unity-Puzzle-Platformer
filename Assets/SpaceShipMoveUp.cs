using UnityEngine;

public class SpaceShipMoveUp : MonoBehaviour
{
    public bool isGoingUp = false;
    public float flightSpeed = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    public GameObject WinUi;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {

                isGoingUp = true;
                animator.SetBool("isGoingUp", true);
            }
        }
    }



    void FixedUpdate()
    {
        if (isGoingUp)
        {

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flightSpeed);
        }
    }
}