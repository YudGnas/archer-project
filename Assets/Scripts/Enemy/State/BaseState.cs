public abstract class BaseState 
{   

    public Enemy enemy;
    public StateMachine _stateMachine;
    public abstract void Enter();
    public abstract void Exit();

    public abstract void Perform();
}
