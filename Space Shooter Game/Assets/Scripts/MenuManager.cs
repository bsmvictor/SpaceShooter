using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [Header("UI Elements")]
    public GameObject mainMenuUI; // Painel do menu principal
    public GameObject optionsUI; // Painel de opções

    private void Start()
    {
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
}