using UnityEngine;

public class ArcherAI : MonoBehaviour
{
    public enum NodeStatus { Failure, Success, Running }

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

        UpdateSensors();
        RunBehaviourTree();
    }

    private void UpdateSensors()
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

    private void RunBehaviourTree()
    {
        TryAttackSequence();
        MovementSelector();
    }

    private NodeStatus TryAttackSequence()
    {
        if (currentTarget == null) return NodeStatus.Failure;

        if (Time.time < nextAttackTime) return NodeStatus.Failure;

        Vector2 combatPos = GetCombatPosition();
        float distanceToIdealPos = Vector2.Distance(transform.position, combatPos);

        if (distanceToIdealPos > 0.5f) return NodeStatus.Failure;

        PerformAttack();
        return NodeStatus.Success;
    }

    private NodeStatus MovementSelector()
    {
        if (currentTarget != null)
        {
            MoveCombat();
            return NodeStatus.Running;
        }

        MoveIdle();
        return NodeStatus.Running;
    }

    private void PerformAttack()
    {
        if (arrowPrefab != null && firePoint != null)
        {
            Vector2 direction = currentTarget.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle - 45f);

            Instantiate(arrowPrefab, firePoint.position, rotation);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void MoveCombat()
    {
        Vector2 targetPosition = GetCombatPosition();
        MoveTowards(targetPosition);
        HandleFlip(currentTarget.position);
    }

    private void MoveIdle()
    {
        Vector2 targetPosition = (Vector2)playerTransform.position + currentOffset;
        MoveTowards(targetPosition);
        HandleFlip(playerTransform.position);
    }

    private Vector2 GetCombatPosition()
    {
        Vector2 directionToEnemy = (currentTarget.position - playerTransform.position).normalized;
        return (Vector2)playerTransform.position + (directionToEnemy * orbitRadius);
    }

    private void MoveTowards(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        Vector2 finalDirection = (transform.position - playerTransform.position).normalized;
        transform.position = (Vector2)playerTransform.position + (finalDirection * orbitRadius);

        if (currentTarget == null)
        {
            currentOffset = finalDirection * orbitRadius;
        }
    }

    private void HandleFlip(Vector3 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;

            if (firePoint.localPosition.x > 0)
            {
                Vector3 newPos = firePoint.localPosition;
                newPos.x *= -1;
                firePoint.localPosition = newPos;
            }
        }
        else
        {
            spriteRenderer.flipX = false;

            if (firePoint.localPosition.x < 0)
            {
                Vector3 newPos = firePoint.localPosition;
                newPos.x *= -1;
                firePoint.localPosition = newPos;
            }
        }
        
        transform.rotation = Quaternion.identity;
    }
}