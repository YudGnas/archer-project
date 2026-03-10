using UnityEngine;
using SLT;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] Transform _girdLayout;
    [SerializeField] List<ItemBase> _item = new List<ItemBase>();
    public List<ItemBase> Item => _item;
    [SerializeField] List<Transform> _itemSlot = new List<Transform>();
    public List<Transform> itemSlot => _itemSlot;
    [SerializeField] private GameObject _currentSelectedItem;
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

    public bool AddItem(GameObject itemPrefab)
    {
        // Tìm slot trống đầu tiên
        Transform targetSlot = _itemSlot
            .FirstOrDefault(slot => slot.childCount == 0);

        if (targetSlot == null)
        {
            Debug.Log("Inventory Full!");
            return false;
        }

        // Tạo item trong inventory
        GameObject newItem = Instantiate(itemPrefab, targetSlot);

        // Reset scale nếu là UI
        newItem.transform.localScale = Vector3.one;

        // Thêm EventTrigger nếu chưa có
        EventTrigger trigger = newItem.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = newItem.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        // Pointer Click
        var pClick = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        pClick.callback.AddListener(eventData =>
        {
            SelectItem(trigger.gameObject);
        });

        trigger.triggers.Add(pClick);

        // Pointer Down
        EventTrigger.Entry pDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };

        pDown.callback.AddListener((eventData) =>
        {
            DragItem.Instant.setMovingItem(newItem);
        });

        trigger.triggers.Add(pDown);

        // Pointer Up
        EventTrigger.Entry pUp = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };

        pUp.callback.AddListener((eventData) =>
        {
            DragItem.Instant.removeMovingItem();
        });

        trigger.triggers.Add(pUp);

        return true;
    }
    void Update()
    {

    }

    public void UsingItem()
    {
        NormalItem itemeffect = _currentSelectedItem.GetComponent<NormalItem>();
        itemeffect.Use();
    }
}