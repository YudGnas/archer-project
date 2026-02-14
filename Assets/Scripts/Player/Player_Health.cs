using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    [SerializeField] private Player_Infor Player_Infor;

    public Player_Infor _Infor => Player_Infor;

    private Player_Controller _Controller;
    public float chipSpeed = 2f;

    private float leftTime;

    public Image frontHealthBar;
    public Image backHealthBar;
    public Image backManaBar;
    public Image frontManaBar;
    public Text hp;
    //public Text Mana;


    Color frontHP = new Color(113f, 209f, 40f);
    Color backHP = new Color(255f, 0f, 0f);
    Color frontMana = new Color(25f, 229f, 229f);
    Color backMana = new Color(196f, 226f, 223f);

    void Start()
    {
        _Controller = GetComponent<Player_Controller>();
        Player_Infor._HP = Player_Infor._maxHP;
        Player_Infor._Mana = Player_Infor._maxMana;
        Player_Infor._Exp = Player_Infor._maxExp;

    }

    // Update is called once per frame
    void Update()
    {
        Player_Infor._HP = Mathf.Clamp(Player_Infor._HP, 0, Player_Infor._maxHP);
        Player_Infor._Mana = Mathf.Clamp(Player_Infor._Mana, 0, Player_Infor._maxMana);
        UpdateHealthUI();
        UpdateManaUI();
    }


    public void UpdateHealthUI()
    {
        hp.text = Player_Infor._HP + "/" + Player_Infor._maxHP;
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFration = Player_Infor._HP / Player_Infor._maxHP;



        if (fillB > hFration)
        {
            frontHealthBar.fillAmount = hFration;
            backHealthBar.color = backHP;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFration, percentComplete);
        }        
        if(fillF < hFration)
        {   
            backHealthBar.color = frontHP;
            backHealthBar.fillAmount = hFration;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void UpdateManaUI()
    {
        //Mana.text = Player_Infor._Mana + "/" + Player_Infor._maxMana;
        float fillF = frontManaBar.fillAmount;
        float fillB = backManaBar.fillAmount;
        float hFration = Player_Infor._Mana / Player_Infor._maxMana;

        if (fillB > hFration)
        {
            frontManaBar.fillAmount = hFration;
            backManaBar.color = backMana;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backManaBar.fillAmount = Mathf.Lerp(fillB, hFration, percentComplete);
        }
        if (fillF < hFration)
        {
            backManaBar.color = frontMana;
            backManaBar.fillAmount = hFration;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontManaBar.fillAmount = Mathf.Lerp(fillF, backManaBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamege(float damege)
    {
        _Controller._animator.SetTrigger("GetHit");
        Player_Infor._HP -= damege;
        leftTime = 0;
    }


    public void Healing(float heal)
    {
        Player_Infor._HP += heal;
        leftTime = 0;
    }

    public void Energyconsumption(float energy)
    {
        Player_Infor._Mana -= energy;
        leftTime = 0;
    }
    public void Energyrecovery(float energy)
    {
        Player_Infor._Mana += energy; 
        leftTime = 0;
    }
}
