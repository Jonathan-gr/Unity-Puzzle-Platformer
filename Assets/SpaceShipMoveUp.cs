using UnityEngine;
using UnityEngine.U2D.IK;

public class SpaceShipMoveUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool isGoingUp = false;

    public float flightSpeed = 1f;
    private Rigidbody2D rb;
    private Animator animator;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                isGoingUp = true;
                animator.SetBool("isGoingUp", true);
                collision.gameObject.SetActive(false);
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
