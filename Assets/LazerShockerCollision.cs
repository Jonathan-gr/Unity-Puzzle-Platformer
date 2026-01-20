using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class LazerCollision : MonoBehaviour
{

    [Header("Detection Settings")]
    public string lizardTag = "Lizard";
    public string[] ignoreTags = { "Ladder" };
    void Start()
    {
        // Automatically destroy after 3 seconds so you don't clutter the scene
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(lizardTag))
        {
            LizardMovement lizard =
                collision.GetComponentInParent<LizardMovement>();

            if (lizard != null)
            {
                lizard.LizardHit();
            }


        }
        if (!ignoreTags.Contains(collision.gameObject.tag))
        {
            Destroy(gameObject);
        }

    }


}