using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 50;

    private Enemy target;
    private Tower tower;

    public void SetTarget(Enemy enemy, Tower sourceTower)
    {
        target = enemy;
        tower = sourceTower;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            target.TakeDamage(damage);

            if (target.health <= 0 && tower != null)
            {
                tower.AddKill(); 
            }

            Destroy(gameObject);
        }
    }
}
