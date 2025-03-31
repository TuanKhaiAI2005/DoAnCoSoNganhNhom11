using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab viên đạn
    public Transform firePoint; // Vị trí bắn đạn
    public float fireRate = 1f; // Tốc độ bắn (mỗi giây)
    private float fireCooldown = 0f;
    
    public AudioSource shootAudioSource; // AudioSource để phát âm thanh bắn
    public AudioClip shootSound; // Âm thanh bắn

    private List<GameObject> enemiesInRange = new List<GameObject>();

    void Update()
    {
        if (this == null || firePoint == null) return; // 🔥 Kiểm tra nếu tháp bị xóa

        fireCooldown -= Time.deltaTime;

        // ✅ Xóa những quái đã bị hủy
        enemiesInRange.RemoveAll(enemy => enemy == null);

        if (fireCooldown <= 0f && enemiesInRange.Count > 0)
        {
            Fire();
            fireCooldown = 1f / fireRate; // Reset thời gian chờ bắn
        }
    }

    void Fire()
    {
        if (this == null || firePoint == null || enemiesInRange.Count == 0) return; // 🔥 Kiểm tra lại trước khi bắn

        GameObject targetEnemy = enemiesInRange[0];
        if (targetEnemy == null) return; // 🔥 Nếu quái bị xóa, không bắn

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetTarget(targetEnemy); // Nhắm vào quái
        
        // 🎵 Phát âm thanh bắn nếu có
        if (shootAudioSource != null && shootSound != null)
        {
            shootAudioSource.PlayOneShot(shootSound);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) 
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void OnDestroy()
    {
        enemiesInRange.Clear(); // 🔥 Khi tháp bị xóa, xóa danh sách quái
    }
}
