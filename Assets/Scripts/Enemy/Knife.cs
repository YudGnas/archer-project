using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public int damage = 20;

    private bool canDamage = true;
    private float cooldown = 1f;

    private void Update()
    {
        
        if (!canDamage)
        {
            cooldown -= Time.deltaTime;
            if(cooldown <= 0 )
            {
                canDamage = true;
                cooldown = 1f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (other.CompareTag("Player"))
        {
            Player_Health player = other.GetComponent<Player_Health>();
            if (player != null)
            {
                player.TakeDamege(damage);
            }
            canDamage = false;
        }
    }
}
