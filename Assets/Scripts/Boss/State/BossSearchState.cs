using UnityEngine;
using UnityEngine.AI;

public class BossSearchState : BossBaseState
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

        boss.Agent.isStopped = false;
        boss.Agent.speed = 5;
        boss._animator.SetBool("isRun", true);

        // Đi tới vị trí cuối cùng thấy player
        boss.Agent.SetDestination(boss.LastKnowPos);

        // Random thời gian đổi hướng
        currentWanderInterval = Random.Range(wanderIntervalMin, wanderIntervalMax);
    }

    public override void Exit()
    {
        boss._animator.SetBool("isRun", false);
    }

    public override void Perform()
    {
        // Nếu thấy lại player → chuyển sang Attack
        if (boss.CanSeePlayer())
        {
                _stateMachine.ChangeState(new BossAttackState());
            return;
        }

        searchTimer += Time.deltaTime;

        // Nếu tới vị trí cuối cùng
        if (!boss.Agent.pathPending &&
            boss.Agent.remainingDistance <= boss.Agent.stoppingDistance)
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
            _stateMachine.ChangeState(new BossPatrolState());
        }
    }

    private void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += boss.transform.position;
        randomDirection.y = boss.transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            boss.Agent.SetDestination(hit.position);
        }
    }
}
