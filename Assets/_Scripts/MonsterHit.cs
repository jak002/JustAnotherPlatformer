using UnityEngine;

public class MonsterHit : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11 && collision.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            Debug.LogWarning("Oof I've been stomp'ded");

            
            if (animator != null)
            {
                animator.SetTrigger("Hit"); // Replace "hitTrigger" with your actual trigger name
            }

            // Take damage!
            GetComponent<Health>()?.TakeDamage(3);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Debug.LogWarning("Ouchie ouch I've been sword'd");

            
            if (animator != null)
            {
                animator.SetTrigger("Hit"); // Replace "hitTrigger" with your actual trigger name
            }

            // Take damage!
            GetComponent<Health>()?.TakeDamage(1);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (boxCollider != null)
            boxCollider.enabled = false;

        if (animator != null)
            animator.SetTrigger("Die");

        GetComponent<MonsterPatrol>()?.Die();

        Destroy(gameObject, 3f);
    }
}
