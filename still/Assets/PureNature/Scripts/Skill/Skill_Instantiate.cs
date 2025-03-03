using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;                                              // Linq �� ���� ������ �ϰ��� Ȯ��
public class Skill_Instantiate : MonoBehaviour
{

    // Script
    public SkillManager manager;                                // SkillManager
    public Skill_CoolTime_Script cool;                          // Skill_CoolTime_Script


    // Image, Text
    public Image skill_Image;                                   // ��ų �̹���
    public Text skill_Key_Setting;                              // Ű ���� Text (���� �������� ��ȭ)



    public Skill skill;


    public Image cooldownImagePrefab;
    public Text cooldownTextPrefab;

    //private bool isStartCool;

    private void Start()
    {
        skill.CoolTimer = PlayerPrefs.GetFloat(skill.skillName, 0);
    }

    // Update (4�ܰ�)
    public void Update()
    {

        if (skill.CoolTimer > 0)
        {
            skill.IsCooldownActive = true;
            skill.CoolTimer -= Time.deltaTime;
            UpdateSkillUI(skill);
            PlayerPrefs.SetFloat(skill.skillName, skill.CoolTimer);
        }
        else
        {
            skill.IsCooldownActive = false;
            cooldownImagePrefab.gameObject.SetActive(false);
            cooldownTextPrefab.gameObject.SetActive(false);
        }
    }

    //******************************************************************************************************************************************************//
    // 3�ܰ� : ��ų ����� �ϰ� �Ǹ�, ���� �Ǵ� ����
    public void StartCooldown(Skill skill)
    {
        if (!skill.isAvail_Cool)
        {
            skill.CoolTimer = skill.CoolTime;    
        }
    }


    public void UpdateSkillUI(Skill skill)
    {
        if (!skill.isAvail_Cool)
        {
            cooldownImagePrefab.fillAmount = skill.CoolTimer / skill.CoolTime;
            cooldownTextPrefab.text = Mathf.CeilToInt(skill.CoolTimer).ToString();
            cooldownImagePrefab.gameObject.SetActive(true);
            cooldownTextPrefab.gameObject.SetActive(true);
        }
    }
}