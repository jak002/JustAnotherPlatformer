using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    public Transform pointA; // Start point
    public Transform pointB; // End point
    public float speed = 2f;  // Patrol speed
    private bool movingToB = true;  // Flag for direction
    private bool isDead = false; // Flag for death state

    private void Update()
    {
        if (!isDead) // Only patrol if not dead
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // Move towards the target point (point A or point B)
        Transform target = movingToB ? pointB : pointA;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Check if the enemy reached the target point
        if ((Vector2)transform.position == (Vector2)target.position)
        {
            movingToB = !movingToB; // Change direction
            Flip(); // Flip the model's direction (turn around)
        }
    }

    private void Flip()
    {
        // Flip the enemy sprite when changing direction
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // Invert the x-axis scale to flip
        transform.localScale = theScale;
    }

    public void Die()
    {
        isDead = true;
    }
}
