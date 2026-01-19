using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public TowerManager towerManager;
    public float enemySpawnDelay = 1f;
    public float speedBoostFactor = 1.5f;
    public Sprite boostedEnemySprite;

    [Header("Spawn Positions")]
    public Transform[] topPositions;
    public Transform[] middlePositions;
    public Transform[] bottomPositions;

    private int[] enemiesPerWave = { 3, 4, 5 };
    private int waveNumber = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int prevTowerKills = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (waveNumber < enemiesPerWave.Length)
        {
            if (GameController.Instance != null && GameController.Instance.GameEnded) yield break;

            int currentWaveEnemies = enemiesPerWave[waveNumber];
            int boostedEnemies = Mathf.Min(prevTowerKills, currentWaveEnemies);

            for (int i = 0; i < currentWaveEnemies; i++)
            {
                if (GameController.Instance != null && GameController.Instance.GameEnded) yield break;

                Transform[] possiblePositions;

                if (waveNumber == 0 || towerManager == null || towerManager.CurrentSlot == null)
                    possiblePositions = CombineArrays(topPositions, middlePositions, bottomPositions);
                else
                {
                    string slotName = towerManager.CurrentSlot.name;
                    if (slotName == "TowerSlot_1")
                        possiblePositions = CombineArrays(topPositions, middlePositions, bottomPositions);
                    else if (slotName == "TowerSlot_2")
                        possiblePositions = bottomPositions;
                    else if (slotName == "TowerSlot_3")
                        possiblePositions = topPositions;
                    else
                        possiblePositions = CombineArrays(topPositions, middlePositions, bottomPositions);
                }

                Transform spawnPos = possiblePositions[Random.Range(0, possiblePositions.Length)];
                GameObject enemyObj = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);

                Enemy enemyScript = enemyObj.GetComponent<Enemy>();
                SpriteRenderer sr = enemyObj.GetComponent<SpriteRenderer>();

                if (enemyScript != null)
                {
                    if (i < boostedEnemies)
                    {
                        enemyScript.moveSpeed *= speedBoostFactor;
                        if (sr != null)
                        {
                            if (boostedEnemySprite != null) sr.sprite = boostedEnemySprite;
                            sr.color = Color.yellow;
                        }
                    }
                    else
                    {
                        if (sr != null) sr.color = Color.red;
                    }

                    enemyScript.onEnemyRemoved += OnEnemyRemoved;
                }

                activeEnemies.Add(enemyObj);
                yield return new WaitForSeconds(enemySpawnDelay);
            }

            while (activeEnemies.Count > 0)
            {
                if (GameController.Instance != null && GameController.Instance.GameEnded) yield break;
                yield return null;
            }

            if (towerManager != null && towerManager.CurrentTower != null)
            {
                Tower towerScript = towerManager.CurrentTower.GetComponent<Tower>();
                prevTowerKills = towerScript != null ? towerScript.kills : 0;
            }
            else
                prevTowerKills = 0;

            waveNumber++;
        }

        GameController.Instance?.AllWavesFinished();
    }

    private void OnEnemyRemoved(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
            activeEnemies.Remove(enemy);
    }

    private Transform[] CombineArrays(params Transform[][] arrays)
    {
        int totalLength = 0;
        foreach (var arr in arrays) totalLength += arr.Length;
        Transform[] result = new Transform[totalLength];
        int index = 0;
        foreach (var arr in arrays)
            foreach (var t in arr)
                result[index++] = t;

        return result;
    }
}
