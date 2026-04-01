using UnityEngine;
using UnityEngine.InputSystem.XR;


public enum PlayerState
{
    stun,
    move,
    attack,
    idel
}
public class Player_Controller : MonoBehaviour
{
    [SerializeField] private Player_Infor Player_Infor;
    [SerializeField] private Player_Fire Player_Fire;
    public AudioSource _audio;
    public AudioClip clip;
    public PlayerState _state;

    public Player_Infor _player_Infor => Player_Infor;
    public Player_Fire _player_Fire => Player_Fire;

    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] public Animator _animator;

    public CharacterController _controller;
    public Vector3 velocity; // dùng cho gravity

    public bool isMoving;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _state = PlayerState.idel;
    }

    void Update()
    {
        if(_state == PlayerState.idel)
        {
            Move();
            //Jump();
            ApplyGravity();
        }
        

        HandleFootstepSound();
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
            
            
            _controller.Move(move * speed * Time.deltaTime); 
            // Xoay hướng theo move
            Quaternion targetRotation = Quaternion.LookRotation(move); 
            transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, rotateSpeed * Time.deltaTime ); 
        } 
        // Animation
        _animator.SetFloat("walk", move.magnitude);
        _animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));

        isMoving = move.magnitude > 0.1f && _controller.isGrounded;
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


    void Jump()
    {
        if (_controller.isGrounded && Input.GetKey(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            _animator.SetTrigger("Jump");
        }
    }

    void HandleFootstepSound()
    {
        if (isMoving)
        {
            if (!_audio.isPlaying)
            {
                _audio.clip = clip;
                _audio.loop = true;
                _audio.Play();
            }
        }
        else
        {
            if (_audio.isPlaying)
            {
                _audio.Stop();
            }
        }
    }

}
