using UnityEngine;

public class BombItem : MonoBehaviour
{
    [SerializeField] private BoxCollider2D pickUpCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                player.CollectBomb();
                Destroy(gameObject);
            }
        }
    }

}
