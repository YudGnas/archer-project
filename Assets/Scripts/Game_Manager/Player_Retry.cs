using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Retry : MonoBehaviour
{
    public GameObject player;
    public Transform towncheckpoint;
    public Transform checkpoint;
    public GameObject panel;

    public Boss _boss;
    public Player_Controller _Controller;

    private void Start()
    {
        _Controller = player.GetComponent<Player_Controller>();
    }

    public void OnRetry()
    {   

        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.position = _Controller.checkpoint.position;
        controller.enabled = true;
        _boss.ResetBoss();
        Time.timeScale = 1f;
        Player_Health health = player.GetComponent<Player_Health>();
        if (health != null)
        {
            health.ResetPlayer();
        }
        panel.SetActive(false);
    }
    public void OnBacktoTown()
    {

        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.position = towncheckpoint.position;
        controller.enabled = true;
        _boss.ResetBoss();
        Time.timeScale = 1f;

        Player_Health health = player.GetComponent<Player_Health>();
        if (health != null)
        {
            health.ResetPlayer();
        }
        panel.SetActive(false);

    }
}
