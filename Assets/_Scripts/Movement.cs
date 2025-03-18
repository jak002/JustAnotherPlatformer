using UnityEngine;

public class Movement : MonoBehaviour
{
    [field: SerializeField]
    public float speed { get; set; } = 2.0f;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveUp = new Vector2(0, speed);
        rb.MovePosition(rb.position + (moveUp * Time.deltaTime));
    }
}
