using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton para fácil acesso

    [Header("Game Stats")]
    public int score = 0; // Pontuação do jogador

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText; // Exibe a pontuação
    public GameObject scoreUI; // Painel de pontuação
    
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

    public void AddScore(int points)
    {
        // Adiciona pontos à pontuação atual
        score += points;

        // Atualiza o texto da pontuação
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        // Atualiza o texto da pontuação
        scoreText.text = $"Score: {score}";
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    
    public void SetScoreUIActive(bool isActive)
    {
        if (scoreUI != null)
        {
            scoreUI.SetActive(isActive);
        }
    }
}
