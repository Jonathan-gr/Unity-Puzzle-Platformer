using UnityEngine;

public class ConsoleCreatesExplostion : MonoBehaviour
{

    public GameObject textCanvas;
    public bool playerInRange = false;
    public GameObject ExplosionPrefab;
    public float initPrefabXOffset = 0f;
    public float initPrefabYOffset = 2f;
    public float timeToDestroy = 1f;


    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Vector3 offset = new Vector3(initPrefabXOffset, initPrefabYOffset, 0f);
            GameObject explostion = Instantiate(ExplosionPrefab, transform.position + offset, Quaternion.identity);
            Destroy(explostion, timeToDestroy);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        textCanvas.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        textCanvas.SetActive(false);
    }
}
