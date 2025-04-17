using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3; // max health is 3
    public int currentHealth; // current health the player and monster has

    public bool isPlayer = false; // is this the player or a monster? if monster set true on unity editor

    private Animator animator; // set to the animator component on the player or monster to play the hit and daed animation

    private void Start()
    {
        currentHealth = maxHealth; // start with max health
        animator = GetComponent<Animator>(); // get the animator component
    }

    // function called when the player or monster takes damage
    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // subtract the amount of damage taken from current health

        // if there is an animator component, play the hit animation
        if (animator != null)
        {
            animator.SetTrigger("hit");
        }

        // if the player is taking damage, update the health UI
        if (isPlayer)
        {
            FindFirstObjectByType<HealthUI>().UpdateHearts(currentHealth);

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
            Destroy(gameObject, 3f); // remove the monster after 3 second
        }
        else
        {
            // Player dødslogik – fx reload level eller respawn
            Debug.Log("Player died");
        }
    }
}
