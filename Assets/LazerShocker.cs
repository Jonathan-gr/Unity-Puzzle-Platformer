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
        // 1. Better direction detection
        // If the player is rotated 180, we need to know if they are visually facing 'Left' or 'Right'
        // A simple way is to check the localScale.x against the rotation
        bool isUpsideDown = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, 180f)) < 10f;

        // Calculate final direction:
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        // If upside down, the scale logic is inverted, so we flip it back
        if (isUpsideDown) direction *= -1;

        // 2. Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // 3. Set Velocity
        // We set y to 0 to keep the bullet horizontal
        rb.linearVelocity = new Vector2(direction * bulletSpeed, 0);

        // 4. Flip the bullet sprite to match
        Vector3 bScale = bullet.transform.localScale;
        bScale.x = direction > 0 ? Mathf.Abs(bScale.x) : -Mathf.Abs(bScale.x);
        bullet.transform.localScale = bScale;
    }


    void ShootSoundEffect()
    {
        //SoundManager.Instance.PlaySFX(lazerFired, 0.3f);
    }
}
