using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para fácil acesso

    [Header("Run Settings")]
    public GameObject playerPrefab; // Prefab da nave do jogador
    private Vector3 spawnPoint = new Vector3(0, 0, 0); // Ponto de spawn do jogador (no centro da tela)

    private GameObject currentPlayer; // Referência ao jogador atual
    public GameObject CurrentPlayer => currentPlayer; // Propriedade pública para acessar o jogador atual

    private float runStartTime; // Tempo de início da run
    private bool isRunActive = false; // Verifica se a run está ativa

    public event System.Action<GameObject> OnPlayerSpawned; // Evento disparado ao spawnar o jogador

    [Header("UI Elements")]
    public UnityEngine.UI.Text timeText; // Exibe o tempo vivo
    public GameObject gameOverUI; // Painel de Game Over
    public GameObject restartButton; // Botão de reinício

    private void Awake()
    {
        // Certifica-se de que este objeto seja único e não seja destruído ao carregar uma nova cena
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null); // Garante que seja um root GameObject
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita duplicação
        }
    }

    private void Start()
    {
        // Garante que o botão de reinício esteja desativado no início
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }

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
        ScoreManager.Instance.ResetScore();
        ScoreManager.Instance.SetScoreUIActive(true); // Ativa a UI do score
        runStartTime = 0;

        // Remove UI de Game Over
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // Garante que o botão de reinício esteja desativado
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }

        // Reseta o estado da run
        isRunActive = true;

        // Reinicia o SpawnerManager
        SpawnerManager.Instance?.ResetSpawner();

        // Spawna o jogador
        SpawnPlayer();

        // Define o tempo de início da run
        runStartTime = Time.time;

        Debug.Log("New run started!");
    }


    public void EndRun()
    {
        isRunActive = false;

        // Para o spawn de meteoros
        SpawnerManager.Instance?.StopSpawning();

        // Destrói todos os meteoros presentes na cena
        SpawnerManager.Instance?.DestroyAllMeteoroids();

        // Desativa a UI do score
        ScoreManager.Instance.SetScoreUIActive(false);

        // Exibe o painel de Game Over
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Ativa o botão de reinício
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }

        Debug.Log($"Run ended! Final Score: {ScoreManager.Instance.score}");
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

            // Dispara o evento de spawn
            OnPlayerSpawned?.Invoke(currentPlayer);
        }
        else
        {
            Debug.LogError("PlayerPrefab não configurado no GameManager!");
        }
    }

    private void UpdateUI(float runTime)
    {
        // Atualiza o tempo de jogo na interface
        if (timeText != null)
        {
            timeText.text = $"Time: {runTime:F2}s";
        }

        ScoreManager.Instance.UpdateScoreText();
    }

    public void RestartGame()
    {
        // Chama StartNewRun para reiniciar o jogo sem recarregar a cena
        StartNewRun();
    }
}
