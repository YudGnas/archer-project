using UnityEngine;

public class BossStunState : BossBaseState
{
    public float stuntime;

    public override void Enter()
    {
        boss.Agent.isStopped = true;
    }

    public override void Exit()
    {
        boss.Agent.isStopped = false;
    }

    public override void Perform()
    {
        stuntime -= Time.deltaTime;
        if (stuntime < 0)
        {
            _stateMachine.ChangeState(new BossSearchState());
        }
    }
}
