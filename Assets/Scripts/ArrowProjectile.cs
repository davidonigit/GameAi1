using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);

        // --- A CORREÇÃO MÁGICA ---
        // Definimos a direção LOCAL (diagonal da imagem da flecha)
        Vector2 localDirection = new Vector2(1, 1).normalized;

        // Convertemos essa direção local para a rotação real do objeto no Mundo
        Vector3 globalDirection = transform.TransformDirection(localDirection);

        // Aplicamos a velocidade na direção corrigida
        rb.linearVelocity = globalDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Companion")) return;

        EnemyIA enemy = other.GetComponent<EnemyIA>();
        if (enemy == null) enemy = other.GetComponentInParent<EnemyIA>();

        if (enemy != null)
        {
            enemy.TakeDamage();
            Destroy(gameObject);
        }
        else if (!other.isTrigger) 
        {
            Destroy(gameObject);
        }
    }
}