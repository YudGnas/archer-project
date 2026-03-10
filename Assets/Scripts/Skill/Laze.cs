using UnityEngine;

public class Laze : SkillBase
{
    public bool isUsing;

    private float _time = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsing)
        {
            _time -= Time.deltaTime;
            if( _time < 0f )
            {
                enemyList.Clear();
                BossList.Clear();
                _time = 1f;
            }
        }
    }
    public override void Shoot(GameObject skillbullet, Transform firepoint)
    {
        isUsing = true;
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
