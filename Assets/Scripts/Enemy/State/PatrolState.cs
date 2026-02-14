using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseState
{
    private int waypointIndex;
    private float waitTime;

    [SerializeField] private float waitDuration = 1f;
    [SerializeField] private float waypointOffsetRadius = 1.5f;

    public override void Enter()
    {
        waypointIndex = 0;
        waitTime = 0f;

        enemy.Agent.speed = 3.5f;
        enemy._animator.SetBool("isRun", false);


        SetupAgent();
        MoveToWaypoint();
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        PatrolCircle();

        if (enemy.CanSeePlayer())
        {   
            if(enemy.enemy_Infor.role == EnemyRole.CloseCombat)
                _stateMachine.ChangeState(new CloseCombatAttackState());

            else if (enemy.enemy_Infor.role == EnemyRole.Ranged)
                _stateMachine.ChangeState(new RangedAttackState());
        }
    }

    private void SetupAgent()
    {
        // Tránh va chạm thông minh hơn
        enemy.Agent.avoidancePriority = Random.Range(10, 90);

        // Không cần đứng đúng tâm waypoint
        enemy.Agent.stoppingDistance = 1.2f;
    }

    private void PatrolCircle()
    {
        // Nếu chưa tới nơi thì return
        if (enemy.Agent.pathPending) return;

        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            waitTime += Time.deltaTime;

            if (waitTime >= waitDuration)
            {
                NextWaypoint();
                MoveToWaypoint();
                waitTime = 0f;
            }
        }
    }

    private void NextWaypoint()
    {
        if (waypointIndex < enemy._path.waypoint.Count - 1)
            waypointIndex++;
        else
            waypointIndex = 0;
    }

    private void MoveToWaypoint()
    {
        Vector3 basePosition = enemy._path.waypoint[waypointIndex].position;

        // Random vị trí xung quanh waypoint
        Vector3 randomOffset = Random.insideUnitSphere * waypointOffsetRadius;
        randomOffset.y = 0;

        Vector3 targetPosition = basePosition + randomOffset;

        enemy.Agent.SetDestination(targetPosition);
    }
}
