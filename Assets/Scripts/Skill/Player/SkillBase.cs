using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{   
    public Player_Fire Player_Fire;
    public Player_Controller _player;
    public Skill_infor infor;
    
    protected List<Enemy> enemyList = new List<Enemy>();
    protected List<Boss> BossList = new List<Boss>();
    protected float trueDamege => infor.damege + _player._player_Infor._Attack;


    void DestroyBullet()
    {
        Destroy(gameObject);
        enemyList.Clear();
        BossList.Clear();
    }
    protected Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Shoot(GameObject skillbullet, Transform firepoint)
    {

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
    }
}
