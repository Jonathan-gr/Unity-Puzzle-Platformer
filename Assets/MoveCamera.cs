using UnityEngine;

public class CameraLookScroll : MonoBehaviour
{
    [Header("Movement Settings")]

    public bool allowLookUp = false;
    public bool allowLookDown = false;
    public float offsetAmount = 2.0f;
    public float smoothSpeed = 3.0f;
    public float activationDelay = 1.0f; // Time in seconds before moving

    private Vector3 initialLocalPos;
    private float holdTimer = 0f;
    private KeyCode currentKey = KeyCode.None;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        // 1. Detect which key is being held
        KeyCode heldKey = GetHeldKey();

        // 2. Handle the Timer
        if (heldKey != KeyCode.None)
        {
            // If we just started pressing or switched keys, reset timer
            if (heldKey != currentKey)
            {
                holdTimer = 0f;
                currentKey = heldKey;
            }

            holdTimer += Time.deltaTime;
        }
        else
        {
            // Reset everything if no keys are held
            holdTimer = 0f;
            currentKey = KeyCode.None;
        }

        // 3. Determine Target Position
        Vector3 targetPos = initialLocalPos;

        // Only apply offset if the timer exceeds the delay
        if (holdTimer >= activationDelay)
        {
            if (currentKey == KeyCode.DownArrow && allowLookDown)
                targetPos += Vector3.down * offsetAmount;
            else if (currentKey == KeyCode.UpArrow && allowLookUp)
                targetPos += Vector3.up * offsetAmount;
        }

        // 4. Smooth Movement
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }

    // Helper to check for both keys
    private KeyCode GetHeldKey()
    {
        if (Input.GetKey(KeyCode.DownArrow)) return KeyCode.DownArrow;
        if (Input.GetKey(KeyCode.UpArrow)) return KeyCode.UpArrow;
        return KeyCode.None;
    }
}
