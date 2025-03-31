using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public GameObject towerPrefab1;
    public GameObject towerPrefab2;
    public GameObject towerPrefab3;

    private GameObject selectedTowerPrefab;
    private int selectedTowerCost;

    public int towerCost1 = 50;
    public int towerCost2 = 75;
    public int towerCost3 = 100;

    public AudioClip placeSound;
    public AudioClip removeSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTowerPrefab = towerPrefab1;
            selectedTowerCost = towerCost1;
            Debug.Log("Đã chọn tháp 1, giá: " + selectedTowerCost);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTowerPrefab = towerPrefab2;
            selectedTowerCost = towerCost2;
            Debug.Log("Đã chọn tháp 2, giá: " + selectedTowerCost);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTowerPrefab = towerPrefab3;
            selectedTowerCost = towerCost3;
            Debug.Log("Đã chọn tháp 3, giá: " + selectedTowerCost);
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceTower();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            CancelTowerPlacement();
            TryRemoveTower();
        }
    }

    void TryPlaceTower()
    {
        if (selectedTowerPrefab == null)
        {
            Debug.Log("❌ Chưa chọn loại tháp!");
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("TowerPosition"));

        Debug.DrawRay(mousePos, Vector2.up * 0.1f, Color.red, 1f);
        Debug.Log("🛠️ Raycast thử đặt tại: " + mousePos);

        if (hit != null)
        {
            if (hit.transform.childCount > 0)
            {
                Debug.Log("⚠️ Đã có tháp ở vị trí này!");
                return;
            }

            if (GameManager.Instance.SpendGold(selectedTowerCost))
            {
                GameObject newTower = Instantiate(selectedTowerPrefab, hit.transform.position, Quaternion.identity);
                newTower.transform.SetParent(hit.transform);
                newTower.tag = "Tower";
                Debug.Log("🏰 Đã đặt tháp: " + newTower.name);
                Debug.Log("🔗 Cha của tháp: " + newTower.transform.parent.name);
                
                // Phát âm thanh đặt trụ
                if (placeSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(placeSound);
                }
            }
            else
            {
                Debug.Log("❌ Không đủ vàng!");
            }
        }
        else
        {
            Debug.Log("❌ Không có object nào bị trúng!");
        }
    }

    void TryRemoveTower()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

        Debug.DrawRay(mousePos, Vector2.up * 0.1f, Color.blue, 1f);
        Debug.Log("🛠️ Raycast thử xóa tại: " + mousePos);

        if (hits.Length > 0)
        {
            GameObject closestTower = null;
            float closestDistance = float.MaxValue;

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Tower"))
                {
                    float distance = Vector2.Distance(mousePos, hit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTower = hit.gameObject;
                    }
                }
            }

            if (closestTower != null)
            {
                Debug.Log("🗑️ Xóa tháp: " + closestTower.name);
                Destroy(closestTower);
                GameManager.Instance.AddGold(10);
                Debug.Log("🗑️ Tháp đã bị hủy, nhận lại 10 vàng!");
                
                // Phát âm thanh xóa trụ
                if (removeSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(removeSound);
                }

                Physics2D.SyncTransforms();
            }
            else
            {
                Debug.Log("⚠️ Không thể xóa! Hãy nhấn đúng vào tháp.");
            }
        }
        else
        {
            Debug.Log("⚠️ Không có object nào bị trúng!");
        }
    }

    void CancelTowerPlacement()
    {
        selectedTowerPrefab = null;
        selectedTowerCost = 0;
        Debug.Log("🚫 Hủy đặt tháp!");
    }
}
