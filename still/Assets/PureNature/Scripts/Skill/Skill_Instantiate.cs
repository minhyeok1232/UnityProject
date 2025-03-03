using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;                                              // Linq 를 통해 데이터 일관성 확보
public class Skill_Instantiate : MonoBehaviour
{

    // Script
    public SkillManager manager;                                // SkillManager
    public Skill_CoolTime_Script cool;                          // Skill_CoolTime_Script


    // Image, Text
    public Image skill_Image;                                   // 스킬 이미지
    public Text skill_Key_Setting;                              // 키 세팅 Text (추후 유동적인 변화)



    public Skill skill;


    public Image cooldownImagePrefab;
    public Text cooldownTextPrefab;

    //private bool isStartCool;

    private void Start()
    {
        skill.CoolTimer = PlayerPrefs.GetFloat(skill.skillName, 0);
    }

    // Update (4단계)
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
    // 3단계 : 스킬 사용을 하게 되면, 들어가게 되는 로직
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