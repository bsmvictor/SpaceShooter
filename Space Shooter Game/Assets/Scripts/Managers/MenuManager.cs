using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject mainMenuUI;
    public GameObject optionsUI;
    public GameObject comoJogarUI;

    [Header("Background Settings")]
    public SpriteRenderer backgroundRenderer;

    public AudioClip buttonSound;
    private void Start()
    {
        AdjustBackgroundToCamera();
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 0f;
        mainMenuUI.SetActive(true);
        optionsUI.SetActive(false);
        comoJogarUI.SetActive(false);
    }

    public void StartGame()
    {
        OnclickSound();
        mainMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameplayScene");
    }
    
    public void ShowComoJogar()
    {
        OnclickSound();
        comoJogarUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }
    
    public void CloseComoJogar()
    {
        OnclickSound();
        comoJogarUI.SetActive(false);
        optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ShowOptions()
    {
        OnclickSound();
        optionsUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void CloseOptions()
    {
        OnclickSound();
        optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ExitGame()
    {
        OnclickSound();
        Application.Quit();
    }
    
    public void OnclickSound()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
    }

    private void AdjustBackgroundToCamera()
    {
        if (backgroundRenderer == null) return;

        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(cameraHeight * screenAspect, cameraHeight);

        Vector2 spriteSize = backgroundRenderer.sprite.bounds.size;

        Vector3 scale = backgroundRenderer.transform.localScale;
        scale.x = cameraSize.x / spriteSize.x;
        scale.y = cameraSize.y / spriteSize.y;
        backgroundRenderer.transform.localScale = scale;
    }
}
