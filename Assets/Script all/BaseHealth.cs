using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBarUI healthBarUI; // Thêm biến tham chiếu đến UI thanh máu

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Nhà chính bị tấn công! Máu còn lại: " + currentHealth);

        if (healthBarUI != null)
        {
            healthBarUI.ReduceHealth(); // Gọi hàm cập nhật UI máu
        }
        else
        {
            Debug.LogWarning("HealthBarUI chưa được gán trong BaseHealth!");
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOver"); // Chuyển scene khi thua
    }
}
