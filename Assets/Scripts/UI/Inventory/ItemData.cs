using System.Globalization;
using System;
//using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "itemdata", menuName = "item")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string _name;
    public Sprite _image;

    [Header("Only Equip")]
    public float damage;
    public float hp;
    public float mana;
    public EquipType equipType;
}





