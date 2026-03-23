using SLT;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : Singleton<DragItem>
{
    [SerializeField] GameObject _movingItem;
    [SerializeField]
    Transform _parent;

    Transform _targetSlot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_movingItem == null)
            return;
        Vector2 mousePos = Input.mousePosition;
        _movingItem.transform.position = mousePos;

        _targetSlot = null;
        foreach (Transform t in InventoryManager.Instant.itemSlot)
        {
            if (Vector2.Distance(_movingItem.transform.position, t.position) <= 50f)
            {
                _targetSlot = t;
            }
        }
    }
    bool IsEquipSlot(Transform slot)
    {
        var slots = InventoryManager.Instant.itemSlot;
        int index = slots.IndexOf(slot);
        return index >= slots.Count - 4;
    }

    bool CanPlaceItem(Transform slot, ItemBase item)
    {
        bool isEquipSlot = IsEquipSlot(slot);

        // Equip slot → chỉ nhận equip
        if (isEquipSlot)
            return item.roll == Item_roll.equip;

        // Slot thường → nhận tất cả (kể cả equip)
        return true;
    }

    public void setMovingItem(GameObject g)
    {
        Debug.Log("set moving item");
        _movingItem = g;
        _parent = g.transform.parent;

        g.transform.SetParent(this.transform);
    }

    public void removeMovingItem()
    {
        if (_movingItem != null)
        {
            ItemBase movingItemBase = _movingItem.GetComponent<ItemBase>();

            //  Không có slot → trả về
            if (_targetSlot == null)
            {
                _movingItem.transform.SetParent(_parent);
            }
            else
            {
                //  Check slot hợp lệ
                if (!CanPlaceItem(_targetSlot, movingItemBase))
                {
                    Debug.Log("Không thể đặt item vào slot này!");
                    _movingItem.transform.SetParent(_parent);
                }
                else if (_targetSlot.childCount == 0)
                {
                    //  Slot trống
                    _movingItem.transform.SetParent(_targetSlot);
                }
                else
                {
                    //  Swap (cũng phải check ngược lại)
                    Transform oldItem = _targetSlot.GetChild(0);
                    ItemBase oldItemBase = oldItem.GetComponent<ItemBase>();

                    //  Nếu item trong slot không thể về slot cũ → cancel
                    if (!CanPlaceItem(_parent, oldItemBase))
                    {
                        Debug.Log("Swap không hợp lệ!");
                        _movingItem.transform.SetParent(_parent);
                    }
                    else
                    {
                        oldItem.SetParent(_parent);
                        oldItem.localPosition = Vector3.zero;

                        _movingItem.transform.SetParent(_targetSlot);
                    }
                }

                _movingItem.transform.localPosition = Vector3.zero;
            }

            _movingItem = null;
            _targetSlot = null;
        }
    }
}