using UnityEngine;

public class Movement2ax : MonoBehaviour
{
    [field: SerializeField]
    public float speedx { get; set; } = 2.0f;

    [field: SerializeField]
    public float speedy { get; set; } = 2.0f;

    private Rigidbody2D rb;
    private SpriteRenderer spirt;

    Vector2 move = new Vector2();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spirt = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move.x = speedx;
        move.y = speedy;
        rb.MovePosition(rb.position + (move * Time.deltaTime));
        move = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            rb.MovePosition(rb.position - (move * Time.deltaTime));
            speedx = speedx * -1;
            speedy = speedy * -1;
            spirt.flipX = !spirt.flipX;
        }
        Debug.Log(collision.gameObject.layer);
    }
}
