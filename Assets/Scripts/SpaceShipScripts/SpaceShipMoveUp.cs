using UnityEngine;

public class SpaceShipMoveUp : MonoBehaviour
{
    public bool isGoingUp = false;
    public float flightSpeed = 1f;
    public float coolDownSpeed = 0.1f;
    public bool isCanFlySpaceship = false;
    public float horizontalSpeed = 1f;
    public float acceleration = 10f;

    [Header("Tilt Settings")]
    public float maxTiltAngle = 25f;     // How much the ship tilts (in degrees)
    public float tiltSpeed = 8f;
    private float targetTilt = 0f;
    private Rigidbody2D rb;
    private Animator animator;



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



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Vertical movement (your existing code)
        if (isGoingUp && !isCanFlySpaceship)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flightSpeed);
            // Horizontal Movement
        }
        else
        {
            if (isCanFlySpaceship && isGoingUp)
            {
                float horizontalInput = 0f;

                if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
                if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
                if (Input.GetKey(KeyCode.W))
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, flightSpeed);
                }
                else
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, coolDownSpeed);
                }

                // Smooth horizontal velocity
                Vector2 targetVelocity = new Vector2(horizontalInput * horizontalSpeed, rb.linearVelocity.y);
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

                // Calculate target tilt
                targetTilt = -horizontalInput * maxTiltAngle;   // Negative because left should tilt left
            }
            // Smoothly rotate the ship to target tilt
            float currentZ = transform.rotation.eulerAngles.z;
            if (currentZ > 180) currentZ -= 360;   // Fix for negative angles

            float newZ = Mathf.LerpAngle(currentZ, targetTilt, tiltSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newZ);
        }


    }
}