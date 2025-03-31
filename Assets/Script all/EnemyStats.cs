using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 100;
    public int goldReward = 10;
    public AudioSource audioSource; // Audio Source của quái
    public AudioClip deathSound; // Âm thanh khi quái chết

    public event System.Action OnDeath;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Quái nhận " + damage + " sát thương! Máu còn lại: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

   void Die()
{
    if (health > 0) return; // Đảm bảo không gọi Die() nhiều lần

    Debug.Log("Quái bị tiêu diệt! Nhận " + goldReward + " vàng!");

    // Cộng vàng khi quái chết
    if (GameManager.Instance != null)
    {
        GameManager.Instance.AddGold(goldReward);
    }

    // Tạo một GameObject riêng để phát âm thanh
    if (audioSource != null && deathSound != null)
    {
        GameObject soundObject = new GameObject("DeathSound");
        AudioSource tempAudio = soundObject.AddComponent<AudioSource>();
        tempAudio.clip = deathSound;
        tempAudio.volume = audioSource.volume;
        tempAudio.Play();
        Destroy(soundObject, deathSound.length); // Hủy sau khi phát xong
    }

    // Gọi sự kiện OnDeath nếu có
    OnDeath?.Invoke();
    OnDeath = null;

    // Hủy quái ngay lập tức
    Destroy(gameObject);
}
}