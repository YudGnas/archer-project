using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private Player_Infor Player_Infor;
    [SerializeField] private Player_Fire Player_Fire;

    public Player_Infor _player_Infor => Player_Infor;
    public Player_Fire _player_Fire => Player_Fire;

    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] public Animator _animator;

    public CharacterController _controller;
    public Vector3 velocity; // dùng cho gravity


    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        ApplyGravity();
    }

    void Move()
    {
        float playermovex = Input.GetAxisRaw("Horizontal"); 
        float playermovey = Input.GetAxisRaw("Vertical");


        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        Vector3 move = (camForward * playermovey + camRight * playermovex).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? Player_Infor._Speedrun : Player_Infor._Speedwalk; 
        if (move != Vector3.zero)
        { 
            // Di chuyển
            _controller.Move(move * speed * Time.deltaTime); 
            // Xoay hướng theo move
            Quaternion targetRotation = Quaternion.LookRotation(move); 
            transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, rotateSpeed * Time.deltaTime ); 
        } 
        // Animation
        _animator.SetFloat("walk", move.magnitude);
        _animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift)); 
    } 
    void ApplyGravity() 
    { 
        if (_controller.isGrounded && velocity.y < 0) 
        { 
            velocity.y = -2f; 
        }
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
    }
}
