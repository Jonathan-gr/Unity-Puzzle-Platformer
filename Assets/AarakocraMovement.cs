using UnityEngine;

public class AarakocraMovement : MonoBehaviour
{
    [Header("Chasing Settings")]
    public Transform player;
    public float moveSpeed = 3f;
    public float stopDistance = 4f;        // Horizontal stop distance
    public float hoverHeight = 3f;         // How high above player it should hover

    [Header("Hover Settings")]
    public bool enableHover = true;
    public float hoverSpeed = 2f;
    public float hoverAmount = 0.6f;
    public float yUpdateInterval = 2f; // how often to adjust height
    private float yTimer = 0f;
    private float currentTargetY;



    private float startY;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        startY = transform.position.y;
        currentTargetY = transform.position.y;
    }


    void Update()
    {
        if (player == null) return;

        HandleMovement();
        ApplyHover();

    }

    void HandleMovement()
    {
        // Calculate horizontal distance only
        float distanceX = Mathf.Abs(transform.position.x - player.position.x);

        if (distanceX > stopDistance)
        {
            // Move horizontally toward player
            Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    void ApplyHover()
    {
        if (!enableHover) return;

        // Update target Y only every X seconds
        yTimer += Time.deltaTime;

        if (yTimer >= yUpdateInterval)
        {
            currentTargetY = player.position.y + hoverHeight;
            yTimer = 0f;
        }

        // Add sine hover
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;

        float newY = currentTargetY + hoverOffset;

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, newY, Time.deltaTime * 4f),
            transform.position.z
        );
    }


}