using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject gameInfoPanel;
    public GameObject highScorePanel;

    [Header("Gameplay Scene")]
    public string gameplaySceneName = "GameScene"; 

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; 
#else
        Application.Quit(); // Quit in build
#endif
    }

    public void ShowGameInfo()
    {
        if (gameInfoPanel != null)
        {
            gameInfoPanel.SetActive(true);
        }
    }

    
    public void ShowHighScore()
    {
        if (highScorePanel != null)
        {
            highScorePanel.SetActive(true);
        }
    }

  
    public void BackToMainMenu()
    {
        if (gameInfoPanel != null)
            gameInfoPanel.SetActive(false);

        if (highScorePanel != null)
            highScorePanel.SetActive(false);
    }
}
