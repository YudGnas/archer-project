using UnityEngine;
using UnityEngine.AI;

public class BossPatrolState : BossBaseState
{    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float waitTime;

    [SerializeField] private float waitDuration = 1f;

    public override void Enter()
    {
        waitTime = 0f;

        boss.Agent.speed = 3.5f;
        


        SetupAgent();
        MoveToRandomPoint();
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
        if (boss.Agent.pathPending) return;

        if (boss.Agent.remainingDistance <= boss.Agent.stoppingDistance)
        {
            waitTime += Time.deltaTime;

            if (waitTime >= waitDuration)
            {
                MoveToRandomPoint();
                waitTime = 0f;
            }
        }
    }

    private void MoveToRandomPoint()
    {
        Vector3 randomPos = GetRandomPointInRoom();
        boss.Agent.SetDestination(randomPos);
    }


    private Vector3 GetRandomPointInRoom()
    {
        float randomX = Random.Range(-boss.roomWidth / 2f, boss.roomWidth / 2f);
        float randomZ = Random.Range(-boss.roomLength / 2f, boss.roomLength / 2f);

        Vector3 localOffset = new Vector3(randomX, 0, randomZ);
        Vector3 worldPoint = boss.roomCenter.TransformPoint(localOffset);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(worldPoint, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return boss.roomCenter.position;
    }
}
