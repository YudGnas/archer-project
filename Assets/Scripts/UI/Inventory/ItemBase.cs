using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

// Đây là Component vật lý của vật phẩm trên mặt đất
public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    protected ItemData _infor;
    [SerializeField] public Image _imageItem;
    [SerializeField] public TextMeshProUGUI _NameItem;
    [SerializeField] public TextMeshProUGUI _quantilyItem;

    public ItemData Infor => _infor;
    [SerializeField]
    public int quantity;

    public Item_roll roll;
    public abstract void Use();
    [SerializeField] protected Player_Health _player_Health;
    [SerializeField] protected Player_Infor _player_infor;
    private void Start()
    {
        if (_infor == null) return;
        _imageItem.sprite = _infor._image;
        _NameItem.text = _infor.name;
        _quantilyItem.text = this.quantity.ToString();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _player_Health = player.GetComponent<Player_Health>();
        if (player != null) _player_infor = _player_Health._Infor;
    }
}
public enum Item_roll
{
    normal,
    special,
    equip,
    consume,
}
public enum EquipType
{
    Weapon,
    Cloak,
    Hat,
    Ring,
    None
}
