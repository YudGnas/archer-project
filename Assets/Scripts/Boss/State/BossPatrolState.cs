using UnityEngine;

public class BossPatrolState : BossBaseState
{    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int waypointIndex;
    private float waitTime;

    [SerializeField] private float waitDuration = 1f;
    [SerializeField] private float waypointOffsetRadius = 1.5f;

    public override void Enter()
    {
        waypointIndex = 0;
        waitTime = 0f;

        boss.Agent.speed = 3.5f;
        


        SetupAgent();
        MoveToWaypoint();
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        PatrolCircle();

        if (boss.CanSeePlayer())
        {
                _stateMachine.ChangeState(new BossAttackState());
        }
    }

    private void SetupAgent()
    {
        // Tránh va chạm thông minh hơn
        boss.Agent.avoidancePriority = Random.Range(10, 90);

        // Không cần đứng đúng tâm waypoint
        boss.Agent.stoppingDistance = 1.2f;
    }

    private void PatrolCircle()
    {
        // Nếu chưa tới nơi thì return
        if (boss.Agent.pathPending) return;

        if (boss.Agent.remainingDistance <= boss.Agent.stoppingDistance)
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
        if (waypointIndex < boss._path.waypoint.Count - 1)
            waypointIndex++;
        else
            waypointIndex = 0;
    }

    private void MoveToWaypoint()
    {
        Vector3 basePosition = boss._path.waypoint[waypointIndex].position;

        // Random vị trí xung quanh waypoint
        Vector3 randomOffset = Random.insideUnitSphere * waypointOffsetRadius;
        randomOffset.y = 0;

        Vector3 targetPosition = basePosition + randomOffset;

        boss.Agent.SetDestination(targetPosition);
    }
}
