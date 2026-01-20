using UnityEngine;
using System.Collections;

public class MovingWall : MonoBehaviour, IButtonListener
{
    public Vector3 moveOffset = new Vector3(0, 3f, 0);
    public float moveSpeed = 0.5f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Coroutine moveRoutine;

    public int buttonsRequired = 2;
    private int pressCount = 0;

    [Header("Sound")]
    public AudioClip openSesame;
    public float openDoorVolume = 0.2f;
    private AudioSource activeLoop;


    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + moveOffset;
    }

    public void OnButtonPressed()
    {
        pressCount++;

        if (pressCount == buttonsRequired)
        {
            MoveTo(targetPos);
        }

    }

    public void OnButtonReleased()
    {
        pressCount--;

        if (pressCount == buttonsRequired - 1)
            MoveTo(startPos);
    }

    void MoveTo(Vector3 target)
    {
        if (moveRoutine != null)

        {
            activeLoop.Stop();
            Destroy(activeLoop.gameObject);
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
    }

}
