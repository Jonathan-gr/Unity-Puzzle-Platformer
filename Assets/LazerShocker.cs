using UnityEngine;

public class LazerShocker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 13f;
    // Update is called once per frame

    [Header("Sound")]
    public AudioClip lazerFired;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            animator.SetTrigger("LazerShockerFired");


        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Check the scale of the object this script is attached to (the player)
        // If scale.x is positive, shoot right (1). If negative, shoot left (-1).
        float direction = transform.lossyScale.x > 0 ? 1f : -1f;

        rb.linearVelocity = new Vector2(direction * bulletSpeed, 0);

        // Optional: Flip the bullet sprite to match direction

        Vector3 scale = bullet.transform.localScale;
        if (direction > 0)
            scale.x = Mathf.Abs(scale.x);
        else if (direction < 0)
            scale.x = -Mathf.Abs(scale.x);
        bullet.transform.localScale = scale;
    }

    void ShootSoundEffect()
    {
        //SoundManager.Instance.PlaySFX(lazerFired, 0.3f);
    }
}
