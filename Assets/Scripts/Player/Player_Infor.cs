using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player_Infor")]
public class Player_Infor : ScriptableObject
{
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;
    
    [SerializeField] private float Mana;
    [SerializeField] private float maxMana;
    [SerializeField] private float Exp;
    [SerializeField] private float maxExp;
    [SerializeField] private float Speedwalk;
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
    public float _Exp
    {
        get => Exp;
        set => Exp = value;
    }
    public float _maxExp => maxExp;
    public float _Speedwalk => Speedwalk;
    public float _Speedrun => Speedrun;
    public float _Attack => Attack;
    public float _Def => Def;
}
