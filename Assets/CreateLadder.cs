using UnityEngine;

public class CreateLadder : MonoBehaviour, IButtonListener
{
    [SerializeField] private GameObject ladderPrefab;

    public float offsetX = -1f;
    public float offsexY = 4.5f;

    public void OnButtonPressed()
    {
        Vector3 offset = new Vector3(offsetX, offsexY, 0f);
        Instantiate(ladderPrefab, transform.position + offset, Quaternion.identity);
        //Destroy(gameObject);
    }

    public void OnButtonReleased() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnButtonPressed();
        }
    }
}