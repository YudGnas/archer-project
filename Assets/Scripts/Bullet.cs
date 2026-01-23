using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private float speed;
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

    void OnCollisionEnter(Collision collision)
    {
        // Lấy normal của bề mặt va chạm
        Vector3 normal = collision.contacts[0].normal;

        // Hướng hiện tại của đạn
        Vector3 currentDir = rb.linearVelocity.normalized;

        // Tính hướng phản xạ
        Vector3 reflectDir = Vector3.Reflect(currentDir, normal);

        // Giữ nguyên tốc độ
        rb.linearVelocity = reflectDir * speed;

        // Xoay viên đạn theo hướng mới (để nhìn cho đúng)
        transform.rotation = Quaternion.LookRotation(reflectDir);
    }
}
