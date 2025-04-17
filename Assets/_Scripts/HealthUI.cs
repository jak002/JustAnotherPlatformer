using UnityEngine;
using UnityEngine.UI; // for UI elements like Images

public class HealthUI : MonoBehaviour
{
    // An array of Image objects to represent the hearts in the UI on the screen
    public Image[] hearts; // Assign in Inspector (do it yourself)
    public Sprite fullHeart; // iamge for full heart
    public Sprite emptyHeart; // image for empty heart

    // Called when player takes damage
    public void UpdateHearts(int currentHealth)
    {
        // Loop through the hearts array and set the sprite based on current health
        for (int i = 0; i < hearts.Length; i++)
        {
            // Check if the index is less than the current health
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                // show empty heart
                hearts[i].sprite = emptyHeart;
        }
    }
}
