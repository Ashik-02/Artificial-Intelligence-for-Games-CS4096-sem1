using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    //void Awake()
    //{
    //    PlayerPrefs.DeleteAll();
      //  PlayerPrefs.Save();
   // }
    void OnEnable()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }
}
