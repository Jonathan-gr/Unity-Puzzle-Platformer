using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardMovement : MonoBehaviour, IMoveable
{
    public float moveSpeed = 2f;


    public Transform visual;

    private Rigidbody2D rb;
    private int direction = 1;

    private bool canMove = true;
    private bool isStunnedFromGun = false;
    public Animator animator;
    public bool isShocked = false;

    private float lastX;
    private float stuckTimer = 0f;
    private const float stuckThreshold = 0.05f;
    private const float minMoveDelta = 0.001f;

    public float pushStrength = 5;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    private bool isKnockedBack = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        lastX = rb.position.x;
    }

    void FixedUpdate()
    {
        if (isStunnedFromGun)
        {
            rb.linearVelocity = Vector2.zero; // gun stun: fully freeze
            return;
        }
        if (!canMove)
        {
            if (!isKnockedBack) // only zero velocity if it's NOT a knockback
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            lastX = rb.position.x;
            stuckTimer = 0f;
            return;
        }

        // Apply movement
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        float deltaX = rb.position.x - lastX;
        bool isTryingToMove = Mathf.Abs(rb.linearVelocity.x) > 0.01f;
        bool isBlocked = Mathf.Abs(deltaX) < minMoveDelta;

        if (isTryingToMove && isBlocked)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= stuckThreshold)
            {
                Flip();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastX = rb.position.x;

        // --- NEW: UPSIDE DOWN VISUAL CORRECTION ---
        UpdateVisualOrientation();
    }

    void UpdateVisualOrientation()
    {

        if (visual == null)
        {
            Debug.Log(visual);
            return;
        }

        // Check the child's rotation instead of root
        bool isUpsideDown = Mathf.Abs(Mathf.DeltaAngle(visual.eulerAngles.z, 180f)) < 15f;

        Vector3 scale = transform.localScale;
        float baseScale = Mathf.Abs(scale.y);

        if (isUpsideDown)
        {
            scale.x = (direction == 1) ? baseScale : -baseScale;
        }
        else
        {
            scale.x = (direction == 1) ? -baseScale : baseScale;
        }

        transform.localScale = scale;
    }


    void Flip()
    {
        direction *= -1;
    }

    public void LizardHit()
    {
        isStunnedFromGun = !isStunnedFromGun;
        isShocked = !isShocked;
        animator.SetBool("isShocked", isShocked);

        if (!isStunnedFromGun)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    public void GetKnockedBack(Vector2 force, float stunDuration)
    {
        StartCoroutine(KnockbackRoutine(force, stunDuration));
    }

    private IEnumerator KnockbackRoutine(Vector2 force, float stunDuration)
    {
        canMove = false; // Stop the movement script from overwriting velocity
        isKnockedBack = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Instead of zeroing velocity, cancel only downward momentum
        if (rb.linearVelocity.y < 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(stunDuration); // Duration of the stun
        canMove = true;
        isKnockedBack = false;
    }
}
