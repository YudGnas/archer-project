using UnityEngine;
using SLT;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif
public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] Transform _girdLayout;
    [SerializeField] List<ItemBase> _item = new List<ItemBase>();
    public List<ItemBase> Item => _item;
    [SerializeField] List<Transform> _itemSlot = new List<Transform>();
    public List<Transform> itemSlot => _itemSlot;
    public GameObject _currentSelectedItem;


    int equipSlotCount = 4;

    bool IsEquipSlot(Transform slot)
    {
        int index = _itemSlot.IndexOf(slot);
        return index >= _itemSlot.Count - equipSlotCount;
    }

    public void ShowUI(bool show)
    {
        gameObject.SetActive(show);
    }

    void Start()
    {
        _itemSlot = _girdLayout.GetComponentsInChildren<Transform>().ToList();
        _itemSlot.RemoveAt(0);
        Init();
    }


    public void SelectItem(GameObject item)
    {
        // Nếu đã có item được chọn trước đó → bỏ chọn
        if (_currentSelectedItem != null)
        {
            Image oldImage = _currentSelectedItem.GetComponent<Image>();
            oldImage.color = Color.white; // trả về màu bình thường
        }

        // Set item mới
        _currentSelectedItem = item;

        // Highlight item mới
        Image newImage = _currentSelectedItem.GetComponent<Image>();
        newImage.color = Color.yellow; // màu khi được chọn
    }
    public bool AddItem(GameObject itemPrefab)
    {
        ItemBase newItemData = itemPrefab.GetComponent<ItemBase>();
        if (newItemData == null) return false;

        // 1. Tìm item cùng loại trong inventory
        foreach (var item in _item)
        {
            if(item.Infor.itemID == newItemData.Infor.itemID)
            {
                item.quantity++;
                item._quantilyItem.text = item.quantity.ToString();
                return true;
            }
        }

        // 2. Nếu chưa có → tìm slot trống
        Transform targetSlot = _itemSlot.FirstOrDefault(slot =>
        {
            if (slot.childCount > 0) return false;

            bool isEquipSlot = IsEquipSlot(slot);

            // Nếu là equip slot → chỉ nhận item equip
            if (isEquipSlot)
                return newItemData.roll == Item_roll.equip;

            // Nếu là slot thường → không nhận item equip (tuỳ bạn)
            return newItemData.roll != Item_roll.equip;
        });
        // 3. Tạo item mới
        GameObject newItem = Instantiate(itemPrefab, targetSlot);
        newItem.transform.localScale = Vector3.one;

        ItemBase itemBase = newItem.GetComponent<ItemBase>();
        itemBase.quantity = 1;
        _item.Add(itemBase);
        SetupEvent(newItem);

        return true;
    }

    public void Init()
    {
        int i = 0;

        foreach (ItemBase item in _item)
        {
            EventTrigger g = Instantiate(item.gameObject, _itemSlot[i])
                .GetComponent<EventTrigger>();

            g.triggers.Clear();

            // ===== Pointer Click (Chọn item) =====
            var pClick = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };

            pClick.callback.AddListener(eventData =>
            {
                SelectItem(g.gameObject);
            });

            g.triggers.Add(pClick);

            // ===== Pointer Down (Drag start) =====
            var pDow = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };

            pDow.callback.AddListener(eventData =>
            {
                DragItem.Instant.setMovingItem(g.gameObject);
            });

            g.triggers.Add(pDow);

            // ===== Pointer Up (Drag end) =====
            var pUp = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };

            pUp.callback.AddListener(eventData =>
            {
                DragItem.Instant.removeMovingItem();
            });

            g.triggers.Add(pUp);

            i++;
        }
    }

    void SetupEvent(GameObject newItem)
    {
        EventTrigger trigger = newItem.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = newItem.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        // Click
        var pClick = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        pClick.callback.AddListener((eventData) =>
        {
            SelectItem(newItem);
        });
        trigger.triggers.Add(pClick);

        // Down
        var pDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pDown.callback.AddListener((eventData) =>
        {
            DragItem.Instant.setMovingItem(newItem);
        });
        trigger.triggers.Add(pDown);

        // Up
        var pUp = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pUp.callback.AddListener((eventData) =>
        {
            DragItem.Instant.removeMovingItem();
        });
        trigger.triggers.Add(pUp);
    }
    void Update()
    {

    }

    public void UsingItem()
    {
        if (_currentSelectedItem == null) return;
        ItemBase itemeffect = _currentSelectedItem.GetComponent<ItemBase>();
        itemeffect.Use();
        if(itemeffect.roll == Item_roll.consume)
        {
            itemeffect.quantity--;
            if(itemeffect.quantity <= 0)
            {
                Destroy(_currentSelectedItem);
                _item.Remove(itemeffect);
            }
            
        }
    }
}