using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingWall : MonoBehaviour, IButtonListener
{
    public Vector3 moveOffset = new Vector3(0, 3f, 0);
    public float moveSpeed = 0.5f;

    [Header("Return Settings")]
    public float returnDelay = 0f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Coroutine moveRoutine;

    public int buttonsRequired = 3;

    // Track WHICH buttons are pressed, not how many times
    private System.Collections.Generic.HashSet<MonoBehaviour> activeButtons = new HashSet<MonoBehaviour>();

    [Header("Sound")]
    public AudioClip openSesame;
    public float openDoorVolume = 0.2f;
    private AudioSource activeLoop;

    public MonoBehaviour[] listeners;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + moveOffset;
    }

    public void OnButtonPressed(MonoBehaviour sender)
    {
        activeButtons.Add(sender); // HashSet ignores duplicates automatically
        Debug.Log($"[MovingWall] Button pressed: {sender.gameObject.name} — active: {activeButtons.Count}/{buttonsRequired}");

        if (activeButtons.Count >= buttonsRequired)
        {
            MoveTo(targetPos);
        }
    }

    public void OnButtonReleased(MonoBehaviour sender)
    {
        activeButtons.Remove(sender);
        Debug.Log($"[MovingWall] Button released: {sender.gameObject.name} — active: {activeButtons.Count}/{buttonsRequired}");

        if (activeButtons.Count < buttonsRequired)
        {
            MoveTo(startPos);
        }
    }

    void MoveTo(Vector3 target)
    {
        if (moveRoutine != null)

        {
            if (activeLoop)
            {
                activeLoop.Stop();
                Destroy(activeLoop.gameObject);

            }

            StopCoroutine(moveRoutine);
        }


        moveRoutine = StartCoroutine(MoveRoutine(target));
    }

    IEnumerator MoveRoutine(Vector3 target)
    {

        //activeLoop = SoundManager.Instance.PlayLoopingSFX(openSesame);
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = target;
        moveRoutine = null;
        if (activeLoop != null)
        {
            activeLoop.Stop();
            Destroy(activeLoop.gameObject);
        }

        // If we just reached targetPos, go back to startPos
        if (target == targetPos)
        {

            foreach (var mb in listeners) // you'd need a reference to the buttons
            {
                if (mb is ButtonCollision btn) btn.ResetButton();
            }

            if (returnDelay > 0f)
                yield return new WaitForSeconds(returnDelay);
            moveRoutine = StartCoroutine(MoveRoutine(startPos));
        }
    }

}
