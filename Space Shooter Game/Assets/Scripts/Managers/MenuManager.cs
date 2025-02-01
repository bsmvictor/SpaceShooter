using System;
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
        mainMenuUI.SetActive(true);
        optionsUI.SetActive(false);
        comoJogarUI.SetActive(false);
    }

    private void Update() => AdjustBackgroundToCamera();

    public void StartGame()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
        SceneManager.LoadScene("GameplayScene");
    }
    
    public void ShowComoJogar()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
        comoJogarUI.SetActive(true);
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(false);
    }
    
    public void CloseComoJogar()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
        comoJogarUI.SetActive(false);
        optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ShowOptions()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
        comoJogarUI.SetActive(false);
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void CloseOptions()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
        optionsUI.SetActive(false);
        comoJogarUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ExitGame()
    {
        SoundFXManager.instance.PlaySoundFXClip(buttonSound, transform, 1f);
        Application.Quit();
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
