using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public GameObject aoePrefab;
    public GameObject warningCirclePrefab;
    public GameObject fireballPrefab;
    public Transform firePoint;


    public float maxPoise = 100f;
    public float currentPoise;

    public float staggerDuration = 5f;
    private bool isStaggered;

    public bool IsPhase2 => enemy_Infor._HP <= enemy_Infor._maxHP * 0.5f;


    public Enemy_Infor enemy_Infor;
    [SerializeField] public Animator _animator;
    private BossStateMachine _StateMachine;
    private GameObject _player;
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
        enemy_Infor._HP = enemy_Infor._maxHP;
        agent = GetComponent<NavMeshAgent>();
        _StateMachine = GetComponent<BossStateMachine>();
        _StateMachine.Initialise();
        _player = GameObject.FindGameObjectWithTag("Player");
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
                Vector3 targetDirection = Vector3.ProjectOnPlane(_player.transform.position - transform.position,Vector3.up).normalized;
                float angletoPlayer = Vector3.Angle(targetDirection, transform.forward);

                if (angletoPlayer >= -fieldview && angletoPlayer <= fieldview)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHigh), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == _player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }

                }
            }
        }

        return false;
    }

    /*public bool CanSeePlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, sightDistance);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Vector3 dir = hit.transform.position - transform.position;
                Ray ray = new Ray(transform.position + Vector3.up * eyeHigh, dir);

                if (Physics.Raycast(ray, out RaycastHit info, sightDistance))
                {
                    if (info.transform.CompareTag("Player"))
                        return true;
                }
            }
        }

        return false;
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);
    }

    public void TakeDamage(int damage, float poiseDamage)
    {
        enemy_Infor._HP -= damage;
        _animator.SetTrigger("GetHit");
        Debug.Log(enemy_Infor._HP);

        currentPoise -= poiseDamage;
        Debug.Log(currentPoise);

        if (currentPoise <= 0 && !isStaggered)
        {
            StartCoroutine(Stagger());
        }

        if (enemy_Infor._HP <= 0)
        {
            _animator.SetTrigger("die");
            Destroy(gameObject, 1f);
        }
    }

    /*private void Start()
    {
        currentPoise = maxPoise;
    }*/

    private IEnumerator Stagger()
    {
        isStaggered = true;
        currentPoise = maxPoise;

        Agent.isStopped = true;
        _animator.SetTrigger("stagger");

        yield return new WaitForSeconds(staggerDuration);

        Agent.isStopped = false;
        isStaggered = false;
    }
}
