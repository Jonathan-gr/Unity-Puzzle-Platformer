using System.Collections.Generic;
using UnityEngine;

public class ButtonCollision : MonoBehaviour
{
    public Animator animator;

    [Header("Listeners")]
    public MonoBehaviour[] listeners;

    private HashSet<GameObject> pressers = new HashSet<GameObject>();

    void Start()
    {
        if (!animator)
            animator = GetComponent<Animator>();
    }

    //on collision

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsValidPresser(collision.gameObject))
            return;

        // Add returns false if already present
        bool wasAdded = pressers.Add(collision.gameObject);

        if (wasAdded && pressers.Count == 1)
        {
            animator.SetBool("ButtonPushedDown", true);
            NotifyPressed();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsValidPresser(collision.gameObject))
            return;

        bool wasRemoved = pressers.Remove(collision.gameObject);

        if (wasRemoved && pressers.Count == 0)
        {
            animator.SetBool("ButtonPushedDown", false);
            NotifyReleased();
        }
    }

    bool IsValidPresser(GameObject obj)
    {
        return obj.CompareTag("Player") || obj.CompareTag("Lizard") || obj.CompareTag("MoveableBox");
    }

    void NotifyPressed()
    {
        foreach (var mb in listeners)
        {
            if (mb is IButtonListener listener)
                listener.OnButtonPressed();
        }
    }

    void NotifyReleased()
    {
        foreach (var mb in listeners)
        {
            if (mb is IButtonListener listener)
                listener.OnButtonReleased();
        }
    }
}
