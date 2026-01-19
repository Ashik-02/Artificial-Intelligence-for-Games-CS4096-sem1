using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;          
    public Button pauseOptionButton;          
    public Button quitButton;                 
    public Button retryButton;                
    public Image pauseOverlay;                

    [Header("Sprites")]
    public Sprite pauseSprite;
    public Sprite unpauseSprite;

    private bool isPaused = false;

    void Start()
    {
        settingsPanel.SetActive(false);
        pauseOverlay.gameObject.SetActive(false);
        pauseOptionButton.onClick.AddListener(TogglePause);
        quitButton.onClick.AddListener(QuitToMainMenu);
        retryButton.onClick.AddListener(RetryLevel); 
    }

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        pauseOptionButton.image.sprite = isPaused ? unpauseSprite : pauseSprite;
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        settingsPanel.SetActive(false);
        pauseOverlay.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void RetryLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainscreen"); 
    }
}
