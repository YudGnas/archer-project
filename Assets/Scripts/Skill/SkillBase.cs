using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{   
    public Player_Fire Player_Fire;
    public float damage ;
    public float poiseDamage;
    public float cooldown;
    public float ManaCost;
    public float speed;
    protected List<Enemy> enemyList = new List<Enemy>();
    protected List<Boss> BossList = new List<Boss>();


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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Player_Fire = player.GetComponent<Player_Fire>();
    }

    public virtual void Shoot(GameObject skillbullet, Transform firepoint)
    {

    }
}
