using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image healthImage; // Ảnh hiển thị thanh máu
    public Sprite[] healthSprites; // Các sprite thể hiện mức máu (10 trạng thái)
    private int healthIndex = 10; // Bắt đầu với 10 (5 tim đầy)

    public void ReduceHealth()
    {
        healthIndex -= 1; // Mất nửa tim
        if (healthIndex < 0) healthIndex = 0;

        healthImage.sprite = healthSprites[healthIndex]; // Cập nhật UI máu
    }
}
