using UnityEngine;
using UnityEngine.UI;

public class AarakocraFacesPlayer : MonoBehaviour
{
    [Header("Face Player Settings")]
    public Transform player;
    public bool shouldFacePlayer = true;



    public Transform ParentvisualsContainer;

    private SpriteRenderer sr;

    void Awake()
    {
        // Get the SpriteRenderer from the visuals object
        sr = GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError("No SpriteRenderer found on ParentvisualsContainer!");
        }
    }

    void Update()
    {
        if (shouldFacePlayer)
        {
            FacePlayer();
        }
    }

    void FacePlayer()
    {
        // Flip sprite based on player position
        sr.flipX = player.position.x < transform.position.x;
    }
}