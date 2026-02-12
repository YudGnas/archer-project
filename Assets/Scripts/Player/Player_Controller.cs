using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private Player_Infor Player_Infor;
    [SerializeField] private Player_Fire Player_Fire;
    public Player_Infor _player_Infor => Player_Infor;
    public Player_Fire _player_Fire => Player_Fire;

    
    [SerializeField] public Rigidbody _rb;
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] public Animator _animator;

    void Start()
    {
        
    }

    /*void Update()
    {
        float playermovex = Input.GetAxisRaw("Horizontal");
        float playermovey = Input.GetAxisRaw("Vertical");

        // Lấy hướng theo local của player
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Loại bỏ thành phần Y để tránh bay lên/xuống nếu player bị nghiêng
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Di chuyển theo hướng player đang nhìn
        Vector3 move = forward * playermovey + right * playermovex;

        _rb.linearVelocity = move.normalized * Player_Infor._Speed;
    }*/

    private void FixedUpdate()
    {
        float playermovex = Input.GetAxisRaw("Horizontal");
        float playermovey = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(playermovex, 0f, playermovey).normalized;

        // Di chuyển theo world axis
        _rb.linearVelocity = move * Player_Infor._Speedwalk;


        //Animation
        _animator.SetFloat("walk", Mathf.Abs(playermovey + playermovex));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _animator.SetBool("isRunning", true);
            _rb.linearVelocity = move * Player_Infor._Speedrun;
        }
        else
        {
            _animator.SetBool("isRunning", false);
            _rb.linearVelocity = move * Player_Infor._Speedwalk;
        }
        // Nếu có di chuyển thì quay mặt theo hướng đó
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }
    }
}
