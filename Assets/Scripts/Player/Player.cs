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

    private bool isWalking = false; // será usado nas animações dps
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

    public void TakeDamage()
    {
        health--;
        //Check if health 0
        //end game
    }
}
