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
    [SerializeField] private float skillpoints;
    [SerializeField] private float attributepoints;
    [SerializeField] private float Attack;
    [SerializeField] private float Level;
    [SerializeField] private float Speedrun;
    [SerializeField] private float Speedwalk;
    [SerializeField] private float Lucky;

    public float _lucky
    {
        get => Lucky; 
        set => Lucky = value;
    }
    public float _Speedrun => Speedrun;
    public float _Speedwalk => Speedwalk;

    public float _HP
    {
        get => HP;
        set => HP = value;
    }
    public float _maxHP
    {
        get => maxHP;
        set => maxHP = value;
    }
    public float _Mana
    {
        get => Mana;
        set => Mana = value;
    }
    public float _maxMana 
    {
        get => maxMana;
        set => maxMana = value;
    }
    public float _Exp
    {
        get => Exp;
        set => Exp = value;
    }
    public float _maxExp
    {
        get => maxExp;
        set => maxExp = value;
    }
    public float _skillpoints
    {
        get => skillpoints;
        set => skillpoints = value;
    }
    public float _attributepoints
    {
        get => attributepoints;
        set => attributepoints = value;
    }
    public float _Attack
    {
        get => Attack;
        set => Attack = value;
    }
    public float _level
    {
        get => Level;
        set => Level = value;
    }
}
