using System.Collections;
using UnityEngine;

public class ConsoleCreatesExplostion : MonoBehaviour
{
    public GameObject textCanvas;
    public bool playerInRange = false;
    public GameObject ExplosionPrefab;
    public float initPrefabXOffset = 0f;
    public float initPrefabYOffset = 2f;
    public float timeToDestroy = 1f;
    public float delayTime = 2f;

    // --- New Variables ---
    private bool isLooping = false;
    private Coroutine loopCoroutine;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ToggleExplosions();
        }
    }

    void ToggleExplosions()
    {
        isLooping = !isLooping; // Switch between true and false

        if (isLooping)
        {
            // Start the loop and store a reference to it
            loopCoroutine = StartCoroutine(ExplosionLoop());
        }
        else
        {
            // Stop the loop if it is running
            if (loopCoroutine != null)
            {
                StopCoroutine(loopCoroutine);
            }
        }
    }

    IEnumerator ExplosionLoop()
    {
        // This while loop runs as long as isLooping is true
        while (isLooping)
        {
            yield return new WaitForSeconds(delayTime);
            SpawnExplosion();



        }
    }

    void SpawnExplosion()
    {
        Vector3 offset = new Vector3(initPrefabXOffset, initPrefabYOffset, 0f);
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position + offset, Quaternion.identity);
        Destroy(explosion, timeToDestroy);
    }

    // --- Trigger Logic ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            textCanvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            textCanvas.SetActive(false);
        }
    }
}
