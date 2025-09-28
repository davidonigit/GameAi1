using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D pickUpCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Assuming the player has a Player script with a method to increase health
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.CollectHealth(); // Increase player's health by 1
                Destroy(gameObject); // Destroy the health item after pickup
            }
        }
    }

}
