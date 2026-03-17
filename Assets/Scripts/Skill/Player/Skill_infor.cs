using UnityEngine;



[CreateAssetMenu(fileName = "Skill", menuName = "Skill_Infor")]
public class Skill_infor : ScriptableObject 
{
    public float damege;
    public float cooldown;
    public float manacost;
    public float poiseDamage;
    public float speed;
    public int Level;
}
