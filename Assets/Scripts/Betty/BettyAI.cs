using UnityEngine;
using UnityEngine.AI;

public class BettyAI : MonoBehaviour
{
    // most movement stats will come from Betty's Navmesh Agent
    [SerializeField] Transform target;

    [Header("Sounds")]
    [SerializeField] private AudioSource footStepSource;
    [SerializeField] private AudioClip[] footStepClips;

    // misc components
    NavMeshAgent agent;
    Animator animator;

    private bool isWalking;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        Movement();
        animator.SetBool("isWalking", isWalking);
    }

    void Movement()
    {
        agent.SetDestination(target.position);

        isWalking = agent.velocity != Vector3.zero;
    }

    public void PlayFootStepSFX()
    {
        RandomSound.Play(footStepSource, footStepClips);
    }
}
