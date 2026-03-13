using System.Collections.Generic;
using UnityEngine;

public class falling : MonoBehaviour
{
    private List<Player_Health> playerList = new List<Player_Health>();

    [SerializeField] private float damege;
    void Start()
    {
        Invoke("DestroyBullet", 2f);
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
        playerList.Clear();
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Health player_Infor = other.GetComponent<Player_Health>();
            if (player_Infor != null && !playerList.Contains(player_Infor))
            {
                player_Infor.TakeDamege(damege);
                playerList.Add(player_Infor);
            }
        }
    }
}
