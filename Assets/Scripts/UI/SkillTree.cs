using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;


public class SkillTree : MonoBehaviour
{
    [SerializeField] Player_Infor skillpoint;
    [SerializeField] private TextMeshProUGUI skillpoint_index;

    [SerializeField] private List<Skill_infor> skillQ_list = new List<Skill_infor>();
    [SerializeField] private List<Skill_infor> skillE_list = new List<Skill_infor>();
    [SerializeField] private List<Skill_infor> skillR_list = new List<Skill_infor>();
    [SerializeField] private List<shield> _shield = new List<shield>();
    [SerializeField] private List<Player_Health> healing = new List<Player_Health>();

    [Header("SkillQ")]
    [SerializeField] private TextMeshProUGUI skillQ_damege;
    [SerializeField] private TextMeshProUGUI skillQ_damege_update;
    [SerializeField] private TextMeshProUGUI skillQ_cooldown;
    [SerializeField] private TextMeshProUGUI skillQ_cooldown_update;
    [SerializeField] private TextMeshProUGUI skillQ_manacost;
    [SerializeField] private TextMeshProUGUI skillQ_manacost_update;

    [Header("SkillE")]
    [SerializeField] private TextMeshProUGUI skillE_damege;
    [SerializeField] private TextMeshProUGUI skillE_damege_update;
    [SerializeField] private TextMeshProUGUI skillE_cooldown;
    [SerializeField] private TextMeshProUGUI skillE_cooldown_update;
    [SerializeField] private TextMeshProUGUI skillE_manacost;
    [SerializeField] private TextMeshProUGUI skillE_manacost_update;

    [Header("SkillR")]
    [SerializeField] private TextMeshProUGUI skillR_damege;
    [SerializeField] private TextMeshProUGUI skillR_damege_update;
    [SerializeField] private TextMeshProUGUI skillR_cooldown;
    [SerializeField] private TextMeshProUGUI skillR_cooldown_update;
    [SerializeField] private TextMeshProUGUI skillR_manacost;
    [SerializeField] private TextMeshProUGUI skillR_manacost_update;

    [Header("Shield")]
    [SerializeField] private TextMeshProUGUI _Shield_text;
    [SerializeField] private TextMeshProUGUI _shield_text_update;

    [Header("Healing")]
    [SerializeField] private TextMeshProUGUI _Healing_text;
    [SerializeField] private TextMeshProUGUI _Healing_text_update;

    [Header("Skill prefab")]
    [SerializeField] private SkillBase skillQ;
    [SerializeField] private SkillBase skillE;
    [SerializeField] private SkillBase skillR;

    [SerializeField] private Skill_infor skillQ_current;
    [SerializeField] private Skill_infor skillE_current;
    [SerializeField] private Skill_infor skillR_current;
    [SerializeField] private Skill_infor skillQ_next;
    [SerializeField] private Skill_infor skillE_next;
    [SerializeField] private Skill_infor skillR_next;

    void Start()
    {
        skillQ_current = skillQ.infor; 
        skillE_current = skillE.infor; 
        skillR_current = skillR.infor;
        
        skillQ_next = skillQ_list[skillQ_list.IndexOf(skillQ_current) + 1];
        skillE_next = skillE_list[skillE_list.IndexOf(skillE_current) + 1];
        skillR_next = skillR_list[skillR_list.IndexOf(skillR_current) + 1];

        if (skillQ_current != null)
        {
            skillQ_damege.text = skillQ_current.damege.ToString();
            skillQ_cooldown.text = skillQ_current.cooldown.ToString();
            skillQ_manacost.text = skillQ_current.manacost.ToString();
        }

        if (skillQ_next != null)
        {
            skillQ_damege_update.text = skillQ_next.damege.ToString();
            skillQ_cooldown_update.text = skillQ_next.cooldown.ToString();
            skillQ_manacost_update.text = skillQ_next.manacost.ToString();
        }
        else
        {
            skillQ_damege_update.text = "MAX";
            skillQ_cooldown_update.text = "-";
            skillQ_manacost_update.text = "-";
        }

        if (skillE_current != null)
        {
            skillE_damege.text = skillE_current.damege.ToString();
            skillE_cooldown.text = skillE_current.cooldown.ToString();
            skillE_manacost.text = skillE_current.manacost.ToString();
        }

        if (skillE_next != null)
        {
            skillE_damege_update.text = skillE_next.damege.ToString();
            skillE_cooldown_update.text = skillE_next.cooldown.ToString();
            skillE_manacost_update.text = skillE_next.manacost.ToString();
        }
        else
        {
            skillE_damege_update.text = "MAX";
            skillE_cooldown_update.text = "-";
            skillE_manacost_update.text = "-";
        }
        if (skillR_current != null)
        {
            skillR_damege.text = skillR_current.damege.ToString();
            skillR_cooldown.text = skillR_current.cooldown.ToString();
            skillR_manacost.text = skillR_current.manacost.ToString();
        }

        if (skillR_next != null)
        {
            skillR_damege_update.text = skillR_next.damege.ToString();
            skillR_cooldown_update.text = skillR_next.cooldown.ToString();
            skillR_manacost_update.text = skillR_next.manacost.ToString();
        }
        else
        {
            skillR_damege_update.text = "MAX";
            skillR_cooldown_update.text = "-";
            skillR_manacost_update.text = "-";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSkill(SkillBase skill, ref Skill_infor current, ref Skill_infor next, List<Skill_infor> skill_list)
    {
        current = skill_list[skill_list.IndexOf(skill.infor) + 1];

        int index = skill_list.IndexOf(current);

        if (index < 0 || index + 1 >= skill_list.Count)
        {
            next = null;
            return;
        }
        skillpoint._skillpoints -= 1;
        skillpoint_index.text = skillpoint._skillpoints.ToString();
        next = skill_list[index + 1];
    }

    public void UpdateTextQ()
    {
        if (skillpoint._skillpoints <= 0)
            return;
        
        UpdateSkill(skillQ,ref skillQ_current, ref skillQ_next, skillQ_list);

        if (skillQ_current != null)
        {
            skillQ_damege.text = skillQ_current.damege.ToString();
            skillQ_cooldown.text = skillQ_current.cooldown.ToString();
            skillQ_manacost.text = skillQ_current.manacost.ToString();
        }

        if (skillQ_next != null)
        {
            skillQ_damege_update.text = skillQ_next.damege.ToString();
            skillQ_cooldown_update.text = skillQ_next.cooldown.ToString();
            skillQ_manacost_update.text = skillQ_next.manacost.ToString();
            skillQ.infor = skillQ_next;
        }
        else
        {
            skillQ_damege_update.text = "MAX";
            skillQ_cooldown_update.text = "-";
            skillQ_manacost_update.text = "-";
        }
      
    }    
    public void UpdateTextE()
    {
        if (skillpoint._skillpoints <= 0)
            return;

        UpdateSkill(skillE,ref skillE_current, ref skillE_next, skillE_list);
        if (skillE_current != null)
        {
            skillE_damege.text = skillE_current.damege.ToString();
            skillE_cooldown.text = skillE_current.cooldown.ToString();
            skillE_manacost.text = skillE_current.manacost.ToString();
        }

        if (skillE_next != null)
        {
            skillE_damege_update.text = skillE_next.damege.ToString();
            skillE_cooldown_update.text = skillE_next.cooldown.ToString();
            skillE_manacost_update.text = skillE_next.manacost.ToString();
            skillE.infor = skillE_next;
        }
        else
        {
            skillE_damege_update.text = "MAX";
            skillE_cooldown_update.text = "-";
            skillE_manacost_update.text = "-";
        }

        
       
    }    
    public void UpdateTextR()
    {
        if(skillpoint._skillpoints <= 0)
            return;

        UpdateSkill(skillR, ref skillR_current, ref skillR_next, skillR_list);
        if (skillR_current != null)
        {
            skillR_damege.text = skillR_current.damege.ToString();
            skillR_cooldown.text = skillR_current.cooldown.ToString();
            skillR_manacost.text = skillR_current.manacost.ToString();
        }

        if (skillR_next != null)
        {
            skillR_damege_update.text = skillR_next.damege.ToString();
            skillR_cooldown_update.text = skillR_next.cooldown.ToString();
            skillR_manacost_update.text = skillR_next.manacost.ToString();
            skillR.infor = skillR_next;
        }
        else
        {
            skillR_damege_update.text = "MAX";
            skillR_cooldown_update.text = "-";
            skillR_manacost_update.text = "-";
        }       
    }    
}
