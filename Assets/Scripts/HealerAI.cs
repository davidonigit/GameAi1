using System.Collections;
using UnityEngine;

public class HealerAI : MonoBehaviour
{
    [Header("Alvo Principal")]
    [SerializeField] private Player player;

    [Header("Configurações de Órbita")]
    [SerializeField] private float orbitRadius = 2.0f;
    [SerializeField] private float moveSpeed = 5.0f;

    [Header("Configurações de Combate")]
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float healCooldown = 15f;

    private float nextHealTime = 0f;
    private Vector3 currentEnemiesPos = Vector3.zero;
    private bool hasEnemies = false;
    private Vector2 currentOffset;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.GetComponent<Player>();
        }

        if (player != null)
            currentOffset = (transform.position - player.transform.position).normalized * orbitRadius;
    }

    private void Update()
    {
        if (player == null) return;

        GetAverageEnemyPosition();
        HandleMovement();
        HandleHeal();
    }

    private void GetAverageEnemyPosition()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, detectionRadius, enemyLayer);

        if (enemies.Length == 0)
        {
            hasEnemies = false;
            return;
        }

        Vector3 sumPositions = Vector3.zero;

        foreach (Collider2D enemy in enemies)
        {
            sumPositions += enemy.transform.position;
        }

        // Calcula a média dividindo a soma total das posições pela quantidade de inimigos
        currentEnemiesPos = sumPositions / enemies.Length;
        hasEnemies = true;
    }

    private void HandleMovement()
    {
        Vector2 targetPositionOnCircle;

        if (hasEnemies)
        {
            // MODO COMBATE: Fica atrás do player em relação ao inimigo
            Vector2 directionToEnemy = (player.transform.position - currentEnemiesPos).normalized;
            targetPositionOnCircle = (Vector2)player.transform.position + (directionToEnemy * orbitRadius);
            
            HandleFlip(player.transform.position);
        }
        else
        {
            // MODO IDLE: Segue o offset relativo
            targetPositionOnCircle = (Vector2)player.transform.position - currentOffset;

            HandleFlip(player.transform.position);
        }

        // Move suavemente
        transform.position = Vector2.MoveTowards(transform.position, targetPositionOnCircle, moveSpeed * Time.deltaTime);
    }

    private void HandleFlip(Vector3 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        
        transform.rotation = Quaternion.identity;
    }

    private void HandleHeal()
    {
        if (Time.time >= nextHealTime)
        {
            player.CollectHealth();
            nextHealTime = Time.time + healCooldown;
        }
    }

    
}