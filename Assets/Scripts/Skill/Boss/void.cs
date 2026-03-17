using System.Collections.Generic;
using UnityEngine;

public class VoidSkill : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 20f;
    public float lifeTime = 5f;

    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime); // tự hủy sau thời gian
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player_Health>();
            if (player != null)
            {
                player.TakeDamege(damage);
            }

            Destroy(gameObject);
        }
    }
}
