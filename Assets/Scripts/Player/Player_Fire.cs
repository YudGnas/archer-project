using UnityEngine;

public class Player_Fire : MonoBehaviour
{
    [SerializeField] private Player_Controller controller;
    [SerializeField] private Player_Health player_Health;
    [SerializeField] private Player_Infor player_Infor;

    [SerializeField] public Player_Controller _Controller => controller;


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float reconvertMana = 2f;

    private bool isAttacking;



    private float _timebetweefire;
    public float timebetweefire = 0.5f;
    private float _timeE;
    public float timeE = 5f;
    public float timeReconvertMana;
    [Header("Skill E")]
    [SerializeField] private GameObject aoePrefab;
    [SerializeField] private float aoeManaCost = 30f;

    
    void Start()
    {
        controller = GetComponent<Player_Controller>();
    }

    void Update()
    {   
        _timebetweefire -= Time.deltaTime;
        timeReconvertMana += Time.deltaTime;
        _timeE -= Time.deltaTime;

        if (timeReconvertMana > 1)
        {
            player_Health.Energyrecovery(reconvertMana);
            timeReconvertMana = 0;
        }
        if (Input.GetMouseButtonDown(0) && _timebetweefire <= 0 && player_Infor._Mana >= 10)
        {   
            Player_Rotation();           
            Attack("attack");
            Invoke("Shoot", 0.5f);
            
        }
        if (Input.GetKeyDown(KeyCode.E) && player_Infor._Mana >= aoeManaCost && _timeE <= 0) 
        {
            Player_Rotation();
            Attack("SkillE");
            Invoke("CastAOE", 0.5f);
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
        _timebetweefire = timebetweefire;

        player_Health.Energyconsumption(10);
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;
    }

    private void Attack(string skillname)
    {
        //isAttacking = true;

        // Dừng di chuyển khi đánh (nếu muốn)
        controller._rb.linearVelocity = Vector3.zero;

        // Trigger animation
        controller._animator.SetTrigger(skillname);
        
    }
    public void CastAOE()
    {
        _timeE = timeE;
        player_Health.Energyconsumption(aoeManaCost);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 spawnPosition = ray.GetPoint(distance);

            GameObject aoe = Instantiate(aoePrefab, spawnPosition + new Vector3(0, 0.3f, 0), Quaternion.identity);
        }
    }
}
