using UnityEngine;

public class CloseCombatAttackState : BaseState
{
    private float losePlayerTime;
    private float attackTimer;

    [SerializeField] private float loseDuration = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    public override void Enter()
    {
        losePlayerTime = 0f;
        attackTimer = 0f;
    }

    public override void Exit()
    {
        enemy.Agent.isStopped = false;
    }

    public override void Perform()
    {
        if (enemy.Player == null) return;

        attackTimer += Time.deltaTime;

        float distance = Vector3.Distance(
            enemy.transform.position,
            enemy.Player.transform.position
        );

        if (enemy.CanSeePlayer())
        {
            losePlayerTime = 0f;
            enemy.LastKnowPos = enemy.Player.transform.position;

            if (distance > attackRange)
            {
                // Đuổi theo
                enemy.Agent.isStopped = false;
                enemy.Agent.SetDestination(enemy.Player.transform.position);
            }
            else
            {
                // Trong tầm đánh
                enemy.Agent.isStopped = true;

                RotateToPlayer();

                if (attackTimer >= attackCooldown)
                {
                    enemy._animator.SetTrigger("attack");
                    attackTimer = 0f;
                }
            }
        }
        else
        {
            losePlayerTime += Time.deltaTime;

            if (losePlayerTime >= loseDuration)
            {
                _stateMachine.ChangeState(new SearchState());
            }
        }
    }

    private void RotateToPlayer()
    {
        Vector3 targetPosition = enemy.Player.transform.position;
        targetPosition.y = enemy.transform.position.y;

        Vector3 direction = (targetPosition - enemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            lookRotation,
            rotateSpeed * Time.deltaTime
        );
    }
}
