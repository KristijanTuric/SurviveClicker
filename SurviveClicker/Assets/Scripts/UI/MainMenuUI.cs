using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject gamePanel;

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameManager.InitializeGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
    }
}
