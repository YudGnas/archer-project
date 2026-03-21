using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Retry : MonoBehaviour
{
    public GameObject player;
    public Transform towncheckpoint;
    public Transform checkpoint;

    public Boss _boss;

    public void OnRetry()
    {
        Player_Fire playerFire = GetComponent<Player_Fire>();
        if (playerFire != null)
        {
            playerFire.RespawnPlayer(checkpoint);
        }
        _boss.ResetBoss();
    }
    public void OnBacktoTown()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.position = towncheckpoint.position;
        controller.enabled = true;
    }
}
