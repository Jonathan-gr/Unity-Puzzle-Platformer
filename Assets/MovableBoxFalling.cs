
using UnityEngine;

public class MovableBoxFalling : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;


    public GameObject LizardDiedPrefab;
    private float originalGravity;
    public float killVelocityThreshold = 5.0f; // How fast must it fall to kill?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }




    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Lizard") && !collision.gameObject.CompareTag("Player"))
            return;

        // 1. Get the first contact point
        ContactPoint2D contact = collision.GetContact(0);

        // 2. Check if the box hit the lizard FROM ABOVE 
        // (Normal points UP from the lizard's perspective)

        if (contact.normal.y > 0.5f)
        {
            // 3. Check the impact speed (How hard did they hit?)
            // relativeVelocity.y tells us the vertical speed difference
            if (Mathf.Abs(collision.relativeVelocity.y) > killVelocityThreshold)
            {
                if (collision.gameObject.TryGetComponent(out LizardMovement lizard))
                {
                    if (lizard.isShocked) return;
                }

                Instantiate(LizardDiedPrefab, collision.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);

            }

        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.gravityScale = originalGravity;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckIfPlayerBoxCollision(collision);
        }
    }

    void CheckIfPlayerBoxCollision(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        {

            // Check normal is pointing roughly up
            if (contact.normal.y < 0f)
            {
                rb.gravityScale = originalGravity + 2;

                return;
            }

        }

        // Player not on top, reset gravity
        rb.gravityScale = originalGravity;
    }





}
