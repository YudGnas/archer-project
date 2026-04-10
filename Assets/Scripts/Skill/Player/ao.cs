using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ao : SkillBase
{
    private Vector3 startPos;
    private bool isActivated = false;

    [SerializeField] float maxDistance = 10f;
    [SerializeField] float pullForce = 20f;


    public float attackCooldown = 1f;

    private float timer;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();

        
    }

    void Update()
    {
        timer += Time.deltaTime;
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
            if (timer >= attackCooldown)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamege(trueDamege);
                    Destroy(gameObject, 3f);
                }
                timer = 0f;
            }
            
        }

        if (other.CompareTag("Boss"))
        {
            if (timer >= attackCooldown)
            {
                Boss boss = other.GetComponent<Boss>();
                if (boss != null)
                {
                
                        boss.TakeDamage(trueDamege, infor.poiseDamage);
                        Destroy(gameObject, 3f);
                }
            }
        }
    }
}