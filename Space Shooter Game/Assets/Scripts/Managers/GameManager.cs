using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para fácil acesso

    [Header("Run Settings")]
    public GameObject playerPrefab; // Prefab da nave do jogador
    private Vector3 spawnPoint = new Vector3(0, 0, 0); // Ponto de spawn do jogador

    private GameObject currentPlayer; // Referência ao jogador atual
    public GameObject CurrentPlayer => currentPlayer; // Propriedade pública para acessar o jogador atual
    
    private bool isRunActive = false; // Verifica se a run está ativa
    private bool isPaused = false; // Verifica se o jogo está pausado

    public event System.Action<GameObject> OnPlayerSpawned; // Evento disparado ao spawnar o jogador
    public event System.Action OnSceneChanged; // Evento disparado ao mudar de cena

    [Header("UI Elements")]
    public GameObject gameOverUI; // Painel de Game Over
    public GameObject pauseMenuUI; // Painel de pause
    public AudioClip buttonSound; // Som do botão
    
    [Header("Background Settings")]
    public SpriteRenderer backgroundRenderer; // Renderer para exibir o background
    public Sprite[] backgrounds; // Array de backgrounds para selecionar aleatoriamente

    [Header("Gameplay Musics")]
    public AudioClip[] gameplayMusics; // Array de músicas de gameplay
    
    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartNewRun();
        MusicManager.instance.PlayRandomMusic(gameplayMusics, transform, 0.5f);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneChanged?.Invoke();

        if (scene.name == "GameplayScene")
        {
            gameOverUI = GameObject.Find("GameOverUI");
            pauseMenuUI = GameObject.Find("PauseMenuUI");

            if (gameOverUI != null) gameOverUI.SetActive(false);
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

            SpawnPlayer();
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && isRunActive)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void StartNewRun()
    {
        isRunActive = true;
        isPaused = false;

        ScoreManager.Instance.ResetScore();
        ScoreManager.Instance.SetScoreUIActive(true);

        SpawnerManager.Instance?.ResetSpawner();

        SpawnPlayer();
        SetRandomBackground(); // Define um background aleatório

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        Debug.Log("New run started!");
    }

    public void EndRun()
    {
        isRunActive = false;

        SpawnerManager.Instance?.StopSpawning();
        SpawnerManager.Instance?.DestroyAllMeteoroids();

        ScoreManager.Instance.SetScoreUIActive(false);

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        
        Debug.Log("Run ended!");
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        OnclickSound();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        OnclickSound();
    }

    private void SpawnPlayer()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        if (playerPrefab != null)
        {
            currentPlayer = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
            OnPlayerSpawned?.Invoke(currentPlayer);
        }
        else
        {
            Debug.LogError("PlayerPrefab não configurado!");
        }
    }

    private void UpdateUI(float runTime)
    {
        ScoreManager.Instance.UpdateScoreText();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameplayScene");
        OnclickSound();
    }

    public void ReturnToMainMenu()
    {
        OnclickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void OnclickSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
    }
    
    private void AdjustBackgroundToCamera(SpriteRenderer backgroundRenderer)
    {
        if (backgroundRenderer == null) return;

        // Obtém a câmera principal
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // Obtém o tamanho da câmera
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(cameraHeight * screenAspect, cameraHeight);

        // Obtém o tamanho do sprite
        Vector2 spriteSize = backgroundRenderer.sprite.bounds.size;

        // Calcula a escala necessária para o sprite preencher a tela
        Vector3 scale = backgroundRenderer.transform.localScale;
        scale.x = cameraSize.x / spriteSize.x;
        scale.y = cameraSize.y / spriteSize.y;
        backgroundRenderer.transform.localScale = scale;
    }

    private void SetRandomBackground()
    {
        if (backgrounds.Length == 0 || backgroundRenderer == null) return;

        // Seleciona um sprite aleatório
        Sprite selectedBackground = backgrounds[UnityEngine.Random.Range(0, backgrounds.Length)];
        backgroundRenderer.sprite = selectedBackground;

        // Ajusta o tamanho do background para preencher a câmera
        AdjustBackgroundToCamera(backgroundRenderer);
    }
}
