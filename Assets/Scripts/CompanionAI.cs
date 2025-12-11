using UnityEngine;

public class CompanionAI : MonoBehaviour
{
    [Header("Alvo Principal")]
    [SerializeField] private Transform playerTransform;

    [Header("Configurações de Órbita")]
    [SerializeField] private float orbitRadius = 2.0f;
    [SerializeField] private float moveSpeed = 5.0f;

    [Header("Configurações de Combate")]
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;

    private float nextAttackTime = 0f;
    private Transform currentTarget;
    private Vector2 currentOffset;

    // Referência para virar o sprite
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }

        if (playerTransform != null)
            currentOffset = (transform.position - playerTransform.position).normalized * orbitRadius;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        FindNearestEnemy();
        HandleMovement();
        HandleAttack();
    }

    private void FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerTransform.position, detectionRadius, enemyLayer);
        
        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider2D enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(playerTransform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearest = enemy.transform;
            }
        }
        currentTarget = nearest;
    }

    private void HandleMovement()
    {
        Vector2 targetPositionOnCircle;

        if (currentTarget != null)
        {
            // MODO COMBATE: Fica entre o Player e o Inimigo
            Vector2 directionToEnemy = (currentTarget.position - playerTransform.position).normalized;
            targetPositionOnCircle = (Vector2)playerTransform.position + (directionToEnemy * orbitRadius);
            
            // Olha para o INIMIGO
            HandleFlip(currentTarget.position);
        }
        else
        {
            // MODO IDLE: Segue o offset relativo
            targetPositionOnCircle = (Vector2)playerTransform.position + currentOffset;
            
            // Olha para o JOGADOR
            HandleFlip(playerTransform.position);
        }

        // Move suavemente
        transform.position = Vector2.MoveTowards(transform.position, targetPositionOnCircle, moveSpeed * Time.deltaTime);

        // Trava na órbita exata
        Vector2 finalDirection = (transform.position - playerTransform.position).normalized;
        transform.position = (Vector2)playerTransform.position + (finalDirection * orbitRadius);

        // Atualiza o offset para manter a posição relativa quando sair de combate
        currentOffset = finalDirection * orbitRadius;
    }

    // --- NOVA LÓGICA DE FLIP (Mantém o NPC em pé) ---
    private void HandleFlip(Vector3 targetPosition)
    {
        // Verifica se precisa virar
        if (targetPosition.x < transform.position.x)
        {
            // Vira para ESQUERDA
            spriteRenderer.flipX = true;

            // Inverte a posição do FirePoint para a esquerda (negativo)
            if (firePoint.localPosition.x > 0)
            {
                Vector3 newPos = firePoint.localPosition;
                newPos.x *= -1;
                firePoint.localPosition = newPos;
            }
        }
        else
        {
            // Vira para DIREITA
            spriteRenderer.flipX = false;

            // Restaura a posição do FirePoint para a direita (positivo)
            if (firePoint.localPosition.x < 0)
            {
                Vector3 newPos = firePoint.localPosition;
                newPos.x *= -1;
                firePoint.localPosition = newPos;
            }
        }
        
        transform.rotation = Quaternion.identity;
    }

    private void HandleAttack()
    {
        if (currentTarget == null) return;

        float distanceToTargetPos = Vector2.Distance(transform.position, playerTransform.position + ((currentTarget.position - playerTransform.position).normalized * orbitRadius));
        
        if (distanceToTargetPos < 0.5f && Time.time >= nextAttackTime)
        {
            Shoot();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Shoot()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            // 1. Calcula a direção do inimigo
            Vector2 direction = currentTarget.position - firePoint.position;
            
            // 2. Calcula o ângulo em graus
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 3. Aplica o Offset de -45 graus porque seu sprite aponta para diagonal
            Quaternion rotation = Quaternion.Euler(0, 0, angle - 45f);

            Instantiate(arrowPrefab, firePoint.position, rotation);
        }
    }
}