using UnityEngine;

public abstract class BossBaseState 
{
    public Boss boss;
    public BossStateMachine _stateMachine;
    public abstract void Enter();
    public abstract void Exit();

    public abstract void Perform();
}
