using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{   
    public Player_Fire Player_Fire;
    public Player_Controller _player;
    public Skill_infor infor;
    
    protected List<Enemy> enemyList = new List<Enemy>();
    protected List<Boss> BossList = new List<Boss>();
    public float trueDamege;


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
        _player = player.GetComponent<Player_Controller>();
    }

    public virtual void Shoot(GameObject skillbullet, Transform firepoint)
    {

    }
}
