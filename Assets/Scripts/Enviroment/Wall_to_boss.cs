using UnityEngine;
using DG.Tweening;

public class Wall_to_boss : MonoBehaviour
{
    public GameObject[] enemy;

    [Header("Door Setting")]
    public float moveDownAmount = 5f; // khoảng cách đi xuống
    public float duration = 1.5f;     // thời gian di chuyển

    private bool isOpened = false;

    void Update()
    {
        if (isOpened) return;

        if (AllEnemiesDead())
        {
            OpenDoor();
        }
    }

    bool AllEnemiesDead()
    {
        foreach (var e in enemy)
        {
            if (e != null) return false; // còn 1 con sống
        }
        return true;
    }

    void OpenDoor()
    {
        isOpened = true;

        // Di chuyển xuống theo trục Y
        transform.DOMoveY(transform.position.y - moveDownAmount, duration)
                 .SetEase(Ease.InOutQuad);
    }
}
