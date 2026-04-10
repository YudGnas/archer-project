using UnityEngine;
using UnityEngine.UI;

public class Enemy_hud : MonoBehaviour
{
    public Enemy _enemy;


    public Image frontHealthBar;
    public Image backHealthBar;


    public float chipSpeed = 2f;

    private float leftTime;

    private void Update()
    {
        UpdateHealthUI();   
    }
    public void UpdateHealthUI()
    {
        
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFration = _enemy.currentHP / _enemy.enemy_Infor.maxHP;
        if (fillB > hFration)
        {
            frontHealthBar.fillAmount = hFration;
            backHealthBar.color = Color.red;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;

            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFration, percentComplete);
        }
        if (fillF < hFration)
        {
            backHealthBar.color = Color.red;
            backHealthBar.fillAmount = hFration;
            leftTime += Time.deltaTime;
            float percentComplete = leftTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }
}
