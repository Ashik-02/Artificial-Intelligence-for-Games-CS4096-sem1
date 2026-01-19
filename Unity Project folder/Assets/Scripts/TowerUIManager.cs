using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    public Tower tower; 
    public Text killsText;
    public Text pointsText;
    public Button moveButton;
    public Button rangeButton;
    public TowerManager towerManager;

    void Update()
    {
        if (tower != null)
        {
            killsText.text = "Kills: " + tower.kills;
            pointsText.text = "Points: " + tower.points;

            moveButton.interactable = tower.points >= 5;
            rangeButton.interactable = tower.points >= 25;
        }
        else
        {
            moveButton.interactable = false;
            rangeButton.interactable = false;
        }
    }

    public void MoveTower()
    {
        if (tower == null || tower.points < 5) return;

        tower.points -= 5;

        if (towerManager != null)
            towerManager.RemoveTower();

        tower = null; 
    }

    public void IncreaseRange()
    {
        if (tower == null || tower.points < 25) return;

        tower.points -= 25;

        if (tower != null)
            tower.IncreaseRange(1f); 
    }
}
