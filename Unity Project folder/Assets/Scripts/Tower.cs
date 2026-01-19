using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 3f;
    public float attackInterval = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Health Bar")]
    public Transform greenHealthBar;
    public int maxHealth = 100;
    private int currentHealth;
    private Vector3 originalBarScale;

    [Header("Points Tracking")]
    public int kills = 0;
    public int points = 0;

    [Header("Targeting")]
    private float attackTimer = 0f;
    private Enemy lastTarget = null;
    private List<Enemy> enemiesAttackingTower = new List<Enemy>();

    [Header("Range Visualization")]
    public GameObject rangeCircle; 

    public int CurrentHealth => currentHealth;

    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
    }

    void Awake()
    {
        if (currentHealth <= 0)
            currentHealth = maxHealth;

        if (greenHealthBar != null)
            originalBarScale = greenHealthBar.localScale;

        UpdateHealthBar();
        UpdateRangeCircle(); 
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            ShootAtEnemy();
            attackTimer = 0f;
        }

        if (lastTarget != null)
            Debug.DrawLine(transform.position, lastTarget.transform.position, Color.red);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();
        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    public void AddKill()
    {
        kills++;
        points += 5;
    }

    public void RegisterAttackingEnemy(Enemy enemy)
    {
        if (!enemiesAttackingTower.Contains(enemy))
            enemiesAttackingTower.Add(enemy);
    }

    public void UnregisterAttackingEnemy(Enemy enemy)
    {
        if (enemiesAttackingTower.Contains(enemy))
            enemiesAttackingTower.Remove(enemy);
    }

    void ShootAtEnemy()
    {
        Enemy target = null;

        if (enemiesAttackingTower.Count > 0)
            target = enemiesAttackingTower[0];
        else
        {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);
            float bestScore = float.MinValue;

            foreach (Collider2D col in enemiesInRange)
            {
                Enemy enemy = col.GetComponent<Enemy>();
                if (enemy == null) continue;

                float distanceToCastle = Vector2.Distance(enemy.transform.position, enemy.castle.position);
                float healthFactor = enemy.health;
                float score = (100f - healthFactor) + (100f - distanceToCastle);

                if (score > bestScore)
                {
                    bestScore = score;
                    target = enemy;
                }
            }
        }

        lastTarget = target;

        if (target != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
                bullet.SetTarget(target, this);
        }
    }

    private void UpdateHealthBar()
    {
        if (greenHealthBar == null) return;
        float scaleX = (float)currentHealth / maxHealth;
        greenHealthBar.localScale = new Vector3(scaleX * originalBarScale.x, originalBarScale.y, originalBarScale.z);
    }

    public void IncreaseRange(float amount)
    {
        attackRange += amount;
        UpdateRangeCircle();
    }

    public void UpdateRangeCircle()
    {
        if (rangeCircle == null) return;
        float diameter = attackRange * 2f; 
        rangeCircle.transform.localScale = new Vector3(diameter, diameter, 1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
