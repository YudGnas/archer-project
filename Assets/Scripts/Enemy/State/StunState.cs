using UnityEngine;

public class StunState : BaseState
{
    private float stuntime;

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
    }

    public override void Exit()
    {
        enemy.Agent.isStopped = false;
    }

    public override void Perform()
    {
        stuntime -= Time.deltaTime;
        if( stuntime < 0 )
        {
            _stateMachine.ChangeState(new SearchState());
        }
    }
}
