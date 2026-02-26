using System.Collections.Generic;
using UnityEngine;

public class fallingice : SkillBase
{
    private Rigidbody rb;
    //private float speed;
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();


    void DestroyBullet()
    {
        Destroy(gameObject);
    }



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {   
        
        Invoke("DestroyBullet", 2);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamege(damage);
                
            }
            Destroy(gameObject, 2f);
        }
        if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage, poiseDamage);
                
            }
            Destroy(gameObject, 2f);
        }
    }
}
