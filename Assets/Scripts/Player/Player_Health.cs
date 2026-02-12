using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    [SerializeField] private Player_Infor Player_Infor;

    public Player_Infor _Infor => Player_Infor;
    public float chipSpeed = 2f;

    private float leftTime;

    public Image frontHealthBar;
    public Image backHealthBar;

    void Start()
    {
        Player_Infor._HP = Player_Infor._maxHP;
        Player_Infor._Mana = Player_Infor._maxMana;
        Player_Infor._Exp = Player_Infor._maxExp;

    }

    // Update is called once per frame
    void Update()
    {
        Player_Infor._HP = Mathf.Clamp(Player_Infor._HP, 0, Player_Infor._maxHP);
        UpdateUI();
    }


    public void UpdateUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFration = Player_Infor._HP / Player_Infor._maxExp;

        if(fillB > hFration)
        {
            frontHealthBar.fillAmount = hFration;
            backHealthBar.color = Color.red;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFration, percentComplete);
        }
    }

    public void TakeDamege(float damege)
    {
        Player_Infor._HP -= damege;
        leftTime = 0;
    }


    public void Healing(float heal)
    {
        Player_Infor._HP += heal;
        leftTime = 0;
    }

}
