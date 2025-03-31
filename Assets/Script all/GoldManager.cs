using UnityEngine;
using TMPro; // Import thư viện để dùng TextMeshPro

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance; // Singleton để dễ gọi từ chỗ khác
    public int gold = 100; // Số vàng ban đầu
    public TextMeshProUGUI goldText; // UI hiển thị vàng

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }

    public void SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldUI();
        }
        else
        {
            Debug.Log("Không đủ vàng!");
        }
    }

    void UpdateGoldUI()
    {
        goldText.text = "Vàng: " + gold.ToString();
    }
}
