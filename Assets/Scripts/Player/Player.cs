using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float bombRadius = 3f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Efeitos e Animações")]
    [SerializeField] private GameObject bombEffectPrefab;

    private Rigidbody2D rb;

    private const int MAX_HEALTH = 5;
    private const int MAX_BOMBS = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private int bombs = 0;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject gameOverPanel; // Arraste seu painel de Game Over aqui

    private bool isWalking = false;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = MAX_HEALTH;
        healthBar.SetMaxHealth(MAX_HEALTH);

        // Garante que a tela de Game Over comece desativada
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Start()
    {
        // Se inscreve no evento de "bomba" do GameInput
        gameInput.OnAttackAction += GameInput_OnAttackAction;
    }

    private void GameInput_OnAttackAction(object sender, System.EventArgs e)
    {
        // Chama o método UseBomb se o jogador tiver bombas e não estiver morto
        if (bombs > 0 && !isDead)
        {
            UseBomb();
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero; // Garante que o jogador pare de se mover ao morrer
            return;
        }

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        isWalking = inputVector != Vector2.zero;
        rb.linearVelocity = inputVector * speed;
    }

    public void CollectHealth()
    {
        if (health < MAX_HEALTH) health++;
        healthBar.SetHealth(health); // Atualiza a UI ao coletar vida
        print("Health collected: " + health);
    }

    public void CollectBomb()
    {
        if (bombs < MAX_BOMBS) bombs++;
        // TODO: Atualizar a UI de bombas aqui
        print("Bomb collected: " + bombs);
    }

    private void UseBomb()
    {
        bombs--;
        print("Bomba usada! Bombas restantes: " + bombs);
        // TODO: Atualizar a UI de bombas aqui

        if (bombEffectPrefab != null)
        {
            // 1. Instancia o prefab e GUARDA a referência dele em uma variável.
            GameObject bombInstance = Instantiate(bombEffectPrefab, transform.position, Quaternion.identity);

            // 2. Pega o script de escala da instância que acabamos de criar.
            BombEffectScaler scaler = bombInstance.GetComponent<BombEffectScaler>();

            // 3. Se o script existir, chama o método para ajustar a escala, passando o raio.
            if (scaler != null)
            {
                scaler.SetScaleFromRadius(bombRadius);
            }
        }

        // Detecta todos os colisores dentro de um círculo na posição do jogador
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, enemyLayer);

        // Passa por cada inimigo atingido e aplica o dano
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyIA enemy = enemyCollider.GetComponent<EnemyIA>();
            if (enemy != null)
            {
                enemy.TakeDamage(); // Chama o método de dano do inimigo
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // Não pode tomar dano se já estiver morto

        health -= damageAmount;
        healthBar.SetHealth(health);
        print("Player recebeu dano! Vida atual: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        print("Player Morreu! Game Over.");

        // Pausa o jogo
        Time.timeScale = 0f;

        // Ativa a tela de Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}