using UnityEngine;

public class HealerAI : MonoBehaviour
{
    // Enum para controlar o estado dos nós da árvore
    public enum NodeStatus { Failure, Success, Running }

    [Header("Alvo Principal")]
    [SerializeField] private Player player;

    [Header("Configurações de Órbita")]
    [SerializeField] private float orbitRadius = 2.0f;
    [SerializeField] private float moveSpeed = 5.0f;

    [Header("Configurações de Combate")]
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float healCooldown = 15f;

    // Variáveis de Estado
    private float nextHealTime = 0f;
    private Vector3 currentEnemiesPos = Vector3.zero;
    private bool hasEnemies = false;
    private Vector2 idleOffset; // Offset fixo para quando não há combate

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Garante referência ao player
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.GetComponent<Player>();
        }

        // Define um offset inicial para o modo Idle
        if (player != null)
            idleOffset = (transform.position - player.transform.position).normalized * orbitRadius;
    }

    private void Update()
    {
        if (player == null) return;

        // 1. SENSORES: Atualiza os dados do mundo
        UpdateSensors();

        // 2. CÉREBRO: Roda a árvore de comportamento
        RunBehaviourTree();
    }

    // --- SENSORES (Coleta de Dados) ---
    private void UpdateSensors()
    {
        // Lógica de média de posição dos inimigos
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

        currentEnemiesPos = sumPositions / enemies.Length;
        hasEnemies = true;
    }

    // --- BEHAVIOUR TREE ---
    private void RunBehaviourTree()
    {
        // A raiz é um "Selector": Tenta curar -> Se não der, tenta se mover.
        // Neste caso, queremos curar SE possível, mas SEMPRE queremos nos mover. 

        // Nó 1: Tentativa de Cura (Prioridade 1)
        TryHealSequence();

        // Nó 2: Lógica de Movimento (Sempre roda, mas muda o alvo baseado no estado)
        MovementSelector();
    }

    // --- NÓS ---

    // Sequence: Verifica Cooldown -> Verifica se Player precisa -> Cura
    private NodeStatus TryHealSequence()
    {
        // Condição: Cooldown
        if (Time.time < nextHealTime) return NodeStatus.Failure;

        // Condição: Player Full Health
        if (player.IsFullHealth()) return NodeStatus.Failure;

        // Ação: Curar
        PerformHeal();
        return NodeStatus.Success;
    }

    // Selector: "Posicionamento de Combate" ou "Posicionamento Idle"
    private NodeStatus MovementSelector()
    {
        // Combate
        if (hasEnemies)
        {
            MoveToSafeSpot();
            return NodeStatus.Running;
        }

        // Idle
        MoveIdle();
        return NodeStatus.Running;
    }

    // --- AÇÕES ---

    private void PerformHeal()
    {
         player.CollectHealth();
         nextHealTime = Time.time + healCooldown;
    }

    private void MoveToSafeSpot()
    {
        // Ficar no lado oposto aos inimigos
        Vector2 directionToEnemy = (player.transform.position - currentEnemiesPos).normalized;

        Vector2 targetPosition = (Vector2)player.transform.position + (directionToEnemy * orbitRadius);

        MoveTowards(targetPosition);
    }

    private void MoveIdle()
    {
        // Manter posição relativa suave
        Vector2 targetPosition = (Vector2)player.transform.position - idleOffset;

        MoveTowards(targetPosition);
    }

    private void MoveTowards(Vector2 targetPos)
    {
        // Movimento
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // Visual (Flip)
        HandleFlip(player.transform.position);
    }

    private void HandleFlip(Vector3 focusPoint)
    {
        // Olha sempre para o player ou foco
        if (focusPoint.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        transform.rotation = Quaternion.identity;
    }

    // Debug Visual
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, orbitRadius);
        }
    }
}
