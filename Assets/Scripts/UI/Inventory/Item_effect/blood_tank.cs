using UnityEngine;

public class blood_tank : NormalItem
{
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _Health = player.GetComponent<Player_Health>();
    }
    [SerializeField] private Player_Health _Health;

    public override void Use()
    {
        Debug.Log("Use");
        _Health.Healing(_Health._Infor._maxHP * 0.3f);
    }
}
