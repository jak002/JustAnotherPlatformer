using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    private float attackTimer;
    private Animator animator;
    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        if (distance < attackRange && attackTimer <= 0)
        {
            animator.SetTrigger("Attack");
            attackTimer = attackCooldown;

            // Do damage to player here if in range
            if (Vector2.Distance(transform.position, player.position) < 1.5f)
            {
                player.GetComponent<Health>()?.TakeDamage(1);
            }
        }
    }
}
