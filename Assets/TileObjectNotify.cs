using UnityEngine;
using System.Collections.Generic;

public class TileObjectNotify : MonoBehaviour, IButtonListener
{
    [SerializeField] private List<GameObject> tilesToMakeFallInOrder = new List<GameObject>();

    private int currentIndex = 0;
    private bool resetNeeded = false;
    private int resetIndex;

    // Store original state for each tile
    private List<Vector3> originalPositions = new List<Vector3>();
    private List<Quaternion> originalRotations = new List<Quaternion>();
    private List<Rigidbody2D> tileRigidbodies = new List<Rigidbody2D>();


    void Start()
    {
        SaveOriginalStates();

    }

    private void SaveOriginalStates()
    {
        foreach (GameObject tile in tilesToMakeFallInOrder)
        {
            if (tile != null)
            {
                originalPositions.Add(tile.transform.position);
                originalRotations.Add(tile.transform.rotation);

                Rigidbody2D rb = tile.GetComponent<Rigidbody2D>();
                tileRigidbodies.Add(rb);

                if (rb != null)
                    rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    public void OnButtonPressed()
    {
        // === SWITCH TO RESET MODE ===
        if (!resetNeeded && currentIndex >= tilesToMakeFallInOrder.Count)
        {
            resetNeeded = true;
            resetIndex = tilesToMakeFallInOrder.Count - 1;

        }

        // === FALL MODE ===
        if (!resetNeeded)
        {
            GameObject tile = tilesToMakeFallInOrder[currentIndex];
            Rigidbody2D rb = tileRigidbodies[currentIndex];

            if (tile == null || rb == null) return;

            rb.bodyType = RigidbodyType2D.Dynamic;

            Debug.Log($"Tile {currentIndex} → FALLING");

            currentIndex++;
            return;
        }

        // === RESET MODE ===
        if (resetIndex >= 0)
        {
            ResetTile(resetIndex);

            Debug.Log($"Tile {resetIndex} → RESET");

            resetIndex--;
        }

        // === DONE RESETTING ===
        if (resetIndex < 0)
        {
            currentIndex = 0;
            resetNeeded = false;

        }
    }

    private void ResetTile(int index)
    {

        Debug.Log("reset here");
        Rigidbody2D rb = tileRigidbodies[index];
        GameObject tile = tilesToMakeFallInOrder[index];

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        tile.transform.position = originalPositions[index];
        tile.transform.rotation = originalRotations[index];
    }

    public void OnButtonReleased() { }
}