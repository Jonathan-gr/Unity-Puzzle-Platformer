using UnityEngine;

public class SpaceShipMoveUp : MonoBehaviour
{
    public bool isGoingUp = false;
    public float flightSpeed = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    public GameObject WinUi;
    private GameObject storedPlayer;

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

                storedPlayer = collision.gameObject;

                isGoingUp = true;
                animator.SetBool("isGoingUp", true);

                storedPlayer.SetActive(false);
            }
        }
    }

    void Update()
    {

        if (storedPlayer != null && Input.GetKeyDown(KeyCode.Space))
        {
            storedPlayer.SetActive(true);

            // Optional: place player back near spaceship
            storedPlayer.transform.position = transform.position + Vector3.up * 1.5f;

            isGoingUp = false;
            animator.SetBool("isGoingUp", false);
            WinUi.SetActive(false);

            storedPlayer = null;
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