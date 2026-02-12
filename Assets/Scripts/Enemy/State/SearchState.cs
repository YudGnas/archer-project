using Unity.VisualScripting;
using UnityEngine;

public class SearchState : BaseState
{

    private float searchTime;
    private float moveTime;
    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnowPos);
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if(enemy.CanSeePlayer())
        {
            _stateMachine.ChangeState(new AttackState());
            return;
        }

        if(!enemy.Agent.pathPending &&
            enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            searchTime += Time.deltaTime;
            moveTime += Time.deltaTime;
            if (moveTime > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + Random.insideUnitSphere * 5);
                moveTime = 0;
            }
            if (searchTime > 10f)
            {
                _stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    
}
