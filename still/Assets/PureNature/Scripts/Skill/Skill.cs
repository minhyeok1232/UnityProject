using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName; // 스킬 이름
    public Sprite icon; // 스킬 아이콘
    public int requiredLevel; // 요구 레벨
    public bool isUnlocked; // 스킬 잠금 해제 여부
    public string description; // 스킬 설명
    public float CoolTime; // 스킬 쿨타임
    public float CoolTimer;
    public int ManaUse; // 스킬 마나 소비량

    public int Skill_Level; // 스킬의 레벨
    public int maxLevel = 20; // 스킬의 최대 레벨
    public float baseAttackPercentage = 100f; // 레벨 1에서의 공격력 퍼센트
    public float attackIncreasePerLevel = 4f; // 레벨당 공격력 증가량

    public Skill RequireSkill; // 이 스킬을 잠금 해제하기 위한 선행 스킬
    public int RequireSkillLevel;   // 선행스킬 요구조건 레벨

    public bool isAvail_Cool = false; // 쿨타임 여부

    public GameObject skill_Effects;
    public bool IsCooldownActive = false;
    public bool _Start;

    public Transform uiParent;

    // 레벨에 따른 설명을 가져오는 메서드
    public string GetLevelDescription(int level)
    {
        float attackPercentage = baseAttackPercentage + (level - 1) * attackIncreasePerLevel;
        return $"Level {level}: {attackPercentage}% attack";
    }

    public bool CanUnlock()                                                                     // 스킬 잠금 메소드
    {
        if (RequireSkill == null) return true;                                                  // 선행 스킬이 설정되어 있지 않으면 잠금을 해제할 수 있음

        return RequireSkill.isUnlocked && RequireSkill.Skill_Level >= RequireSkillLevel;        // 선행 스킬의 현재 레벨이 요구 레벨 이상인지 확인
    }

    public string GetAllLevelsDescription()                                                     // 모든 레벨에 대한 설명을 반환하는 메서드
    {
        string descriptions = "";                                                               
        for (int i = 1; i <= maxLevel; i++)
        {
            descriptions += GetLevelDescription(i) + "\n";
        }
        return descriptions;
    }
    public Transform GetUiPanelTransform()
    {
        return uiParent;
    }
}