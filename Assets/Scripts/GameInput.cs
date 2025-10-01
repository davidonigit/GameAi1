using UnityEngine;
using System; // Necessário para usar o EventHandler

public class GameInput : MonoBehaviour
{
    PlayerInputActions inputActions;

    public event EventHandler OnAttackAction; // 1. O evento que outros scripts vão "ouvir"

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        // 2. "Escuta" a ação "Bomb" e chama o método Bomb_performed quando ela acontece
        inputActions.Player.Attack.performed += Attack_performed; 
    }

    // 3. Este método é chamado pelo sistema de input quando a ação "Bomb" é realizada
    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // 4. Dispara o evento público para avisar quem estiver ouvindo (o Player)
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}