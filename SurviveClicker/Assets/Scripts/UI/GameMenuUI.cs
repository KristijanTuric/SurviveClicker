using UnityEngine;

public class GameMenuUI : MonoBehaviour
{
    [Header("Win/Lose Panels")] 
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    
    [Header("Other Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject mainMenuPanel;

    public void ShowGameOverPanel()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void ShowGameWinPanel()
    {
        gamePanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void ShowPlayAgain()
    {
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
