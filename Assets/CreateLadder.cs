using UnityEngine;

public class CreateLadder : MonoBehaviour
{
    // 1. This exposes a slot in the Unity Inspector. 
    // Drag your Ladder Prefab from your project folder into this slot!
    [SerializeField] private GameObject ladderPrefab;

    public float offsetX = -1f;
    public float offsexY = 4.5f;
    // 2. This built-in Unity function runs automatically when something hits the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("!");
        // Optional: Check if the object touching the trigger is the Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("2");
            Vector3 offset = new Vector3(offsetX, offsexY, 0f);
            Instantiate(ladderPrefab, transform.position + offset, Quaternion.identity);

            // Optional: Destroy this trigger so it doesn't spawn infinite ladders
            Destroy(gameObject);
        }
    }
}