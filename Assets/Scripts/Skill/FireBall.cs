using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody rb;
    private float speed;
    private float damage = 10;
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
        speed = rb.linearVelocity.magnitude; // lưu lại tốc độ ban đầu
        Invoke("DestroyBullet", 10);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Health player = other.GetComponent<Player_Health>();
            if (player != null)
            {
                player.TakeDamege(damage);
            }
            Destroy(gameObject);
        }
        /*if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamege(damage);
            }
        }*/
    }
}
