using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementplat : MonoBehaviour
{
    [field: SerializeField]
    private float horizontalSpeed { get; set; } = 12.0f; // Run speed
    public float maxVerticalSpeed = 15f; // Maximum vertical speed
    public int maxJumps = 3;
    private int jumps;

    [field: SerializeField]
    private float jumpVelocity { get; set; } = 1000f; // Jump velocity

    private Rigidbody2D rb;

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
        jumps = maxJumps;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && jumps > 0) //if the "jump" key is pressed (not held) and the player still has jumps left)
        {
            Jump();
        }
        float moveInput = Input.GetAxisRaw("Horizontal");
        // returns 1 if moving to the right, -1 if moving to the left, 0 if nada. "raw" means there's no smoothing.

        RunAnimation(moveInput);
        RunPhysics(moveInput);

    }

    private void RunPhysics(float moveInput)
    {
        rb.linearVelocity = new Vector2(moveInput * horizontalSpeed, rb.linearVelocity.y);
        // sets x-axis velocity to whatever the speed is, times whatever direction is pressed. if no direction is pressed, it'll be multiplied by 0.

        if (rb.linearVelocity.y > maxVerticalSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxVerticalSpeed);
        }
        else if (rb.linearVelocity.y < -maxVerticalSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxVerticalSpeed);
        } // sets maximum vertical velocity
    }

    private void Jump()
    {
        animator.SetTrigger("jump");
        animator.SetBool("grounded", false); //also handles animation
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset velocity, so in-air jumps don't launch the player into the stratosphere
        rb.AddForce(new Vector2(rb.linearVelocity.x, jumpVelocity)); // we have liftoff!
        jumps--; // one jump used up.
    }

    private void RunAnimation(float moveInput)
    {
        //all of this is just animation stuff
        if (moveInput < 0)
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
        }
        else
        {
            animator.SetBool("running", false);
        }
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
            jumps = maxJumps;
            animator.SetBool("grounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 && collision.transform.position.y < gameObject.transform.position.y-0.5)
        {
            animator.SetBool("grounded", false);
        }
    }
}
