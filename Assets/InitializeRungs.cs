using System.Collections;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private SpriteRenderer[] rungRenderers;

    // 🟢 This exposes a time slot in the inspector (e.g., 0.2 seconds per rung)
    [SerializeField] private float timeBetweenRungs = 0.5f;

    void Start()
    {
        // Get all SpriteRenderers from child objects
        rungRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Disable all rungs at the beginning
        foreach (var renderer in rungRenderers)
        {
            renderer.enabled = false;
        }

        // Enable only the first rung
        if (rungRenderers.Length > 0)
        {
            rungRenderers[0].enabled = true;

            // 🟢 Start the timer loop to enable the rest!
            StartCoroutine(RevealLadderRoutine());
        }
    }

    // 🟢 This function handles the waiting logic
    private IEnumerator RevealLadderRoutine()
    {
        // Start at index 1 because index 0 is already turned on
        for (int i = 1; i < rungRenderers.Length; i++)
        {
            // Wait for the specific amount of seconds
            yield return new WaitForSeconds(timeBetweenRungs);

            // Turn on the current rung
            rungRenderers[i].enabled = true;
        }
    }
}