using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Menu Panels")] 
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject mainMenuPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (!mainMenuPanel.activeInHierarchy && !optionsMenuPanel.activeInHierarchy)
        {
            if (Time.timeScale == 0)
            {
                pauseMenuPanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
            }
            
        }
    }
}
