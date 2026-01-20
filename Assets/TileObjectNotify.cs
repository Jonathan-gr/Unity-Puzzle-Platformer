using UnityEngine;
using System.Collections.Generic;

public class TileObjectNotify : MonoBehaviour, IButtonListener
{
    [SerializeField] private List<GameObject> tilesToDestroyInOrder = new List<GameObject>();
    private int currentIndex = 0;

    public void OnButtonPressed()
    {
        if (currentIndex >= tilesToDestroyInOrder.Count)
        {
            Debug.Log("No more tiles to destroy");
            return;
        }

        GameObject tile = tilesToDestroyInOrder[currentIndex];
        if (tile != null)
        {
            Destroy(tile);
            Debug.Log($"Destroyed tile #{currentIndex + 1}");
        }

        currentIndex++;
    }

    public void OnButtonReleased()
    {
        // still optional / for sound/feedback/whatever
        Debug.Log("Button released");
    }
}