using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public GameObject aoePrefab;
    public GameObject fireballPrefab;
    public GameObject rockprefab;
    public Transform firePoint;
    [SerializeField] private LayerMask playerLayer;


    public float maxPoise = 100f;
    public float currentPoise;
    public float currentHP;

    public float staggerDuration = 5f;
    private bool isStaggered;

    [SerializeField] private int bulletCount => IsPhase2() ? 60 : 20;
    [SerializeField] private float spreadAngle = 5f;

    public bool IsPhase2()
    {   
        if(currentHP < enemy_Infor.maxHP / 2)
        {
            currentHP = enemy_Infor.maxHP;
            return true;
        }
        else 
            return false;       
    }


    public Enemy_Infor enemy_Infor;
    [SerializeField] public Animator _animator;
    private BossStateMachine _StateMachine;
    private GameObject _player;

    private Player_Health _Player_Health;
    private NavMeshAgent agent;
    private Vector3 lastKnowPos;
    public NavMeshAgent Agent => agent;
    public GameObject Player => _player;

    public Vector3 LastKnowPos { get => lastKnowPos; set => lastKnowPos = value; }

    [SerializeField]
    private string currentState;
    public Pathh _path;
    public GameObject DebugSphere;

    [Header("SightDistance")]
    public float sightDistance = 20f;
    public float fieldview = 85f;
    public float eyeHigh;

    void Start()
    {
        currentHP = enemy_Infor.maxHP;
        agent = GetComponent<NavMeshAgent>();
        _StateMachine = GetComponent<BossStateMachine>();
        _StateMachine.Initialise();
        _player = GameObject.FindGameObjectWithTag("Player");
        _Player_Health = _player.GetComponent<Player_Health>();
    }

    void Update()
    {
        float speed = Agent.velocity.magnitude;
        Debug.Log(CanSeePlayer());
        if (speed > 0.1f)
        {
            _animator.SetBool("ismoving", true);
        }
        else
        {
            _animator.SetBool("ismoving", false);
        }



        CanSeePlayer();
        currentState = _StateMachine.activeState.ToString();
        DebugSphere.transform.position = lastKnowPos;
    }
    public bool CanSeePlayer()
    {
        if (_path != null)
        {
            if (Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(transform.position.x, 0, transform.position.z)) < sightDistance)
            {
                //Vector3 targetDirection = Vector3.ProjectOnPlane(_player.transform.position - transform.position,Vector3.up).normalized;
                Vector3 targetDirection = (_player.transform.position
                            - transform.position).normalized;
                float angletoPlayer = Vector3.Angle(targetDirection, transform.forward);

                if (angletoPlayer >= -fieldview && angletoPlayer <= fieldview)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHigh), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance, playerLayer))
                    {
                        if (hitInfo.transform.gameObject == _player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                            return true;
                        }
                    }

                }
            }
        }

        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);
    }

    public void TakeDamage(float damage, float poiseDamage)
    {
        currentHP -= damage;
        _animator.SetTrigger("GetHit");
        Debug.Log(currentHP);

        currentPoise -= poiseDamage;
        Debug.Log(currentPoise);

        //if (currentPoise <= 0 && !isStaggered)
        //{
            //StartCoroutine(Stagger());
        //}

        if (currentHP <= 0)
        {
            _animator.SetTrigger("die");
            _Player_Health.GetXp(100);
            Destroy(gameObject, 1f);
        }
    }

    /*private void Start()
    {
        currentPoise = maxPoise;
    }*/
    public IEnumerator AOEAttack()
    {


        yield return new WaitForSeconds(1f);

        float duration = 3f;        // thời gian mưa đá
        float spawnDelay = 0.2f;    // khoảng cách giữa mỗi viên
        float radius = 8f;          // bán kính quanh boss

        float timer = 0f;

        while (timer < duration)
        {
            Vector3 randomPos = Player.transform.position +
                new Vector3(
                    Random.Range(-radius, radius),
                    0,
                    Random.Range(-radius, radius)
                );


            GameObject rocks = GameObject.Instantiate(aoePrefab, randomPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
            timer += spawnDelay;
        }
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.5f);

        if (firePoint == null || fireballPrefab == null) yield break;

        Transform gunBarrel = firePoint;

        int half = bulletCount / 2;

        for (int i = -half; i <= half; i++)
        {
            GameObject bullet = GameObject.Instantiate(
                fireballPrefab,
                gunBarrel.position,
                Quaternion.identity
            );

            Vector3 targetPos = Player.transform.position;
            targetPos.y = gunBarrel.position.y;

            Vector3 shootDirection = (targetPos - gunBarrel.position).normalized;

            Vector3 spreadDirection =
                Quaternion.AngleAxis(i * spreadAngle, Vector3.up) * shootDirection;

            float bulletSpeed = IsPhase2() ? 45f : 25f;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = spreadDirection * bulletSpeed;
            }
        }
    }
    public IEnumerator RockAttack()
    {
        yield return new WaitForSeconds(0.5f);

        if (firePoint == null || fireballPrefab == null) yield break;

        Transform gunBarrel = firePoint;

        GameObject bullet = GameObject.Instantiate(
            rockprefab,
            gunBarrel.position,
            Quaternion.identity
        );

        Vector3 targetPos = Player.transform.position;
        targetPos.y = gunBarrel.position.y;

        Vector3 shootDirection = (targetPos - gunBarrel.position).normalized;

        Vector3 spreadDirection =
            Quaternion.AngleAxis(0, Vector3.up) * shootDirection;

        float bulletSpeed = IsPhase2() ? 45f : 25f;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = spreadDirection * bulletSpeed;
        }
    }
}
