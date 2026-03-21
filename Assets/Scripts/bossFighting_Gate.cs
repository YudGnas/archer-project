using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bossFighting_Gate : MonoBehaviour
{
    public Transform checkpoint;

    [SerializeField]private Player_Retry retry;
    [SerializeField]private Boss _boss;

    private bool isFighting;

    [SerializeField]private Player_Fire player_fire;
    [SerializeField]private GameObject player;

    public GameObject confirmPanel;

    private void Start()
    {
        retry._boss = _boss;
        retry.checkpoint = checkpoint;
        confirmPanel.SetActive(false);
    }

    private void Update()
    {
        if (isFighting && Input.GetKeyDown(KeyCode.F))
        {
            confirmPanel.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFighting = true;
        }
    }

    public void Fight_Button()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.position = checkpoint.position;
        controller.enabled = true;
        player_fire.checkpoint = checkpoint;
        confirmPanel.SetActive(false);
    }    
    public void Leave_Button()
    {
        confirmPanel.SetActive(false);
    }
}
