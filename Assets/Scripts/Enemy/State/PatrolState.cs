using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTime;
    public override void Enter()
    {
        
    }
    public override void Exit()
    {

    }
    public override void Perform()
    {
        PatrolCircle();
        if (enemy.CanSeePlayer())
        {
            _stateMachine.ChangeState(new AttackState());
        }
    }

    public void PatrolCircle()
    {
        if(enemy.Agent.remainingDistance < 0.2)
        {
            waitTime += Time.deltaTime;

            if(waitTime > 3)
            {
                if(waypointIndex < enemy._path.waypoint.Count - 1)
                {
                    waypointIndex++;
                }
                else
                    waypointIndex = 0;

                enemy.Agent.SetDestination(enemy._path.waypoint[waypointIndex].position);
                waitTime = 0;
            }
            
        }
    }
}
