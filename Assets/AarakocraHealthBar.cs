using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

public class AarakocraHealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;

    public void AarakocraHit(float damage)
    {
        slider.value -= damage;

        if (slider.value <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        // 1. Hide the health bar immediately
        if (slider != null) slider.gameObject.SetActive(false);

        // 2. Disable movement scripts and colliders so he doesn't keep attacking while dying
        if (TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;

        // Disable your movement script (assuming it's called AarakocraMovement)
        if (TryGetComponent<AarakocraMovement>(out AarakocraMovement mov)) mov.enabled = false;

        // 3. Start the death animation
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation()
    {
        float duration = 1.5f; // How long the death takes
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Vector3 fallTarget = startPos + new Vector3(0, -5, 0); // Falls 5 units down

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            // Move Down
            transform.position = Vector3.Lerp(startPos, fallTarget, percent);

            // Rotate (Spins 360 degrees)
            transform.Rotate(0, 0, 500 * Time.deltaTime);

            // Optional: Fade out (Requires SpriteRenderer)
            if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1, 0, percent);
                sr.color = c;
            }

            yield return null; // Wait for the next frame
        }

        // 4. Finally destroy the object
        Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("aarakocra started");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
