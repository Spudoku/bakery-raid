using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class FlourProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.5f;
    [SerializeField] private float flourTime = 5f;
    [SerializeField] private float radius = 3f;

    private AudioSource audioSource;

    private ParticleSystem particle;
    private bool hasHitBetty = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            lifeTime += audioSource.clip.length;
        }
        hasHitBetty = false;
        particle.Play();
        StartCoroutine(DespawnSequence());

    }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.TryGetComponent<BettyAI>(out var bettyAI) && !hasHitBetty)
    //     {
    //         Debug.Log($"[FlourProjectile] hit betty!");
    //         hasHitBetty = true;
    //         bettyAI.flourTimer += flourTime;
    //     }
    // }

    private IEnumerator DespawnSequence()
    {
        float timeElapsed = 0f;
        Collider2D collider = GetComponent<Collider2D>();
        while (timeElapsed < lifeTime)
        {
            timeElapsed += Time.deltaTime;
            if (!hasHitBetty)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
                foreach (Collider2D testCOllider in colliders)
                {


                    if (testCOllider.gameObject.TryGetComponent<BettyAI>(out var bettyAI) && !hasHitBetty)
                    {
                        Debug.Log($"[FlourProjectile] hit betty!");
                        hasHitBetty = true;
                        bettyAI.flourTimer += flourTime;
                        break;
                    }

                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }




}
