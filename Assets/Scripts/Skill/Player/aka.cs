using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class aka : SkillBase
{
    [SerializeField] GameObject murasaki_prefab;
    void Update()
    {
        Invoke("DestroyBullet", 10);
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
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemyList.Contains(enemy))
            {
                enemy.TakeDamege(trueDamege);
                enemyList.Add(enemy);
            }
            Destroy(gameObject, 2f);
        }
        if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null && !BossList.Contains(boss))
            {
                boss.TakeDamage(trueDamege, infor.poiseDamage);
                BossList.Add(boss);
            }
            Destroy(gameObject, 2f);
        }

        if(other.CompareTag("ao"))
        {
            Debug.Log("murasaki!!!");
            GameObject murasaki = Instantiate(murasaki_prefab, transform.position, Quaternion.identity );
            
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

}
