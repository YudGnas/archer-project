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
                yield return boss.RockAttack();
            }
            else
            {
                boss._animator.SetTrigger("attack");
                yield return boss.AOEAttack();
            }
                
        }
        else if (dist <= 40f * 40f)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                boss._animator.SetTrigger("attack");
                yield return boss.Shoot();
            }
            else
            {
                boss._animator.SetTrigger("attack");
                yield return boss.AOEAttack();
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


}
