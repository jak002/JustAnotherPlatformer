using UnityEngine;

public class MonsterHit : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        }
    }
}
