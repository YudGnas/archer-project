using UnityEngine;
using UnityEngine.AI;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;

    [SerializeField] private float searchDuration = 10f;
    [SerializeField] private float wanderRadius = 6f;
    [SerializeField] private float wanderIntervalMin = 2f;
    [SerializeField] private float wanderIntervalMax = 4f;

    private float currentWanderInterval;

    public override void Enter()
    {
        searchTimer = 0f;
        moveTimer = 0f;

        enemy.Agent.isStopped = false;
        enemy.Agent.speed = 5;
        enemy._animator.SetBool("isRun", true);

        // Đi tới vị trí cuối cùng thấy player
        enemy.Agent.SetDestination(enemy.LastKnowPos);

        // Random thời gian đổi hướng
        currentWanderInterval = Random.Range(wanderIntervalMin, wanderIntervalMax);
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        // Nếu thấy lại player → chuyển sang Attack
        if (enemy.CanSeePlayer())
        {
            
                if (enemy.enemy_Infor.role == EnemyRole.CloseCombat)
                    _stateMachine.ChangeState(new CloseCombatAttackState());

                else if (enemy.enemy_Infor.role == EnemyRole.Ranged)
                    _stateMachine.ChangeState(new RangedAttackState());
            
            return;
        }

        searchTimer += Time.deltaTime;

        // Nếu tới vị trí cuối cùng
        if (!enemy.Agent.pathPending &&
            enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            moveTimer += Time.deltaTime;

            // Sau một khoảng thời gian thì đi random xung quanh
            if (moveTimer >= currentWanderInterval)
            {
                Wander();
                moveTimer = 0f;
                currentWanderInterval = Random.Range(wanderIntervalMin, wanderIntervalMax);
            }
        }

        // Hết thời gian tìm kiếm → quay lại Patrol
        if (searchTimer >= searchDuration)
        {
            _stateMachine.ChangeState(new PatrolState());
        }
    }

    private void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += enemy.transform.position;
        randomDirection.y = enemy.transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            enemy.Agent.SetDestination(hit.position);
        }
    }
}
