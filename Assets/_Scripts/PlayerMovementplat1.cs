using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementplat1 : MonoBehaviour
{
    [field: SerializeField]
    private float speed { get; set; } = 5.0f;
    public float maxVerticalSpeed = 15f; // Maximum vertical speed
    public int maxjumps = 3;
    private int jumps;

    [field: SerializeField]
    private float jump { get; set; }

    private Rigidbody2D rb;

    Vector2 move = new Vector2();

    [field: SerializeField]
    public UnityEvent OnCollect { get; set;}

    [field: SerializeField]
    public UnityEvent OnHitEnemy { get; set;}

    [field: SerializeField]
    public UnityEvent OnDie { get; set;}

    private Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        jumps = maxjumps;
    }

    private void Update()
    {
        animator.SetBool("jump", false);
        if (Input.GetKeyDown(KeyCode.W) && jumps > 0)
        {
            animator.SetBool("jump", true);
            animator.SetBool("grounded", false);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(rb.linearVelocity.x, jump));
            jumps--;

        }
        float moveInput = Input.GetAxis("Horizontal");
        if(moveInput < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (moveInput > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        if (moveInput != 0)
        {
            animator.SetBool("running", true);
        } else
        {
            animator.SetBool("running", false);
        }

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        if (rb.linearVelocity.y > maxVerticalSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxVerticalSpeed);
        }
        else if (rb.linearVelocity.y < -maxVerticalSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxVerticalSpeed);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //move = Vector2.zero;

        //if (Input.GetKey(KeyCode.A))
        //{
        //    move.x -= speed;
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    move.x += speed;
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    move.y -= speed;
        //}

        //rb.MovePosition(rb.position + (move * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision: " +  collision.gameObject);
        Debug.Log("Tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Monster1")
        {
            OnDie?.Invoke();
        }

        if (collision.gameObject.layer == 9 && collision.GetContact(0).point.y < gameObject.transform.position.y)
        {
            jumps = maxjumps;
            animator.SetBool("grounded", true);
        }
    }
}
