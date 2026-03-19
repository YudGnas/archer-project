using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_IndexManager : MonoBehaviour
{
    public Player_Infor Player_Infor;
    public InventoryManager _InventoryManager;
    public GameObject Player_index;

    private bool isActive = false;

    public TextMeshProUGUI maxhp;
    public TextMeshProUGUI maxMana;
    public TextMeshProUGUI Damege;
    public TextMeshProUGUI lucky;
    public TextMeshProUGUI indexpoint;


    public TextMeshProUGUI maxHP_plus;
    public TextMeshProUGUI maxMana_plus;
    public TextMeshProUGUI Damege_plus;
    public TextMeshProUGUI lucky_plus;
    void Start()
    {   
        Player_index.SetActive(false);
        maxhp.text = Player_Infor._maxHP.ToString();
        maxMana.text = Player_Infor._maxMana.ToString();
        Damege.text = Player_Infor._Attack.ToString();
        lucky.text = Player_Infor._lucky.ToString();
        indexpoint.text = Player_Infor._attributepoints.ToString();

        maxHP_plus.text = maxhp.text;
        maxMana_plus.text = maxMana.text;
        Damege_plus.text = Damege.text;
        lucky_plus.text = lucky.text;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isActive)
            {
                Player_index.SetActive(true);
                isActive = true;
            }
            else
            {
                Player_index.SetActive(false);
                isActive = false;
                if(_InventoryManager._currentSelectedItem != null)
                {
                    Image oldImage = _InventoryManager._currentSelectedItem.GetComponent<Image>();
                    oldImage.color = Color.white; // trả về màu bình thường
                    _InventoryManager._currentSelectedItem = null;
                }
                
            }
        }
    }
    public void AddHPIndex()
    {
        if (Player_Infor._attributepoints > 0)
        {
            Player_Infor._attributepoints -= 1;
            int currentHP = int.Parse(maxHP_plus.text);
            currentHP += 10;
            maxHP_plus.text = currentHP.ToString();
            indexpoint.text = Player_Infor._attributepoints.ToString();
        }
    }
    public void AddManaIndex()
    {
        if (Player_Infor._attributepoints > 0)
        {
            Player_Infor._attributepoints -= 1;
            int currentMana = int.Parse(maxMana_plus.text);
            currentMana += 10;
            maxMana_plus.text = currentMana.ToString();
            indexpoint.text = Player_Infor._attributepoints.ToString();
        }
    }
    public void AddDamegeIndex()
    {
        if (Player_Infor._attributepoints > 0)
        {
            Player_Infor._attributepoints -= 1;
            int currentDamege = int.Parse(Damege_plus.text);
            currentDamege += 10;
            Damege_plus.text = currentDamege.ToString();
            indexpoint.text = Player_Infor._attributepoints.ToString();
        }
    }
    public void AddLuckyIndex()
    {
        if (Player_Infor._attributepoints > 0)
        {
            Player_Infor._attributepoints -= 1;
            int currentlucky = int.Parse(lucky_plus.text);
            currentlucky += 10;
            lucky_plus.text = currentlucky.ToString();
            indexpoint.text = Player_Infor._attributepoints.ToString();
        }
    }
    public void SaveIndex()
    {
        int currentHP = int.Parse(maxHP_plus.text);
        int currentMana = int.Parse(maxMana_plus.text);
        int currentDamege = int.Parse(Damege_plus.text);
        int currentLucky = int.Parse(lucky_plus.text);
        Player_Infor._maxHP = currentHP;
        Player_Infor._maxMana = currentMana;
        Player_Infor._Attack = currentDamege;
        Player_Infor._lucky = currentLucky;

        maxhp.text = Player_Infor._maxHP.ToString();
        maxMana.text = Player_Infor._maxMana.ToString();
        Damege.text = Player_Infor._Attack.ToString();
        lucky.text = Player_Infor._lucky.ToString();
    }
    public void ReturnIndex()
    {

    }
}
