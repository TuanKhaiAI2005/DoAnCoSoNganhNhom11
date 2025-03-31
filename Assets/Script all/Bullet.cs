using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; // Tốc độ bay
    public int damage = 10; // Sát thương của viên đạn
    private Transform target; // Mục tiêu của đạn

    public void SetTarget(GameObject enemy)
    {
        if (enemy != null)
        {
            target = enemy.transform;
        }
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Nếu quái chết trước khi trúng đạn thì xóa đạn
            return;
        }

        // Di chuyển đạn về phía quái
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đạn va chạm với quái nhưng quái đã bị xóa thì không làm gì
        if (target == null) 
        {
            Destroy(gameObject);
            return;
        }

        if (other.transform == target) // Nếu chạm đúng quái đang nhắm tới
        {
            EnemyStats enemyStats = target.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damage); // Gây sát thương
            }

            Destroy(gameObject); // Xóa viên đạn
            Debug.Log("Đạn trúng mục tiêu, gây " + damage + " sát thương!");
        }
    }
}
