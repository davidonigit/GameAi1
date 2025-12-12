using UnityEngine;
using UnityEngine.InputSystem; // Necessário para o novo sistema de input

public class StatueController : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private int killsRequired = 5;
    [SerializeField] private Key interactionKey = Key.E; // Mudança: Usa 'Key' em vez de 'KeyCode'

    [Header("Feedback Visual")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private Color unlockedColor = Color.green;

    private bool isPlayerInRange = false;
    private bool isUnlocked = false;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject npcPrefab;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(interactPrompt != null) 
            interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !isUnlocked)
        {
            // Verifica se o teclado está conectado e se a tecla configurada foi pressionada
            if (Keyboard.current != null && Keyboard.current[interactionKey].wasPressedThisFrame)
            {
                CheckRequirementAndUnlock();
            }
        }
    }

    private void CheckRequirementAndUnlock()
    {
        if (UIManager.instance == null)
        {
            Debug.LogError("UIManager não encontrado!");
            return;
        }

        int currentKills = UIManager.instance.GetEnemiesKilled();

        if (currentKills >= killsRequired)
        {
            UnlockCompanion();
        }
        else
        {
            Debug.Log($"Kills insuficientes. Você tem {currentKills}/{killsRequired}.");
        }
    }

    private void UnlockCompanion()
    {
        isUnlocked = true;
        Debug.Log("Requisito atendido! NPC liberado.");

        if (spriteRenderer != null)
            spriteRenderer.color = unlockedColor;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        Instantiate(npcPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (!isUnlocked && interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}