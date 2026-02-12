using UnityEngine;

public class AttackState : BaseState
{
    private float moveTime;
    private float losePlayerTime;

    private float ShootTime;

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTime = 0;
            moveTime += Time.deltaTime;
            ShootTime += Time.deltaTime;

            enemy.transform.LookAt(enemy.Player.transform);
            if(ShootTime > enemy.fireRate)
            {
                Shoot();
            }

            /*if(moveTime > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + Random.insideUnitSphere * 5);
                moveTime = 0;
            }*/
            enemy.LastKnowPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTime += Time.deltaTime;
            if (losePlayerTime > 5)
            {
                // Change to Search State
                _stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        Transform gunbarrel = enemy.gunBarrel;

        GameObject bullet = GameObject.Instantiate(enemy.bullet, gunbarrel.position, enemy.transform.rotation);

        Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;

        bullet.GetComponent<Rigidbody>().linearVelocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;

        ShootTime = 0;
    }
}
