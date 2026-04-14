using UnityEngine;

public class PlayerOutOfBounds : MonoBehaviour
{

    public GameUIManager uiManager;
    public float minY;
    public float maxY = Mathf.Infinity;
    public float minX = -Mathf.Infinity;
    public float maxX = Mathf.Infinity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < minY || transform.position.y > maxY || transform.position.x < minX || transform.position.x > maxX)
        {
            Debug.Log(transform.position);
            uiManager.TryShowLose();
        }
    }
}
