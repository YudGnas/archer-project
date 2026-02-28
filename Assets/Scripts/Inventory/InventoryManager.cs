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


    public void Init()
    {
        int i = 0;
        foreach (ItemBase item in _item)
        {
            EventTrigger g = Instantiate(item.gameObject, _itemSlot[i]).GetComponent<EventTrigger>();
            var pDow = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };

            pDow.callback.AddListener(eventData =>
            {
                DragItem.Instant.setMovingItem(g.gameObject);
            });
            g.triggers.Add(pDow);
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
    public void AddItem()
    {
        int i = _item.Count;

        Transform targetSlot = _itemSlot.FirstOrDefault(slot => slot.childCount == 0);

        if (targetSlot == null)
            return;
        EventTrigger g = Instantiate(_item[_item.Count - 1].gameObject, targetSlot).GetComponent<EventTrigger>();
        var pDow = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };

        pDow.callback.AddListener(eventData =>
        {
            DragItem.Instant.setMovingItem(g.gameObject);
        });
        g.triggers.Add(pDow);
        var pUp = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };

        pUp.callback.AddListener(eventData =>
        {
            DragItem.Instant.removeMovingItem();
        });
        g.triggers.Add(pUp);
    }
    void Update()
    {

    }
}