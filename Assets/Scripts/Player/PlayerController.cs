using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 2f;

    Animator animator;
    Rigidbody2D rb;

    [Header("Sounds")]
    [SerializeField] private AudioSource footStepSource;
    [SerializeField] private AudioClip[] footStepClips;

    private bool isWalking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.Translate(speed * Time.deltaTime * InputManager.Movement);
        Move();
        animator.SetBool("isWalking", isWalking);
    }

    void Move()
    {
        Vector2 movement = InputManager.Movement.normalized;
        isWalking = movement != Vector2.zero;
        rb.linearVelocity = movement * speed;
    }

    public void PlayFootStepSFX()
    {
        RandomSound.Play(footStepSource, footStepClips);
    }
}
