using UnityEngine;

public class fallingice : MonoBehaviour
{
    private Rigidbody rb;
    //private float speed;
    private float damage = 30;
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
    }
}
