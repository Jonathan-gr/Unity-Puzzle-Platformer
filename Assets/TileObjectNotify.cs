using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TileObjectNotify : MonoBehaviour, IButtonListener
{
    [SerializeField] private List<GameObject> tilesToDestroyInOrder = new List<GameObject>();
    private int currentIndex = 0;
    public float fallSpeed = 2.4f;
    public float mass = 10f;

    // Using Hashtable as requested to store rigidbodies of falling tiles
    private Hashtable fallingRigidBodies = new Hashtable();

    public void OnButtonPressed()
    {
        if (currentIndex >= tilesToDestroyInOrder.Count)
        {
            Debug.Log("No more tiles to fall");
            return;
        }
        Debug.Log("1");
        GameObject tile = tilesToDestroyInOrder[currentIndex];
        if (tile != null)
        {
            Debug.Log("2");
            Rigidbody2D rb = tile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Debug.Log("3");
                // 1. Make the tile fall by switching to Dynamic
                rb.bodyType = RigidbodyType2D.Dynamic;



                // 2. Add it to your hashtable (using index as key)
                if (!fallingRigidBodies.ContainsKey(currentIndex))
                {
                    fallingRigidBodies.Add(currentIndex, rb);
                }
            }
        }

        currentIndex++;
    }

    public void OnButtonReleased()
    {
        Debug.Log("Button released");
    }

    // FixedUpdate is not strictly needed for gravity, 
    // but if you want to add horizontal force while they fall:
    void FixedUpdate()
    {
        foreach (DictionaryEntry entry in fallingRigidBodies)
        {
            Rigidbody2D rb = (Rigidbody2D)entry.Value;
            // Optional: add a slight horizontal drift or logic here
            // rb.linearVelocity = new Vector2(1f, rb.linearVelocity.y); 
        }
    }
}
