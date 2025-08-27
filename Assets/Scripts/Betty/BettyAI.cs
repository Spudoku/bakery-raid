using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BettyAI : MonoBehaviour
{
    // most movement stats will come from Betty's Navmesh Agent
    [SerializeField] GameObject target;

    [SerializeField] bool isPursuing = false;
    [SerializeField] bool isWandering = false;
    [SerializeField] bool canSeeTarget = false;
    [SerializeField] float sightDistance = 10f;

    [SerializeField] LayerMask cantSeeThru;
    [Header("Wandering")]
    [SerializeField] float wanderSearchRadius = 10f;
    [Header("Timers")]
    [SerializeField] float targetCheckInterval = 2f;
    [SerializeField] float blindChaseInterval = 3f;         // how long Betty can chase target after line of sight is broken
    [SerializeField] float wanderInterval = 10f;            // how long Betty waits without a target (after blindChaseInterval runs out)
                                                            // until she starts wandering

    [Header("Sounds")]
    [SerializeField] private AudioSource footStepSource;
    [SerializeField] private AudioClip[] footStepClips;

    // misc components
    NavMeshAgent agent;
    Animator animator;

    [Header("Debug")]
    [SerializeField] private float targetCheckTimer;
    [SerializeField] private float blindChaseTimer;
    [SerializeField] private float wanderWaitTimer;
    [SerializeField] private Vector2 lastKnownPosition;

    private bool isWalking;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        SetSearchingState();
        isWalking = agent.velocity != Vector3.zero;
        animator.SetBool("isWalking", isWalking);
    }



    void MoveTowardsTarget(Vector2 pos)
    {
        if (canSeeTarget)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            agent.SetDestination(lastKnownPosition);
        }

    }



    #region Target Finding

    // control CanSeeTarget, isPursuing and related timers
    private void SetSearchingState()
    {
        canSeeTarget = CanSeeTarget();
        if (canSeeTarget)
        {
            lastKnownPosition = target.transform.position;

            wanderWaitTimer = wanderInterval;
            blindChaseTimer = blindChaseInterval;
            isPursuing = true;
            isWandering = false;
            SetTargetDestination(lastKnownPosition);
        }
        else
        {
            blindChaseTimer -= Time.deltaTime;
            if (blindChaseTimer < 0)
            {
                wanderWaitTimer -= Time.deltaTime;
                if (wanderWaitTimer < 0)
                {


                }
                else
                {
                    // hold in place until allowed to wander
                    SetTargetDestination(transform.position);
                }


            }
            else
            {
                wanderWaitTimer = wanderInterval;
                SetTargetDestination(lastKnownPosition);
                isPursuing = true;
                isWandering = false;
            }
        }
    }

    private void SetTargetDestination(Vector2 destination)
    {
        agent.SetDestination(destination);
    }

    private Vector2 ChooseWanderPoint(Vector2 defaultPos, float radius, int tries = 5)
    {

        Vector2 position = defaultPos;
        for (int i = 0; i < tries; i++)
        {
            Vector2 point = Random.insideUnitCircle * radius;
            if ()
            {

            }
        }
        return position;
    }

    private bool CanSeeTarget()
    {
        bool canSeeTarget = LineOfSight.CanSeeGO(transform, target, sightDistance, cantSeeThru);
        Debug.Log($"[BettyAI.CanSeeTarget] trying to see target!");
        return canSeeTarget;
    }
    #endregion

    #region Sound Effects
    public void PlayFootStepSFX()
    {
        RandomSound.Play(footStepSource, footStepClips);
    }
    #endregion
}
