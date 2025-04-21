using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3; // max health is 3
    public int currentHealth; // current health the player and monster has
    private float damageCooldown = 1f; // 1 second cooldown for monster contact damage
    private float lastDamageTime = -999f; // time of last damage taken

    public bool isPlayer = false; // is this the player or a monster? if monster set true on unity editor

    private Animator animator; // set to the animator component on the player or monster to play the hit and daed animation

    private void Start()
    {
        currentHealth = maxHealth; // start with max health
        // get the animator component from the child object
        animator = GetComponentInChildren<Animator>();
        Debug.Log("Animator object: " + animator.gameObject.name);
    }

    // function called when the player or monster takes damage
    public void TakeDamage(int amount)
    {
        if (isPlayer)
        {
            // Only apply cooldown to the player (when monster attacks player)
            if (Time.time - lastDamageTime < damageCooldown) return;
            lastDamageTime = Time.time;
        }

        currentHealth -= amount; // subtract the amount of damage taken from current health

        // if there is an animator component, play the hit animation
        if (animator != null)
        {
            animator.SetTrigger("Hit");
            Debug.Log("Hit trigger called on animator");
        }

        // if the player is taking damage, update the health UI
        if (isPlayer)
        {
            // FindFirstObjectByType<HealthUI>().UpdateHearts(currentHealth);-----------------

            var sound = GetComponent<PlayerSounds>();
            sound?.HitByEnemy(); // this plays the "hit" sound when the player takes damage

        }

        // check if the current health is less than or equal to 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // function called when the player or monster dies
    private void Die()
    {
        // play the death animation
        if (animator != null)
        {
            
            animator.SetTrigger("died");
        }

        // disable the collider so the player or monster can't move
        if (!isPlayer)
        {
            // Disable all colliders
            foreach (var col in GetComponents<Collider2D>())
            {
                col.enabled = false;
            }

            // stop patrolling
            GetComponent<MonsterPatrol>()?.Die(); // calls the stop method in the monster patrol script
            Destroy(gameObject, 3f); // remove the monster after 3 second
        }
        else
        {
            // Player deathlogic – fx reload level eller respawn
            Debug.Log("Player died");
            FindFirstObjectByType<PlayerMovementTest>().TriggerDeath();
        }
    }
}
