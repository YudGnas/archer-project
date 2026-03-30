using UnityEngine;

public class Gate_Logic : MonoBehaviour
{
    public GameObject green_gate;
    public GameObject red_gate;
    public GameObject green_gem;
    public GameObject red_gem;

    bool can_contact;
    void Start()
    {
        green_gate.SetActive(false);
        green_gem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(can_contact && Input.GetKey(KeyCode.F))
        {
            red_gate.SetActive(false);
            red_gem.SetActive(false);

            green_gate.SetActive(true);
            green_gem.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            can_contact = true;
        }
    }
}
