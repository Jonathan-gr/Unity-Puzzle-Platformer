using UnityEngine;
public class PlayerOnBox : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Rigidbody2D boxRb;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("MoveableBox") || !collision.gameObject.CompareTag("FallingTIle"))
            return;

        ContactPoint2D contact = collision.GetContact(0);

        // Player standing on top
        if (contact.normal.y > 0.5f)
        {
            boxRb = collision.rigidbody;
        }
    }

    void FixedUpdate()
    {
        if (boxRb)
        {
            playerRb.linearVelocity += boxRb.linearVelocity;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody == boxRb)
        {
            boxRb = null;
        }
    }
}
