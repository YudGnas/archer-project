using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem.XR;
public class Player_Fire : MonoBehaviour
{
    [SerializeField] private Player_Controller controller;
    [SerializeField] private Player_Health player_Health;
    [SerializeField] private Player_Infor player_Infor;
    [SerializeField] private shield _shield;

    [SerializeField] public Player_Controller _Controller => controller;


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;


    private bool isAttacking;



    private float _timebetweefire;
    public float timebetweefire = 0.5f;

    public Transform checkpoint;
    [Header("Skill")]
    [SerializeField] private GameObject SkillQPrefab;
    [SerializeField] private GameObject SkillEPrefab;
    [SerializeField] private GameObject SkillRPrefab;

    [SerializeField] private SkillBase SkillQ;
    [SerializeField] private SkillBase SkillE;
    [SerializeField] private SkillBase SkillR;


    
    public Image CooldownQ;
    public Image CooldownE;
    public Image CooldownR;
    public Image CooldownShield;
    public TextMeshProUGUI cooldownQText;
    public TextMeshProUGUI cooldownEText;
    public TextMeshProUGUI cooldownRText;
    private float timeQ;
    private float timeE;
    private float timeR;
    private float time_Shield;
    




    void Start()
    {
        controller = GetComponent<Player_Controller>(); 
        
        SkillQ = SkillQPrefab.GetComponent<SkillBase>();
        SkillE = SkillEPrefab.GetComponent<SkillBase>();
        SkillR = SkillRPrefab.GetComponent<SkillBase>();
    }

    void Update()
    {   
        _timebetweefire -= Time.deltaTime;
        timeQ -= Time.deltaTime; timeQ = Mathf.Clamp(timeQ, 0f, SkillQ.infor.cooldown);
        timeE -= Time.deltaTime; timeE = Mathf.Clamp(timeE, 0f, SkillE.infor.cooldown);
        timeR -= Time.deltaTime; timeR = Mathf.Clamp(timeR, 0f, SkillR.infor.cooldown);
        time_Shield -= Time.deltaTime; time_Shield = Mathf.Clamp(time_Shield, 0f, _shield.cooldown);
        UpdateSkillUI(CooldownQ, SkillQ, timeQ);
        UpdateSkillUI(CooldownE, SkillE, timeE);
        UpdateSkillUI(CooldownR, SkillR, timeR);
        UpdateShieldUI();
        if (Input.GetMouseButtonDown(1) && _timebetweefire <= 0 && player_Infor._Mana >= 10)
        {   
            Player_Rotation();           
            Attack("attack");
            StartCoroutine(ShootDelay(0.5f, bulletPrefab));
            controller._state = PlayerState.idel;

        }
        if (Input.GetKeyDown(KeyCode.E) && player_Infor._Mana >= SkillE.infor.manacost && timeE <= 0) 
        {        
            Player_Rotation();
            Attack("SkillE");
            timeE = SkillE.infor.cooldown;
            SkillE.Shoot(SkillEPrefab, firePoint);
            controller._state = PlayerState.idel;
        }
        if(Input.GetKeyDown(KeyCode.Q) && timeQ <= 0 && player_Infor._Mana >= SkillQ.infor.manacost)
        {
            
            Player_Rotation();
            Attack("SkillQ");
            timeQ = SkillQ.infor.cooldown;
            SkillQ.Shoot(SkillQPrefab, firePoint);
            player_Health.Energyconsumption(SkillQ.infor.manacost);
            controller._state = PlayerState.idel;
        }        
        if(Input.GetKeyDown(KeyCode.R) && timeR <= 0 && player_Infor._Mana >= SkillR.infor.manacost)
        {
            timeR = SkillR.infor.cooldown;
            Player_Rotation();
            Attack("SkillQ");
            SkillR.Shoot(SkillRPrefab, firePoint);
            player_Health.Energyconsumption(SkillR.infor.manacost);
            controller._state = PlayerState.idel;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            time_Shield = _shield.cooldown;
            _shield.UsingShield();
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

    void Shoot(GameObject bulletpre)
    {
        _timebetweefire = timebetweefire;

        player_Health.Energyconsumption(10);
        GameObject bullet = Instantiate(
            bulletpre,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;
    }

    private void Attack(string skillname)
    {
        //isAttacking = true;
        controller._state = PlayerState.attack;
        // Dừng di chuyển khi đánh (nếu muốn)
        controller._controller.Move(Vector3.zero);

        // Trigger animation
        controller._animator.SetTrigger(skillname);
        
    }
    public void CastAOE()
    {          
        player_Health.Energyconsumption(SkillE.infor.manacost);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 spawnPosition = ray.GetPoint(distance);

            GameObject aoe = Instantiate(SkillEPrefab, spawnPosition + new Vector3(0, 0.3f, 0), Quaternion.identity);
        }
    }
    public void UpdateSkillUI(Image skillimg, SkillBase skill, float cooldown)
    {
        float percent = cooldown / skill.infor.cooldown;
        skillimg.fillAmount = Mathf.Lerp(skillimg.fillAmount, percent, 10f*Time.deltaTime);
    }    
    public void UpdateShieldUI()
    {
        float percent = time_Shield / _shield.cooldown;
        CooldownShield.fillAmount = Mathf.Lerp(CooldownShield.fillAmount, percent, 10f*Time.deltaTime);
    }

    IEnumerator ShootDelay(float delay, GameObject bulletpre)
    {
        yield return new WaitForSeconds(delay);
        Shoot(bulletpre);
    }



}
