using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementTest : MonoBehaviour
{
    [field: SerializeField]
    private float horizontalSpeed { get; set; } = 12.0f; // Run speed
    public float maxVerticalSpeed = 15f; // Maximum vertical speed
    public int maxJumps = 3;
    private int jumps;

    [field: SerializeField]
    private float jumpVelocity { get; set; } = 1000f; // Jump velocity

    private Rigidbody2D rb;
    private BoxCollider2D hitbox;
    private bool isAttacking;
    private float attackDuration = 0.2f;
    private float attackTimer;

    [field: SerializeField]
    public UnityEvent OnCollect { get; set; }

    [field: SerializeField]
    public UnityEvent OnHitEnemy { get; set; }

    [field: SerializeField]
    public UnityEvent OnDie { get; set; }

    [field: SerializeField]
    public UnityEvent OnJump { get; set; }

    [field: SerializeField]
    public UnityEvent OnAttack { get; set; }

    private Animator animator;

    [field: SerializeField]
    private LayerMask groundLayer;

    private bool isJumping;
    private float jumpCooldown = 0.2f; // Cooldown period after jumping
    private float jumpTimer;

    private bool dead = false;
    private float deathLength = 3f;
    private float deathTimer;
    private Vector2 startpos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        hitbox = transform.GetChild(2).GetComponent<BoxCollider2D>();
        jumps = maxJumps;
        startpos = GameObject.Find(gameObject.name).transform.position;
    }

    private void Update()
    {

        if (dead)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                gameObject.transform.position = startpos;
                GetComponent<Health>().currentHealth = GetComponent<Health>().maxHealth;
                // FindFirstObjectByType<HealthUI>().UpdateHearts(GetComponent<Health>().currentHealth); ------------------------------------------------
                dead = false;
                animator.SetTrigger("respawned");
            }
            return;
        }
        ;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && jumps > 0) //if the "jump" key is pressed (not held) and the player still has jumps left)
        {
            Jump();
        }
        float moveInput = Input.GetAxisRaw("Horizontal");
        // returns 1 if moving to the right, -1 if moving to the left, 0 if nada. "raw" means there's no smoothing.

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("attack");
            OnAttack.Invoke();
            hitbox.enabled = true;
            attackTimer = attackDuration;
            isAttacking = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("died");
        }

        RunAnimation(moveInput);
        RunPhysics(moveInput);
        CheckGroundAndWalls();

        if (isJumping)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
            {
                isJumping = false;
            }
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                hitbox.enabled = false;
                isAttacking = false;
            }
        }
    }

    public void TriggerDeath()
    {
        dead = true;
        deathTimer = deathLength;
        animator.SetTrigger("died");
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
        OnJump.Invoke();
        animator.SetTrigger("jump");
        animator.SetBool("grounded", false); //also handles animation
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset velocity, so in-air jumps don't launch the player into the stratosphere
        rb.AddForce(new Vector2(rb.linearVelocity.x, jumpVelocity)); // we have liftoff!
        jumps--; // one jump used up.
        isJumping = true;
        jumpTimer = jumpCooldown;
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
        Debug.Log("Collision: " + collision.gameObject);
        Debug.Log("Tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Monster1" && gameObject.transform.position.y <= collision.gameObject.transform.position.y)
        {
            Health playerHealth = gameObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
            // Removed dead logic to be handled elsewhere
            //OnDie?.Invoke();
            //animator.SetTrigger("died");
            //dead = true;
            //deathTimer = deathLength;
        }
    }

    private void CheckGroundAndWalls()
    {

        if (!isJumping)
        {
            RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 0.52f, groundLayer);
            RaycastHit2D hitLeftWall = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, groundLayer);
            RaycastHit2D hitRightWall = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, groundLayer);

            if (hitGround.collider != null)
            {
                jumps = maxJumps;
                animator.SetBool("grounded", true);
            }
            else
            {
                animator.SetBool("grounded", false);
            }

            bool wallDetected = (hitLeftWall.collider != null || hitRightWall.collider != null);
            if (wallDetected)
            {
                animator.SetBool("walld", true);
                jumps = maxJumps;
            }
            else if (!wallDetected)
            {
                animator.SetBool("walld", false);
            }
        }
    }
}
