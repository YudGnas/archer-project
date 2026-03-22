using UnityEngine;

public class RangedAttackState : BaseState
{
    private float losePlayerTime;
    private float shootTimer;

    [SerializeField] private float loseDuration = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float spreadAngle = 5f;
    [SerializeField] private int bulletCount = 2;

    public override void Enter()
    {
        losePlayerTime = 0f;
        shootTimer = 0f;

        // Dừng di chuyển khi tấn công
        enemy.Agent.isStopped = true;
    }

    public override void Exit()
    {
        // Cho phép di chuyển lại
        enemy.Agent.isStopped = false;
    }

    public override void Perform()
    {
        if (enemy.Player == null) return;

        shootTimer += Time.deltaTime;

        if (enemy.CanSeePlayer())
        {
            losePlayerTime = 0f;

            RotateToPlayer();

            if (shootTimer >= enemy.fireRate)
            {
                enemy._animator.SetTrigger("attack");
                Shoot();
                shootTimer = 0f;
            }

            enemy.LastKnowPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTime += Time.deltaTime;

            if (losePlayerTime >= loseDuration)
            {
                _stateMachine.ChangeState(new SearchState());
            }
        }
    }

    private void RotateToPlayer()
    {
        Vector3 targetPosition = enemy.Player.transform.position;
        targetPosition.y = enemy.transform.position.y;

        Vector3 direction = (targetPosition - enemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            lookRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    private void Shoot()
    {
        if (enemy.gunBarrel == null || enemy.bullet == null) return;

        Transform gunBarrel = enemy.gunBarrel;

        int half = bulletCount / 2;

        for (int i = -half; i <= half; i++)
        {
            GameObject bullet = GameObject.Instantiate(
                enemy.bullet,
                gunBarrel.position,
                Quaternion.identity
            );

            Vector3 targetPos = enemy.Player.transform.position;
            targetPos.y = gunBarrel.position.y;

            Vector3 shootDirection = (targetPos - gunBarrel.position).normalized;

            Vector3 spreadDirection =
                Quaternion.AngleAxis(i * spreadAngle, Vector3.up) * shootDirection;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spreadDirection * bulletSpeed;
            }
        }
    }
}
