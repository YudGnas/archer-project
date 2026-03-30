using UnityEngine;

public class Gate_Tele : MonoBehaviour
{
    public Transform target;

    public GameObject player;
    public GameObject plane_target;
    public GameObject plane_current;

    bool cantele;

    void Start()
    {
        
    }
    void Update()
    {
        if(cantele && Input.GetKey(KeyCode.F))
        {
            Teleport();
        }
    }

    public void Teleport()
    {
        plane_target.SetActive(true);

        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.position = target.position;
        controller.enabled = true;


        plane_current.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cantele = true;
        }
    }
}
