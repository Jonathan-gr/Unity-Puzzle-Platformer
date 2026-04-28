using UnityEngine;

public class ConsolePrefabManager : MonoBehaviour
{
    [Header("Movement Toggles")]
    public bool moveUp = true;
    public bool moveLeft = false;
    public bool moveRight = false;

    [Header("Speeds")]
    public float upSpeed = 2f;
    public float horizontalSpeed = 2f;




    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (moveUp) direction += Vector3.up;
        if (moveLeft) direction += Vector3.left;
        if (moveRight) direction += Vector3.right;

        // Apply each axis with its own speed
        Vector3 movement = new Vector3(
            direction.x * horizontalSpeed,
            direction.y * upSpeed,
            0f
        ) * Time.deltaTime;

        transform.position += movement;
    }
}