using System.Collections;
using UnityEngine;

public class Gate_Tele : MonoBehaviour
{
    public LoadingScene _loading;

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
            StartCoroutine(Teleport());
        }
    }

    public IEnumerator Teleport()
    {
        _loading.FadeOut();

        yield return new WaitForSeconds(1f);

        plane_target.SetActive(true);

        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.position = target.position;
        controller.enabled = true;
      
        yield return new WaitForSeconds(1f);


        _loading.FadeIn();
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
