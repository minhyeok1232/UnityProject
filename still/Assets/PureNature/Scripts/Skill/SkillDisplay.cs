using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour
{
    // Script
    public Skill skill;                         // 이 스킬 디스플레이가 표시할 스킬 객체
    public SkillManager skillManager;           // SkillManager 스크립트

    // UI
    public Image skillImage;                    // 스킬 아이콘을 표시할 이미지
    public Text skillNameText;                  // 스킬 이름을 표시할 텍스트
    public Text skillLevelText;                 // 스킬 레벨을 표시할 텍스트

    public Button Lv_Button;                    // 스킬 레벨업 버튼

    // Start
    public void Start()
    {
        Lv_Button.onClick.AddListener(() => SkillManager.instance.Skill_LevelUp(skill));    // 싱글톤 스킬매니저 전역으로 현재 skill 객체에 대하여, 스킬레벨업 메소드를 준다.
    }                                                                                       // 리스너를 통해서

    // Method
    public void SkillDisplayUI()
    {
        skillLevelText.text = skill.Skill_Level.ToString();                     // 스킬의 레벨은 텍스트로 표시가 된다.
        Lv_Button.gameObject.SetActive(skill.Skill_Level < skill.maxLevel);     // 스킬의 최대치 레벨을 달성할 시 버튼은 보이지 않게 된다.
    }

    public void UpdateSkillUI(Skill skill)                                      // 스킬이 잠긴 상태이면, 강화하지 못하게 하는 로직
    {
        skillLevelText.text = skill.Skill_Level.ToString();
        skillManager.Lock.gameObject.SetActive(!skill.isUnlocked);              // 잠금 이미지가 있을 경우
    }

    /************************************************************* 스킬 데이터로 UI 세팅 ***********************************************************************/
    public void Initialize(Skill skillData)                                     // 스킬 데이터로 UI를 초기화 하는 메소드
    {
        skill = skillData;
        UpdateSkillDisplay();
    }

    public void UpdateSkillDisplay()                                            // 실제로 UI 컴포넌트를 업데이트하는 메소드
    {
        skillImage.sprite = skill.icon;
        skillNameText.text = skill.skillName;
        skillLevelText.text = skill.Skill_Level.ToString();
    }
/***********************************************************************************************************************************************************/
}
