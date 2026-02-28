using UnityEngine;
using SLT;
using System.Collections.Generic;
using System.Linq;
public class DataManager : Singleton<DataManager>
{
    [Header("----------------Item--------------")]
    [SerializeField] List<ItemData> _genaralDataItem = new List<ItemData>();
    public List<ItemData> GenaralDataItem => _genaralDataItem;

    private void OnValidate()
    {
        _genaralDataItem = Resources.LoadAll<ItemData>("Item").ToList();
    }
}
