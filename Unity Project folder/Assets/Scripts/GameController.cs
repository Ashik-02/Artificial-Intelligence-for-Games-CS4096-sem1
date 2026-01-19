using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("UI Panels")]
    public GameObject victoryPanel;
    public GameObject gameOverPanel;

    [Header("UI Score Texts")]
    public TextMeshProUGUI victoryScoreText;
    public TextMeshProUGUI gameOverScoreText;

    private int castleHits = 0;
    private bool allWavesDone = false;
    private bool gameEnded = false;
    private int totalScore = 0; 

    public bool GameEnded => gameEnded;
  


    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
      
        totalScore += amount;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (totalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", totalScore);
            PlayerPrefs.Save();
        }
    }

    public void EnemyReachedCastle()
    {
        if (gameEnded) return;

        castleHits++;

        if (castleHits >= 4)
            GameOver();
    }

    public void AllWavesFinished()
    {
        allWavesDone = true;
        CheckVictory();
    }

    void CheckVictory()
    {
        if (gameEnded) return;

        if (allWavesDone && castleHits <= 3)
            Victory();
    }

    void GameOver()
    {
        gameEnded = true;
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score: " + totalScore; 
    }

    void Victory()
    {
        gameEnded = true;
        Time.timeScale = 0;
        victoryPanel.SetActive(true);
        victoryScoreText.text = "Score: " + totalScore; 
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainscreen");
    }
}
