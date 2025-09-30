using UnityEngine;

// Garante que o Player tenha um Animator
[RequireComponent(typeof(Animator))] 
public class Player : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float bombRadius = 3f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Efeitos e Animações")]
    [SerializeField] private GameObject bombEffectPrefab;
    private Animator animator; // 1. Referência para o Animator

    private Rigidbody2D rb;

    private const int MAX_HEALTH = 5;
    private const int MAX_BOMBS = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private int bombs = 0;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private BombUI bombUI;

    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // 2. Pega o componente Animator no início
        
        health = MAX_HEALTH;
        healthBar.SetMaxHealth(MAX_HEALTH);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if(bombUI != null)
        {
            bombUI.UpdateBombIcons(bombs);
        }
    }

    // ... (Start, GameInput_OnAttackAction, etc. continuam iguais) ...
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
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        bool isWalking = inputVector != Vector2.zero;
        rb.linearVelocity = inputVector * speed;
        
        // --- LÓGICA DA ANIMAÇÃO ---
        // 3. Envia os dados de movimento para o Animator
        animator.SetBool("isWalking", isWalking);
        
        // Só atualiza a direção se estiver andando, para manter a última direção ao parar
        if(isWalking)
        {
            animator.SetFloat("moveX", inputVector.x);
            animator.SetFloat("moveY", inputVector.y);
        }
    }

    // ... (O resto do seu código, CollectHealth, UseBomb, Die, etc. continua igual) ...
    public void CollectHealth()
    {
        if (health < MAX_HEALTH) health++;
        healthBar.SetHealth(health); // Atualiza a UI ao coletar vida
        print("Health collected: " + health);
    }

    public void CollectBomb()
    {
        if (bombs < MAX_BOMBS)
        {
            bombs++;
            
            // 3. Atualiza a UI ao coletar uma bomba
            if (bombUI != null)
            {
                bombUI.UpdateBombIcons(bombs);
            }
            
            print("Bomb collected: " + bombs);
        }
    }

     private void UseBomb()
    {
        bombs--;
        print("Bomba usada! Bombas restantes: " + bombs);
        if (bombUI != null)
        {
            bombUI.UpdateBombIcons(bombs);
        }

        if (bombEffectPrefab != null)
        {
            GameObject bombInstance = Instantiate(bombEffectPrefab, transform.position, Quaternion.identity);
            BombEffectScaler scaler = bombInstance.GetComponent<BombEffectScaler>();
            if (scaler != null)
            {
                scaler.SetScaleFromRadius(bombRadius);
            }
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, enemyLayer);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyIA enemy = enemyCollider.GetComponent<EnemyIA>();
            if (enemy != null)
            {
                enemy.TakeDamage(); 
            }
        }
    }
    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

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
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}