using UnityEngine;
using UnityEngine.EventSystems;

public class DragTower : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject towerPrefab;  // Prefab của tháp
    public GameObject previewTower; // Hiển thị trước khi đặt
    public Vector3 offset;          // Để điều chỉnh vị trí

    public void SetTowerPrefab(GameObject prefab)
    {
        towerPrefab = prefab;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (towerPrefab == null) return;

        // Tạo tháp ảo để xem trước
        previewTower = Instantiate(towerPrefab);
        previewTower.GetComponent<Collider2D>().enabled = false; // Tắt va chạm
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (previewTower != null)
        {
            // Di chuyển theo vị trí chuột
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            worldPosition.z = 0; // Đặt Z để hiển thị đúng
            previewTower.transform.position = worldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (previewTower != null)
        {
            // Kiểm tra có thể đặt tháp hay không
            if (CanPlaceTower(previewTower.transform.position))
            {
                Instantiate(towerPrefab, previewTower.transform.position, Quaternion.identity);
            }
            Destroy(previewTower);
        }
    }

    private bool CanPlaceTower(Vector3 position)
    {
        // Kiểm tra vị trí hợp lệ (ví dụ: không chồng lên vật thể khác)
        Collider2D hit = Physics2D.OverlapCircle(position, 0.5f);
        return hit == null; // Chỉ đặt tháp nếu không có vật cản
    }
}
