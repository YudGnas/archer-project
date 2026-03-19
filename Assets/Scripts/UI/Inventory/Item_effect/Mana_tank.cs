using UnityEngine;

public class Mana_tank : ItemBase
{
    public override void Use()
    {
        Debug.Log("Use");
        _player_Health.effect[1].Play();
        _player_Health.Energyrecovery(_player_Health._Infor._maxMana * 0.5f);
        _player_Health.Turnoffeffect(1);
    }
}
