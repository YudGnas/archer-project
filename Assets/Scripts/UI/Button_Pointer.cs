using UnityEngine;
using UnityEngine.EventSystems;

public class Button_Pointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject impact;
    private void Start()
    {
        impact.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        impact.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        impact.SetActive(false);
    }
}
