using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Face Player Settings")]
    public Transform player;
    public bool shouldFacePlayer = true;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Auto find player if not assigned
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (shouldFacePlayer && player != null)
        {
            FacePlayer();
        }
    }

    void FacePlayer()
    {
        // Check if player is to the left or right of the boss
        if (player.position.x > transform.position.x)
        {
            // Player is on the LEFT → Face Left
            spriteRenderer.flipX = true;
        }
        else
        {
            // Player is on the RIGHT → Face Right
            spriteRenderer.flipX = false;
        }
    }
}