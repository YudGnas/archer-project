using UnityEngine;
using System;

// Đây là Component vật lý của vật phẩm trên mặt đất
public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    protected ItemData _infor;

    // ✨ THÊM ACCESSOR CÔNG KHAI
    public ItemData Infor => _infor;

    [SerializeField]
    protected int _quantity = 1; // Số lượng hiện tại của vật phẩm này trên mặt đất
    public int quantity => _quantity;

    // ✨ XÓA: Xóa _maxCapacity ở đây, chuyển sang ItemData

    // Ví dụ về hàm logic vật phẩm

    // Ghi chú: Cần đảm bảo _infor được gán trong Inspector cho Prefab vật phẩm

}