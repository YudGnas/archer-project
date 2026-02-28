using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NormalItem : ItemBase
{
    [SerializeField] public Image _imageItem;
    [SerializeField] public TextMeshProUGUI _NameItem;
    [SerializeField] public TextMeshProUGUI _quantilyItem;
    void Start()
    {
        if (_infor == null) return;
        _imageItem.sprite = _infor._image;
        _NameItem.text = _infor.name;
        _quantilyItem.text = this._quantity.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
         if(_infor == null) return;
        _imageItem.sprite = _infor._image;
        _NameItem.text = _infor._name;
        _quantilyItem.text = this._quantity.ToString();
    }
}
