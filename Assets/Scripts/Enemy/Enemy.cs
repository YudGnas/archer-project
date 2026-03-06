using GLTFast.Schema;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyRole
{
    CloseCombat,
    Ranged,
    Boss
}

public class Enemy : MonoBehaviour
{
    public Enemy_Infor enemy_Infor;
    [SerializeField] public Animator _animator;
    private StateMachine _StateMachine;

    public StateMachine stateMachine => _StateMachine;
    private GameObject _player;
    private NavMeshAgent agent;
    private Vector3 lastKnowPos;
    public NavMeshAgent Agent => agent;
    public GameObject Player => _player;
    private Player_Health _Player_Health;

    public Vector3 LastKnowPos { get => lastKnowPos; set => lastKnowPos = value; }

    [SerializeField]
    private string currentState;
    public Pathh _path;
    public GameObject DebugSphere;


    public float currentHP;

    [Header("SightDistance")]
    public float sightDistance = 20f;
    public float fieldview = 85f;
    public float eyeHigh;

    [Header("Weapons Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10)]
    public float fireRate;
    public GameObject bullet;
    void Start()
    {   
        currentHP = enemy_Infor.maxHP;
        agent = GetComponent<NavMeshAgent>();
        _StateMachine = GetComponent<StateMachine>();
        _StateMachine.Initialise();
        _player = GameObject.FindGameObjectWithTag("Player");
        _Player_Health = _player.GetComponent<Player_Health>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Agent.velocity.magnitude;

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

    public virtual bool CanSeePlayer()
    {
        if(_path != null)
        {
            if(Vector3.Distance(transform.position, _player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = (_player.transform.position
                                            - transform.position);//.normalized;
                float angletoPlayer = Vector3.Angle(targetDirection, transform.forward);

                if(angletoPlayer >= -fieldview && angletoPlayer <= fieldview)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHigh), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if(hitInfo.transform.gameObject == _player)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);
    }

    public virtual void TakeDamege(float damege)
    {
        currentHP -= damege;
        _animator.SetTrigger("GetHit");
        Debug.Log(currentHP);
        if (currentHP <= 0)
        {
            _animator.SetTrigger("die");
            _Player_Health.GetXp(30);
            Destroy(gameObject, 1f);
        }
    }
}
