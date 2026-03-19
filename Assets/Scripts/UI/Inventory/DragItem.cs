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
            if (_targetSlot == null)
            {
                // Không có slot → trả về vị trí cũ
                _movingItem.transform.SetParent(_parent);
            }
            else if (_targetSlot.childCount == 0)
            {
                // Slot trống → đặt vào
                _movingItem.transform.SetParent(_targetSlot);
            }
            else
            {
                // Slot đã có item → swap
                Transform oldItem = _targetSlot.GetChild(0);

                oldItem.SetParent(_parent);
                oldItem.localPosition = Vector3.zero;

                _movingItem.transform.SetParent(_targetSlot);
            }

            _movingItem.transform.localPosition = Vector3.zero;
        }

        _movingItem = null;
        _targetSlot = null;
    }
}