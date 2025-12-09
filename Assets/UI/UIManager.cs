using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI do HUD")]
    [SerializeField] private TextMeshProUGUI killCountText;

    [Header("Painéis de Menu")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    private int enemiesKilled = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateKillCountUI();
    }

    public void RegisterEnemyKill()
    {
        enemiesKilled++;
        UpdateKillCountUI();
    }

    private void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            killCountText.text = "Inimigos: " + enemiesKilled.ToString();
        }
    }

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    public void ShowVictoryPanel()
    {
        if (victoryPanel != null)
        {
            Time.timeScale = 0f;
            victoryPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}