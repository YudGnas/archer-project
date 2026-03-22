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

    [SerializeField] private float circleDistance = 30f;
    [SerializeField] private float circleSpeed = 2f;
    private float circleAngle;


    public override void Enter()
    {
        losePlayerTime = 0f;
        attackTimer = 0f;
        isAttacking = false;

        boss.Agent.isStopped = true;
        boss.Agent.updateRotation = false;
        boss.Agent.angularSpeed = 0;
        if (boss.IsPhase2())
        {
            boss.Agent.speed = 6f;
            boss.Agent.acceleration = 20f;
        }


        
    }

    public override void Exit()
    {
        boss.Agent.isStopped = false;
        boss.Agent.updateRotation = true;
        boss.Agent.angularSpeed = 120f;
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
                boss.Agent.isStopped = true;

                attackTimer = 0f;

                if (boss.IsPhase2())
                    boss.StartCoroutine(ComboAttack());
                else
                    boss.StartCoroutine(SmartAttack());
            }
            else
            {
                MoveAroundPlayer();
                RotateToPlayer();
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

        if (dist <= 40f * 40f) // trong tầm đánh
        {
            int rand = Random.Range(0, 4); // 3 skill

            boss._animator.SetTrigger("attack");

            switch (rand)
            {
                case 0:
                    yield return boss.Skill1();
                    break;
                case 1:
                    yield return boss.Skill2();
                    break;
                case 2:
                    yield return boss.Skill3();
                    break;
                case 3: 
                    yield return boss.Skill4();
                    break;
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

    private void MoveAroundPlayer()
    {
        circleAngle += circleSpeed * Time.deltaTime;

        Vector3 offset = new Vector3(
            Mathf.Cos(circleAngle),
            0,
            Mathf.Sin(circleAngle)
        ) * circleDistance;

        Vector3 targetPos = boss.Player.transform.position + offset;

        boss.Agent.isStopped = false;
        boss.Agent.SetDestination(targetPos);
    }
}
