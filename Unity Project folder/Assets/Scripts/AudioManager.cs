using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("UI References")]
    public Button musicButton;       
    public Sprite playIcon;        
    public Sprite pauseIcon;         

    private AudioSource musicSource;
    private Image buttonImage;
    private bool isPlaying = true;

    void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            Debug.LogError("AudioManager requires an AudioSource component on the same GameObject.");
            enabled = false;
            return;
        }
        if (musicButton != null)
            buttonImage = musicButton.GetComponent<Image>();
        else
        {
            Debug.LogError("Assign the musicButton in the Inspector.");
            enabled = false;
            return;
        }
        musicSource.loop = true;   
        musicSource.Play();
        isPlaying = true;
        buttonImage.sprite = playIcon;
        musicButton.onClick.AddListener(ToggleMusic);
    }

    void ToggleMusic()
    {
        if (isPlaying)
        {
            musicSource.Pause();
            buttonImage.sprite = pauseIcon;
        }
        else
        {
            musicSource.Play();
            buttonImage.sprite = playIcon;
        }

        isPlaying = !isPlaying;
    }
}
