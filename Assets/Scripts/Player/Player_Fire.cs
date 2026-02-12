using UnityEngine;

public class Player_Fire : MonoBehaviour
{
    [SerializeField] private Player_Controller controller;

    [SerializeField] Player_Controller _Controller => controller;


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;

    private bool isAttacking;
    private float _timebetweefire;

    public float timebetweefire;
    void Start()
    {
        controller = GetComponent<Player_Controller>();
    }

    void Update()
    {   
        timebetweefire -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && timebetweefire <= 0)
        {   
            Player_Rotation();
            Attack();
            Invoke("Shoot", 0.5f);
            
        }
    }

    void Player_Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            Vector3 lookDir = hitPoint - transform.position;
            lookDir.y = 0f; // khóa trục Y

            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = targetRotation;
            }
        }
    }

    void Shoot()
    {
        timebetweefire = _timebetweefire;
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;
    }

    private void Attack()
    {
        //isAttacking = true;

        // Dừng di chuyển khi đánh (nếu muốn)
        controller._rb.linearVelocity = Vector3.zero;

        // Trigger animation
        controller._animator.SetTrigger("attack");
        
    }

}
