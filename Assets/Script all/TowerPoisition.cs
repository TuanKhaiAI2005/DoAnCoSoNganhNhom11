    using UnityEngine;

    public class TowerPosition : MonoBehaviour
    {
        [HideInInspector]
        public bool hasTower = false; // Kiểm tra vị trí đã có tháp chưa

        public bool CanPlaceTower()
        {
            return !hasTower; // Chỉ cho phép đặt khi chưa có tháp
        }

        public void PlaceTower()
        {
            hasTower = true; // Đánh dấu đã có tháp
        }
    }
