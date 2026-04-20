using UnityEngine;

public class AarakocraMovement : MonoBehaviour
{
    [Header("Chasing Settings")]
    public Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 3f; // Boss stops this far from player

    [Header("Hover Settings")]
    public bool enableHover = true;
    public float hoverSpeed = 2f;   // How fast it bobs up/down
    public float hoverAmount = 0.5f; // How high it bobs

    private float startY;

    void Start()
    {
        // Save the starting Y position for the hover effect
        startY = transform.position.y;
    }

    void Update()
    {
        if (player != null)
        {
            HandleMovement();
        }

        if (enableHover)
        {
            ApplyHover();
        }
    }

    void HandleMovement()
    {
        // 1. Calculate distance to player
        float distance = Vector2.Distance(transform.position, player.position);

        // 2. Move towards player only if further than stopDistance
        if (distance > stopDistance)
        {
            // Move only on the X axis to keep it from "diving" into the ground
            Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    void ApplyHover()
    {
        // Use Sin wave to calculate a smooth up/down offset
        float newY = startY + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
