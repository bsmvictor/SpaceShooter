using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject mainMenuUI; // Painel do menu principal
    public GameObject optionsUI; // Painel de opções

    [Header("Background Settings")]
    public SpriteRenderer backgroundRenderer; // Referência ao SpriteRenderer do Background

    private void Start()
    {
        AdjustBackgroundToCamera();
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 0f; // Pausa o jogo
        mainMenuUI.SetActive(true);
        optionsUI.SetActive(false);
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        Time.timeScale = 1f; // Retoma o jogo
        SceneManager.LoadScene("GameplayScene");
    }

    public void ShowOptions()
    {
        optionsUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void AdjustBackgroundToCamera()
    {
        if (backgroundRenderer == null) return;

        // Obtém a câmera principal
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // Calcula o tamanho da câmera
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
}