using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class LazerCollision : MonoBehaviour
{

    [Header("Detection Settings")]
    public string lizardTag = "Lizard";
    public string gemTag = "Gem";
    public string aarakocraTag = "Aarakocra";
    public string[] ignoreTags = { "Ladder", "SpaceShip" };
    public float lazerShockerDamage = 0.1f;
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

        else if (collision.CompareTag(gemTag))
        {
            GemHitByLazer gem =
                collision.GetComponentInParent<GemHitByLazer>();

            if (gem != null)
            {
                gem.gemHitByLazer();
            }


        }
        else if (collision.CompareTag(aarakocraTag))
        {
            AarakocraHealthBar aarakocra = collision.GetComponentInParent<AarakocraHealthBar>();
            if (aarakocra != null)
            {
                aarakocra.AarakocraHit(lazerShockerDamage);

            }

        }


        if (!ignoreTags.Contains(collision.gameObject.tag))
        {
            Destroy(gameObject);
        }

    }


}