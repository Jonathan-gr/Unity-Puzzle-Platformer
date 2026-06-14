using System.Collections; // Needed for Coroutines
using System.Collections.Generic;
using UnityEngine;

public class ButtonCollision : MonoBehaviour
{
    public Animator animator;

    [Header("Listeners")]
    public MonoBehaviour[] listeners;

    [Header("Timer Settings")]
    public float bulletPressDuration = 2f; // How long to stay pressed
    private Coroutine timerCoroutine;

    private HashSet<GameObject> pressers = new HashSet<GameObject>();
    public List<string> targetTags = new List<string> { "Player", "Lizard", "MoveableBox", "LazerBullet" };

    [Header("Press Once Settings")]
    public bool pressOnce = false;
    private bool hasBeenPressed = false;

    void Start()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    // --- 1. HANDLE TRIGGERS (Bullets) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LazerBullet"))
        {
            StartPressTimer();
        }
    }

    private void StartPressTimer()
    {
        // If the button is already counting down, stop it and restart
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(ButtonTimerRoutine());
    }

    private IEnumerator ButtonTimerRoutine()
    {
        SetButtonState(true);
        yield return new WaitForSeconds(bulletPressDuration);

        if (pressers.Count == 0)
        {
            SetButtonState(false); // timer button always releases
        }

        timerCoroutine = null;
    }

    // --- 2. HANDLE PHYSICAL COLLISIONS (Player/Box) ---
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsValidPresser(collision.gameObject)) return;

        if (pressers.Add(collision.gameObject) && pressers.Count == 1)
        {
            // If a timer was running, stop it so the physical object takes control
            if (timerCoroutine != null) { StopCoroutine(timerCoroutine); timerCoroutine = null; }
            SetButtonState(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsValidPresser(collision.gameObject)) return;

        if (pressers.Remove(collision.gameObject) && pressers.Count == 0)
        {
            SetButtonState(false);
        }
    }

    // --- 3. HELPER METHODS ---
    void SetButtonState(bool isDown)
    {
        if (pressOnce && hasBeenPressed)
        {
            if (!isDown) return; // NEVER release a pressOnce button
        }

        if (pressOnce && isDown) hasBeenPressed = true;

        animator.SetBool("ButtonPushedDown", isDown);
        if (isDown)
            NotifyPressed();
        else
            NotifyReleased();
    }

    bool IsValidPresser(GameObject obj)
    {
        return targetTags.Contains(obj.tag);
    }

    // In ButtonCollision
    void NotifyPressed()
    {
        foreach (var mb in listeners)
            if (mb is IButtonListener listener) listener.OnButtonPressed(this); // pass this
    }

    void NotifyReleased()
    {
        foreach (var mb in listeners)
            if (mb is IButtonListener listener) listener.OnButtonReleased(this); // pass this
    }
}
