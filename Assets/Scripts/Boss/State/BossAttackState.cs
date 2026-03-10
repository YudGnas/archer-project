using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttackState : BossBaseState
{

    private float attackTimer;
    private float losePlayerTime;
    private bool isAttacking;

    private float phase1Cooldown = 4f;
    private float phase2Cooldown = 2f;

    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private int bulletCount => boss.IsPhase2() ? 60 : 20;
    [SerializeField] private float spreadAngle = 5f;
    [SerializeField] private float loseDuration = 5f;

    public override void Enter()
    {
        losePlayerTime = 0f;
        attackTimer = 0f;
        isAttacking = false;

        boss.Agent.isStopped = true;
        if (boss.IsPhase2())
        {
            boss.Agent.speed = 6f;
            boss.Agent.acceleration = 20f;
        }


        
    }

    public override void Exit()
    {
        boss.Agent.isStopped = false;
    }

    public override void Perform()
    {
        if (boss.Player == null) return;
        if (isAttacking) return;

        attackTimer += Time.deltaTime;

        float cooldown = boss.IsPhase2() ? phase2Cooldown : phase1Cooldown;


        if (boss.CanSeePlayer())
        {
            losePlayerTime = 0f;

            RotateToPlayer();

            if (attackTimer >= cooldown)
            {
                attackTimer = 0f;

                if (boss.IsPhase2())
                    boss.StartCoroutine(ComboAttack());
                else
                    boss.StartCoroutine(SmartAttack());
            }

            boss.LastKnowPos = boss.Player.transform.position;
        }
        else
        {
            losePlayerTime += Time.deltaTime;

            if (losePlayerTime >= loseDuration)
            {
                _stateMachine.ChangeState(new BossSearchState());
            }
        }

    }

    // ==================================
    // SMART ATTACK (chọn theo khoảng cách)
    // ==================================
    private IEnumerator SmartAttack()
    {
        isAttacking = true;

        Vector3 dir = boss.Player.transform.position - boss.transform.position;
        dir.y = 0;
        float dist = dir.sqrMagnitude;

        if (dist <= 20f * 20f)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                boss._animator.SetTrigger("attack");
                yield return RockAttack();
            }
            else
            {
                boss._animator.SetTrigger("attack");
                yield return AOEAttack();
            }
                
        }
        else if (dist <= 40f * 40f)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                boss._animator.SetTrigger("attack");
                yield return Shoot();
            }
            else
            {
                boss._animator.SetTrigger("attack");
                yield return AOEAttack();
            }
                
        }
        else
        {
            _stateMachine.ChangeState(new BossSearchState());
        }

        isAttacking = false;
    }

    // ==================================
    // COMBO PHASE 2
    // ==================================
    private IEnumerator ComboAttack()
    {
        isAttacking = true;

        yield return SmartAttack();
        yield return new WaitForSeconds(0.5f);
        yield return SmartAttack();

        isAttacking = false;
    }

    // ==================================
    // AOE + WARNING
    // ==================================
    private IEnumerator AOEAttack()
    {
        

        yield return new WaitForSeconds(1f);

        float duration = 3f;        // thời gian mưa đá
        float spawnDelay = 0.2f;    // khoảng cách giữa mỗi viên
        float radius = 8f;          // bán kính quanh boss

        float timer = 0f;

        while (timer < duration)
        {
            Vector3 randomPos = boss.Player.transform.position +
                new Vector3(
                    Random.Range(-radius, radius),
                    0,
                    Random.Range(-radius, radius)
                );


            GameObject rocks = GameObject.Instantiate(boss.aoePrefab, randomPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
            timer += spawnDelay;
        }
    }

    // ==================================
    // Gaint Fireball
    // ==================================
    private IEnumerator RockAttack()
    {
        yield return new WaitForSeconds(0.5f);

        if (boss.firePoint == null || boss.fireballPrefab == null) yield break;

        Transform gunBarrel = boss.firePoint;

        GameObject bullet = GameObject.Instantiate(
            boss.rockprefab,
            gunBarrel.position,
            Quaternion.identity
        );

        Vector3 targetPos = boss.Player.transform.position;
        targetPos.y = gunBarrel.position.y;

        Vector3 shootDirection = (targetPos - gunBarrel.position).normalized;

        Vector3 spreadDirection =
            Quaternion.AngleAxis(0, Vector3.up) * shootDirection;

        float bulletSpeed = boss.IsPhase2() ? 45f : 25f;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = spreadDirection * bulletSpeed;
        }        
    }

    // ==================================
    // FIREBALL
    // ==================================

    private void RotateToPlayer()
    {
        Vector3 targetPosition = boss.Player.transform.position;
        targetPosition.y = boss.transform.position.y;

        Vector3 direction = (targetPosition - boss.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        boss.transform.rotation = Quaternion.Slerp(
            boss.transform.rotation,
            lookRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    private IEnumerator  Shoot()
    {
        yield return new WaitForSeconds(0.5f);

        if (boss.firePoint == null || boss.fireballPrefab == null) yield break;

        Transform gunBarrel = boss.firePoint;

        int half = bulletCount / 2;

        for (int i = -half; i <= half; i++)
        {
            GameObject bullet = GameObject.Instantiate(
                boss.fireballPrefab,
                gunBarrel.position,
                Quaternion.identity
            );

            Vector3 targetPos = boss.Player.transform.position;
            targetPos.y = gunBarrel.position.y;

            Vector3 shootDirection = (targetPos - gunBarrel.position).normalized;

            Vector3 spreadDirection =
                Quaternion.AngleAxis(i * spreadAngle, Vector3.up) * shootDirection;

            float bulletSpeed = boss.IsPhase2() ? 45f : 25f;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spreadDirection * bulletSpeed;
            }
        }
    }
}
