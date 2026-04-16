using UnityEditor;
using UnityEngine;

public class GemHitByLazer : MonoBehaviour
{

    private Animator animator;
    public bool hit = false;

    private BoxCollider2D boxCollider2D;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;


    }
    public void gemHitByLazer()
    {
        hit = !hit;

        //rb.gravityScale = (rb.gravityScale == 0) ? defaultGravity : 0f;
        animator.SetBool("isHit", hit);

        if (hit)
        {
            boxCollider2D.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
        else
        {
            boxCollider2D.isTrigger = true;
            // Converts "Default" to its layer index (0)
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        Debug.Log("gem hit");

    }
}
