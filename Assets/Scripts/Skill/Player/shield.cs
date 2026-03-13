using UnityEngine;

public class shield : MonoBehaviour
{
    [SerializeField] private float Timelife;
    private float timelife;

    [SerializeField] private float _shield;
    private float currentShield;
    [SerializeField] private float manacost;
    public float manacost_public => manacost;   

    public float shield_public => _shield;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Player_Health player_Health;


    public float cooldown;
    
    void Start()
    {
        shieldPrefab.SetActive(false);
    }

    private void Update()
    {
        if (shieldPrefab.activeSelf)
        {
            timelife -= Time.deltaTime;
            if (timelife <= 0)
            {
                shieldPrefab.SetActive(false);
                
            }
        }
    }
    public void UsingShield()
    {
        shieldPrefab.SetActive(true);
        timelife = Timelife;
        currentShield = _shield;
        player_Health.Energyconsumption(manacost);
    }

    public float TakeShieldDamage(float damage)
    {
        if (!shieldPrefab.activeSelf)
            return damage; // không có shield → damage đi thẳng vào HP

        if (currentShield >= damage)
        {
            currentShield -= damage;
            Debug.Log(currentShield);
            return 0; // shield chặn hết
        }
        else
        {
            float remainDamage = damage - currentShield;
            currentShield = 0;
            shieldPrefab.SetActive(false);
            currentShield -= damage;
            return remainDamage; // damage còn lại trừ HP
        }
    }
}
