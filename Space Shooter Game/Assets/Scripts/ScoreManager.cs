using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton para fácil acesso
    
    [Header("Game Stats")]
    public int score = 0; // Pontuação do jogador
    //public int highScore = 0; // Recorde de pontuação
    
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText; // Exibe a pontuação
    //public TextMeshProUGUI highScoreText; // Exibe o recorde de pontuação
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
        // Carrega o recorde de pontuação salvo
        //highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        // Atualiza o texto do recorde de pontuação
        UpdateHighScoreText();
    }
    
    public void AddScore(int points)
    {
        // Adiciona pontos à pontuação atual
        score += points;
        
        // Atualiza o texto da pontuação
        UpdateScoreText();
        
        // Verifica se a pontuação atual é maior que o recorde
        /*if (score > highScore)
        {
            highScore = score; // Atualiza o recorde
            PlayerPrefs.SetInt("HighScore", highScore); // Salva o recorde
            UpdateHighScoreText(); // Atualiza o texto do recorde
        }*/
    }
    
    public void UpdateScoreText()
    {
        // Atualiza o texto da pontuação
        scoreText.text = $"Score: {score}";
    }
    
    public void UpdateHighScoreText()
    {
        // Atualiza o texto do recorde de pontuação
        //highScoreText.text = $"High Score: {highScore}";
    }
}
