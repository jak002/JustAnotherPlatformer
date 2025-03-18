using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [field: SerializeField]
    private float speed { get; set; } = 5.0f;

    private Rigidbody2D rb;

    Vector2 move = new Vector2();

    [field: SerializeField]
    public UnityEvent OnCollect { get; set;}

    [field: SerializeField]
    public UnityEvent OnHitEnemy { get; set;}

    [field: SerializeField]
    public UnityEvent OnDie { get; set;}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            move.y += speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            move.x -= speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            move.x += speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            move.y -= speed;
        }

        rb.MovePosition(rb.position + (move * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision: " +  collision.gameObject);
        Debug.Log("Tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Monster1")
        {
            OnDie?.Invoke();
        }
    }
}
