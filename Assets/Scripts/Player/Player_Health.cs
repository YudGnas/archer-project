using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    public GameObject GameOverPanel;
    [SerializeField] private Player_Infor player_Infor;
    [SerializeField] private shield playerShield;


    public Player_Infor _Infor => player_Infor;

    private Player_Controller _Controller;
    public float chipSpeed = 2f;

    private float leftTime;

    public Image frontHealthBar;
    public Image backHealthBar;
    public Image backManaBar;
    public Image frontManaBar;
    public Image frontXPBar;
    public Image backXPBar;
    public Image healing;

    public float timeH;
    private float cooldown = 4f;
    public ParticleSystem[] effect;


    public Text hp;
    //public Text Mana;
    public float timeReconvertMana;
    [SerializeField] private float reconvertMana = 2f;




    public float healingcost;


    public void ResetPlayer()
    {
        player_Infor._HP = player_Infor._maxHP;
        player_Infor._Mana = player_Infor._maxMana;
    }
    void Start()
    {
        GameOverPanel.SetActive(false);
        _Controller = GetComponent<Player_Controller>();
        player_Infor._HP = player_Infor._maxHP;
        player_Infor._Mana = player_Infor._maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        timeReconvertMana += Time.deltaTime;
        timeH -= Time.deltaTime;    
        if (timeReconvertMana > 1)
        {
            Energyrecovery(reconvertMana);
            timeReconvertMana = 0;
        }
        
        player_Infor._HP = Mathf.Clamp(player_Infor._HP, 0, player_Infor._maxHP);
        player_Infor._Mana = Mathf.Clamp(player_Infor._Mana, 0, player_Infor._maxMana);
        UpdateHealthUI();
        UpdateManaUI();
        UpdateXPUI();
        UpdateHealingUI();
        if(Input.GetKeyDown(KeyCode.Alpha1) && timeH <= 0)
        {
            Healing(player_Infor._maxHP * healingcost / 10);
            timeH = cooldown;
            StartCoroutine(Turnoffeffect(0));
        }
    }


    public void UpdateHealthUI()
    {
        hp.text = player_Infor._HP + "/" + player_Infor._maxHP;
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFration = player_Infor._HP / player_Infor._maxHP;
        if (fillB > hFration)
        {
            frontHealthBar.fillAmount = hFration;
            backHealthBar.color = Color.white;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFration, percentComplete);
        }        
        if(fillF < hFration)
        {   
            backHealthBar.color = Color.white;
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
        float hFration = player_Infor._Mana / player_Infor._maxMana;

        if (fillB > hFration)
        {
            frontManaBar.fillAmount = hFration;
            backManaBar.color = Color.white;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backManaBar.fillAmount = Mathf.Lerp(fillB, hFration, percentComplete);
        }
        if (fillF < hFration)
        {
            backManaBar.color = Color.white;
            backManaBar.fillAmount = hFration;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontManaBar.fillAmount = Mathf.Lerp(fillF, backManaBar.fillAmount, percentComplete);
        }
    }    
    public void UpdateXPUI()
    {
        //Mana.text = Player_Infor._Mana + "/" + Player_Infor._maxMana;
        float fillF = frontXPBar.fillAmount;
        float fillB = backXPBar.fillAmount;
        float hFration = player_Infor._Exp / player_Infor._maxExp;

        if (fillB < hFration)
        {
            backXPBar.color = Color.white;
            backXPBar.fillAmount = hFration;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontXPBar.fillAmount = Mathf.Lerp(fillF, backXPBar.fillAmount, percentComplete);
        }
    }
    public void TakeDamege(float damage)
    {
       
        damage = playerShield.TakeShieldDamage(damage);

        if (damage > 0)
        {
            player_Infor._HP -= damage;
            _Controller._animator.SetTrigger("GetHit");
        }
        if(_Infor._HP <= 0)
        {
            GameOverPanel.SetActive(true);
        }
        leftTime = 0;
    }


    public void Healing(float heal)
    {
        effect[0].Play();

        player_Infor._HP += heal;
        leftTime = 0;
    }

    public void Energyconsumption(float energy)
    {
        player_Infor._Mana -= energy;
        leftTime = 0;
    }
    public void Energyrecovery(float energy)
    {
        player_Infor._Mana += energy; 
        leftTime = 0;
    }

    public void GetXp(float xp)
    {
        player_Infor._Exp += xp;
        leftTime = 0;

        if(player_Infor._Exp >= player_Infor._maxExp)
        {
            Debug.Log("Level Up!!!");
            player_Infor._Exp = 0;
            frontXPBar.fillAmount = 0;
            player_Infor._level += 1;
            player_Infor._skillpoints += 1;
            player_Infor._attributepoints += 3;           
            player_Infor._maxExp += 50*player_Infor._level;
        }
    }
    public void UpdateHealingUI()
    {
        float percent = timeH / cooldown;
        healing.fillAmount = Mathf.Lerp(healing.fillAmount, percent, 10f * Time.deltaTime);
    }
    public IEnumerator Turnoffeffect(int x)
    {
        yield return new WaitForSeconds(1.5f); 
        effect[x].Stop();
    }
}
