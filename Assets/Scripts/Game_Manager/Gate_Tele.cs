using System.Collections;
using UnityEngine;
using DG.Tweening;


public class Gate_Tele : MonoBehaviour
{
    public LoadingScene _loading;

    public Transform target;

    public GameObject player;
    public GameObject plane_target;
    public GameObject plane_current;

    //public AudioSource _audio;


    private Player_Controller _Controller;
    bool cantele;

    void Start()
    {
        _Controller = player.GetComponent<Player_Controller>();
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


        AudioSource _audio = plane_target.GetComponent<AudioSource>();            
        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.position = target.position;
        controller.enabled = true;
        _Controller.checkpoint = target;
        yield return new WaitForSeconds(1f);


        StartCoroutine(_loading.FadeIn());
        if (_audio != null)
        {
            _Controller.local_audio = _audio;
            _Controller.local_audio.Play();
        }
        
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
