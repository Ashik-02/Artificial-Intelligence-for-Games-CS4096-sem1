using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public int health = 100;
    public float obstacleAvoidForce = 3f;
    public float obstacleDetectRadius = 0.3f;
    public float obstacleDetectDistance = 1f;

    [Header("Tower Attack Settings")]
    public float enemyAttackRange = 1.5f;
    public float attackInterval = 1f;
    public int attackDamage = 20;
    public LayerMask towerLayer;

    [Header("References")]
    public Transform castle;
    public LayerMask obstacleLayer;

    private Tower targetTower = null;
    private bool isAttackingTower = false;
    private float attackTimer = 0f;
    private Vector3 basePosition;
    private Rigidbody2D rb;

    private bool isAvoiding = false;
    private Vector2 avoidDirection = Vector2.zero;

    public event System.Action<GameObject> onEnemyRemoved;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (castle == null)
        {
            GameObject castleObj = GameObject.FindWithTag("Castle");
            if (castleObj != null)
                castle = castleObj.transform;
        }
    }

    void FixedUpdate()
    {
        Vector2 toCastle = ((Vector2)castle.position - rb.position).normalized;

        if (GameController.Instance != null && GameController.Instance.GameEnded) return;
        if (castle == null) return;

        if (!isAttackingTower)
        {
            Collider2D hit = Physics2D.OverlapCircle(rb.position, enemyAttackRange, towerLayer);
            if (hit != null)
            {
                targetTower = hit.GetComponent<Tower>();
                if (targetTower != null)
                {
                    isAttackingTower = true;
                    basePosition = transform.position;
                    attackTimer = 0f;
                }
            }
        }

        if (isAttackingTower && targetTower != null)
        {
            targetTower.RegisterAttackingEnemy(this);
            attackTimer += Time.fixedDeltaTime;

            float amplitude = 0.1f;
            float speed = 5f;
            transform.position = basePosition + new Vector3(0, Mathf.Sin(Time.time * speed) * amplitude, 0);

            if (attackTimer >= attackInterval)
            {
                targetTower.TakeDamage(attackDamage);
                attackTimer = 0f;
            }

            if (targetTower.CurrentHealth <= 0)
            {
                targetTower.UnregisterAttackingEnemy(this);
                isAttackingTower = false;
                targetTower = null;
                transform.position = basePosition;
            }
            return;
        }
        if (!isAvoiding)
        {
            RaycastHit2D hitObstacle = Physics2D.CircleCast(rb.position, obstacleDetectRadius, toCastle, obstacleDetectDistance, obstacleLayer);
            if (hitObstacle.collider != null)
            {
                Vector2 perp = Vector2.Perpendicular(toCastle);
                avoidDirection = (Vector2.Dot(perp, hitObstacle.normal) > 0 ? perp : -perp).normalized;
                isAvoiding = true;
            }
        }

        if (isAvoiding)
        {
            RaycastHit2D forwardCheck = Physics2D.CircleCast(rb.position, obstacleDetectRadius, toCastle, obstacleDetectDistance, obstacleLayer);
            if (forwardCheck.collider == null)
            {
                isAvoiding = false;
                avoidDirection = Vector2.zero;
            }
        }

        Vector2 moveDir = (toCastle + avoidDirection).normalized;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Castle"))
        {
            GameController.Instance?.EnemyReachedCastle();
            onEnemyRemoved?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0) Kill();
    }

    public void Kill()
    {
        if (isAttackingTower && targetTower != null)
            targetTower.UnregisterAttackingEnemy(this);

       
        GameController.Instance?.AddScore(1);

        onEnemyRemoved?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
