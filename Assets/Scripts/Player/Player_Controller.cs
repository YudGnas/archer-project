using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private Player_Infor Player_Infor;
    public Player_Infor _player_Infor => Player_Infor;

    [SerializeField] private Rigidbody _rb;

    void Start()
    {
        
    }

    void Update()
    {
        float playermovex = Input.GetAxisRaw("Horizontal");
        float playermovey = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(playermovex, 0f, playermovey).normalized;
        _rb.linearVelocity = move * Player_Infor._Speed;
    }
}
