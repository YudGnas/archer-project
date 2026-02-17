using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public BossBaseState activeState;

    public void Initialise()
    {
        ChangeState(new BossPatrolState());
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BossBaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;


        if (activeState != null)
        {
            activeState._stateMachine = this;
            activeState.boss = GetComponent<Boss>();
            activeState.Enter();
        }
    }
}
