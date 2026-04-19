using System.Collections;
using UnityEngine;

public class AarakocraAttackController : MonoBehaviour
{
    private Animator animator;

    [Header("Attack Settings")]
    public float attackInterval = 5f;     // Time between attacks
    public float attack1Duration = 2f;
    public float attack2Duration = 2.5f;

    private bool isAttacking = false;

    public GameObject damageAreaPrefab;

    public Transform damageAreaSpawnPoint1;
    public Transform damageAreaSpawnPoint2;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            if (isAttacking) continue;   // Safety: don't start new attack while already attacking

            // Randomly choose attack 1 or 2 (you can change logic later)
            if (Random.value > 0.5f)
                StartCoroutine(DoAttackOne());
            else
                StartCoroutine(DoAttackTwo());
        }
    }

    private IEnumerator DoAttackOne()
    {
        isAttacking = true;
        animator.SetBool("isAttackOne", true);

        yield return new WaitForSeconds(attack1Duration);

        animator.SetBool("isAttackOne", false);
        isAttacking = false;
    }

    private IEnumerator DoAttackTwo()
    {
        isAttacking = true;
        animator.SetBool("isAttackTwo", true);     // Make sure this parameter exists

        yield return new WaitForSeconds(attack2Duration);

        animator.SetBool("isAttackTwo", false);
        isAttacking = false;
    }

    private void SpawnDamageArea1()
    {
        Transform spawnPoint = damageAreaSpawnPoint1 != null ? damageAreaSpawnPoint1 : transform;

        Instantiate(damageAreaPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void SpawnDamageArea2()
    {
        Transform spawnPoint = damageAreaSpawnPoint2 != null ? damageAreaSpawnPoint2 : transform;

        Instantiate(damageAreaPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}