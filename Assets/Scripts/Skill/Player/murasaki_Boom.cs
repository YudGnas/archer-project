using UnityEngine;

public class murasaki_Boom : MonoBehaviour
{

    public float damage = 100;
    void Start()
    {
        Invoke(nameof(Explode), 1.25f);
    }

    void Explode()
    {
        float radius = 10f;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Debug.Log("takedame Murasaki");
                    enemy.TakeDamege(damage); // damage tùy bạn
                }
            }
            if (hit.CompareTag("Boss"))
            {
                Boss boss = hit.GetComponent<Boss>();
                if(boss != null)
                {
                    Debug.Log("takedame Murasaki");
                    boss.TakeDamage(damage, damage / 10);
                }
            }
        }

        Destroy(gameObject); // hủy sau khi gây damage
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
