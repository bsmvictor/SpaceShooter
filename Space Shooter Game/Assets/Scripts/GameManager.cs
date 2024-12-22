using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para fácil acesso

    [Header("Run Settings")]
    public GameObject playerPrefab; // Prefab da nave do jogador
    private Vector3 spawnPoint = new Vector3(0, 0, 0); // Ponto de spawn do jogador (no centro da tela)

    private GameObject currentPlayer; // Referência ao jogador atual
    private float runStartTime; // Tempo de início da run
    private bool isRunActive = false; // Verifica se a run está ativa

    [Header("UI Elements")]
    public UnityEngine.UI.Text timeText; // Exibe o tempo vivo
    public GameObject gameOverUI; // Painel de Game Over

    private void Awake()
    {
        // Configura o Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartNewRun(); // Inicia uma nova run ao carregar o jogo
    }

    private void Update()
    {
        if (isRunActive)
        {
            // Atualiza o tempo vivo
            float runTime = Time.time - runStartTime;
            UpdateUI(runTime);
        }
    }

    public void StartNewRun()
    {
        // Reinicia as estatísticas
        ScoreManager.Instance.score = 0; // Reinicia a pontuação
        runStartTime = 0;

        // Remove UI de Game Over
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // Reseta o estado da run
        isRunActive = true;

        // Spawna o jogador
        SpawnPlayer();

        // Define o tempo de início da run
        runStartTime = Time.time;

        Debug.Log("New run started!");
    }

    private void SpawnPlayer()
    {
        // Destroi qualquer jogador existente
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        // Instancia o jogador no centro da tela
        if (playerPrefab != null)
        {
            currentPlayer = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
            Debug.LogError("PlayerPrefab não configurado no GameManager!");
        }
    }

    public void EndRun()
    {
        isRunActive = false;

        // Exibe o painel de Game Over
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Salva o recorde, se aplicável
        Debug.Log($"Run ended! Final Score: {ScoreManager.Instance.score}");
    }

    public void AddScore(int points)
    {
        // Adiciona pontos no ScoreManager
        ScoreManager.Instance.AddScore(points);
    }

    private void UpdateUI(float runTime)
    {
        // Atualiza o tempo de jogo na interface
        if (timeText != null)
        {
            timeText.text = $"Time: {runTime:F2}s";
        }

        // Atualiza o score no ScoreManager
        ScoreManager.Instance.UpdateScoreText();
    }

    public void RestartRun()
    {
        // Reinicia a cena atual para iniciar uma nova run
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
