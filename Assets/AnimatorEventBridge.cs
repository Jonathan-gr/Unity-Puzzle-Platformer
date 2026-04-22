using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private AarakocraAttackController parent;

    void Awake()
    {
        parent = GetComponentInParent<AarakocraAttackController>();
    }

    // This is what the animation event will call
    public void SpawnDamageArea()
    {
        if (parent != null)
            parent.SpawnDamageArea();
    }
}