using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyIA : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 1.5f;

    [Header("Configurações de Combate")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float deathAnimationDuration = 1f;

    [SerializeField] private int attackDamage = 1;

    private Rigidbody2D rb;
    private Animator animator;
    private GameObject playerObject;
    private Transform playerTransform;
    
    private Player playerScript;

    private bool isFacingRight = true;
    private float nextAttackTime = 0f;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            playerScript = playerObject.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player não encontrado! A IA do inimigo foi desativada. Verifique se o seu player tem a tag 'Player'.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (isDead || playerTransform == null) return;

        HandleAttack();
    }

    private void FixedUpdate()
    {
        if (isDead || playerTransform == null) return;

        HandleMovement();
        HandleFlip();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > attackRange)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime);
            animator.SetBool("isWalking", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
    }

    private void HandleAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackCooldown;

            if(playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);
            }
        }
    }

    private void HandleFlip()
    {
        bool shouldFaceRight = playerTransform.position.x < transform.position.x;

        if (isFacingRight != shouldFaceRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void TakeDamage()
    {
        if (isDead) return;

        isDead = true;

        if (UIManager.instance != null)
        {
            UIManager.instance.RegisterEnemyKill();
        }

        animator.SetTrigger("Hurt");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        GetComponent<Collider2D>().enabled = false;
        
        Destroy(gameObject, deathAnimationDuration);
    }
}