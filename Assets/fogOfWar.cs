using System.Collections;
using UnityEngine;

public class RevealZone : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;

    private SpriteRenderer fogRenderer;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        fogRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartFade(0f); // fade out (reveal)
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartFade(1f); // fade back in (hide)
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeTo(targetAlpha));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        Color c = fogRenderer.color;
        float startAlpha = c.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            fogRenderer.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        fogRenderer.color = c;
    }
}