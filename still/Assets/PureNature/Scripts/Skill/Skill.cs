using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName; // ��ų �̸�
    public Sprite icon; // ��ų ������
    public int requiredLevel; // �䱸 ����
    public bool isUnlocked; // ��ų ��� ���� ����
    public string description; // ��ų ����
    public float CoolTime; // ��ų ��Ÿ��
    public float CoolTimer;
    public int ManaUse; // ��ų ���� �Һ�

    public int Skill_Level; // ��ų�� ����
    public int maxLevel = 20; // ��ų�� �ִ� ����
    public float baseAttackPercentage = 100f; // ���� 1������ ���ݷ� �ۼ�Ʈ
    public float attackIncreasePerLevel = 4f; // ������ ���ݷ� ������

    public Skill RequireSkill; // �� ��ų�� ��� �����ϱ� ���� ���� ��ų
    public int RequireSkillLevel;   // ���ེų �䱸���� ����

    public bool isAvail_Cool = false; // ��Ÿ�� ����

    public GameObject skill_Effects;
    public bool IsCooldownActive = false;
    public bool _Start;

    public Transform uiParent;

    // ������ ���� ������ �������� �޼���
    public string GetLevelDescription(int level)
    {
        float attackPercentage = baseAttackPercentage + (level - 1) * attackIncreasePerLevel;
        return $"Level {level}: {attackPercentage}% attack";
    }

    public bool CanUnlock()                                                                     // ��ų ��� �޼ҵ�
    {
        if (RequireSkill == null) return true;                                                  // ���� ��ų�� �����Ǿ� ���� ������ ����� ������ �� ����

        return RequireSkill.isUnlocked && RequireSkill.Skill_Level >= RequireSkillLevel;        // ���� ��ų�� ���� ������ �䱸 ���� �̻����� Ȯ��
    }

    public string GetAllLevelsDescription()                                                     // ��� ������ ���� ������ ��ȯ�ϴ� �޼���
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