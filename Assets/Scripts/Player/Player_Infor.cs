using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player_Infor")]
public class Player_Infor : ScriptableObject
{
    [SerializeField] private float HP;
    
    [SerializeField] private float Mana;
    [SerializeField] private float Exp;
    [SerializeField] private float Speedwalk;
    [SerializeField] private float Speedrun;
    [SerializeField] private float Attack;
    [SerializeField] private float Def;
    public float _HP => HP;
    public float _Mana => Mana;
    public float _Exp => Exp;
    public float _Speedwalk => Speedwalk;
    public float _Speedrun => Speedrun;
    public float _Attack => Attack;
    public float _Def => Def;
}
