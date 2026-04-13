using UnityEngine;

public class PlayerOutOfBounds : MonoBehaviour
{

    public GameObject LoseUi;
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
        if (transform.position.y < minY || transform.position.y > maxY || transform.position.x < minX || transform.position.x > maxY)
        {
            Debug.Log(transform.position);
            LoseUi.SetActive(true);
        }
    }
}
