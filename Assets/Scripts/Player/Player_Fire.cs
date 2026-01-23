using UnityEngine;

public class Player_Fire : MonoBehaviour
{
    [SerializeField] private Player_Controller controller;


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    private float _timebetweefire;

    public float timebetweefire;
    void Start()
    {
        
    }

    void Update()
    {   
        timebetweefire -= Time.deltaTime;

        Player_Rotation();

        if (Input.GetMouseButtonDown(0) && timebetweefire <= 0)
        {
            Shoot();
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
}
