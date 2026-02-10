using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine _StateMachine;
    private GameObject _player;
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    public GameObject Player => _player;

    [SerializeField]
    private string currentState;
    public Pathh _path;

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
        agent = GetComponent<NavMeshAgent>();
        _StateMachine = GetComponent<StateMachine>();
        _StateMachine.Initialise();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        currentState = _StateMachine.activeState.ToString();
    }

    public bool CanSeePlayer()
    {
        if(_path != null)
        {
            if(Vector3.Distance(transform.position, _player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = _player.transform.position - transform.position - (Vector3.up * eyeHigh);
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
}
