using System.Globalization;
using System;
//using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "itemdata", menuName = "item")]
public class ItemData : ScriptableObject
{
    public int _id;
    public string _name;
    public Sprite _image;
    public float _price;
}



