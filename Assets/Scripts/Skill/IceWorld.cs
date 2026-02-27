using UnityEngine;
using System.Collections.Generic;

public class IceWorld : SkillBase
{

    void Update()
    {
        Invoke("DestroyBullet", 1f);
    }
    public override void Shoot(GameObject skillbullet, Transform firepoint)
    {

        GameObject bullet = Instantiate(
            skillbullet,
            firepoint.position,
            firepoint.rotation
        );
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemyList.Contains(enemy))
            {
                enemy.TakeDamege(damage);
                enemyList.Add(enemy);
            }
            Destroy(gameObject, 2f);
        }
        if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null && !BossList.Contains(boss))
            {
                boss.TakeDamage(damage, poiseDamage);
                BossList.Add(boss);
            }
            Destroy(gameObject, 2f);
        }
    }
}
