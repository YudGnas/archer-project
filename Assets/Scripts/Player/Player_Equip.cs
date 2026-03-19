using UnityEngine;

public class Player_Equip : MonoBehaviour
{
    public Player_Infor playerInfor;

    public ItemData weapon;
    public ItemData cloak;
    public ItemData hat;
    public ItemData ring;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Equip item
    public void EquipItem(ItemData newItem)
    {
        switch (newItem.equipType)
        {
            case EquipType.Weapon:
                ChangeItem(ref weapon, newItem);
                break;

            case EquipType.Cloak:
                ChangeItem(ref cloak, newItem);
                break;

            case EquipType.Hat:
                ChangeItem(ref hat, newItem);
                break;

            case EquipType.Ring:
                ChangeItem(ref ring, newItem);
                break;
        }
    }
    void ChangeItem(ref ItemData currentItem, ItemData newItem)
    {
        // tháo item cũ
        if (currentItem != null)
        {
            playerInfor.RemoveStats(currentItem);
        }

        // gán item mới
        currentItem = newItem;

        // cộng stat item mới
        if (newItem != null)
        {
            playerInfor.AddStats(newItem);
        }
    }
    public void UpdateAllStats()
    {
        playerInfor.ResetStats();

        if (weapon != null) playerInfor.AddStats(weapon);
        if (cloak != null) playerInfor.AddStats(cloak);
        if (hat != null) playerInfor.AddStats(hat);
        if (ring != null) playerInfor.AddStats(ring);
    }
}
