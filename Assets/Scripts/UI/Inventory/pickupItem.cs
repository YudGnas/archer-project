using UnityEngine;
using UnityEngine.UI;

public class pickupItem : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    private ItemBase _items;
    [SerializeField] private GameObject _itemUI;
    [SerializeField] private GameObject _itemUI_canvas;

    public bool _canpickup = false;
    void Start()
    {
        _items = _itemUI.GetComponent<ItemBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _canpickup)
        {
            bool added = inventoryManager.AddItem(_itemUI);

            if (added)
            {
                Destroy(gameObject); // Xóa item ngoài world
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canpickup = true;
            _itemUI_canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canpickup = false;
            _itemUI_canvas.SetActive(true);
        }
    }
}
