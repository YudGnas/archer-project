using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PatrolState : BaseState
{
    private float waitTime;

    [SerializeField] private float waitDuration = 1f;

    public override void Enter()
    {       
        waitTime = 0f;

        enemy.Agent.speed = 3.5f;
        enemy._animator.SetBool("isRun", false);


        SetupAgent();
        MoveToRandomPoint();
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
        if (enemy.Agent.pathPending) return;

        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
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
        enemy.Agent.SetDestination(randomPos);
    }


    private Vector3 GetRandomPointInRoom()
    {
        float randomX = Random.Range(- enemy.roomWidth / 2f, enemy.roomWidth / 2f);
        float randomZ = Random.Range(- enemy.roomLength / 2f, enemy.roomLength / 2f);

        Vector3 localOffset = new Vector3(randomX, 0, randomZ);
        Vector3 worldPoint = enemy.roomCenter.TransformPoint(localOffset);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(worldPoint, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return enemy.roomCenter.position;
    }


}
