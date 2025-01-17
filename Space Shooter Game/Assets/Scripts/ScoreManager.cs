using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Game Stats")]
    public int score = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public GameObject scoreUI;

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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSceneChanged += HandleSceneChange;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSceneChanged -= HandleSceneChange;
        }
    }

    private void HandleSceneChange()
    {
        ResetScore();
        SetScoreUIActive(false);
    }


    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
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