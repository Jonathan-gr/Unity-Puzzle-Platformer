using System.Collections;
using UnityEngine;

public class AarakocraAttackController : MonoBehaviour
{
    private Animator animator;

    [Header("Attack Settings")]
    public float attackInterval = 2f;     // Time between attacks
    public float attack1Duration = 1f;
    public float attack2Duration = 2f;

    [Header("Attack Movement")]
    public float attackMoveDistance = 1.5f;
    public float attackMoveVariance = 0.5f;
    public float attackMoveDuration = 0.3f;

    private bool isAttacking = false;

    public GameObject damageAreaPrefab;

    public Transform damageAreaSpawnPointLeft;
    public Transform damageAreaSpawnPointRight;

    private SpriteRenderer sr;



    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            // --- 1. RANDOMIZE THE WAIT TIME ---
            // Instead of waiting exactly 2s, wait between 1.5s and 3.5s
            float nextWait = attackInterval + Random.Range(-0.5f, 1.5f);
            yield return new WaitForSeconds(nextWait);

            if (isAttacking) continue;

            // --- 2. WEIGHTED RANDOM SELECTION ---
            float choice = Random.value;

            if (choice < 0.1f)
            {
                // 10% chance to "Feint" (Wait a bit longer then attack twice)
                yield return new WaitForSeconds(0.5f);
                yield return DoAttackOne();
                yield return DoAttackTwo();
            }
            else if (choice < 0.55f)
            {
                // 45% chance for Attack One
                yield return DoAttackOne();
            }
            else
            {
                // 45% chance for Attack Two
                yield return DoAttackTwo();
            }
        }
    }


    private IEnumerator DoAttackOne()
    {
        isAttacking = true;

        // Randomize how fast the animation plays (0.8x to 1.4x speed)
        float speedMult = Random.Range(0.8f, 1.4f);
        animator.speed = speedMult;

        animator.SetBool("isAttackOne", true);
        yield return StartCoroutine(MoveDuringAttack());

        // Adjust wait time based on the new speed
        yield return new WaitForSeconds(attack1Duration / speedMult);

        animator.SetBool("isAttackOne", false);
        animator.speed = 1f; // Reset speed
        isAttacking = false;
    }


    private IEnumerator DoAttackTwo()
    {
        isAttacking = true;
        animator.SetBool("isAttackTwo", true);     // Make sure this parameter exists
        StartCoroutine(MoveDuringAttack());
        yield return new WaitForSeconds(attack2Duration);

        animator.SetBool("isAttackTwo", false);
        isAttacking = false;
    }

    public void SpawnDamageArea()
    {
        // Determine facing direction
        bool facingLeft = sr.flipX;

        Transform spawnPoint = null;

        if (facingLeft)
            spawnPoint = damageAreaSpawnPointLeft;
        else
            spawnPoint = damageAreaSpawnPointRight;

        // Fallback safety
        if (spawnPoint == null)
            spawnPoint = transform;

        Instantiate(damageAreaPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    IEnumerator MoveDuringAttack()
    {
        float direction = sr.flipX ? -1f : 1f;

        float distance = attackMoveDistance + Random.Range(-attackMoveVariance, attackMoveVariance);

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(direction * distance, 0, 0);

        float time = 0f;

        while (time < attackMoveDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time / attackMoveDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }
}