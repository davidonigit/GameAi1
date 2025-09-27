using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float speed = 5.0f;

    private const int MAX_HEALTH = 5;
    private const int MAX_BOMBS = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private int bombs = 0;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;

    private bool isWalking = false; // sera usado nas animacoes dps
    void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        isWalking = inputVector != Vector2.zero;
        characterController.Move(inputVector * Time.deltaTime * speed);
    }

    public void CollectHealth()
    {
        if (health < MAX_HEALTH) health++;
        print("Health collected: " + health);
    }

    public void CollectBomb()
    {
        if (bombs < MAX_BOMBS) bombs++;
        print("Bomb collected: " + bombs);
    }

    private void UseBomb()
    {
        // if bomb key pressed
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        // Atualiza a UI da barra de vida com o novo valor
        healthBar.SetHealth(health);

        print("Player recebeu dano! Vida atual: " + health);

        if (health <= 0)
        {
            //Die();
        }
    }

    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        health = MAX_HEALTH;

        // Diz para a HealthBar qual é a vida máxima e a preenche
        healthBar.SetMaxHealth(MAX_HEALTH);
    }
}
