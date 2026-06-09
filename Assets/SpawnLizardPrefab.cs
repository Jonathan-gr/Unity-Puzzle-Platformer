using UnityEngine;

public class SpawnLizardPrefab : MonoBehaviour, IButtonListener
{
    [Header("Spawn Settings")]
    public GameObject lizardPrefab;
    public Transform spawnX;
    public Transform spawnY;
    public float offsetX = 0;
    public float offsetY = 0;

    private GameObject currentLizard;
    public void OnButtonPressed()
    {
        if (currentLizard != null)
            Destroy(currentLizard);
        Vector3 pos = new Vector3(spawnX.position.x + offsetX, spawnY.position.y + offsetY, 0f);
        currentLizard = Instantiate(lizardPrefab, pos, Quaternion.identity);
    }

    public void OnButtonReleased() { }
}