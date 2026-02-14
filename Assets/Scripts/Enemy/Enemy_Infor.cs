using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy_Infor")]
public class Enemy_Infor : ScriptableObject
{
    public EnemyRole role;
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;

    [SerializeField] private float Mana;
    [SerializeField] private float maxMana;
    [SerializeField] private float Speed;
    [SerializeField] private float Speedrun;
    [SerializeField] private float Attack;
    [SerializeField] private float Def;
    public float _HP
    {
        get => HP;
        set => HP = value;
    }
    public float _maxHP => maxHP;
    public float _Mana
    {
        get => Mana;
        set => Mana = value;
    }
    public float _maxMana => maxMana;

    public float _Speed => Speed;
    public float _Attack => Attack;
    public float _Def => Def;
}
