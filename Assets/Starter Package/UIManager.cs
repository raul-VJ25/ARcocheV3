using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;

    [Header("Package Counter UI")]
    public TextMeshProUGUI packageCountText;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI packagesCollectedText;
    public Button restartButton;

    [Header("Start Panel")]
    public GameObject startPanel;
    public Button startButton;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Configurar botones
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        // Mostrar panel de inicio
        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        // Ocultar Game Over al inicio
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void UpdateTimer(float timeRemaining)
    {
        if (timerText == null) return;

        // Formatear tiempo como MM:SS
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdatePackageCount(int count)
    {
        if (packageCountText == null) return;

        packageCountText.text = "Paquetes: " + count.ToString();
    }

    public void ShowGameOver(int packagesCollected)
    {
        if (gameOverPanel == null) return;

        gameOverPanel.SetActive(true);

        if (packagesCollectedText != null)
        {
            packagesCollectedText.text = "Paquetes recogidos: " + packagesCollected.ToString();
        }

        // Desactivar otros elementos de UI
        if (timerText != null) timerText.gameObject.SetActive(false);
        if (packageCountText != null) packageCountText.gameObject.SetActive(false);
    }

    public void HideGameOver()
    {
        if (gameOverPanel == null) return;

        gameOverPanel.SetActive(false);

        // Reactivar elementos de UI
        if (timerText != null) timerText.gameObject.SetActive(true);
        if (packageCountText != null) packageCountText.gameObject.SetActive(true);
    }

    private void OnStartButtonClick()
    {
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        // Iniciar el juego
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    private void OnRestartButtonClick()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }

    private void OnDestroy()
    {
        // Limpiar listeners
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonClick);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }
    }
}