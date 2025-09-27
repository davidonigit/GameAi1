using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyIA : MonoBehaviour
{
    // --- Variáveis de Configuração---
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float stoppingDistance = 0.5f; // Distância para parar perto do player

    // --- Referências de Componentes ---
    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;

    // --- Controle Interno ---
    private bool isFacingRight = true;

    private void Awake()
    {
        // Awake é chamado antes do Start. Ideal para pegar referências.
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Encontra o player pela Tag, que é mais performático e confiável que FindObjectOfType.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            // Se não encontrar o player, desativa a IA para evitar erros.
            Debug.LogError("Player não encontrado! A IA do inimigo foi desativada. Verifique se o seu player tem a tag 'Player'.");
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        // Se o player for destruído ou não existir, não faz nada.
        if (playerTransform == null) return;

        HandleMovement();
        HandleFlip();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            // Movimento Ativo
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            
            // Usamos MovePosition para um movimento mais suave e que respeita a física.
            rb.MovePosition(rb.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime);
            
            animator.SetBool("Andando", true);
        }
        else
        {
            // Para o movimento quando está perto o suficiente.
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("Andando", false);
        }
    }

    private void HandleFlip()
    {
        // A direção para virar o sprite é baseada na posição do player, não na velocidade.
        // Isso evita viradas indesejadas se o inimigo colidir com algo.
        bool shouldFaceRight = playerTransform.position.x > transform.position.x;

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
}