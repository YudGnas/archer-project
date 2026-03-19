using UnityEngine;

public class blood_tank : ItemBase
{
    public override void Use()
    {
        Debug.Log("Use");
        _player_Health.Healing(_player_Health._Infor._maxHP * 0.3f);
    }
}
