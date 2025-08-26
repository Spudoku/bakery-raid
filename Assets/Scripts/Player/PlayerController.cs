using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.Translate(speed * Time.deltaTime * InputManager.Movement);
        Move();
    }

    void Move()
    {
        Vector2 movement = InputManager.Movement.normalized;
        rb.linearVelocity = movement * speed;
    }
}
