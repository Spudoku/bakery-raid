
using NavMeshPlus.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BettyAI : MonoBehaviour
{
    // most movement stats will come from Betty's Navmesh Agent
    public GameObject target;

    [SerializeField] bool isPursuing = false;
    [SerializeField] bool isWandering = false;
    [SerializeField] bool canSeeTarget = false;

    [SerializeField] bool isStunned = false;
    [SerializeField] float sightDistance = 10f;

    [SerializeField] LayerMask cantSeeThru;
    [Header("Pursuing")]
    [SerializeField] float minPursueSoundInterval = 3f;
    [SerializeField] float maxPursueSoundInterval = 10f;


    [Header("Wandering")]
    [SerializeField] float wanderSearchRadius = 10f;
    [SerializeField] Vector2 wanderPoint;
    [Header("Timers")]
    [SerializeField] float blindChaseInterval = 3f;         // how long Betty can chase target after line of sight is broken
    [SerializeField] float wanderInterval = 10f;            // how long Betty waits without a target (after blindChaseInterval runs out)
                                                            // until she starts wandering
    [SerializeField] float flourStun = 5f;

    [SerializeField] float pursuitSoundTimer;

    [Header("Sounds")]
    [SerializeField] private AudioSource footStepSource;
    [SerializeField] private AudioClip[] footStepClips;

    [SerializeField] private AudioSource laughterSource;
    [SerializeField] private AudioClip[] laughterClips;

    [SerializeField] private AudioSource coughSource;
    [SerializeField] private AudioClip[] coughClips;

    [SerializeField] private float minCoughInterval = 1f;
    [SerializeField] private float maxCoughInterval = 2f;
    private float coughTimer;

    // misc components
    NavMeshAgent agent;
    Animator animator;

    [Header("Debug")]
    [SerializeField] private float targetCheckTimer;
    [SerializeField] private float blindChaseTimer;
    [SerializeField] private float wanderWaitTimer;
    [SerializeField] private Vector2 lastKnownPosition;

    [SerializeField] private float laughTimer;

    [SerializeField] private float stunTimer;
    public float flourTimer;

    public LevelManager levelManager;

    private bool isWalking;

    [SerializeField] private bool hardMode = false;
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
        stunTimer -= Time.deltaTime;
        coughTimer -= Time.deltaTime;
        flourTimer -= Time.deltaTime;
        // trigger "Hardmode" if player has the recipe
        if (levelManager.playerHasRecipe && !hardMode)
        {
            HardMode();
        }
        if (stunTimer > 0 || flourTimer > 0)
        {
            isStunned = true;
            laughterSource.Stop();
        }
        else
        {
            isStunned = false;
            coughTimer = minCoughInterval;
        }

        if (flourTimer > 0 && coughTimer < 0)
        {
            // play sounds
            PlayCoughSFX();

        }



        SetSearchingState();
        isWalking = agent.velocity != Vector3.zero;
        animator.SetBool("isWalking", isWalking);
        if (isPursuing)
        {
            if (laughTimer < 0)
            {
                laughTimer = Random.Range(minPursueSoundInterval, maxPursueSoundInterval);
                AudioClip clip = laughterClips[Random.Range(0, laughterClips.Length)];
                laughterSource.clip = clip;
                laughTimer += clip.length;
                laughterSource.Play();

            }
            else
            {
                laughTimer -= Time.deltaTime;
            }
        }
        else
        {
            laughTimer = 0f;
        }
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
        if (isStunned)
        {
            isPursuing = false;
            isWandering = false;
            SetTargetDestination(transform.position);
            return;
        }
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
                isPursuing = false;
                wanderWaitTimer -= Time.deltaTime;
                if (wanderWaitTimer < 0)
                {
                    if (!isWandering || Vector2.Distance(transform.position, wanderPoint) < 0.25f)
                    {
                        wanderPoint = ChooseWanderPoint(transform.position, wanderSearchRadius, 10);
                    }
                    isWandering = true;

                    SetTargetDestination(wanderPoint);
                    Debug.DrawLine(transform.position, wanderPoint, Color.green, 2f);
                }
                else
                {
                    isWandering = false;
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
            Vector2 point = UnityEngine.Random.insideUnitCircle * radius;
            if (NavMesh.SamplePosition(point, out var hit, radius, NavMesh.AllAreas))
            {

                return point;
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

    private void PlayCoughSFX()
    {
        AudioClip clip = coughClips[Random.Range(0, coughClips.Length)];
        coughTimer += clip.length + Random.Range(minCoughInterval, maxCoughInterval);
        coughSource.clip = clip;
        coughSource.Play();
    }
    #endregion

    public void Stun(float time)
    {
        stunTimer += time;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            levelManager?.Lose();
        }
    }

    // make betty a lot scarier once the player has the recipe
    private void HardMode()
    {
        wanderInterval *= 0.5f;
        blindChaseInterval *= 1.5f;
        agent.speed *= 1.25f;
        hardMode = true;
    }
}
