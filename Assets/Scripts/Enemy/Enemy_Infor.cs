using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy_Infor")]
public class Enemy_Infor : ScriptableObject
{
    public EnemyRole role;
    public float maxHP;
    public float Speed;
    public float Speedrun;
    public float Attack;
    public float Def;
}
