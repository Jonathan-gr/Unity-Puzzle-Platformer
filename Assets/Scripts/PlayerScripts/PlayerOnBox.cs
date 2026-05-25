using UnityEngine;
using System.Collections.Generic;

public class PlayerOnBox : MonoBehaviour
{
    public List<string> allowedTags = new List<string> { "MovingPlatform", "MoveableBox", "Lizard" };

    private HashSet<string> tagSet;
    private Rigidbody2D playerRb;
    private Rigidbody2D currentPlatform;
    private Vector2 lastPlatformPos;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        tagSet = new HashSet<string>(allowedTags);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (tagSet.Contains(collision.gameObject.tag))
        {

            // Ensure we are hitting the top of the box
            if (collision.GetContact(0).normal.y > 0.7f)
            {

                currentPlatform = collision.rigidbody;
                if (currentPlatform != null)
                    lastPlatformPos = currentPlatform.position;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (currentPlatform != null && collision.rigidbody == currentPlatform)
        {
            currentPlatform = null;
        }
    }

    void FixedUpdate()
    {
        if (currentPlatform != null)
        {
            Vector2 currentPlatformPos = currentPlatform.position;
            Vector2 delta = currentPlatformPos - lastPlatformPos;

            // Move player
            playerRb.position += delta;

            // Sync for next frame
            lastPlatformPos = currentPlatformPos;
        }
    }
}
