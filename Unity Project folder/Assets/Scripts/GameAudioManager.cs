using UnityEngine;
using UnityEngine.UI;

public class GameAudioManager : MonoBehaviour
{
    public Button musicButton;
    public Sprite playIcon;   
    public Sprite pauseIcon;  

    private AudioSource musicSource;
    private Image buttonImage;
    private bool isPlaying = true;

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        buttonImage = musicButton.GetComponent<Image>();
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
