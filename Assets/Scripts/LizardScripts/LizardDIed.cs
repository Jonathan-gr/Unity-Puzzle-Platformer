using UnityEngine;

public class LizardDIed : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpriteRenderer sr;
    public Rigidbody2D rb;
    public float upForce = 1f;      // vertical velocity
    public float sideForce = 1.5f;    // horizontal velocity
    private Vector2 startPos;

    public bool goRight = true;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        sr.flipY = true;
    }
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        Vector2 initialVelocity = new Vector2(goRight ? sideForce : -sideForce, upForce);
        rb.linearVelocity = initialVelocity;

        Destroy(gameObject, 3f); // auto-cleanup
    }


}
