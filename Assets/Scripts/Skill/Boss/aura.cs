using UnityEngine;

public class aura : MonoBehaviour
{
    public float damage = 30f;
    public float attackCooldown = 1f;

    private float timer;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer += Time.deltaTime;

            if (timer >= attackCooldown)
            {
                Player_Health player = other.GetComponent<Player_Health>();
                if (player != null)
                {
                    player.TakeDamege(damage);
                }
                timer = 0f;
            }
        }
    }
}
