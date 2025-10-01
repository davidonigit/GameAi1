using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para gerenciar cenas!

public class UIManager : MonoBehaviour
{

    [Header("Painéis de Menu")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    public void ShowVictoryPanel()
    {
        // 2. Adicione esta nova função
        if (victoryPanel != null)
        {
            Time.timeScale = 0f; // Pausa o jogo
            victoryPanel.SetActive(true); // Mostra o painel de vitória
        }
    }
    public void RestartGame()
    {
        // 1. "Despausa" o jogo, caso ele esteja pausado.
        Time.timeScale = 1f;

        // 2. Recarrega a cena atual.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}