using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public float gameTime = 120f; // 2 minutos en segundos

    [Header("State")]
    private float currentTime;
    private int packagesCollected;
    private bool isGameActive;

    [Header("References")]
    public PackageSpawner PackageSpawner;
    public DrivingSurfaceManager DrivingSurfaceManager; // ✅ Referencia pública para asignar en Inspector

    public float CurrentTime => currentTime;
    public int PackagesCollected => packagesCollected;
    public bool IsGameActive => isGameActive;

    private void Awake()
    {
        // Singleton pattern para GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: mantener entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if (isGameActive)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        currentTime = gameTime;
        packagesCollected = 0;

        // Notificar a la UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimer(currentTime);
            UIManager.Instance.UpdatePackageCount(packagesCollected);
        }
    }

    public void CollectPackage()
    {
        if (!isGameActive) return;

        packagesCollected++;

        // Actualizar UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdatePackageCount(packagesCollected);
        }

        // Spawnear nuevo paquete
        if (PackageSpawner != null && DrivingSurfaceManager != null)
        {
            var lockedPlane = DrivingSurfaceManager.LockedPlane; // ✅ Usar referencia directa
            if (lockedPlane != null)
            {
                PackageSpawner.SpawnPackage(lockedPlane);
            }
        }
    }

    public void EndGame()
    {
        isGameActive = false;

        // Mostrar pantalla de Game Over
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver(packagesCollected);
        }
    }

    public void ResetGame()
    {
        currentTime = gameTime;
        packagesCollected = 0;
        isGameActive = false;

        // Ocultar Game Over si está visible
        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideGameOver();
            UIManager.Instance.UpdateTimer(currentTime);
            UIManager.Instance.UpdatePackageCount(packagesCollected);
        }
    }

    public void RestartGame()
    {
        // Reiniciar la escena completa
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}