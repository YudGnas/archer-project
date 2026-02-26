using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public GameObject aoePrefab;
    public GameObject warningCirclePrefab;
    public GameObject fireballPrefab;
    public Transform firePoint;
    [SerializeField] private LayerMask playerLayer;


    public float maxPoise = 100f;
    public float currentPoise;
    public float currentHP;


    public float staggerDuration = 5f;
    private bool isStaggered;

    public bool IsPhase2 => currentHP <= enemy_Infor.maxHP * 0.5f;


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

        if (currentPoise <= 0 && !isStaggered)
        {
            StartCoroutine(Stagger());
        }

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
