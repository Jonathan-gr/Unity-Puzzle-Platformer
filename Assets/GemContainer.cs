using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GemContainer : MonoBehaviour
{
    [Header("Settings")]
    public int maxGems = 3;

    public int currentGems = 0;

    [Header("UI References")]
    public GameObject gemPrefab;
    public Transform gemContainer;

    private List<Image> gems = new List<Image>();

    void Start()
    {
        CreateGems();
        UpdateGems();        // Show current amount
    }

    void CreateGems()
    {
        // Clear existing
        foreach (Transform child in gemContainer)
            Destroy(child.gameObject);

        gems.Clear();

        if (gemPrefab == null)
        {
            Debug.LogError("Gem Prefab is NOT assigned in Inspector!", this);
            return;
        }

        for (int i = 0; i < maxGems; i++)
        {
            GameObject gemObj = Instantiate(gemPrefab, gemContainer);

            Image img = gemObj.GetComponent<Image>();

            if (img == null)
            {
                Debug.LogError("Gem Prefab does not have an Image component!", gemPrefab);
                Destroy(gemObj); // Clean up
                continue;
            }

            gems.Add(img);
        }
    }

    public void UpdateGems(int currentAmount)   // ← Better: accept parameter
    {
        for (int i = 0; i < gems.Count; i++)
        {
            if (gems[i] != null)
            {
                gems[i].enabled = i < currentAmount;
            }
        }
    }

    // Optional: overload for backward compatibility
    public void UpdateGems()
    {
        UpdateGems(currentGems); // default to full
    }

    public void AddGem()
    {
        if (currentGems < maxGems)
        {
            currentGems++;
            UpdateGems(currentGems);
        }
    }
}