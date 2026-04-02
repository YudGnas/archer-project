using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Boss_UI : MonoBehaviour
{
    public GameObject BossHPUI;
    public Boss enemy_Infor;
    public TextMeshProUGUI bossname;


    public Image frontHealthBar;
    public Image backHealthBar;
    public Image backStunBar;
    public Image frontStunBar;

    private float leftTime;
    public float chipSpeed = 2f;
    void Start()
    {
        BossHPUI.SetActive(false);
        bossname.text = "Boss";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI(frontHealthBar, backHealthBar, enemy_Infor.currentHP, enemy_Infor.enemy_Infor.maxHP);
        UpdateUI(frontStunBar, backStunBar, enemy_Infor.currentPoise, enemy_Infor.maxPoise);


        if (enemy_Infor.currentHP <= 0) BossHPUI.SetActive(false);
    }

    public void UpdateUI(Image front, Image back, float current, float max)
    {
        float fillF = front.fillAmount;
        float fillB = back.fillAmount;
        float hFration = current / max;



        if (fillB > hFration)
        {
            front.fillAmount = hFration;
            back.color = Color.red;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;

            back.fillAmount = Mathf.Lerp(fillB, hFration, percentComplete);
        }
        if (fillF < hFration)
        {
            back.color = Color.red;
            back.fillAmount = hFration;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            front.fillAmount = Mathf.Lerp(fillF, back.fillAmount, percentComplete);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossHPUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossHPUI.SetActive(false);
        }
    }
}
