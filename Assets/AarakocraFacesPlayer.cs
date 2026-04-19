using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    [Header("Face Player Settings")]
    public Transform player;
    public bool shouldFacePlayer = true;

    public Slider slider;

    public Transform ParentvisualsContainer;
    private Vector3 originalScale;

    void Awake()
    {

        originalScale = ParentvisualsContainer.localScale;   // Save original scale

    }

    void Update()
    {
        if (shouldFacePlayer && player != null)
        {
            FacePlayer();
        }
    }

    void FacePlayer()
    {
        if (player.position.x < transform.position.x)
        {
            // Face Left

            ParentvisualsContainer.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            slider.direction = Slider.Direction.RightToLeft;
        }
        else
        {
            // Face Right
            ParentvisualsContainer.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            slider.direction = Slider.Direction.RightToLeft;
        }
    }
}