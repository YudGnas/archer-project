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

            Vector3 targetPosition = enemy.Player.transform.position;

            // Giữ nguyên chiều cao của enemy
            targetPosition.y = enemy.transform.position.y;

            enemy.transform.LookAt(targetPosition);
            if (ShootTime > enemy.fireRate)
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

       

        for (int i = -2; i <= 2; i++)
        {   
            GameObject bullet = GameObject.Instantiate(enemy.bullet, gunbarrel.position, enemy.transform.rotation);

            Vector3 targetPos = enemy.Player.transform.position;
            targetPos.y = gunbarrel.position.y; // maintain the same altitude

            Vector3 shootDirection = (targetPos - gunbarrel.position).normalized;

            Vector3 spreadDirection =
                Quaternion.AngleAxis(i * 5f, Vector3.up) * shootDirection;

            bullet.GetComponent<Rigidbody>().linearVelocity = spreadDirection * 40f;
        }
        ShootTime = 0;
    }
}
