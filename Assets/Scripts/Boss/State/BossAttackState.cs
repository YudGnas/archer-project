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
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float spreadAngle = 5f;
    [SerializeField] private float loseDuration = 5f;

    public override void Enter()
    {
        losePlayerTime = 0f;
        attackTimer = 0f;
        isAttacking = false;

        boss.Agent.isStopped = true;
        if (boss.IsPhase2)
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

        float cooldown = boss.IsPhase2 ? phase2Cooldown : phase1Cooldown;


        if (boss.CanSeePlayer())
        {
            losePlayerTime = 0f;

            RotateToPlayer();

            if (attackTimer >= cooldown)
            {
                attackTimer = 0f;

                if (boss.IsPhase2)
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

        if (dir.sqrMagnitude <= 10f * 10f)
        {
            yield return ChargeAttack();
        }
        else if (dir.sqrMagnitude <= 15f * 15f && dir.sqrMagnitude > 10f *10f)
        {
            yield return AOEAttack();
        }
        else if (dir.sqrMagnitude <= 20f * 20f && dir.sqrMagnitude > 15f *15f)
        {
            yield return Shoot();
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
        float distance = Vector3.Distance(
            boss.transform.position,
            boss.Player.transform.position
        );

        Vector3 targetPos = boss.Player.transform.position;

        GameObject warning = GameObject.Instantiate(
            boss.warningCirclePrefab,
            targetPos,
            Quaternion.identity
        );

        float warningTime = 1.5f;
        float timer = 0f;

        while (timer < warningTime)
        {
            warning.transform.localScale += new Vector3(2f * Time.deltaTime, 0f, 2f * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        GameObject aoe = GameObject.Instantiate(
            boss.aoePrefab,
            targetPos,
            Quaternion.identity
        );

    }

    // ==================================
    // CHARGE + TELEGRAPH
    // ==================================
    private IEnumerator ChargeAttack()
    {
        float distance = Vector3.Distance(
            boss.transform.position,
            boss.Player.transform.position
        );

        if (distance > 10f)
            yield break;

        boss._animator.SetTrigger("charge_prepare");

        yield return new WaitForSeconds(1f);

        Vector3 direction =
            (boss.Player.transform.position - boss.transform.position).normalized;

        float chargeTime = 1.2f;
        float chargeSpeed = boss.IsPhase2 ? 22f : 15f;

        float timer = 0f;

        while (timer < chargeTime)
        {
            boss.transform.position += direction * chargeSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    // ==================================
    // FIREBALL
    // ==================================
    private IEnumerator FireballAttack()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject fireball = GameObject.Instantiate(
            boss.fireballPrefab,
            boss.firePoint.position,
            Quaternion.identity
        );

        Vector3 dir =
            (boss.Player.transform.position - boss.firePoint.position).normalized;

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float speed = boss.IsPhase2 ? 35f : 25f;
            rb.linearVelocity = dir * speed;
        }
    }

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

            float bulletSpeed = boss.IsPhase2 ? 45f : 25f;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spreadDirection * bulletSpeed;
            }
        }
    }
}
