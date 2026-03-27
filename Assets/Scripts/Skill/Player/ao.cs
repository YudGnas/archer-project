using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ao : SkillBase
{
    private Vector3 startPos;
    private bool isActivated = false;

    [SerializeField] float maxDistance = 10f;
    [SerializeField] float pullForce = 20f;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();

        
    }

    void Update()
    {
        if (isActivated) return;

        float distance = Vector3.Distance(startPos, transform.position);
        if (distance >= maxDistance)
        {
            rb.linearVelocity = Vector3.zero;
            Destroy(gameObject, 3f);
        }
    }

    public override void Shoot(GameObject skillbullet, Transform firepoint)
    {
        GameObject bullet = Instantiate(
            skillbullet,
            firepoint.position,
            firepoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firepoint.forward * infor.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    Vector3 dir = (transform.position - enemy.transform.position).normalized;
                    agent.Move(dir * pullForce * Time.deltaTime);
                }

                enemy.TakeDamege(trueDamege);
                Destroy(gameObject, 3f);
            }
        }

        if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                NavMeshAgent agent = boss.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    Vector3 dir = (transform.position - boss.transform.position).normalized;
                    agent.Move(dir * pullForce * Time.deltaTime);
                }

                boss.TakeDamage(trueDamege, infor.poiseDamage);
                Destroy(gameObject, 3f);
            }
        }
    }
}