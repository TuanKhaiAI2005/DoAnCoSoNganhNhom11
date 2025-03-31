using UnityEngine;
using TMPro; // Import thư viện để dùng TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton để gọi từ chỗ khác
    public int gold = 100; // Số vàng ban đầu
    public TextMeshProUGUI goldText; // UI hiển thị vàng

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateGoldUI(); // Cập nhật UI ngay khi game bắt đầu
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI(); // Cập nhật UI khi nhận vàng
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldUI(); // Cập nhật UI khi tiêu vàng
            return true;
        }
        else
        {
            Debug.Log("Không đủ vàng!");
            return false;
        }
    }

    void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Vàng: " + gold.ToString();
        }
        else
        {
            Debug.LogWarning("GoldText chưa được gán trong Inspector!");
        }
    }
}
