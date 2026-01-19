using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Tower Settings")]
    public GameObject towerPrefab;
    public Transform[] towerSlots;

    private GameObject currentTower;
    private Transform currentSlot;

    public TowerUIManager towerUIManager;
    public bool canPlaceTower = true;

    public GameObject CurrentTower => currentTower;
    public Transform CurrentSlot => currentTower != null ? currentSlot : null;

    private int cachedKills = 0;
    private int cachedPoints = 0;
    private int cachedHealth = 0;
    private float cachedRange = 0f; 

    void Update()
    {
        if (GameController.Instance != null && GameController.Instance.GameEnded) return;

        if (Input.GetMouseButtonDown(0) && canPlaceTower)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            foreach (Transform slot in towerSlots)
            {
                Collider2D col = slot.GetComponent<Collider2D>();
                if (col != null && col.OverlapPoint(mousePos))
                {
                    PlaceTower(slot);
                    break;
                }
            }
        }
    }

    public void PlaceTower(Transform slot)
    {
        if (!canPlaceTower) return;

        currentTower = Instantiate(towerPrefab, slot.position, Quaternion.identity);
        currentSlot = slot;

        Tower towerScript = currentTower.GetComponent<Tower>();

        if (towerUIManager != null)
        {
            towerUIManager.tower = towerScript;
            towerUIManager.towerManager = this;
        }
        towerScript.kills = cachedKills;
        towerScript.points = cachedPoints;
        if (cachedHealth > 0)
            towerScript.SetHealth(cachedHealth);

        if (cachedRange > 0)
        {
            towerScript.attackRange = cachedRange;
            towerScript.UpdateRangeCircle(); 
        }

        canPlaceTower = false;
    }

    public void RemoveTower()
    {
        if (currentTower != null)
        {
            Tower towerScript = currentTower.GetComponent<Tower>();
            if (towerScript != null)
            {
                cachedKills = towerScript.kills;
                cachedPoints = towerScript.points;
                cachedHealth = towerScript.CurrentHealth;
                cachedRange = towerScript.attackRange;
            }

            Destroy(currentTower);
            currentTower = null;
            currentSlot = null;
            canPlaceTower = true;
        }
    }

    public void ResetCachedStats()
    {
        cachedKills = 0;
        cachedPoints = 0;
        cachedHealth = 0;
        cachedRange = 0f;
    }
}
